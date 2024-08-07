using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EducationPopup : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private float _textShowDuration;
    [SerializeField] private float _textPauseDuration; 

    private bool _animationPlays = false;

    // private IEnumerator Start()
    // {
    //     yield return new WaitForSeconds(1f);
    //     Debug.Log("nachalo");
    //     Show(new string[] 
    //         { 
    //             "ТЫ СУЧКА НИКОГДА НЕ СМОЖЕШЬ СТАТЬ СИГМОЙ", 
    //             "соси хуй чурка cbo cbo cbo cbo cbo cbo",
    //             "aaaa",
    //             "..." 
    //         }, () => { Debug.Log("vse"); });
    // }

    public void Show(string[] textQueue, UnityAction callback)
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowRoutine(textQueue, callback));
    }

    private IEnumerator ShowRoutine(string[] textQueue, UnityAction callback)
    {
        foreach (String text in textQueue)
        {
            _animationPlays = true;
            float time = _textShowDuration / text.Length;
            string t = "";
            for (int i = 0; i < text.Length && _animationPlays; i++)
            {
                t += text[i];
                _label.text = t;
                yield return new WaitForSeconds(time);
            }
            _label.text = text;
            _animationPlays = false;
            yield return new WaitForSeconds(_textPauseDuration);
        }

        callback();
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (_animationPlays)
        {
            _animationPlays = false;
        }
    }
}
