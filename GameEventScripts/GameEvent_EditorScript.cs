#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEvent_EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameEvent _target = (GameEvent)target;

        GUI.enabled = EditorApplication.isPlaying;
        if(GUILayout.Button("Raise Event"))
        {
           _target.Raise();
        }
    }
}
#endif