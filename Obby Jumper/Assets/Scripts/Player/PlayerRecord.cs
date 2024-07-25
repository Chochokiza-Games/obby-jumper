using UnityEngine;
using YG;

public class PlayerRecord : MonoBehaviour
{
    public float AnimationDuration
    {
        get => _infoLabel.AnimationDuration;
    }

    [SerializeField] private PlayerRecordInfo _infoLabel;
    [SerializeField] private ProgressBar _bar;

    private int _current;


    public void OnLoadEvent()
    {
        _current = YandexGame.savesData.playerRecord;
    }

    public void OnSaveEvent()
    {
        YandexGame.savesData.playerRecord = _current;
    }

    public bool TryUpdateRecord(int record)
    {
        if (_current < record)
        {
            _current = record;
            _infoLabel.Show(record);
            return true;
        }

        return false;
    }
}
