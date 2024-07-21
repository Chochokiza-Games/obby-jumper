using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageTranslator : MonoBehaviour
{
    public Languages CurrentLangunage 
    {
        get => _language;
    }

    public enum Languages
    {
        Russian,
        English,
        Turkish
    }

    [SerializeField] private Languages _language;

    public void InitLanguage(string lang)
    {
        switch (lang)
        {
            case "ru":
                _language = Languages.Russian;
                break;
            default:
                _language = Languages.English;
                break;
        }

        Debug.Log($"Language({lang}) inited {_language}");
    }


}
