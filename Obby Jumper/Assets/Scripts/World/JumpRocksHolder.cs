using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpRocksHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] _jumpRocksPrefabs;
    [SerializeField] private Transform _jumpRocksHolder;
    [SerializeField] private GameObject _checkPoint;
    private GameObject _jumpRocks;
    private int _currentPrefabId = 0;

    public void SpawnRocks()
    {
        if (_jumpRocks != null) 
        {
            Destroy(_jumpRocks);
        }
        _jumpRocks = Instantiate(_jumpRocksPrefabs[_currentPrefabId], _jumpRocksHolder);
        _currentPrefabId = _currentPrefabId == _jumpRocksPrefabs.Length - 1 ?  0 : _currentPrefabId + 1;
        _checkPoint.SetActive(true);
    }

    public void Start()
    {
        SpawnRocks();
    }
}
