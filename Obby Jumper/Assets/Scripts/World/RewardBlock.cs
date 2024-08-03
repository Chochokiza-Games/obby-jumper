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
    [Header("Colors")]
    [SerializeField] private MeshRenderer _renderer;
    [Header("Start Of Counting")]
    [SerializeField] private TextMeshProUGUI _3dLabel;
    [SerializeField] private int _startOfCountingValue;

    private RewardTrace _trace;
    private int _id;
    private int _humanId;

    public void Init(int id, RewardTrace trace, int level, Color color)
    {
        
        _id = id - 1;
        _humanId = id + Mathf.RoundToInt(_startOfCountingValue * level);
        _trace = trace;
        _3dLabel.text = HumanId.ToString();
        _renderer.material = Instantiate(_renderer.material);
        _renderer.material.color = color;

    }

    public void OnPlayerEnter() 
    {
        _trace.PlayerEntered(_id, _humanId, _baseMoneyRewardCount, _basePowerRewardCount);
    }
}
