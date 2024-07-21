using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AutoInteract : MonoBehaviour
{
    [SerializeField] private UnityEvent _autoInteract;
    [SerializeField] private float _timeDelta;
    [Space]
    [SerializeField] private bool _shouldInteract = false;

    private void Start()
    {
        StartCoroutine(InteractRoutine());
    }   

    private IEnumerator InteractRoutine() 
    {
        while (true)
        {
            while (_shouldInteract)
            {
                yield return new WaitForSeconds(_timeDelta);
                _autoInteract.Invoke();
            }

            yield return new WaitForSeconds(_timeDelta);
        }

    }

    public void SetEnabled(bool enabled)
    {
        _shouldInteract = enabled;
    }
/*    public void Enable()
    {
        _shouldInteract = true;
    }

    public void Disable()
    {
        _shouldInteract = false;
    }*/
}
