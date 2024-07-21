using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableByTime : MonoBehaviour
{
    [SerializeField] private float _lifetime;

    private void Start()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}
