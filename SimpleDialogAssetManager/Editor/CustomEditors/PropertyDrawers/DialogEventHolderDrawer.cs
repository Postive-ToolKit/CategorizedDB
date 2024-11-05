using System;
using System.Collections.Generic;
using System.Linq;
using Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events;
using UnityEditor;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(DialogEventHolder))]
    public class DialogEventHolderDrawer : PropertyDrawer {
        private List<Type> _eventTypes;
        private string[] _eventTypeNames;
        private int _selectedIndex = 0;
        private float _currentArrayHeight = 0;
        public DialogEventHolderDrawer() {
            _eventTypes = TypeCache.GetTypesDerivedFrom<DialogEvent>().ToList();
            _eventTypeNames = _eventTypes.Select(t => t.Name).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.DrawRect(new Rect(position.x, position.y, position.width, 1), new Color(.5f,.5f,.5f));
            position.y += 2;
            SerializedProperty currentType = property.FindPropertyRelative("_type");
            _selectedIndex = _eventTypes.IndexOf(_eventTypes.Find(t => t.Name == currentType.stringValue));
            _selectedIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), "Event Type", _selectedIndex, _eventTypeNames);
            if (GUI.changed) {
                _selectedIndex = Mathf.Max(0, _selectedIndex);
                Type selectedType = _eventTypes[_selectedIndex];
                currentType.stringValue = selectedType.Name;
            }
            if (GUI.Button(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), "Create Event")) {
                SerializedProperty dialogEventProperty = property.FindPropertyRelative("_dialogEvents");
                dialogEventProperty.arraySize++;
                SerializedProperty newEvent = dialogEventProperty.GetArrayElementAtIndex(dialogEventProperty.arraySize - 1);
                newEvent.managedReferenceValue = Activator.CreateInstance(_eventTypes[_selectedIndex]);
            }
            //draw event array
            var array = property.FindPropertyRelative("_dialogEvents");

            _currentArrayHeight = 0;
            for (int i = 0; i < array.arraySize; i++) {
                SerializedProperty element = array.GetArrayElementAtIndex(i);
                //calculate rect with last height
                Rect elementRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2 + _currentArrayHeight, position.width, EditorGUI.GetPropertyHeight(element, true));
                //get name of class
                string typeName = element.managedReferenceFullTypename.Split('.').Last();
                EditorGUI.PropertyField(elementRect, element, new GUIContent($"{i:d2} : {typeName}"), true);
                
                if (GUI.Button(new Rect(elementRect.x, elementRect.y + elementRect.height, elementRect.width, EditorGUIUtility.singleLineHeight), "Remove Event")) {
                    array.DeleteArrayElementAtIndex(i);
                }
                _currentArrayHeight +=  elementRect.height + EditorGUIUtility.singleLineHeight;

            }
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
            //get property height
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int constantHeight = 2;
            //float childHeight = 0f;
            // var array = property.FindPropertyRelative("_dialogEvents");
            // for (int i = 0; i < array.arraySize; i++) {
            //     SerializedProperty element = array.GetArrayElementAtIndex(i);
            //     childHeight += EditorGUI.GetPropertyHeight(element, true); // + EditorGUIUtility.singleLineHeight;
            // }
            return EditorGUIUtility.singleLineHeight * constantHeight + 4 + _currentArrayHeight;
        }
        /*
                    //draw event array
            var array = property.FindPropertyRelative("_dialogEvents");
            float lastHeight = 0;
            
            for (int i = 0; i < array.arraySize; i++) {
                SerializedProperty element = array.GetArrayElementAtIndex(i);
                //calculate rect with last height
                Rect elementRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2 + lastHeight * i, position.width, EditorGUI.GetPropertyHeight(element, true));
                EditorGUI.PropertyField(elementRect, element, true);
                lastHeight = EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.singleLineHeight;
                if (GUI.Button(new Rect(elementRect.x, elementRect.y + elementRect.height, elementRect.width, EditorGUIUtility.singleLineHeight), "Remove Event")) {
                    array.DeleteArrayElementAtIndex(i);
                }

            }
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
            //get property height
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int constantHeight = 2;
            float childHeight = 0f;
            var array = property.FindPropertyRelative("_dialogEvents");
            for (int i = 0; i < array.arraySize; i++) {
                SerializedProperty element = array.GetArrayElementAtIndex(i);
                childHeight += EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.singleLineHeight;
            }
            return EditorGUIUtility.singleLineHeight * constantHeight + 4 + childHeight;
        }
        =================================
                    var array = property.FindPropertyRelative("_dialogEvents");
            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, position.height - EditorGUIUtility.singleLineHeight * 2), array, true);
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int constantHeight = 2;
            float childHeight = 0f;
            var array = property.FindPropertyRelative("_dialogEvents");
            childHeight = EditorGUI.GetPropertyHeight(array, true);
            return EditorGUIUtility.singleLineHeight * constantHeight + 4 + childHeight + 2f;
        }
         */
    }
}