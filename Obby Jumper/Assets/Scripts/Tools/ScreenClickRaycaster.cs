using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScreenClickRaycaster : MonoBehaviour
{
    public bool Locked
    {
        set => _locked = value;
    }

    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance;
    [SerializeField] private UnityEvent<GameObject> _click;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private TouchField _touchField;

    private bool _locked;
    private bool _isMobile = false;

    private void Start()
    {
        _isMobile = FindObjectOfType<PlayerProfile>().RunOnMobile();
    }

    private void Update()
    {
        if (_isMobile)
        {
            if (Input.GetMouseButtonUp(0) && !_locked && !_joystick.Drag)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _maxDistance, LayerMask.GetMask("Block")))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject != null)
                        {
                            _click.Invoke(hit.collider.gameObject);
                        }
                        //Debug.Log(hit.collider.gameObject);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) && !_locked && !_eventSystem.IsPointerOverGameObject())
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _maxDistance, LayerMask.GetMask("Block")))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject != null)
                        {
                            _click.Invoke(hit.collider.gameObject);
                        }
                        //Debug.Log(hit.collider.gameObject);
                    }
                }
            }
        }

    }
}
