using System;
using System.Reflection;
using Entitas;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Entitas.Unity.CodeGenerator {
    public static class CodeGeneratorEditor {

        [MenuItem("Entitas/Generate")]
        public static void Generate() {
            var types = Assembly.GetAssembly(typeof(Entity)).GetTypes();
            var config = new CodeGeneratorConfig(EntitasPreferencesEditor.LoadConfig());
            Entitas.CodeGenerator.CodeGenerator.Generate(types, config.pools, config.generatedFolderPath);
            AssetDatabase.Refresh();
        }
	}
}
