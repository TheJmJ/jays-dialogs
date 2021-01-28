#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(Dialog))]
public class Dialog_EditorScript : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 60f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        position.y += 10f; // Add some margin to the top
        EditorGUI.BeginProperty(position, label, property);
        string number = label.text.Split(' ')[1];
        label.text = "Line #" + number;

        // Draw Label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        bool showText = true;
        SerializedProperty conditionField = property.FindPropertyRelative("useExcel");
        showText = conditionField.boolValue;

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUI.PrefixLabel(new Rect(new Vector2(60f, position.y + 20f), new Vector2(20, 20)), GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Use Excel:"));
        EditorGUI.PropertyField(new Rect(new Vector2(125f, position.y + 20f), Vector2.one * 20), property.FindPropertyRelative("useExcel"), GUIContent.none);

        // Calculate rects
        var durationRect = new Rect(position.x, position.y + 20f, 50f, 20f);
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        GUIContent durationTitle = new GUIContent("Dur (s)");
        var durationTitleRect = durationRect;
        durationTitleRect.y = position.y;

        EditorGUI.PrefixLabel(durationTitleRect, GUIUtility.GetControlID(FocusType.Passive), durationTitle);
        EditorGUI.PropertyField(durationRect, property.FindPropertyRelative("duration"), GUIContent.none);

        if (!showText)
        {
            var textRect = new Rect(position.x + 56f, position.y, position.width - durationRect.width - 20f, 60f);
            GUIContent textTitle = new GUIContent("Subtitle Text");
            var textTitleRect = textRect;
            textTitleRect.y = position.y;
            EditorGUI.PropertyField(textRect, property.FindPropertyRelative("text"), GUIContent.none);
            EditorGUI.PrefixLabel(textTitleRect, GUIUtility.GetControlID(FocusType.Passive), textTitle);
        }
        else
        {
            var textRect = new Rect(position.x + 56f, position.y + 20f, 70f, 20f);
            GUIContent textTitle = new GUIContent("Subtitle ID");
            var textTitleRect = textRect;
            textTitleRect.y = position.y;
            EditorGUI.PropertyField(textRect, property.FindPropertyRelative("subtitleID"), GUIContent.none);
            EditorGUI.PrefixLabel(textTitleRect, GUIUtility.GetControlID(FocusType.Passive), textTitle);
        }


        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        // Would draw a line to help with seperating the 2 input fields, but with no success
        //Handles.DrawLine(new Vector2(position.x + 54f, position.y), new Vector2(position.x + 54f, position.y + position.height));
        //EditorGUI.DrawRect(new Rect(new Vector2(position.x + 54f, position.y), new Vector2(2, position.height)), Color.gray);

        EditorGUI.EndProperty();
    }
}

#endif