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
        GUIStyle _foldoutStyle;

        void Awake() {
            _foldoutStyle = new GUIStyle(EditorStyles.foldout);
            _foldoutStyle.fontStyle = FontStyle.Bold;

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
            if (GUILayout.Button("▸", GUILayout.Width(21), GUILayout.Height(14))) {
                debugBehaviour.FoldAllComponents();
            }
            if (GUILayout.Button("▾", GUILayout.Width(21), GUILayout.Height(14))) {
                debugBehaviour.UnfoldAllComponents();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            var indices = entity.GetComponentIndices();
            var components = entity.GetComponents();
            for (int i = 0; i < components.Length; i++) {
                drawComponent(debugBehaviour, entity, indices[i], components[i]);
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

        void drawComponent(EntityDebugBehaviour debugBehaviour, Entity entity, int index, IComponent component) {
            var componentType = component.GetType();
            var fields = componentType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            if (fields.Length == 0) {
                EditorGUILayout.LabelField(componentType.RemoveComponentSuffix(), EditorStyles.boldLabel);
            } else {
                debugBehaviour.unfoldedComponents[index] = EditorGUILayout.Foldout(debugBehaviour.unfoldedComponents[index], componentType.RemoveComponentSuffix(), _foldoutStyle);
            }

            EditorGUILayout.EndHorizontal();

            if (debugBehaviour.unfoldedComponents[index]) {
                foreach (var field in fields) {
                    var value = field.GetValue(component);
					drawUnsupportedType(field.FieldType, field.Name, value);
                }
            }
            EditorGUILayout.EndVertical();
        }


        static void drawUnsupportedType(Type type, string fieldName, object value) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, value.ToString());
            EditorGUILayout.EndHorizontal();
        }
    }
}

