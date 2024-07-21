using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdaptiveTriggers : MonoBehaviour
{
    public UnityEvent PlayerEnter
    {
        get => _playerEnter;
    }
    public UnityEvent PlayerStay
    {
        get => _playerStay;
    }
    public UnityEvent PlayerExit
    {
        get => _playerExit;
    }

    [SerializeField] private UnityEvent _playerEnter;
    [SerializeField] private UnityEvent _playerStay;
    [SerializeField] private UnityEvent _playerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerExit.Invoke();
        }
    }
}
