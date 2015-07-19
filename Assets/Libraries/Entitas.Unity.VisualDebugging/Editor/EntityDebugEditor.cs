using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGenerator;
using UnityEditor;
using UnityEngine;

namespace Entitas.Unity.VisualDebugging {
    [CustomEditor(typeof(EntityDebugBehaviour)), CanEditMultipleObjects]
    public class EntityDebugEditor : Editor {
		static ITypeDrawer[] _typeDrawers;

        void Awake() {
		var types = Assembly.GetAssembly(typeof(EntityDebugEditor)).GetTypes();
		_typeDrawers = types
			.Where(type => type.GetInterfaces().Contains(typeof(ITypeDrawer)))
			.Select(type => (ITypeDrawer)Activator.CreateInstance(type))
			.ToArray();
        }

        public override void OnInspectorGUI() {
            if (targets.Length == 1) {
                drawSingleTarget();
            } else {
                drawMultiTargets();
            }
            EditorUtility.SetDirty(target);
        }

        void drawSingleTarget() {
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

        void drawMultiTargets() {
            if (GUILayout.Button("Destroy selected entities")) {
                foreach (var t in targets) {
                    var debugBehaviour = (EntityDebugBehaviour)t;
                    var pool = debugBehaviour.pool;
                    var entity = debugBehaviour.entity;
                    pool.DestroyEntity(entity);
                }
            }

            EditorGUILayout.Space();

            foreach (var t in targets) {
                var debugBehaviour = (EntityDebugBehaviour)t;
                var pool = debugBehaviour.pool;
                var entity = debugBehaviour.entity;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(entity.ToString());
                if (GUILayout.Button("Destroy Entity")) {
                    pool.DestroyEntity(entity);
                }
                EditorGUILayout.EndHorizontal();
            }
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
	            var value = field.GetValue(component);
				DrawAndSetElement(field.FieldType, field.Name, value,
				                  entity, index, component, newValue => field.SetValue(component, newValue));
            }
            EditorGUILayout.EndVertical();
        }

	public static void DrawAndSetElement(Type type, string fieldName, object value, Entity entity, int index, IComponent component, Action<object> setValue) {
		var newValue = DrawAndGetNewValue(type, fieldName, value, entity, index, component);
		if (didValueChange(value, newValue)) {
			entity.WillRemoveComponent(index);
			setValue(newValue);
			entity.ReplaceComponent(index, component);
		}
	}

	static bool didValueChange(object value, object newValue) {
		return (value == null && newValue != null) ||
			(value != null && newValue == null) ||
				((value != null && newValue != null &&
				  !newValue.Equals(value)));
	}

	public static object DrawAndGetNewValue(Type type, string fieldName, object value, Entity entity, int index, IComponent component) {
		if (!type.IsValueType) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
		}
		var typeDrawer = getTypeDrawer(type);
		if (typeDrawer != null) {
			value = typeDrawer.DrawAndGetNewValue(type, fieldName, value, entity, index, component);
		} else {
			drawUnsupportedType(type, fieldName, value);
		}

		if (!type.IsValueType) {
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
			
		return value;
	}
		
	static void drawUnsupportedType(Type type, string fieldName, object value) {
		EditorGUILayout.BeginHorizontal();
            	EditorGUILayout.LabelField(fieldName, value.ToString());
            	EditorGUILayout.EndHorizontal();
        }

	static ITypeDrawer getTypeDrawer(Type type) {
		foreach (var drawer in _typeDrawers) {
			if (drawer.HandlesType(type)) {
				return drawer;
			}
		}
		
		return null;
	}
   }
}