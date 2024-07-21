using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _target;
    [SerializeField] private LoadingScreen _loadingScreen;
    
    public void Teleport()
    {
        StopAllCoroutines();

        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        _loadingScreen.Show();

        yield return new WaitForSeconds(0.2f);

        _playerMovement.Teleport(_target.transform.position);
    }

}