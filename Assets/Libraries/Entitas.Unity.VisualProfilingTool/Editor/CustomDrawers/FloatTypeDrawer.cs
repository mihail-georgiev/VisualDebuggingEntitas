using System;
using Entitas;
using UnityEditor;

namespace Entitas.Unity.VisualProfilingTool {
    public class FloatCustomDrawer : ICustomDrawer {
        public bool HandlesType(Type type) {
            return type == typeof(float);
        }

        public object DrawAndGetNewValue(Type type, string fieldName, object value, Entity entity, int index, IComponent component) {
            return EditorGUILayout.FloatField(fieldName, (float)value);
        }
    }
}