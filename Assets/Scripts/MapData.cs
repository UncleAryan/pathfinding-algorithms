using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class MapData : MonoBehaviour {
    public int height;
    public int width;
    public TextAsset text;

    private List<string> getTextFromFile(TextAsset textAsset) {
        List<string> lines = new List<string>();

        if (textAsset != null) {
            string textData = textAsset.text;
            string[] delimiters = { "\r\n", "\n" };
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
        }

        return lines;
    }

    public List<string> getTextFromFile() {
        return getTextFromFile(text);
    }

    public void setDimensions(List<string> textLines) {
        height = textLines.Count;
        foreach(string line in textLines) {
            width = line.Length;
        }
    }

    public int[,] makeMap() {
        List<string> lines = new List<string>();
        lines = getTextFromFile(text);
        setDimensions(lines);
        int[,] map = new int[width, height];

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                map[x, y] = (int)char.GetNumericValue(lines[y][x]);
            }
        }

        return map;
    }
}
