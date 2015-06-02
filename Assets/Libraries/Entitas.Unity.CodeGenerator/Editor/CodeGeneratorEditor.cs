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
		//I did this :)
		[MenuItem("Entitas/Test")]
		public static void Test() {
			String[] lines = File.ReadAllLines("Assets/Logs/2015-05-20_TestLog(7).txt");
			Dictionary<String, List<String>> entityEntries = new Dictionary<String, List<String>>();

			foreach (String line in lines)
			{
				String[] split = line.Split(':');

				if (!entityEntries.ContainsKey(split[0]))
				{
					entityEntries.Add(split[0], new List<string>());
				}
				if(split.Length>1)
					entityEntries[split[0]].Add(split[1]);

			}

			foreach (KeyValuePair<String, List<string>> kvp in entityEntries)
			{
				Debug.Log(kvp.Key);
				Debug.Log("\t" + string.Join("\n\t", kvp.Value.ToArray()));

			}
		}
	}
}
