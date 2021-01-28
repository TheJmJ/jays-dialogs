using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

/// <summary>
/// Class for the data of a singular Dialog such as the Text and Duration.
/// Also sets if we get the data from an excel sheet by using the Row(SubtitleID) and Column(LanguageID)
/// NOTE: Might need to implement LanguageID into the settings once we have one
/// </summary>
[System.Serializable]
public class Dialog
{
    public float duration = 0;
    public bool useExcel = true;
    [TextArea]
    public string text = "";
    public int subtitleID;
}

/// <summary>
/// Keeps tract of all the Data a dialog has. Can contain multiple dialogs and durations for each
/// </summary>
[CreateAssetMenu(fileName = "new DialogData", menuName = "ScriptableObjects/DialogData")]
public class DialogData : ScriptableObject
{

    [SerializeField]
    [FMODUnity.EventRef]
    public List<string> AudioEvents = new List<string>();


    //public bool useExcel;

    [SerializeField]
    private List<Dialog> dialogs = new List<Dialog>();
    //[TextArea]
    //private List<string> dialogStrings;
    //[SerializeField]
    //private List<float> subtitleLength;

        /// <summary>
        /// DEPRECATED: Get a Subtitle/Dialog by inserting how long a dialog has gone
        /// </summary>
        /// <param name="dur"></param>
        /// <returns></returns>
    public string GetSubtitle(float dur)
    {
        //Loop through all of the strings and see where we are
        // For example, we have subtitle durations for 4s, 6s and 2s.
        // Let's say our Dialog has been going for 8 seconds now.
        // We should return the Dialog.text[1] in that case
        for (int i = 0; i < dialogs.Count; i++)
        {
            if (dur > dialogs[i].duration)
            {
                return dialogs[i].text;
            }
        }
        return null;
    }

    /// <summary>
    /// Easier way of getting the dialog. We return the whole Class
    /// And have the receiving end handle the logic
    /// <return>Returns null if no subtitle is given</return>
    /// </summary>
    /// <returns>Dialog(duration, textstring)</returns>
    public Dialog GetSubtitle(int index)
    {
        if (index > (dialogs.Count - 1)) return null;

        return dialogs[index++];
    }

    /// <summary>
    /// Get how long an AudioEvent is.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetAudioLength(int index)
    {
        int toReturn = 0;

        FMODUnity.RuntimeManager.GetEventDescription(AudioEvents[index]).getLength(out toReturn);

        return (float)toReturn / 1000;
    }
}
