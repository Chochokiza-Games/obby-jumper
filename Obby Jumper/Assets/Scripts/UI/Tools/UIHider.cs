using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHider : MonoBehaviour
{
    [SerializeField] private GameObject[] _untouchableObjects;
    [SerializeField] private UnityEvent _objectsShowed;
    [SerializeField] private UnityEvent _objectsHided;

    private List<GameObject> _hidingAnywayGameobjects = new List<GameObject>();
    private Dictionary<Transform, bool> _startStates = new Dictionary<Transform, bool>();

    private void Start()
    {
        foreach (Transform c in transform)
        {
            _startStates[c] = c.gameObject.activeInHierarchy;
        }
    }

    public void ShowOther(GameObject self)
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject != self)
            {
                bool shouldShow = true;
                foreach (GameObject go in _hidingAnywayGameobjects)
                {
                    if (go == t.gameObject)
                    {
                        shouldShow = false;
                        break;
                    }
                }
                if (shouldShow)
                {
                    t.gameObject.SetActive(true);
                }
            }
        }
        _objectsShowed.Invoke();
    }

    public void ReturnToDefault()
    {
        _hidingAnywayGameobjects.Clear();
        foreach(var c in _startStates)
        {  
            if (c.Key == null)
            {
                continue;
            }
            c.Key.gameObject.SetActive(c.Value);
        }
    }

    public void HideOther(GameObject self)
    {
        foreach(Transform t in transform)
        {
            if (t.gameObject != self)
            {
                bool shouldHide = true;
                foreach (GameObject ut in _untouchableObjects)
                {
                    if (ut.gameObject == t.gameObject)
                    {
                        shouldHide = false;
                        break;
                    }
                }

                if (!shouldHide)
                {
                    continue;
                }

                if (t.gameObject.activeInHierarchy)
                {
                    t.gameObject.SetActive(false);
                }
                else
                {
                    _hidingAnywayGameobjects.Add(t.gameObject);
                }
            }
        }
        _objectsHided.Invoke();
    }
}
