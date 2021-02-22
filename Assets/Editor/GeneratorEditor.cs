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
            else if (myGenerator.rulesType == RulesType.Totalistic)
            {
                EditorGUILayout.HelpBox("Totalistic Rule 6-bit\n (0 to 63)", MessageType.None);
                int tempInt = EditorGUILayout.IntField("Rule Number", (int) myGenerator.ruleNumber);
                myGenerator.ruleNumber = (uint) (tempInt < 0 ? 0 : tempInt > 63 ? 63 : tempInt);
            }
            else if (myGenerator.rulesType == RulesType.GrowthTotalistic)
            {
                EditorGUILayout.HelpBox("Growth Totalistic Rule 5-bit\n (0 to 31)", MessageType.None);
                int tempInt = EditorGUILayout.IntField("Rule Number", (int) myGenerator.ruleNumber);
                myGenerator.ruleNumber = (uint) (tempInt < 0 ? 0 : tempInt > 31 ? 31 : tempInt);
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
