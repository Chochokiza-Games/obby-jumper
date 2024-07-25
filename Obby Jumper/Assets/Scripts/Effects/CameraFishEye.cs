using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFishEye : MonoBehaviour
{
    [SerializeField] private PostProcessProfile _profile;
    [SerializeField] private float _intensity;
    [SerializeField] private float _changeTime;
    [SerializeField] private AnimationCurve _changeAnimationCurve;


    private LensDistortion _lensDistorsion;

    private void Start() 
    {
        _lensDistorsion = _profile.GetSetting<LensDistortion>();
    }

    private void OnDestroy() 
    {
        _lensDistorsion.intensity.Override(0);
    }

    public void EnableFishEye()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeIntensityRoutine(_intensity, true));
    }

    private IEnumerator ChangeIntensityRoutine(float value, bool shouldUseCurve = false)
    {
        float start = _lensDistorsion.intensity;
        float timeElapsed = 0;
        while (timeElapsed < _changeTime)
        {
            if (!shouldUseCurve)
            {
                float t = timeElapsed / _changeTime;
                t = t * t * (3f - 2f * t);
                _lensDistorsion.intensity.Override(Mathf.Lerp(start, value, t));
            }
            else
            {
                _lensDistorsion.intensity.Override(value * _changeAnimationCurve.Evaluate(timeElapsed / _changeTime));
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _lensDistorsion.intensity.Override(value);
    }

    public void DisableFishEye()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeIntensityRoutine(0));
    }
}
