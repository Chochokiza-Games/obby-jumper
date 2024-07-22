using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBlock : MonoBehaviour
{
    [SerializeField] private int _baseMoneyRewardCount;
    [SerializeField] private int _basePowerRewardCount;

    private PlayerProfile _profile;

    private void Start()
    {
        _profile = FindObjectOfType<PlayerProfile>();
    }

    public void OnPlayerEnter() 
    {
        _profile.IncreaseMoney(_baseMoneyRewardCount);
        _profile.IncreasePower(_basePowerRewardCount);
    }
}
