using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyingCoin : MonoBehaviour
{
    [SerializeField] private float _startForce;
    [SerializeField] private float _pauseTime;
    [SerializeField] private float _flySpeed;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Vector3 _minRandomAngles;
    [SerializeField] private Vector3 _maxRandomAngles;

    private GameObject _player;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 3);
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.eulerAngles = new Vector3(
            Random.Range(_minRandomAngles.x, _maxRandomAngles.x),
            Random.Range(_minRandomAngles.y, _maxRandomAngles.y),
            Random.Range(_minRandomAngles.z, _maxRandomAngles.z));
        _rigidBody.AddForce(transform.up * _startForce, ForceMode.Impulse);
        StartCoroutine(FlyToPlayerRoutine());
    }

    private IEnumerator FlyToPlayerRoutine()
    {
        yield return new WaitForSeconds(_pauseTime);

        _rigidBody.isKinematic = true;
        while (Vector3.Distance(transform.position, _player.transform.position) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, _player.transform.position, _flySpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
