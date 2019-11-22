using System;
using System.Reflection;
using System.Web.Mvc;

namespace DemoEshop.PresentationLayer.Helpers.Attributes
{
    /// <summary>
    /// Enables multiple HTTP Post action methods per single form.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MultiPostActionAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = $"{Name}:{Argument}";
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
}