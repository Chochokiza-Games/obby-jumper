using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using YG;

public class TrailChanger : MonoBehaviour
{
    public int TrailsCount
    {
        get => _trailsInfo.Length;
    }

    [SerializeField] private TrailInfo[] _trailsInfo;
    [SerializeField] private TrailRenderer[] _trails;
    [SerializeField] private PlayerProfile _profile;
    private Color _currentColor;

    private void Awake()
    {
        SetTrail(_profile.CurrentTrailId);
        _profile.InitTrails(_trailsInfo.Length);
    }

    public void SetTrail(int trailId)
    {
        foreach (TrailInfo trailInfo in _trailsInfo)
        {
            if (trailInfo.ItemId == trailId)
            {
                _currentColor = trailInfo.Color;
            }
        }
        _profile.CurrentTrailId = trailId;
        foreach (TrailRenderer trailRender in _trails)
        {
            trailRender.startColor = _currentColor;
            trailRender.endColor = _currentColor;
        }
    }

    public void OnSaveEvent()
    {
        YandexGame.savesData.lastSkinId = _profile.CurrentTrailId;
    }

    public void OnLoadEvent()
    {
        _profile.CurrentSkinId = YandexGame.savesData.lastSkinId;
    }


}
