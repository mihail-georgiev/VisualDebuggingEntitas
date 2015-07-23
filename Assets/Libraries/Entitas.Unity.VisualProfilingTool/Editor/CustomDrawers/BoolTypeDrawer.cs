using System;
using Entitas;
using UnityEditor;

namespace Entitas.Unity.VisualProfilingTool {
    public class BoolCustomDrawer : ICustomDrawer {
        public bool HandlesType(Type type) {
            return type == typeof(bool);
        }

        public object DrawAndGetNewValue(Type type, string fieldName, object value, Entity entity, int index, IComponent component) {
            return EditorGUILayout.Toggle(fieldName, (bool)value);
        }
    }
}