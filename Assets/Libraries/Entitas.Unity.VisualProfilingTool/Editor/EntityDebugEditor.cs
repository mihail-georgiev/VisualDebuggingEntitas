using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGenerator;
using UnityEditor;
using UnityEngine;

namespace Entitas.Unity.VisualProfilingTool {
    [CustomEditor(typeof(EntityDebugBehaviour))]
    public class EntityDebugEditor : Editor {
	static ICustomDrawer[] _customDrawers;

        void Awake() {
		var types = Assembly.GetAssembly(typeof(EntityDebugEditor)).GetTypes();
		_customDrawers = types
			.Where(type => type.GetInterfaces().Contains(typeof(ICustomDrawer)))
			.Select(type => (ICustomDrawer)Activator.CreateInstance(type))
			.ToArray();
        }

        public override void OnInspectorGUI() {
                drawTarget();
		EditorUtility.SetDirty(target);
        }

        void drawTarget() {
            var debugBehaviour = (EntityDebugBehaviour)target;
            var pool = debugBehaviour.pool;
            var entity = debugBehaviour.entity;

            if (GUILayout.Button("Destroy Entity")) {
                pool.DestroyEntity(entity);
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Components (" + entity.GetComponents().Length + ")", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            var indices = entity.GetComponentIndices();
            var components = entity.GetComponents();
            for (int i = 0; i < components.Length; i++) {
                drawComponent(entity, indices[i], components[i]);
            }
            EditorGUILayout.EndVertical();
        }


        void drawComponent(Entity entity, int index, IComponent component) {
            var componentType = component.GetType();
            var fields = componentType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            if (fields.Length == 0) {
                EditorGUILayout.LabelField(componentType.RemoveComponentSuffix(), EditorStyles.boldLabel);
            }

            EditorGUILayout.EndHorizontal();

            foreach (var field in fields) {
		object value = field.GetValue(component);
		var newValue = drawAndGetNewValue(field.FieldType, field.Name, value, entity, index, component);
		if (didValueChange(value, newValue)) {
			entity.WillRemoveComponent(index);
			field.SetValue(component,newValue);
			entity.ReplaceComponent(index, component);
		}
            }
            EditorGUILayout.EndVertical();
        }

	static bool didValueChange(object value, object newValue) {
		return (value == null && newValue != null) ||
			(value != null && newValue == null) ||
				((value != null && newValue != null &&
				  !newValue.Equals(value)));
	}

	static object drawAndGetNewValue(Type type, string fieldName, object value, Entity entity, int index, IComponent component) {

		var customDrawer = getCustomDrawer(type);
		if (customDrawer != null) {
			value = customDrawer.DrawAndGetNewValue(type, fieldName, value, entity, index, component);
		} else {
			drawUnsupportedType(type, fieldName, value);
		}

		return value;
	}
		
	static void drawUnsupportedType(Type type, string fieldName, object value) {
		EditorGUILayout.BeginHorizontal();
            	EditorGUILayout.LabelField(fieldName, value.ToString());
            	EditorGUILayout.EndHorizontal();
        }

	static ICustomDrawer getCustomDrawer(Type type) {
		foreach (var drawer in _customDrawers) {
			if (drawer.HandlesType(type)) {
				return drawer;
			}
		}
		
		return null;
	}
   }
}