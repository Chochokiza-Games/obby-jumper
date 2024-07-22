using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTeleport : MonoBehaviour
{
    public UnityEvent TeleportStarted
    {
        get => _teleportStarted;
    }

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _target;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private float _delayBeforeTeleport;
    [SerializeField] private UnityEvent _teleportStarted;
    
    public void Teleport()
    {
        StopAllCoroutines();

        StartCoroutine(TeleportRoutine(_target.transform.position));
    }

    public void Teleport(Vector3 target)
    {
        StopAllCoroutines();

        StartCoroutine(TeleportRoutine(target));
    }

    private IEnumerator TeleportRoutine(Vector3 target)
    {
        _loadingScreen.Show();

        yield return new WaitForSeconds(_delayBeforeTeleport);
        _teleportStarted.Invoke();

        _playerMovement.Teleport(target);
    }

}