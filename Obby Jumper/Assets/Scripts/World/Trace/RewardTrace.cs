
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RewardTrace : MonoBehaviour
{
    [SerializeField] private List<RewardBlock> _generatedBlocks;
    [SerializeField] private PlayerProfile _profile;
    [SerializeField] private PlayerRagdoll _ragdoll;
    [SerializeField] private ProgressBar _bar;
    [SerializeField] private PlayerRecord _record;
    [Space]
    [Header("Colors")]
    [SerializeField] private PaletteChanger _paletteChanger;
    [Space]
    [Header("Generating Prefabs")]
    [SerializeField] private Transform _voidVelocityTrigger;
    [SerializeField] private bool _generate;
    [SerializeField] private int _generatedCount;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject _finishBlockPrefab;
    [SerializeField] private Vector3 _blockSize;

    private void OnDrawGizmos() 
    {
        if (_generate)
        {
            Vector3 currentPos = transform.position;
            for (int i = 0; i < _generatedCount - 1; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(currentPos, _blockSize);
                currentPos += Vector3.forward * _blockSize.z;
            }
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(currentPos, _blockSize);
        }

    }

    private void Start()
    {
        if (_generate)
        {
            Vector3 currentPos = transform.position;
            for (int i = 0; i < _generatedCount - 1; i++)
            {
                _generatedBlocks.Add(Instantiate(_blockPrefab, currentPos, Quaternion.identity, transform).GetComponent<RewardBlock>());
                currentPos += Vector3.forward * _blockSize.z;
            }
            _generatedBlocks.Add(Instantiate(_finishBlockPrefab, currentPos, Quaternion.identity, transform).GetComponent<RewardBlock>());

            BoxCollider left = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            left.size = new Vector3(1, 1000, _blockSize.z * _generatedCount);
            left.center = new Vector3(-(_blockSize.x / 2), 500, (_blockSize.z * _generatedCount) / 2 - (_blockSize.z / 2));
            BoxCollider right = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            right.size = new Vector3(1, 1000, _blockSize.z * _generatedCount);
            right.center = new Vector3((_blockSize.x / 2), 500, (_blockSize.z * _generatedCount) / 2 - (_blockSize.z / 2));
            BoxCollider forward = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            forward.size = new Vector3(_blockSize.x, 1000, 3);
            forward.center = new Vector3(0, 500, (_blockSize.z * _generatedCount) - (_blockSize.z / 2) + 1f);
            BoxCollider tr = _voidVelocityTrigger.GetComponent<BoxCollider>();
            tr.size = new Vector3(_blockSize.x, 1000, 20);
            tr.center = new Vector3(0, 500, (_blockSize.z * _generatedCount) - (_blockSize.z / 2));
            _generate = false;
            gameObject.name = "BlockTrack";
            EditorApplication.isPaused = true;
            return;
        }

        _profile = FindObjectOfType<PlayerProfile>();
        _ragdoll = FindObjectOfType<PlayerRagdoll>();
        _bar = FindAnyObjectByType<ProgressBar>();
        _record = FindObjectOfType<PlayerRecord>();
        _paletteChanger = FindObjectOfType<PaletteChanger>();
        Init();
 
    }

    public void Init()
    {
        for (int i = 0; i < _generatedBlocks.Count; i++)
        {   
            if (i == _generatedBlocks.Count - 1)
            {
                (_generatedBlocks[i] as RewardFinishBlock).Init(i + 1, this, _profile.CurrentLevel);
            }
            else
            {
                _generatedBlocks[i].Init(i + 1, this, _profile.CurrentLevel, _paletteChanger.CurrentPalette.Colors[Random.Range(3, _paletteChanger.CurrentPalette.Colors.Length)]);
            }
        }
    }

    public void PlayerEntered(int id, int humanId, float baseMoney, float basePower)
    {
        float money = ((id + 1) * baseMoney) * _profile.CurrentLevel;
        float power = ((id + 1) * basePower);
        _profile.IncreaseMoney(Mathf.RoundToInt(money));
        _profile.IncreasePower(Mathf.RoundToInt(power));
        if (_record.TryUpdateRecord(_generatedBlocks[id].HumanId))
        {
            _bar.RefreshBar((float)((float)(id) / (float)(_generatedBlocks.Count - 1)));
        }
        _ragdoll.OnGroundReached();
    } 

    public void PlayerEnteredFinish(int id, int humanId, float baseMoney, float basePower)
    {
        PlayerEntered(id, humanId, baseMoney, basePower);
        _profile.ShouldChangeLevel();
    }

    public void Alert()
    {
        Debug.LogError("RewardTrace Plz Change event of trigger");
    }
}