using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheelRewardPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private Image _icon;

    public void InitFromInfo(SpinWheelRewardInfo info)
    {
        switch (FindObjectOfType<LanguageTranslator>().CurrentLangunage) 
        {
            case LanguageTranslator.Languages.Russian:
                _label.text = info.RuName;
                break;
            case LanguageTranslator.Languages.English:
                _label.text = info.EnName;
                break;
        }
        _icon.sprite = info.Icon;
    }
}
