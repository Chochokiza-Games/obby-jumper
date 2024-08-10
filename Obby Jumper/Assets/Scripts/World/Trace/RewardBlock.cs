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
    
    [SerializeField] protected int _baseMoneyRewardCount;
    [SerializeField] protected int _basePowerRewardCount;
    [Header("Colors")]
    [SerializeField] protected MeshRenderer _renderer;
    [Header("Start Of Counting")]
    [SerializeField] protected TextMeshProUGUI _3dLabel;
    [SerializeField] protected int _startOfCountingValue;

    protected RewardTrace _trace;
    protected int _id;
    protected int _humanId;

    public void Init(int id, RewardTrace trace, int blocksCount, int level, Color color)
    {
        
        _id = id - 1;
        _humanId = (id * 100) + (blocksCount * 100 * (level - 1)); //id * Mathf.RoundToInt(Mathf.Pow(_startOfCountingValue, level));
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
