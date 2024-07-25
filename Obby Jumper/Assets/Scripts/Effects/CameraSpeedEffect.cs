using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpeedEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _speedLines;

    public void Show()
    {
        _speedLines.Play();
    }

    public void Hide()
    {
        _speedLines.Stop();
    }
}
