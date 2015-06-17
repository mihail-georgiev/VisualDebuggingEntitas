using System.Reflection;
using System;

namespace Entitas {
	public static class ComponentExtension {

		public static string DebugInfo(this IComponent component, int entityIndex, string eventType, DateTime time) {
			double timePassed = (time - AppUtils.startAppTime).TotalMilliseconds;

			if(eventType == "removed")
				return "Entity_" + entityIndex + ": " + eventType + " " + component.GetType().ToString() + " at " + timePassed;

			var fields =  component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			string info = "Entity_" + entityIndex + ": " + eventType + " " + component.GetType().ToString();
			if(fields.Length>0) {
				info += ", fields[ ";
				foreach(var field in fields)
				{
					info+= field.Name + " - " + field.GetValue(component) + " "; 
				}
				info+= "]";
			}

			return info += " at " + timePassed;
        }
    }
}