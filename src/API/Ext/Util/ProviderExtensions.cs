using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace API.Ext.Util
{
    public static class ProviderExtensions
    {
        /// <summary>
        /// This extension function provides a utiliity to support application of parameter
        /// values to an instance of a ProviderProduct.
        /// </summary>
        /// <param name="prod">the target product instance to apply settings to</param>
        /// <param name="prodParams">the collection of parameter details to search for</param>
        /// <param name="paramValues">a dictionary of values to be applied as parameters,
        ///    keyed by the unique name given in the parameter details</param>
        /// <param name="strictNames">if true, will throw an error if there are
        ///    parameter values specified for unknown parameter names</param>
        /// <param name="widenToCollections">if true, when matching parameter
        ///    values to the corresponding parameter, if the property type of the
        ///    parameter on an instance is a collection of elements compatible with
        ///    the supplied value type, the value will be assigned after being wrapped
        ///    in a compatible collection</param>
        /// <param name="tryConversion">if true, when the value type is not
        ///    directly assignable to the property type of a parameter, a type conversion
        ///    will be attempted</param>
        /// <param name="requiredEnforced">if true, will throw an exception if any of
        ///    the required parameters are missing from the given values</param>
        /// <param name="filter">an optional function to invoke upon any found parameters
        ///    which would return a tuple indicating if the parameter should be applied
        ///    and if so, an opportunity to transform the supplied value</param>
        /// <exception cref="ArgumentException">
        /// Thrown if there are any failures to apply the supplied parameter values
        /// to the target product instance in the context of the supplied paramter details.
        /// </exception>
        /// <returns>
        /// Returns the same argument product instance that was provided as an argument
        /// in support of a fluid interface.
        /// </returns>
        /// <remarks>
        /// This routine would typically be used by IProvider
        /// to apply settings to product instances that it produces.
        /// </remarks>
        public static TProd ApplyParameters<TProd>(this TProd prod,
                List<ProviderParameterInfo> prodParams,
                IDictionary<string, object> paramValues,
                bool strictNames = false,
                bool requiredEnforced = true,
                bool widenToCollections = true,
                bool tryConversion = true,
                Func<ProviderParameterInfo, object, Tuple<bool, object>> filter = null)
            where TProd : IProviderProduct
        {
            TypeInfo prodTypeInfo = typeof(TProd).GetTypeInfo();

            List<string> missingParams = new List<string>();
            List<string> unknownParams = new List<string>();
            List<string> unknownProps = new List<string>();
            List<ArgumentException> applyFailed = new List<ArgumentException>();

            int foundParamValues = 0;
            foreach (ProviderParameterInfo param in prodParams)
            {
                if (!paramValues.ContainsKey(param.Name))
                {
                    if (requiredEnforced && param.IsRequired)
                        missingParams.Add(param.Name);
                    continue;
                }

                // We keep a count so we know if we may
                // have had any unexpected params
                ++foundParamValues;

                // Start with the value given
                object value = paramValues[param.Name];

                // See if a filter was supplied
                if (filter != null)
                {
                    Tuple<bool, object> f = filter(param, value);

                    // Skip the value if the filter indicated so
                    if (!f.Item1)
                        continue;

                    // Get the possibly transformed value
                    value = f.Item2;
                }

                PropertyInfo prop = prodTypeInfo.GetProperty(param.Name, BindingFlags.Public | BindingFlags.Instance);
                Type propType = prop.PropertyType;
                Type valueType = value?.GetType();

                if (prop == null)
                {
                    unknownProps.Add(param.Name);
                    continue;
                }

                if (valueType != null && !propType.IsAssignableFrom(valueType))
                {
                    // Check if we can wrap the value as a collection
                    if (widenToCollections)
                    {
                        // Test for compatible value array
                        if (propType.IsArray && propType.GetElementType().IsAssignableFrom(valueType))
                        {
                            Array arr = Array.CreateInstance(valueType, 1);
                            valueType = arr.GetType();
                            arr.SetValue(value, 0);
                            value = arr;
                        }
                        // Test for compatible generic collection
                        else if (propType.IsAssignableFrom(typeof(ICollection<>)
                                .MakeGenericType(valueType)))
                        {
                            object list = Activator.CreateInstance(typeof(List<>)
                                    .MakeGenericType(valueType));
                            valueType = list.GetType();
                            valueType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance)
                                    .Invoke(list, new[] { value });
                            value = list;
                        }
                        // Test for untyped collection
                        else if (propType.IsAssignableFrom(typeof(ICollection)))
                        {
                            ArrayList list = new ArrayList(1);
                            valueType = list.GetType();
                            list.Add(value);
                            value = list;
                        }
                    }

                    // Check if we should/can try to convert the value
                    if (!propType.IsAssignableFrom(valueType) && tryConversion)
                    {
                        TypeConverter typeConv = TypeDescriptor.GetConverter(prop.PropertyType);
                        value = typeConv.ConvertFrom(value);
                    }
                }

                try
                {
                    // Best effort to assign the value
                    prop.SetValue(prod, value);
                }
                catch (Exception ex)
                {
                    applyFailed.Add(new ArgumentException(ex.Message, param.Name, ex));
                }
            }

            if (strictNames && foundParamValues < paramValues.Count)
            {
                // Uh oh, there are some parameters we didn't know about
                IEnumerable<string> paramNames = prodParams.Select(x => x.Name);
                unknownParams.AddRange(paramValues.Keys.Where(x => !paramNames.Contains(x)));
            }

            if (missingParams.Count > 0 || unknownParams.Count > 0 || unknownProps.Count > 0 || applyFailed.Count > 0)
            {
                ArgumentException ex = new ArgumentException("One or more parameters cannot be applied");
                if (missingParams.Count > 0)
                    ex.Data.Add(nameof(missingParams), missingParams);
                if (unknownParams.Count > 0)
                    ex.Data.Add(nameof(unknownParams), unknownParams);
                if (unknownProps.Count > 0)
                    ex.Data.Add(nameof(unknownProps), unknownProps);
                if (applyFailed.Count > 0)
                    ex.Data.Add(nameof(applyFailed), applyFailed);

                throw ex;
            }

            return prod;
        }
    }
}
