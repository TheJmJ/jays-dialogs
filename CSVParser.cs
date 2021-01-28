using UnityEngine;
using System;
using System.Collections.Generic;

public class CSVParser : MonoBehaviour
{
    public TextAsset csvFile;
    string[,] textGrid;
    bool silentMode;

    int expectedColumns = 0;

    /// <summary>
    /// Initialize the subtitleGrid where we get our subtitles from
    /// </summary>
    /// 
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        textGrid = getCSVGrid(csvFile.text);
    }

    /// <summary>
    /// Turns a TextAsset csvFile into a 2D array of strings
    /// </summary>
    /// <param name="csvText">the CSV file as text</param>
    string[,] getCSVGrid(string csvText)
    {
        // Splits the rows into lines by using newLines
        string[] lines = csvText.Split("\n"[0]);

        // Find the column amount
        int totalColumns = 0;
        expectedColumns = SplitCVSLine(lines[0]).Length;
        for (int i = 0; i < lines.Length; i++)
        {
            //int quoteCounter = 0;
            //foreach (char c in lines[i])
            //{
            //    if(c == '"')
            //    {
            //        quoteCounter++;
            //    }
            //}

            //string[] row = lines[i].Split(',');

            string[] row = SplitCVSLine(lines[i]);
            totalColumns = Mathf.Max(totalColumns, row.Length);
        }

        // Create the 2D grid array we return
        string[,] outputGrid = new string[totalColumns + 1, lines.Length + 1];
        for(int y = 0; y < lines.Length; y++)
        {
            //string[] row = lines[y].Split(',');
            string[] row = SplitCVSLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];
            }
        }

        return outputGrid;
    }

    string[] SplitCVSLine(string line)
    {
        // We return an array of strings we split here
        List<string> returnString = new List<string>();
        bool inQuote = false;
        int startPos = 0;
        int quotePos = 0;
        // Process every character
        for (int i = 0; i < line.Length; i++)
        {
            // Process till we find a comma first or a quotation mark
            // Found a comma and we're not processing quotation marks right now
            if (line[i] == ',' && !inQuote || line[i] == '\r')
            {
                // Add the substring from startPos to currentPos of the line
                string text = line.Substring(startPos, i - startPos);
                returnString.Add(text);
                //Debug.Log("On position (" + startPos + "-" + (i - startPos) + ") = "  + text);
                startPos = i + 1;
                //Debug.Log("Found substring:" + text);
                continue;
            }
            
            if(line[i] == '"')
            {
                inQuote = !inQuote;
                quotePos = i;
            }
        }

        // If we're still in a quote after all of that
        if(inQuote)
        {
            Debug.LogError("Odd count of Quote marks in line: " + line + "\n Last quote was found in position: " + quotePos);
        }

        for(int i = 0; i < returnString.Count; i++)
        {
            //Debug.Log("OLD: "+returnString[i]);
            returnString[i] = returnString[i].Replace("\"", "");
            //Debug.Log("NEW: " + returnString[i]);
        }

        //Debug.Log(line);
        //Debug.Log("Found " + returnString.Count + " dialog lines on the line above");

        if(returnString.Count > 3 && !silentMode)
        {
            Debug.LogWarning("CSVParser> More columns than on first line, which had " + expectedColumns + "!\n" + line);
        }

        return returnString.ToArray();
    }

    // We use this to get the text from the file
    // We Initialize the thing if the textGrid is null
    // NOTE: Might need to think about if we need to change the system
    public string GetText(int row, int column)
    {
        try
        {
            if (textGrid == null) Init();
            return textGrid[column, row - 1];
        } catch(IndexOutOfRangeException e)
        {
            Debug.LogError("CSVParser failed on getting text from (" + column + ", " + row + ")");
            Debug.LogError(e.Message);
            return null;
        }
    }
}
