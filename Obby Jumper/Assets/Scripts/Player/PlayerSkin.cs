using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Vector3 PlayerMovementDirection
    {
        set => _playerMovementDirection = value;
    }

    public bool Ragdolled 
    {
        set => _ragdolled = value;
    }

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private float _turnSpeed;

    private Vector3 _playerMovementDirection;
    private bool _ragdolled = false;

    private void FixedUpdate()
    {
        if (_ragdolled)
        {
            return;
        }

        transform.position = _playerMovement.transform.position;
        Vector3 direction = (_playerMovementDirection
            + _playerMovement.transform.position) - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _turnSpeed * Time.fixedDeltaTime);
            transform.eulerAngles = Vector3.up * transform.eulerAngles.y;
        }
    }

    public void PlayAttack()
    {
        _playerAnimator.PlayState(PlayerAnimator.States.Attack);
    }
}
