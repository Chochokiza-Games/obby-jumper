using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslatedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _ru;
    [SerializeField] private string _en;

    private void Start()
    {
        Debug.Log(gameObject);
        LanguageTranslator translator = FindObjectOfType<LanguageTranslator>();
        switch (translator.CurrentLangunage)
        {
            case LanguageTranslator.Languages.English:
                _text.text = _en;
                break;
            case LanguageTranslator.Languages.Russian:
                _text.text = _ru;
                break;
        }
    }
}
