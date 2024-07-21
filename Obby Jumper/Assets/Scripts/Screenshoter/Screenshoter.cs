using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshoter : MonoBehaviour
{
    [SerializeField] private Transform _playerStand;
    [SerializeField] private Transform _petStand;
    [SerializeField] private SkinChanger _skinChanger;
    [SerializeField] private GameObject[] _pets;
    [SerializeField] private string _path;

    private void Start()
    {
        if (!System.IO.Directory.Exists(_path))
        {
            System.IO.Directory.CreateDirectory(_path);
        }

        StartCoroutine(ShotRoutine());
    }
    
    private IEnumerator ShotRoutine()
    {
        if (_skinChanger != null && _playerStand.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < _skinChanger.SkinsCount; i++)
            {
                _skinChanger.SetSkin(i);
                StartCoroutine(CaptureRoutine($"PlayerSkin_{i}"));
                yield return null;
            }
            _playerStand.gameObject.SetActive(false);
            yield return null;
        }

        yield return null;

        if (_pets.Length > 0 && _petStand.gameObject.activeInHierarchy)
        {
            foreach (GameObject pet in _pets)
            {
                pet.SetActive(true);
                string fileName = $"{pet.name}_{System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.png";
                yield return new WaitForEndOfFrame();
                zzTransparencyCapture.captureScreenshot(System.IO.Path.Combine(_path, fileName));
                pet.SetActive(false);
                yield return null;
            }
        }
        Debug.Break();
    }

    private IEnumerator CaptureRoutine(string prefix)
    {
        string fileName = $"{prefix}_{System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.png";
        yield return new WaitForEndOfFrame();
        zzTransparencyCapture.captureScreenshot(System.IO.Path.Combine(_path, fileName));
    }
}
