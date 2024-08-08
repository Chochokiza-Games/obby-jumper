using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducationViewPoint : MonoBehaviour
{
    public Education.Type Type
    {
        get => _type;
    }

    [SerializeField] private Education.Type _type;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position, Vector3.one / 2f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 5f));
    }
}
