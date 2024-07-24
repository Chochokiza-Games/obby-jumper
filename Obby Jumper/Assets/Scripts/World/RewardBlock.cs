using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardBlock : MonoBehaviour
{
    public int HumanId
    {
        get => _humanId;
    }
    
    [SerializeField] private int _baseMoneyRewardCount;
    [SerializeField] private int _basePowerRewardCount;
    [Header("Start Of Counting")]
    [SerializeField] private TextMeshProUGUI _3dLabel;
    [SerializeField] private int _startOfCountingValue;

    private RewardTrace _trace;
    private int _id;
    private int _humanId;

    public void Init(int id, RewardTrace trace)
    {
        _id = id - 1;
        _humanId = id * _startOfCountingValue;
        _trace = trace;
        _3dLabel.text = HumanId.ToString();
    }

    public void OnPlayerEnter() 
    {
        _trace.PlayerEntered(_id, _baseMoneyRewardCount, _basePowerRewardCount);
    }
}
