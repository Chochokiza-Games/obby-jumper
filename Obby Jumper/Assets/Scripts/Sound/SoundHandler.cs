using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public enum Sounds
    {
        Upgrade = 0,
        Teleport,
        Reward,
        BlockDestroy,
        Egg,
        Cut
    }

    [SerializeField] private AudioListener _listener;
    [SerializeField] private AudioSource _soundtrack;
    [SerializeField] private GameObject _upgrade;
    [SerializeField] private GameObject _teleport;
    [SerializeField] private GameObject _reward;
    [SerializeField] private GameObject _blockDestroy;
    [SerializeField] private GameObject _egg;
    [SerializeField] private GameObject _cut;

    private bool _muted = false;

    private void Start()
    {
        AudioListener.volume = 1;
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = Mathf.Clamp(value, 0, 1);
    }

    private void SpawnSound(GameObject go)
    {
        Instantiate(go, transform);
    }

    public void PlaySound(int soundId)
    {
        Sounds sound = (Sounds)soundId;

        switch (sound)
        {
            case Sounds.Upgrade:
                SpawnSound(_upgrade);
                break;
            case Sounds.Teleport:
                SpawnSound(_teleport);
                break;
            case Sounds.Reward:
                SpawnSound(_reward);
                break;
            case Sounds.BlockDestroy:
                SpawnSound(_blockDestroy);
                break;
            case Sounds.Egg:
                SpawnSound(_egg);
                StartCoroutine(MuteSoundtrackCoroutine(5));
                break;
            case Sounds.Cut:
                SpawnSound(_cut);
                break;

        }
    }

    private IEnumerator MuteSoundtrackCoroutine(int t)
    {
        float startVolume = _soundtrack.volume;
        _soundtrack.volume = 0;
        yield return new WaitForSeconds(t);
        _soundtrack.volume = startVolume;
    }

    public void SwitchListenerState()
    {
        _muted = !_muted;
        if (_muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
