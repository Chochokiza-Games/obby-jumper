using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool Locked
    {
        set 
        {
            _locked = value;
            if (_locked)
            {
                _animator.PlayState(PlayerAnimator.States.Idle);
            }
        }
    }

    public Vector3 CameraForwardDirection
    {
        set => transform.forward = value;
    }

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerSkin _skin;
    [SerializeField] private PlayerAnimator _animator; 
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [Range(1f, 3f)]
    [SerializeField] private float _gravityFactor;
    [SerializeField] private Vector3 _boxcastSize;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _jumpButton;

    private Vector3 _moveDirection;
    private bool _locked;
    private bool _isGrounded = true;
    private Vector3 _playerVelocity;
    private bool _jump;

    private void Start()
    {
        bool mobile = FindObjectOfType<PlayerProfile>().RunOnMobile();
        _joystick.gameObject.SetActive(mobile);
        _jumpButton.SetActive(mobile);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + _moveDirection.normalized * 5);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + transform.up / 2, (transform.position + transform.up / 2) + (transform.up * -1) * _groundCheckDistance);
        Gizmos.DrawWireCube(transform.position + transform.up / 2, _boxcastSize);
    }

    private void Update()
    {
        if (_locked)
        {
            return;
        }

        if (!_characterController.enabled)
        {
            return;
        }

        _isGrounded = Physics.BoxCast(transform.position + transform.up / 2, _boxcastSize / 2, -transform.up, transform.rotation, _groundCheckDistance, ~(LayerMask.GetMask("Trigger") + LayerMask.GetMask("Coin") + LayerMask.GetMask("PlayerSkin")));

        //Debug.Log(_isGrounded);
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput == 0 && verticalInput == 0 && _joystick != null)
        {
            horizontalInput = _joystick.Horizontal;
            verticalInput = _joystick.Vertical;
        }
        Vector3 moveForward = transform.forward * verticalInput;
        Vector3 moveRight = transform.right * horizontalInput;

        Vector3 direction = (moveForward + moveRight).normalized * _speed;
        _characterController.Move(direction * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            _skin.PlayerMovementDirection = direction.normalized;
            _moveDirection = direction;
            if (_isGrounded)
            {
                _animator.PlayState(PlayerAnimator.States.Running);
            }
        }
        else
        {
            if (_isGrounded)
            {
                _animator.PlayState(PlayerAnimator.States.Idle);
            }
        }

        if (!_isGrounded)
        {
            _animator.PlayState(PlayerAnimator.States.Jumping);
        }

        if ((Input.GetButtonDown("Jump") || _jump) && _isGrounded)
        {
            _jump = false;
            _playerVelocity.y += Mathf.Sqrt(_jumpForce * -3.0f * (Physics.gravity.y * _gravityFactor));
        }

        _playerVelocity.y += (Physics.gravity.y * _gravityFactor) * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    public void OnJump()
    {
        _jump = true;
    }

    public void Teleport(Vector3 targetPosition)
    {
        StartCoroutine(TeleportRoutine(targetPosition));

    }

    private IEnumerator TeleportRoutine(Vector3 targetPosition)
    {
        _characterController.enabled = false;
        yield return null;
        transform.position = targetPosition;
        _characterController.enabled = true;
    }

    public void Lock()
    {
        Locked = true;
    }

    public void Unlock()
    {
        Locked = false;
    }
}