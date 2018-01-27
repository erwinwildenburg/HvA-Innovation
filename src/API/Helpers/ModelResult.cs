using Microsoft.AspNetCore.Mvc;
using API.Messages.ModelBinding;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace API.Helpers
{
    public static class ModelResultExt
    {
        private const string ContentTypeOctetStream = "application/octet-stream";

        /// <summary>
        /// Defines an extension method for MVC Controllers that supports returning
        /// an <see cref="IActionResult">Action Result</see> that is defined by
        /// a response model class.
        /// This extension provides ToHeader and ToResult.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public static IActionResult Model(this ControllerBase controller, object model)
        {
            PropertyInfo toResultProperty = null;
            IActionResult result = null;

            PropertyInfo[] properties = model.GetType().GetTypeInfo().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute(typeof(ToHeaderAttribute)) is ToHeaderAttribute toHeader)
                {
                    string headerName = toHeader.Name;

                    if (string.IsNullOrEmpty(headerName)) headerName = property.Name;
                    // TODO: Add support for string[]

                    string headerValue = ConvertTo<string>(property.GetValue(model, null));

                    // Append the header to the response
                    if (toHeader.Replace) controller.Response.Headers[headerName] = headerValue;
                    controller.Response.Headers.Add(headerName, headerValue);

                    continue;
                }

                ToResultAttribute toResult = property.GetCustomAttribute(typeof(ToResultAttribute)) as ToResultAttribute;

                if (toResult == null) continue;
                if (toResultProperty != null) throw new InvalidOperationException("Multiple Result-mapping attributes found");

                toResultProperty = property;
                Type toResultType = property.PropertyType;

                if (typeof(IActionResult).IsAssignableFrom(toResultType))
                {
                    result = (IActionResult)property.GetValue(model, null);
                    continue;
                }

                string contentType = toResult.ContentType;

                if (toResultType == typeof(byte[]))
                {
                    byte[] resultValue = (byte[])property.GetValue(model, null);
                    result = new FileContentResult(resultValue, contentType ?? ContentTypeOctetStream);
                    continue;
                }
                if (typeof(Stream).IsAssignableFrom(toResultType))
                {
                    Stream resultValue = (Stream)property.GetValue(model, null);
                    result = new FileStreamResult(resultValue, contentType ?? ContentTypeOctetStream);
                    continue;
                }
                if (typeof(FileInfo).IsAssignableFrom(toResultType))
                {
                    FileInfo resultValue = (FileInfo)property.GetValue(model, null);
                    result = new PhysicalFileResult(resultValue.FullName, contentType ?? ContentTypeOctetStream);
                    continue;
                }
                if (typeof(string) == toResultType)
                {
                    string resultValue = (string)property.GetValue(model, null);
                    result = new ContentResult
                    {
                        Content = resultValue,
                        ContentType = contentType
                    };
                    continue;
                }

                object propertyValue = property.GetValue(model, null);
                if (propertyValue != null) result = new JsonResult(propertyValue);
            }

            return result ?? new OkResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T ConvertTo<T>(object value)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
            return (T)typeConverter.ConvertFrom(value);
        }
    }
}
