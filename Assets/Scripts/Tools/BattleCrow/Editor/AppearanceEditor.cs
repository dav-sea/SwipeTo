using UnityEditor;
//using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace BattleCrow
{
    [CustomEditor(typeof(Appearance))]
    public class AppearenceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Appearance appearance = (Appearance)target;

            appearance.ActivateBeforeShow = EditorGUILayout.ToggleLeft("ActivateBeforeShow", appearance.ActivateBeforeShow);
            appearance.IsAppearance = EditorGUILayout.ToggleLeft("IsAppearance", appearance.IsAppearance);
        }
    }
}