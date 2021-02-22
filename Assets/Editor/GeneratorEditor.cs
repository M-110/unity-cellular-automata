using System.Net;
using Rules;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorEditor : UnityEditor.Editor
    {
        Generator myGenerator;
        public override void OnInspectorGUI()
        {
            myGenerator = (Generator) target;
            
            
            GeneralProperties();
            DynamicProperties();
            Buttons();
        }

        void GeneralProperties()
        {
            myGenerator.depth =  EditorGUILayout.IntField("Depth", myGenerator.depth);
            myGenerator.cube = (GameObject)EditorGUILayout.ObjectField("Cube", myGenerator.cube, typeof(GameObject), true);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            myGenerator.rulesType = (RulesType)EditorGUILayout.EnumPopup("Rules Type", myGenerator.rulesType);
        }

        void DynamicProperties()
        {
            if (myGenerator.rulesType == RulesType.General)
            {
                EditorGUILayout.HelpBox("General Rule 32-bit\n (0 to 4,294,967,295)", MessageType.None);
                DrawDefaultInspector();
            }
            else if (myGenerator.rulesType == RulesType.GrowthTotalistic)
            {
                EditorGUILayout.HelpBox("General Rule 5-bit\n (0 to 32)", MessageType.None);
                int tempInt = EditorGUILayout.IntField("Rule Number", (int) myGenerator.ruleNumber);
                if (tempInt < 0)
                    tempInt = 0;
                else if (tempInt > 32)
                    tempInt = 32;
                myGenerator.ruleNumber = (uint) tempInt;


            }
        }

        void Buttons()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            if (GUILayout.Button("Generate Random Rules"))
                myGenerator.GenerateRandomRules();
            
            if (GUILayout.Button("Generate Random Rules (Even)"))
                myGenerator.GenerateRandomEvenRules();
            
            if (GUILayout.Button("Generate Random Rules (1D)"))
                myGenerator.GenerateRandom1DRules();
        }
    }
}
