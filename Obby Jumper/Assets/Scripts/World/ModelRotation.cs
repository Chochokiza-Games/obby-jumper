using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    [SerializeField] private bool _needRotate = false;
    [SerializeField] private Transform _model;
    [SerializeField] private float _modelRotateSpeed;
    [SerializeField] private Vector3 _rotationAxis;

    private void Update()
    {


        if (_needRotate == true)
        {
            _model.Rotate(_rotationAxis * _modelRotateSpeed * Time.deltaTime);
        }
    }
}
