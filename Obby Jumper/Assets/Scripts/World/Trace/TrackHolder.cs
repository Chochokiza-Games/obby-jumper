using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrackHolder : MonoBehaviour
{
    [SerializeField] private RewardTrace[] _traces;
    
    int _currentTraceId = -1;

    private void Awake()
    {
        int level = FindObjectOfType<PlayerProfile>().CurrentLevel;
        _currentTraceId = InitCurrentTraceIdFromLevel(level);
        ChangeLevel(level);
    }

    public void ChangeLevel(int level)
    {
        _currentTraceId = InitCurrentTraceIdFromLevel(level);
        ActiveCurrentTrack();
    }

    private int InitCurrentTraceIdFromLevel(int level)
    {
        int id = level - 1;
        if (id >= 5)
        {
            id = 4;
        }

        return id;
    }

    private void ActiveCurrentTrack()
    {
        foreach(RewardTrace t in _traces)
        {
            t.gameObject.SetActive(false);
        }

        _traces[_currentTraceId].gameObject.SetActive(true);
        _traces[_currentTraceId].Init();
    }
}
