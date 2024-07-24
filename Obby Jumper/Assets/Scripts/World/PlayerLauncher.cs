using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerLauncher : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private float _speedFactor;
    [SerializeField] private Transform _target;
    [SerializeField] private CameraPivot _cameraPivot;
    [SerializeField] private float _damping;

    public void OnPlayerEnter()
    {
        _movement.MoveToObject(_target, _speedFactor);
        _cameraPivot.SetDamping(_damping);
    }
}
