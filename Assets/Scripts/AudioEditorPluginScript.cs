using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableAudioFile))]
public class AudioEditorPluginScript : Editor
{
    private bool isPlayButtonPressed = false;

    public override void OnInspectorGUI()
    {
        ScriptableAudioFile audioData = (ScriptableAudioFile)target;
        bool originalGUIEnabled = GUI.enabled;

        // Display default inspector property fields
        DrawDefaultInspector();

        GUILayout.Space(10);

        // Custom controls for editing audio parameters
        EditorGUILayout.LabelField("Audio Controls", EditorStyles.boldLabel);
        audioData.volume = EditorGUILayout.Slider("Volume", audioData.volume, 0.0f, 1.0f);
        audioData.treble = EditorGUILayout.Slider("Treble", audioData.treble, -1.0f, 1.0f);
        audioData.bass = EditorGUILayout.Slider("Bass Boost", audioData.bass, -1.0f, 1.0f);
        audioData.pitch = EditorGUILayout.Slider("Pitch", audioData.pitch, 0.1f, 3.0f);

        GUILayout.Space(20);

         GUI.enabled = originalGUIEnabled && !isPlayButtonPressed;
        // "Play" button
        if (GUILayout.Button("Play Audio"))
        {
            isPlayButtonPressed = true;
            audioData.PlayAudio();
        }

        GUI.enabled = originalGUIEnabled;

        GUILayout.Space(5);

        if (GUILayout.Button("Stop Audio"))
        {
            isPlayButtonPressed = false;
            audioData.StopAudio();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Restart Audio"))
        {
            audioData.RestartAudio();
        }
    }     
}
