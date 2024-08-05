using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraSpan : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private CinemachineFreeLook _camera;
    [SerializeField] private AnimationCurve _flyCurve;
    [Header("Time")]
    [SerializeField] private float _pauseTime;
    [SerializeField] private float _timeBetweenPoints;

    private Transform _lookAtCamera;

    private void OnDrawGizmos() 
    {   
        if (_points.Length != 0)
        {
            foreach (Transform p in _points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(p.position, p.position + p.forward * 5f);
            }
        }
    }

    private void Start() 
    {
        Span();
    }

    public void Span()
    {
        _lookAtCamera = _camera.LookAt;
        _camera.LookAt = null;
        _camera.Follow = null;
        StartCoroutine(SpanRoutine());
    }

    private IEnumerator SpanRoutine()
    {
        foreach(Transform p in _points)
        {
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
        }

        _camera.LookAt = _lookAtCamera;
        _camera.Follow = _lookAtCamera;
    }


}

