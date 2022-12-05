using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Parser : MonoBehaviour
{
    public static Parser instance { get; private set; }
    public TextMeshProUGUI text;
    public string muestra;

    void Start()
    {
        instance = this;
    }

    public string GenerationCode()
    {
        string code = GenCode();
        ServerCtrl.Instance.CheckCode(code);
        while (ServerCtrl.Instance.server.response.response == "1")
        {
            code = GenCode();
            ServerCtrl.Instance.CheckCode(code);
        }

        return code;
    }

    public string GenCode()
    {
        //string st = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string st = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string c1 = st[Random.Range(0, 10)].ToString();
        string c2 = st[Random.Range(0, 10)].ToString();
        string c3 = st[Random.Range(0, 10)].ToString();
        string c4 = st[Random.Range(0, st.Length)].ToString();
        string c5 = st[Random.Range(0, st.Length)].ToString();
        string c6 = st[Random.Range(0, st.Length)].ToString();
        string result = c1 + c2 + c3 + c4 + c5 + c6;

        return result;
    }


    //size - startpos - kingpos - eneRo00 - eneRo01 - eneRo02 - eneRo03 - eneRo04 - posTrr - posCab - posAlf - posObst
    public string ParseNewMapJsonToCustom(NewMap _newSpawner)
    {
        string newText = "";
        //Size
        newText += "s" +_newSpawner.size.x + _newSpawner.size.y + "|";
        //Player Start Pos
        newText += "p" + _newSpawner.startPos.x + _newSpawner.startPos.y + "|";
        //King Pos
        newText += "k" + _newSpawner.kingPos.x + _newSpawner.kingPos.y + "|";
        //Enemy Route 01
        newText += "ea";
        for (int i = 0; i < _newSpawner.enemyRoute00.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.enemyRoute00[i].x + "" + _newSpawner.enemyRoute00[i].y;
        }
        newText += "|";
        //Enemy Route 02
        newText += "eb";
        for (int i = 0; i < _newSpawner.enemyRoute01.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.enemyRoute01[i].x + "" + _newSpawner.enemyRoute01[i].y;
        }
        newText += "|";
        //Enemy Route 03
        newText += "ec";
        for (int i = 0; i < _newSpawner.enemyRoute02.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.enemyRoute02[i].x + "" + _newSpawner.enemyRoute02[i].y;
        }
        newText += "|";
        //Enemy Route 04
        newText += "ed";
        for (int i = 0; i < _newSpawner.enemyRoute03.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.enemyRoute03[i].x + "" + _newSpawner.enemyRoute03[i].y;
        }
        newText += "|";
        //Enemy Route 05
        newText += "ee";
        for (int i = 0; i < _newSpawner.enemyRoute04.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.enemyRoute04[i].x + "" + _newSpawner.enemyRoute04[i].y;
        }
        newText += "|";
        //Cartas de torre
        newText += "t";
        for (int i = 0; i < _newSpawner.posTrr_crd.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.posTrr_crd[i].x + "" + _newSpawner.posTrr_crd[i].y;
        }
        newText += "|";
        //Cartas de Caballo
        newText += "c";
        for (int i = 0; i < _newSpawner.posCab_crd.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.posCab_crd[i].x + "" + _newSpawner.posCab_crd[i].y;
        }
        newText += "|";
        //Cartas de Alfil
        newText += "a";
        for (int i = 0; i < _newSpawner.posAlf_crd.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.posAlf_crd[i].x + "" + _newSpawner.posAlf_crd[i].y;
        }
        newText += "|";
        //Obstaculos
        newText += "o";
        for (int i = 0; i < _newSpawner.posObst.Count; i++)
        {
            if (i != 0) newText += "-";
            newText += _newSpawner.posObst[i].x + "" + _newSpawner.posObst[i].y;
        }

        return newText;
    }

    public string ParseNewMapCustomToJson(string _textToParse)
    {
        string newText = "{";

        string[] frst = _textToParse.Split("|");
        string _sz = "";
        string _pjpos = "";
        string _kingpos = "";
        string[] _enemy00 = new string[0];
        string[] _enemy01 = new string[0];
        string[] _enemy02 = new string[0];
        string[] _enemy03 = new string[0];
        string[] _enemy04 = new string[0];
        string[] _trrcrd = new string[0];
        string[] _cabcrd = new string[0];
        string[] _alfcrd = new string[0];
        string[] _obs = new string[0];

        for (int i = 0; i < frst.Length; i++)
        {
            if (frst[i].Contains("s"))  _sz = frst[i].Replace("s", "");
            else if (frst[i].Contains("p"))  _pjpos = frst[i].Replace("p", ""); 
            else if (frst[i].Contains("k"))  _kingpos = frst[i].Replace("k", ""); 
            else if (frst[i].Contains("ea"))  _enemy00 = frst[i].Replace("ea", "").Split("-"); 
            else if (frst[i].Contains("eb"))  _enemy01 = frst[i].Replace("eb", "").Split("-"); 
            else if (frst[i].Contains("ec"))  _enemy02 = frst[i].Replace("ec", "").Split("-"); 
            else if (frst[i].Contains("ed"))  _enemy03 = frst[i].Replace("ed", "").Split("-"); 
            else if (frst[i].Contains("ee"))  _enemy04 = frst[i].Replace("ee", "").Split("-"); 
            else if (frst[i].Contains("t"))  _trrcrd = frst[i].Replace("t", "").Split("-");  
            else if (frst[i].Contains("c"))  _cabcrd = frst[i].Replace("c", "").Split("-");  
            else if (frst[i].Contains("a"))  _alfcrd = frst[i].Replace("a", "").Split("-");  
            else if (frst[i].Contains("o"))  _obs = frst[i].Replace("o", "").Split("-"); 


        }
        
        newText += "\"size\":{\"x\":" + _sz[0] + ",\"y\":" + _sz[1] + "}";
        
        newText += ",\"startPos\":{\"x\":" + _pjpos[0] + ",\"y\":" + _pjpos[1] + "}";

        newText += ",\"kingPos\":{\"x\":" + _kingpos[0] + ",\"y\":" + _kingpos[1] + "}";

        newText += ",\"enemyRoute00" +"\":[";
        for (int e = 0; e < _enemy00.Length; e++)
        {
            if (_enemy00[e].Length > 0)
            {
                if (e != 0) newText += ",";
                newText += "{\"x\":" + _enemy00[e][0] + ",\"y\":" + _enemy00[e][1] + "}";
            }
        }
        newText += "]";
        newText += ",\"enemyRoute01" +"\":[";
        for (int e = 0; e < _enemy01.Length; e++)
        {
            if (_enemy01[e].Length > 0)
            {
                if (e != 0) newText += ",";
                newText += "{\"x\":" + _enemy01[e][0] + ",\"y\":" + _enemy01[e][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"enemyRoute02" +"\":[";
        for (int e = 0; e < _enemy02.Length; e++)
        {
            if (_enemy02[e].Length > 0)
            {
                if (e != 0) newText += ",";
                newText += "{\"x\":" + _enemy02[e][0] + ",\"y\":" + _enemy02[e][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"enemyRoute03" +"\":[";
        for (int e = 0; e < _enemy03.Length; e++)
        {
            if (_enemy03[e].Length > 0)
            {
                if (e != 0) newText += ",";
                newText += "{\"x\":" + _enemy03[e][0] + ",\"y\":" + _enemy03[e][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"enemyRoute04" +"\":[";
        for (int e = 0; e < _enemy04.Length; e++)
        {
            if (_enemy04[e].Length > 0)
            {
                if (e != 0) newText += ",";
                newText += "{\"x\":" + _enemy04[e][0] + ",\"y\":" + _enemy04[e][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"posTrr_crd\":[";
        for (int ti = 0; ti < _trrcrd.Length; ti++)
        {
            if (_trrcrd[ti].Length > 0)
            {
                if (ti != 0) newText += ",";
                newText += "{\"x\":" + _trrcrd[ti][0] + ",\"y\":" + _trrcrd[ti][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"posCab_crd\":[";
        for (int ci = 0; ci < _cabcrd.Length; ci++)
        {
            if (_cabcrd[ci].Length > 0)
            {
                if (ci != 0) newText += ",";
                newText += "{\"x\":" + _cabcrd[ci][0] + ",\"y\":" + _cabcrd[ci][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"posAlf_crd\":[";
        for (int ai = 0; ai < _alfcrd.Length; ai++)
        {
            if (_alfcrd[ai].Length > 0)
            {
                if (ai != 0) newText += ",";
                newText += "{\"x\":" + _alfcrd[ai][0] + ",\"y\":" + _alfcrd[ai][1] + "}";
            }
        }
        newText += "]";

        newText += ",\"posObst\":[";
        for (int oi = 0; oi < _obs.Length; oi++)
        {
            if (_obs[oi].Length > 0)
            {
                if (oi != 0) newText += ",";
                newText += "{\"x\":" + _obs[oi][0] + ",\"y\":" + _obs[oi][1] + "}";
            }
        }
        newText += "]}";

        return newText;
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }
}
