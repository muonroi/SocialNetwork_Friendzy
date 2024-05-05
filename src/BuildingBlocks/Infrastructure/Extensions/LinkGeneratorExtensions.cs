using Commons.Pagination;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Extensions
{
    public static class LinkGeneratorExtensions
    {
        public static T BuildLinks<T>(
               this LinkGenerator linkGenerator,
               T obj,
               HttpContext httpContext,
               Dictionary<string, string> actionInfo,
               object values) where T : new()
        {
            obj ??= new T();

            foreach (string actionName in actionInfo.Keys)
            {
                if (!actionInfo.TryGetValue(actionName, out string? info))
                {
                    continue;
                }

                ControllerActionDescriptor? actionDescriptor = FindActionDescriptor(httpContext, actionName);
                string? generateUri = !HasFromQueryOrFromRoute(actionDescriptor!)
                            ? linkGenerator.GetPathByName(httpContext, actionName)
                            : linkGenerator.GetPathByName(httpContext, actionName, values);

                if (string.IsNullOrEmpty(generateUri))
                {
                    continue;
                }

                string method = GetHttpMethod(actionDescriptor!);
                LinkResponse link = new()
                {
                    Href = generateUri,
                    Method = method
                };

                System.Type type = obj.GetType();
                PropertyInfo? property = type.GetProperty(info);

                if (property != null && property.PropertyType == typeof(LinkResponse))
                {
                    property.SetValue(obj, link);
                }
            }

            return obj;
        }

        private static string GetHttpMethod(ControllerActionDescriptor descriptor)
        {
            if (descriptor.EndpointMetadata != null)
            {
                foreach (object metadata in descriptor.EndpointMetadata)
                {
                    if (metadata is HttpMethodAttribute httpMethodAttribute)
                    {
                        return new HttpMethod(httpMethodAttribute.HttpMethods.FirstOrDefault()!).Method;
                    }
                }
            }

            return HttpMethod.Get.Method;
        }

        private static ControllerActionDescriptor? FindActionDescriptor(HttpContext httpContext, string actionName)
        {
            IActionDescriptorCollectionProvider? actionProvider = httpContext.RequestServices.GetService<IActionDescriptorCollectionProvider>();
            if (actionProvider is null)
            {
                return null;
            }

            IReadOnlyList<ActionDescriptor> actionDescriptors = actionProvider.ActionDescriptors.Items;
            return actionDescriptors
                .OfType<ControllerActionDescriptor>()
                .FirstOrDefault(a =>
                    string.Equals(a.ActionName, actionName, StringComparison.OrdinalIgnoreCase));
        }

        public static bool HasFromQueryOrFromRoute(ControllerActionDescriptor actionDescriptor)
        {
            return actionDescriptor is not null && ActionHasFromQueryOrFromRoute(actionDescriptor.MethodInfo);
        }

        private static bool ActionHasFromQueryOrFromRoute(MethodInfo methodInfo)
        {
            foreach (ParameterInfo parameter in methodInfo.GetParameters())
            {
                if (HasFromQueryOrFromRouteAttribute(parameter))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasFromQueryOrFromRouteAttribute(ParameterInfo parameter)
        {
            return parameter.CustomAttributes.Any(x =>
                x.AttributeType == typeof(FromQueryAttribute) ||
                x.AttributeType == typeof(FromRouteAttribute));
        }
    }
}