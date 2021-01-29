# jays-dialogs
A simple Dialog system with triggerable GameEvents and support for CVS. Designed to print out a string of text with chosen language option, and play an Audio file to go with it from FMOD.

## Requirements
*FMOD For Unity
*TextMeshPro

## Set-up/Usage
See DialogExampleScene for an already setup scene

You need to create two ScriptableObjects: **GameEvent** and **DialogData**.
You can create them from Asset Menu by navigating to: Create > ScriptableObjects 

There is no further setup needed for **GameEvent** upon creating it.
DialogData can have as many Dialogs as you want, and each Dialog can take in a raw text or a row from the .csv file, and a duration for the string to be shown on screen. The AudioEvents get played in a row.

To use a .csv file, you need to save it in UTF-8, with commas `,` as seperators. To have a comma in the string, wrap the string with quotes: `"like this"`

Prepare a subtitle text in a Canvas with **SubtitleHandler** and **CSVParser** scripts attached. Don't forget to add the .csv file to the CSV File parameter in the CSVParser scripts.

After this, add a **GameEventListener** to any gameobject, and add the GameEvent you want to react to the GameEvent parameter. After that, setup the Response accordingly. To start a dialog, you want to add a reference to the **SubtitleHanlder** script and call `SubtitleHandler.StartSubtitle(DialogData)` from it, with the DialogData you want to print out.

You can raise a GameEvent with your own script by `GameEventObject.Raise()` like we have done in RaiseEvent.cs script's `OnTriggerEnter()`.
