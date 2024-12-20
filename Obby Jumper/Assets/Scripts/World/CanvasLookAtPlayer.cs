using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasLooker : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private Transform _playerMovement;



    private void Update()
    {
        _label.transform.LookAt(_playerMovement.position);
        _label.transform.Rotate(Vector3.up * -180);
    }
}
