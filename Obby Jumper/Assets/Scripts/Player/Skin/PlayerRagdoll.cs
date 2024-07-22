using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerSkin _skin;
    [SerializeField] private Rigidbody _hips;

    [Header("Eject")]
    [SerializeField] private Transform _armature;
    [SerializeField] private float _pushForce;
    [SerializeField] private Vector3 _ejectOffset;

    private Vector3 _hipsStartPosition;
    private Quaternion _hipsStartRotation;
    private List<Rigidbody> _ragdoll;

    private void Start()
    {
        _ragdoll = new List<Rigidbody>();

        _hipsStartPosition = _hips.transform.position;
        _hipsStartRotation = _hips.transform.rotation;

        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            Debug.Log(rb.gameObject.name);
            _ragdoll.Add(rb);
            rb.isKinematic = true;
        }
    }

    public void FuckingEject()
    {
        StartCoroutine(FuckingEjectRoutine());
    }

    private IEnumerator FuckingEjectRoutine()
    {
        EnableRagdoll();
        yield return null;
        _hips.transform.parent = null;
        _hips.AddForce((Vector3.forward + Vector3.up) * _pushForce, ForceMode.Impulse);
        float timeElapsed = 0;
        while (timeElapsed < 5f)
        {
            transform.position = _hips.transform.position + _ejectOffset;
            timeElapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        _collider.enabled = true;
        _animator.enabled = false;
        _skin.Ragdolled = true;
        //_rigidbody.isKinematic = false;
        foreach (Rigidbody rb in _ragdoll)
        {
            rb.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        _collider.enabled = false;
        _animator.enabled = true;
        _skin.Ragdolled = false;
        _hips.transform.parent = _armature;
        _hips.transform.position = _hipsStartPosition;
        _hips.transform.rotation = _hipsStartRotation;

        foreach (Rigidbody rb in _ragdoll)
        {
            rb.isKinematic = true;
        }
    }
}
