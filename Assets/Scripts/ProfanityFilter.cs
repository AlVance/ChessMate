using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfanityFilter : MonoBehaviour
{
    public TMP_Text textDisplay;
    public TMP_InputField inputText;

    public TextAsset textAssetBlockList;
    [SerializeField] string[] strBlockList;

    private void Start()
    {
        strBlockList = textAssetBlockList.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public void CheckInput()
    {
        textDisplay.text = ProfanityCheck(inputText.text);
        inputText.text = "";
    }

    string ProfanityCheck(string strToCheck)
    {
        for (int i = 0; i < strBlockList.Length; i++)
        {
            string profanity = strBlockList[i];
            System.Text.RegularExpressions.Regex word = new System.Text.RegularExpressions.Regex("(?i)(\\b" + profanity + "\\b");
            string temp = word.Replace(strToCheck, "****");
            strToCheck = temp;
        }
        return strToCheck;
    }
}
