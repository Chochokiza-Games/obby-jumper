using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardBlock : MonoBehaviour
{
    [SerializeField] private int _baseMoneyRewardCount;
    [SerializeField] private int _basePowerRewardCount;
    [Header("Start Of Counting")]
    [SerializeField] private TextMeshProUGUI _3dLabel;
    [SerializeField] private int _startOfCountingValue;

    private RewardTrace _trace;
    private int _id;

    public void Init(int id, RewardTrace trace)
    {
        _id = id;
        _trace = trace;
        _3dLabel.text = (_startOfCountingValue * _id).ToString();
    }

    public void OnPlayerEnter() 
    {
        _trace.PlayerEntered(_id, _baseMoneyRewardCount, _basePowerRewardCount);
    }
}
