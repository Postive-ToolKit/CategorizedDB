using Postive.CategorizedDB.Editor.CustomEditors;
using Postive.SimpleSoundAssetManager.Runtime.Data;
using UnityEditor;
using UnityEngine;

namespace Postive.SimpleSoundManager.Editor.CustomEditor.PropertyDrawers
{
    [UnityEditor.CustomEditor(typeof(SoundData))]
    public class SoundDataEditor : CategoryScriptableObjectEditor
    {
        public override void ShowOtherProperties()
        {
            SoundData soundData = (SoundData)target;
            //create horizontal line
            EditorGUILayout.LabelField("Sound Settings", EditorStyles.boldLabel);
            var rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, new UnityEngine.Color(0.5f, 0.5f, 0.5f, 1));
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_useRandomVolume"));
            if (!soundData.UseRandomVolume) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_volume"));
            }
            else {
                Vector2 volumeRange = soundData.VolumeRange;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Volume", GUILayout.Width(150));
                volumeRange.x = EditorGUILayout.FloatField(volumeRange.x, GUILayout.Width(50));
                EditorGUILayout.MinMaxSlider(ref volumeRange.x, ref volumeRange.y, 0f, 2f);
                volumeRange.y = EditorGUILayout.FloatField(volumeRange.y, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
                soundData.VolumeRange = volumeRange;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_useRandomPitch"));
            if (!soundData.UseRandomPitch) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_pitch"));
            }
            else {
                Vector2 pitchRange = soundData.PitchRange;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Pitch", GUILayout.Width(150));
                pitchRange.x = EditorGUILayout.FloatField(pitchRange.x, GUILayout.Width(50));
                EditorGUILayout.MinMaxSlider(ref pitchRange.x, ref pitchRange.y, 0f, 2f);
                pitchRange.y = EditorGUILayout.FloatField(pitchRange.y, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
                soundData.PitchRange = pitchRange;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_clips"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}