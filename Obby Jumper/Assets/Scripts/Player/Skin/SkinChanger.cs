using System.Collections.Generic;
using UnityEngine;
using YG;

public class SkinChanger : MonoBehaviour
{
    public int SkinsCount
    {
        get => _materials.Length;
    }

    [SerializeField] private Material[] _materials;
    [SerializeField] private SkinnedMeshRenderer[] _bodyParts;
    [SerializeField] private bool _isBot;
    [SerializeField] private bool _setOnStart;
    [SerializeField] private PlayerProfile _profile;

    private void Awake()
    {
        if (!_setOnStart)
        {
            return;
        }

        if (!_isBot)
        {
            SetSkin(_profile.CurrentSkinId);
            _profile.InitSkins(_materials.Length);
        }
        else
        {
            SetSkin(Random.Range(0, _materials.Length));
        }

    }

    public void SetSkin(int skinId)
    {
        if (!_isBot)
        {
            _profile.CurrentSkinId = skinId;
        }
        foreach (SkinnedMeshRenderer part in _bodyParts)
        {
            part.material = _materials[skinId];

        }
    }

    public void OnSaveEvent()
    {
        YandexGame.savesData.lastSkinId = _profile.CurrentSkinId;
    }

    public void OnLoadEvent()
    {
        _profile.CurrentSkinId = YandexGame.savesData.lastSkinId;
    }



}
