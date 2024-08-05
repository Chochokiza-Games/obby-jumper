using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nick;
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(string nick, string text, Color nickColor)
    {
        _nick.text = nick;
        _text.text = text;
        _nick.color = nickColor;
        _text.color = nickColor;
    }
}
