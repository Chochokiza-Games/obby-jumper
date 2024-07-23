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

    private BlockSpawner _spawner;
    private int _id;
    private PlayerProfile _profile;

    public void Init(int id)
    {
        _profile = FindObjectOfType<PlayerProfile>();
        _id = id;
        _3dLabel.text = (_startOfCountingValue * _id).ToString();
    }

    public void OnPlayerEnter() 
    {
        _profile.IncreaseMoney(_baseMoneyRewardCount * _id);
        _profile.IncreasePower(_basePowerRewardCount * _id);
    }
}
