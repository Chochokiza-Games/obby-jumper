using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private List<RewardBlock> _generatedBlocks;
    [Space]
    [Header("Generating Prefabs")]
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
            for (int i = 0; i < _generatedCount; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(currentPos, _blockSize);
                currentPos += Vector3.forward * _blockSize.z;
            }
        }

    }

    private void Start()
    {
        if (_generate)
        {
            Vector3 currentPos = transform.position;
            for (int i = 0; i < _generatedCount; i++)
            {
                _generatedBlocks.Add(Instantiate(_blockPrefab, currentPos, Quaternion.identity, transform).GetComponent<RewardBlock>());
                currentPos += Vector3.forward * _blockSize.z;
            }
            EditorApplication.isPaused = true;
            return;
        }

        for (int i = 0; i < _generatedBlocks.Count; i++)
        {
            _generatedBlocks[i].Init(i + 1);
        }
    }
}
