using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] private UnityEvent _ejected;
    [SerializeField] private UnityEvent _flyEnded;

    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerSkin _skin;
    [SerializeField] private Rigidbody _hips;
    [SerializeField] private PlayerProfile _profile;

    [Header("Eject")]
    [SerializeField] private Transform _armature;
    [SerializeField] private float _minPushForceFactor;
    [SerializeField] private float _maxPushForceFactor;
    [SerializeField] private float _pushForce;
    [SerializeField] private Vector3 _ejectOffset;
    [SerializeField] private float _delayBeforeTeleport;
    [SerializeField] private CinemachineFreeLook _camera;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerTeleport _teleporter;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private Vector3 _ejectDirectionMin;
    [SerializeField] private Vector3 _ejectDirectionMax;
    [SerializeField] private float _pushToFinishForceFactor;
    [Header("Effects")]
    [SerializeField] private float _effectsGroundCheckDistance;
    [SerializeField] private UnityEvent _nearGround;
    [SerializeField] private float _fallEffectsGroundCheckDistance;
    [SerializeField] private GameObject _fallEffectPrefab;
    [SerializeField] private UnityEvent _fall; 

    private Vector3 _ejectDirection;
    private int _jumpsCount;

    private struct RagdollBone
    {
        public Rigidbody Rigidbody
        {
            get => _rigidbody;
        }

        private Rigidbody _rigidbody;
        private Vector3 _position;
        private Quaternion _quaternion;

        public RagdollBone(Rigidbody rb)
        {
            _rigidbody = rb;
            _position = rb.transform.position;
            _quaternion = rb.transform.rotation;
        }

        public void ReturnBoneBack() 
        {
            _rigidbody.transform.position = _position;
            _rigidbody.transform.rotation = _quaternion;
        }
    }

    private Vector3 _hipsStartPosition;
    private Quaternion _hipsStartRotation;
    private List<RagdollBone> _ragdoll;
    private bool _groundReached = false;

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_hips.position, _hips.position + _ejectDirectionMin * 25f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_hips.position, _hips.position + _ejectDirectionMax * 25f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_hips.position, _hips.position + _ejectDirection * 25f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up * _effectsGroundCheckDistance));
    }

    private void Start()
    {
        _ragdoll = new List<RagdollBone>();

        _hipsStartPosition = _hips.transform.position;
        _hipsStartRotation = _hips.transform.rotation;

        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb.transform == transform)
            {
                continue;
            }
            _ragdoll.Add(new RagdollBone(rb));
            rb.isKinematic = true;
        }
    }

    public void OnGroundReached()
    {
        _flyEnded.Invoke();
        _groundReached = true;
        StartCoroutine(TeleportBackRoutine());
    }

    private IEnumerator TeleportBackRoutine() 
    {
        yield return new WaitForSeconds(_delayBeforeTeleport);
        _teleporter.TeleportStarted.AddListener(OnTeleportStarted);
        _teleporter.Teleport();
    }

    public void PushToFinish()
    {
        foreach(RagdollBone bone in _ragdoll)
        {
            bone.Rigidbody.velocity = Vector3.one * 0.2f;
        }

        StartCoroutine(PushToFinishRoutine());
    }

    private IEnumerator PushToFinishRoutine()
    {
        Vector3 dir = _ejectDirection;
        dir.y = -dir.y;
        while(_groundReached == false)
        {
            _hips.AddForce(dir * _pushToFinishForceFactor, ForceMode.Impulse);
            yield return null;
        }

    }

    public void FuckingEject()
    {
        StartCoroutine(FuckingEjectRoutine());
    }

    private IEnumerator FuckingEjectRoutine()
    {
        _jumpsCount++;
        _ejectDirection = new Vector3(0, Random.Range(_ejectDirectionMin.y, _ejectDirectionMax.y), 1);
        _collider.enabled = false;
        EnableRagdoll();
        _groundReached = false;
        // _camera.LookAt = transform;
        // _camera.Follow = transform;
        _movement.Locked = true;
        yield return null;
        _hips.transform.parent = null;
        float force = _pushForce + _profile.Power;
        float forceFactor = 0;
        if (_jumpsCount % 2 == 0)
        {
            forceFactor = _maxPushForceFactor;
        }
        else 
        {
            forceFactor = Random.Range(_minPushForceFactor, _maxPushForceFactor);
        }

        force *= forceFactor;
        Debug.Log($"Current Force Factor {forceFactor}");
        
        _hips.AddForce(_ejectDirection * force, ForceMode.Impulse);
        _ejected.Invoke();
        StartCoroutine(CheckVelocityRoutine());
        bool nearGround = false;
        bool fallEffectSpawned = false;
        bool fuckup = false;
        while (true)
        {
            transform.position = _hips.transform.position + _ejectOffset;
            if (!nearGround)
            {
                if (Physics.Raycast(transform.position, -transform.up, _effectsGroundCheckDistance, ~(LayerMask.GetMask("Trigger") + LayerMask.GetMask("Coin") + LayerMask.GetMask("Ramp") + LayerMask.GetMask("PlayerSkin") + LayerMask.GetMask("PlayerSkinSkeleton"))) )
                {
                    nearGround = true;
                    _nearGround.Invoke();
                }
            }

            if (!fallEffectSpawned)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -transform.up,  out hit, _fallEffectsGroundCheckDistance, ~(LayerMask.GetMask("Trigger") + LayerMask.GetMask("Coin") + LayerMask.GetMask("Ramp") + LayerMask.GetMask("PlayerSkin") + LayerMask.GetMask("PlayerSkinSkeleton"))) )
                {
                    fallEffectSpawned = true;
                    _fall.Invoke();
                    Instantiate(_fallEffectPrefab, hit.point, Quaternion.identity);
                }
            }


            if (transform.position.y <= -50 && !fuckup)
            {
                fuckup = true;
                _nearGround.Invoke();
                OnGroundReached();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator CheckVelocityRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while ( _hips.velocity.magnitude > .1f)
        {
            yield return new WaitForFixedUpdate();
        }
        _collider.enabled = true;
    }

    private void OnTeleportStarted() 
    {
        StopAllCoroutines();
        DisableRagdoll();
        _movement.Locked = false;
        _teleporter.TeleportStarted.RemoveListener(OnTeleportStarted);
    }

    public void EnableRagdoll()
    {
        _animator.enabled = false;
        _skin.Ragdolled = true;
        foreach (RagdollBone bone in _ragdoll)
        {
            bone.Rigidbody.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        _skin.Ragdolled = false;
        _hips.transform.parent = _armature;

        foreach (RagdollBone bone in _ragdoll)
        {
            bone.Rigidbody.isKinematic = true;
            bone.ReturnBoneBack();
        }

        _animator.enabled = true;
    }
}
