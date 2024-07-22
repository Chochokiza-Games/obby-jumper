using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerSkin _skin;
    [SerializeField] private Rigidbody _hips;
    [SerializeField] private PlayerProfile _profile;

    [Header("Eject")]
    [SerializeField] private Transform _armature;
    [SerializeField] private float _pushForce;
    [SerializeField] private Vector3 _ejectOffset;
    [SerializeField] private float _delayBeforeTeleport;
    [SerializeField] private CinemachineFreeLook _camera;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerTeleport _teleporter;
    [SerializeField] private PlayerAnimator _playerAnimator;

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

    private void Start()
    {
        _ragdoll = new List<RagdollBone>();

        _hipsStartPosition = _hips.transform.position;
        _hipsStartRotation = _hips.transform.rotation;

        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            Debug.Log(rb.gameObject.name);
            _ragdoll.Add(new RagdollBone(rb));
            rb.isKinematic = true;
        }
    }

    public void OnGroundReached()
    {
        StartCoroutine(TeleportBackRoutine());
    }

    private IEnumerator TeleportBackRoutine() 
    {
        yield return new WaitForSeconds(_delayBeforeTeleport);
        _teleporter.TeleportStarted.AddListener(OnTeleportStarted);
        _teleporter.Teleport(Vector3.zero);
    }

    public void FuckingEject()
    {
        StartCoroutine(FuckingEjectRoutine());
    }

    private IEnumerator FuckingEjectRoutine()
    {
        EnableRagdoll();
        _camera.LookAt = transform;
        _camera.Follow = transform;
        _movement.Locked = true;
        yield return null;
        _hips.transform.parent = null;
        _hips.AddForce((Vector3.forward + Vector3.up) * (_pushForce + _profile.Power), ForceMode.Impulse);
        float timeElapsed = 0;
        while (true)
        {
            transform.position = _hips.transform.position + _ejectOffset;
            timeElapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTeleportStarted() 
    {
        _camera.LookAt = _movement.transform;
        _camera.Follow = _movement.transform;
        StopAllCoroutines();
        DisableRagdoll();
        _movement.Locked = false;
        _teleporter.TeleportStarted.RemoveListener(OnTeleportStarted);
    }

    public void EnableRagdoll()
    {
        _collider.enabled = true;
        _animator.enabled = false;
        _skin.Ragdolled = true;
        foreach (RagdollBone bone in _ragdoll)
        {
            bone.Rigidbody.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        _collider.enabled = false;
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
