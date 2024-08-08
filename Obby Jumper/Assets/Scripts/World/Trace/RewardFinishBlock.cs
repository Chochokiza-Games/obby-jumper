using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardFinishBlock : RewardBlock
{
    public void Init(int id, RewardTrace trace, int level)
    {
        
        _id = id - 1;
        _humanId = id * Mathf.RoundToInt(Mathf.Pow(_startOfCountingValue, level));
        _trace = trace;
        _3dLabel.text = HumanId.ToString();
    }

    public new void OnPlayerEnter() 
    {
        _trace.PlayerEnteredFinish(_id, _humanId, _baseMoneyRewardCount, _basePowerRewardCount);
    }

}
