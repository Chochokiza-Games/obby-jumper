using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PetMovement : MonoBehaviour
{
    public float Radius
    {
        set => _radius = value;
    }

    [Header("Angle Calculation")]
    [SerializeField] private float _minAngleDelta;
    [SerializeField] private float _maxAngleDelta;
    [SerializeField] private float _minCalcNewPointTimeDelta;
    [SerializeField] private float _maxCalcNewPointTimeDelta;
    [Header("Fly")]
    [SerializeField] private float _flySinFactor;
    [SerializeField] private float _heightOffset;
    [SerializeField] private float _speed;
    [SerializeField] private float _positionThreshold;
    [SerializeField] private GameObject _player;

    private float _randAngle;
    private Vector3 _targetPositionOffset;
    private const float _oneDegree = Mathf.PI / 180;
    private float _radius = 0;


    private void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        }
        CalcNewPoint();
    }

    private void FixedUpdate() // _physics_process
    {
        float sinHeight = Mathf.Sin(Time.realtimeSinceStartup) * _flySinFactor;
        Vector3 targetPos = _player.transform.position + _targetPositionOffset;
        targetPos.y += sinHeight + _heightOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, _speed * Time.fixedDeltaTime);
        targetPos.y = transform.position.y;
        if (Vector3.Distance(targetPos, transform.position) > _positionThreshold) 
        {
            transform.LookAt(targetPos);
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CalcNewPointRoutine());
    }

    private IEnumerator CalcNewPointRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minCalcNewPointTimeDelta, _maxCalcNewPointTimeDelta));
            CalcNewPoint();
        }
    }

    private void CalcNewPoint()
    {
        if (_randAngle > Mathf.PI * 2)
        {
            _randAngle -= Mathf.PI * 2;
        } 
        else if (_randAngle <= -Mathf.PI * 2) // wtf is this
        {
           _randAngle += Mathf.PI * 2;
        }

        _randAngle += _oneDegree * _minAngleDelta + 
            (Random.Range(0, float.MaxValue) / float.MaxValue) * _oneDegree * _maxAngleDelta; // wtf is this

        float positionZ = _radius * Mathf.Cos(_randAngle);
        float positionX = _radius * Mathf.Sin(_randAngle);

        _targetPositionOffset = new Vector3(positionX, 1, positionZ);
    }

}
