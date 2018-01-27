using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace API.Messages.ModelBinding
{
    /// <summary>
    /// Specifies that a property of a model class should be bound to a response header,
    /// when used in concert with the ModelResultExt Model extension method for MVC Controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ToHeaderAttribute : Attribute, /*IBindingSourceMetadata,*/ IModelNameProvider
    {
        public string Name { get; set; }

        public bool Replace { get; set; }
    }

    /// <summary>
    /// Specifies that a property of a model class should be bound to a response content body or action result,
    /// when used in concert with the ModelResultExt Model extension method for MVC Controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ToResultAttribute : Attribute/*, IBindingSourceMetadata*/
    {
        public string ContentType { get; set; }
    }
}
