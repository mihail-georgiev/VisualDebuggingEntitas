using System;
using Entitas;
using UnityEditor;

namespace Entitas.Unity.VisualProfilingTool {
    public class IntCustomDrawer : ICustomDrawer {
        public bool HandlesType(Type type) {
            return type == typeof(int);
        }

        public object DrawAndGetNewValue(Type type, string fieldName, object value, Entity entity, int index, IComponent component) {
            return EditorGUILayout.IntField(fieldName, (int)value);
        }
    }
}
