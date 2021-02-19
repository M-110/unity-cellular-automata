using System.Net;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Generator myGenerator = (Generator) target;

            if (GUILayout.Button("Generate Random Rules"))
                myGenerator.GenerateRandomRules();
            
            if (GUILayout.Button("Generate Random Rules (Even)"))
                myGenerator.GenerateRandomEvenRules();
            
            if (GUILayout.Button("Generate Random Rules (1D)"))
                myGenerator.GenerateRandom1DRules();
        }
    }
}
