using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Postive.CategorizedDB.Editor.CustomEditors
{
    [CustomEditor(typeof(CategoryScriptableObject),true)]
    public class CategoryScriptableObjectEditor : UnityEditor.Editor
    {
        private List<string> _defaultProperties = new List<string>() { "m_Script", "_parentGUID", "_name", "_path" };
        private CategoryScriptableObject _categoryScriptableObject;
#if ODIN_INSPECTOR
        private PropertyTree _propertyTree;
#endif
        private void OnEnable() {
            _categoryScriptableObject = (CategoryScriptableObject) target;
#if ODIN_INSPECTOR
            _propertyTree = PropertyTree.Create(serializedObject.targetObject);
#endif
        }
        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_parentGUID"));
            //show path
            EditorGUILayout.LabelField("Path", _categoryScriptableObject.GetPath());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));
            if (GUI.changed) {
                serializedObject.ApplyModifiedProperties();
            }
            
            
            EditorGUILayout.LabelField("Contents", EditorStyles.boldLabel);
            //draw thin line
            var rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
            
            ShowOtherProperties();
        }
        public virtual void ShowOtherProperties() {
#if ODIN_INSPECTOR
            _propertyTree.UpdateTree();
            var tree = _propertyTree.EnumerateTree(false);
            _propertyTree.BeginDraw(false);
            foreach (var property in tree) {
                if (!_defaultProperties.Contains(property.Name)) {
                    property.Draw();
                }
            }
            _propertyTree.EndDraw();
            _propertyTree.ApplyChanges();
            
#else
            serializedObject.Update();
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            while (property.NextVisible(false)) {
                if (!_defaultProperties.Contains(property.name)) {
                    EditorGUILayout.PropertyField(property, true);
                }
            }
            serializedObject.ApplyModifiedProperties();
#endif
        }
    }
}