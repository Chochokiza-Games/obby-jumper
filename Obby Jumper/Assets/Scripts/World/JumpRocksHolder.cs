using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpRocksHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] _jumpRocksPrefabs;
    [SerializeField] private GameObject _checkPoint;
    private GameObject _jumpRocks;
    private int _currentPrefabId = 0;
    private int _currentLevel = 0;

    public void SpawnRocks()
    {
        if (_currentLevel >= 3)
        {  
            if (_jumpRocks != null) 
            {
                Destroy(_jumpRocks);
            }
            _jumpRocks = Instantiate(_jumpRocksPrefabs[_currentPrefabId], transform);
            _currentPrefabId = _currentPrefabId == _jumpRocksPrefabs.Length - 1 ?  0 : _currentPrefabId + 1;
            _checkPoint.SetActive(true);
        }
    }

    public void OnChangeLevel(int lvl)
    {
        _currentLevel = lvl;
        SpawnRocks();

    }
}
