using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(DialogContent))]
    public class DialogContentDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_content"), label);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_content"), label);
        }
    }
}