using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraSpan : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cameraLook;
    [SerializeField] private CinemachineBrain _cameraBrain;
    [SerializeField] private AnimationCurve _flyCurve;
    [Header("Time")]
    [SerializeField] private float _pauseTime;
    [SerializeField] private float _timeBetweenPoints;

    public void Span(Transform p, UnityAction callback)
    {
        _cameraLook.enabled = false;
        _cameraBrain.enabled = false;
        StartCoroutine(SpanRoutine(p, callback));
    }

    public void ReturnBack()
    {
        _cameraLook.enabled = true;
        _cameraBrain.enabled = true;
    }

    private IEnumerator SpanRoutine(Transform p, UnityAction callback)
    {
        yield return new WaitForFixedUpdate();
        float timeElapsed = 0;

        Vector3 startPosition = transform.position;
        Vector3 startEulers = transform.eulerAngles;

        while(timeElapsed <= _timeBetweenPoints)
        {
            
            float t = _flyCurve.Evaluate(timeElapsed / _timeBetweenPoints);

            transform.position = Vector3.Lerp(startPosition, p.position, t);
            transform.eulerAngles = Vector3.Lerp(startEulers, p.eulerAngles, t);

            timeElapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        transform.position = p.position;
        transform.eulerAngles = p.eulerAngles;

        yield return new WaitForSeconds(_pauseTime);

        callback();
    }


}

