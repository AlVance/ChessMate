using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Parser : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string muestra;

    // Start is called before the first frame update
    void Start()
    {
        Parse(muestra);
    }

    public void Parse(string _textToParse)
    {
        string newText = "";

        string[] frst = _textToParse.Split("|");
        string _sz = "";
        string _pjpos = "";
        string _kingpos = "";
        List<string[]> _enemies = new List<string[]>(0);
        string[] _trrcrd = new string[0];
        string[] _cabcrd = new string[0];
        string[] _alfcrd = new string[0];
        string[] _obs = new string[0];

        for (int i = 0; i < frst.Length; i++)
        {
            if (frst[i].Contains("s")) _sz = frst[i].Replace("s", "");
            else if (frst[i].Contains("p")) _pjpos = frst[i].Replace("p", "");
            else if (frst[i].Contains("k")) _kingpos = frst[i].Replace("k", "");
            else if (frst[i].Contains("e"))
            {
                string[] _route = frst[i].Replace("e", "").Split("-");

                _enemies.Add(_route);
            }
            else if (frst[i].Contains("t")) _trrcrd = frst[i].Replace("t", "").Split("-");
            else if (frst[i].Contains("c")) _cabcrd = frst[i].Replace("c", "").Split("-");
            else if (frst[i].Contains("a")) _alfcrd = frst[i].Replace("a", "").Split("-");
            else if (frst[i].Contains("o")) _obs = frst[i].Replace("o", "").Split("-");
        }

        newText += "Size X:" + _sz[0] + " Z:" + _sz[1] + "\n";
        newText += "PjStart X:" + _pjpos[0] + " Z:" + _pjpos[1] + "\n";
        newText += "KingPos X:" + _kingpos[0] + " Z:" + _kingpos[1] + "\n";
        for (int i = 0; i < _enemies.Count; i++)
        {
            newText += "- Enemigo " + i + "\n";
            for (int e = 0; e < _enemies[i].Length; e++)
            {
                newText += "     X:" + _enemies[i][e][0] + " Z:" + _enemies[i][e][1] + "\n";
            }
        }
        newText += "Cartas Torre \n";
        for (int ti = 0; ti < _trrcrd.Length; ti++)
        {
            newText += "     X:" + _trrcrd[ti][0] + " Z:" + _trrcrd[ti][1] + "\n";
        }
        newText += "Cartas Caballo \n";
        for (int ci = 0; ci < _cabcrd.Length; ci++)
        {
            newText += "     X:" + _cabcrd[ci][0] + " Z:" + _cabcrd[ci][1] + "\n";
        }
        newText += "Cartas Alfil \n";
        for (int ci = 0; ci < _alfcrd.Length; ci++)
        {
            newText += "     X:" + _alfcrd[ci][0] + " Z:" + _alfcrd[ci][1] + "\n";
        }
        newText += "Obstaculos \n";
        for (int ci = 0; ci < _obs.Length; ci++)
        {
            newText += "     X:" + _obs[ci][0] + " Z:" + _obs[ci][1] + "\n";
        }

        SetText(newText);
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }
}
