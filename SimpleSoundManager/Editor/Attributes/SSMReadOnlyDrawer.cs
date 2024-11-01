using Postive.SimpleSoundAssetManager.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Postive.SimpleSoundAssetManager.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(SSMReadOnlyAttribute))]
    public class SSMReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}