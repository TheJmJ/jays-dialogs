using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]

public class SubtitleHandler : MonoBehaviour
{
    // NOTE: Temporary languages ENUM
    enum LANGUAGES {ENGLISH= 1, FINNISH = 2}
    [Header("Needs to be moved to a global setting place at some point")]
    [Header("TEMPORARY LOCALLY SETTABLE LANGUAGEID.")]
    [SerializeField] LANGUAGES localLanguageID = LANGUAGES.ENGLISH;

    enum STYLES {DEFAULT, TYPEWRITER }
    [SerializeField] STYLES revealStyle;

    // Typewriter variables
    int totalCharacters;
    [Range(0, 1)]
    [SerializeField]
    float typewriterCharDurationMultiplier;

    DialogData data;
    TextMeshProUGUI subtitleTMPRO;
    /// <summary>
    /// The Excel sheet in .CVS format
    /// </summary>
    [SerializeField]CSVParser csv;

    [SerializeField][ReadOnly] float currentDur = 0f;
    [SerializeField][ReadOnly] float curMaxDur = 0f;
    [SerializeField][ReadOnly] int currentSubIndex = 0;
    [SerializeField][ReadOnly] public bool dialogInProgress = false;

    bool inPause;

    private void Awake()
    {
        if(csv == null)
        {
            if (!(csv = GetComponent<CSVParser>()))
            {
                Debug.LogError("Couldn't find CVSParser from " + gameObject.name + ". Disabling");
                enabled = false;
            }
        }

        if(subtitleTMPRO == null)
        {
            if(!(subtitleTMPRO = GetComponent<TextMeshProUGUI>()))
            {
                Debug.LogError("Couldn't find TextmeshproUGUI from " + gameObject.name + ". Disabling");
                enabled = false;
            }
        }
    }

    /// <summary>
    /// Returns the string for the subtitle specified in a Dialog
    /// </summary>
    /// <param name="dialog">A single Dialog from DialogData</param>
    /// <returns>The full dialog string for the Subtitle</returns>
    string GetSubtitle(Dialog dialog)
    {
        string stringToReturn = "";
        inPause = false;

        totalCharacters = 0;
        // This be the wrong way around
        if (dialog.useExcel)
        {
            // Set up the row(SubtitleID) and column(LanguageID)
            int subtitleID = dialog.subtitleID;
            int languageID = PlayerPrefs.GetInt("language",1);

            stringToReturn = csv.GetText(subtitleID, languageID);
        }
        else
        {
            if (dialog.text != "PAUSE")
            {
                stringToReturn = dialog.text;
            }
            else
            {
                stringToReturn = subtitleTMPRO.text;
                inPause = true;
            }
        }

        if (revealStyle == STYLES.TYPEWRITER && !inPause)
        {
            subtitleTMPRO.maxVisibleCharacters = 0;
            totalCharacters = stringToReturn.Length;
        }
        else if(revealStyle == STYLES.TYPEWRITER && inPause)
        {
            totalCharacters = stringToReturn.Length;
        }

        return stringToReturn;
    }

    /// <summary>
    /// Starts to show the subtitle in screen
    /// </summary>
    public void StartSubtitle(DialogData _data)
    {
        dialogInProgress = true;
        //subtitleTMPRO.text = "";
        //subtitleTMPRO.enabled = false;
        currentSubIndex = 0;
        currentDur = 0;

        data = _data;

        curMaxDur = data.GetSubtitle(currentSubIndex).duration;
        subtitleTMPRO.text = GetSubtitle(data.GetSubtitle(currentSubIndex));
        
        if (revealStyle == STYLES.TYPEWRITER)
        {
            subtitleTMPRO.maxVisibleCharacters = 0;
            totalCharacters = subtitleTMPRO.text.Length;
        }

       // subtitleTMPRO.enabled = true;
    }

    // Use this to bypass the revealStyle
    public void StartSubtitleAndOverrideWritestyle(DialogData _data)
    {
        dialogInProgress = true;
        //subtitleTMPRO.text = "";
        //subtitleTMPRO.enabled = false;
        currentSubIndex = 0;
        currentDur = 0;

        data = _data;

        curMaxDur = data.GetSubtitle(currentSubIndex).duration;

        subtitleTMPRO.text = GetSubtitle(data.GetSubtitle(currentSubIndex));

        subtitleTMPRO.maxVisibleCharacters = subtitleTMPRO.text.Length;

        // subtitleTMPRO.enabled = true;
    }


    /// <summary>
    /// Disables the subtitle, and sets data to Null
    /// </summary>
    public void EndSubtitle()
    {
        dialogInProgress = false;
        data = null;
        subtitleTMPRO.text = "";
        //subtitleTMPRO.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogInProgress)
        {
            currentDur += Time.deltaTime;

            if (revealStyle == STYLES.TYPEWRITER && !inPause)
            {
                if(curMaxDur < 0)
                {
                    subtitleTMPRO.maxVisibleCharacters = totalCharacters;
                }
                else
                {
                    float tempCurDur = currentDur;
                    // We need to calculate how many characters we're going to show
                    // Let's say we have 10 characters, and the total duration is 8
                    // We want to reveal a letter every .8 seconds (8 / 10)
                    // If we'd be on 5th character, we'd know that the subtitle has gone for 4 seconds. (.8 * 5)
                    int visibleCharacters = Mathf.Clamp((int)(tempCurDur / ((curMaxDur / totalCharacters) * typewriterCharDurationMultiplier)), 0, totalCharacters);
                    //visibleCharacters = Mathf.Clamp(visibleCharacters, 0, totalCharacters);

                    //Debug.Log(currentDur + " / " + curMaxDur + " / " + totalCharacters + " = " + visibleCharacters);
                    subtitleTMPRO.maxVisibleCharacters = visibleCharacters;
                }
            }

            if (currentDur > curMaxDur && curMaxDur > -1)
            {
                currentDur = 0f;
                Dialog tempDialog = data.GetSubtitle(++currentSubIndex);
                if(tempDialog != null)
                {
                    curMaxDur = tempDialog.duration;
                    subtitleTMPRO.text = GetSubtitle(tempDialog);
                }
                else
                {
                    EndSubtitle();
                }
            }
        }
    }
}
