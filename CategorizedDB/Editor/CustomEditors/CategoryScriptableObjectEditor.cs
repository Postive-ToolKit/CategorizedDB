using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using UnityEditor;

namespace Postive.CategorizedDB.Editor.CustomEditors
{
    [CustomEditor(typeof(CategoryScriptableObject),true)]
    public class CategoryScriptableObjectEditor : UnityEditor.Editor
    {
        private List<string> _defaultProperties = new List<string>() { "m_Script", "_parentGUID", "_name", "_path" };
        public override void OnInspectorGUI() {
            //get object
            CategoryScriptableObject categoryScriptableObject = (CategoryScriptableObject) target;
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_parentGUID"));
            //show path
            EditorGUILayout.LabelField("Path", categoryScriptableObject.GetPath());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));
            serializedObject.ApplyModifiedProperties();
            ShowOtherProperties();
        }
        public virtual void ShowOtherProperties()
        {
            bool isContentExist = false;
            //show other properties
            serializedObject.Update();
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            while (property.NextVisible(false)) {
                if (!_defaultProperties.Contains(property.name)) {
                    if (!isContentExist) {
                        isContentExist = true;
                        EditorGUILayout.LabelField("Contents", EditorStyles.boldLabel);
                        //draw thin line
                        var rect = EditorGUILayout.GetControlRect(false, 1);
                        rect.height = 1;
                        EditorGUI.DrawRect(rect, new UnityEngine.Color(0.5f, 0.5f, 0.5f, 1));
                        
                    }
                    EditorGUILayout.PropertyField(property, true);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}