using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spin Wheel Reward", menuName = "SpinWheel/RewardInfo", order = 51)]
public class SpinWheelRewardInfo : ScriptableObject
{
    public Sprite Icon
    {
        get => _icon;
    }
    public string RuName
    {
        get => _ruName;
    }

    public string EnName
    {
        get => _enName;
    }
    public SpinWheel.RewardType RewardType
    {
        get => _type;
    }
    public int Amount
    {
        get => _amount;
    }

    [SerializeField] private Sprite _icon;
    [SerializeField] private string _ruName;
    [SerializeField] private string _enName;
    [SerializeField] private SpinWheel.RewardType _type;
    [SerializeField] private int _amount;
}
