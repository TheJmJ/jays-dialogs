#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RaiseEvent))]
public class RaiseEvent_Editor : Editor
{
    RaiseEvent m_script;
    List<GameEvent> events;

    void OnEnable()
    {
        m_script = (RaiseEvent)target;
    }

    public override void OnInspectorGUI()
    {
        m_script.type = (RaiseEvent.RaiseType)EditorGUILayout.EnumPopup("Event Trigger Type", m_script.type);
        m_script.triggerOnce = EditorGUILayout.Toggle("Trigger Only Once", m_script.triggerOnce);
        m_script.triggeringTag = EditorGUILayout.TextField("Triggering Tag", m_script.triggeringTag);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Event Settings", EditorStyles.boldLabel);

        m_script.randomFromList = EditorGUILayout.Toggle("Use Random from List", m_script.randomFromList);

        if (m_script.randomFromList)
        {
            events = m_script.possibleEventsToRaise;

            int newCount = Mathf.Max(0, EditorGUILayout.IntField("Array Size:", events.Count));
            while (newCount < m_script.possibleEventsToRaise.Count)
                events.RemoveAt(events.Count - 1);
            while (newCount > events.Count)
                events.Add(null);

            for (int i = 0; i < events.Count; i++)
            {
                events[i] = (GameEvent)EditorGUILayout.ObjectField("GameEvent #"+i.ToString() ,events[i], typeof(GameEvent));
            }
        }
        else
        {
            // text
            m_script.eventToRaise = (GameEvent)EditorGUILayout.ObjectField("GameEvent to Raise",m_script.eventToRaise, typeof(GameEvent));
        }
    }
}
#endif