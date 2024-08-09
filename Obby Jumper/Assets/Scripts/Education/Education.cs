using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Education : MonoBehaviour
{
    public enum Type
    {
        Spinwheel,
        Pohui,
        Ahui,
        NextLevel
    }
    public bool EducationInProgress
    {
        get => _educationInProgress;
    }


    [SerializeField] private LanguageTranslator _language;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private CameraSpan _cameraSpan;
    [SerializeField] private EducationPopup _popUp;
    [SerializeField] private EducationViewPoint[] _viewPoints;

    private Dictionary<Type, EducationViewPoint> _viewPointsMapped;
    private string[] _currentEducationPopupText;
    private bool _educationInProgress;

    private void Start()
    {
        _viewPointsMapped = new Dictionary<Type, EducationViewPoint>();
        foreach (EducationViewPoint p in _viewPoints)
        {
            _viewPointsMapped[p.Type] = p;
        }
        _educationInProgress = true;
        _movement.Lock();
        ShowEducation(Type.Pohui, () => {
            ShowEducation(Type.Ahui, () => {
                _cameraSpan.ReturnBack();
                _movement.Unlock();
                _educationInProgress = false;
            });
        });
    }   

    public void OnChangeLevel(int level)
    {
        if (level == 2)
        {
            _educationInProgress = true;
            _movement.Lock();
            ShowEducation(Type.NextLevel, () => {
                _cameraSpan.ReturnBack();
                _movement.Unlock();
                _educationInProgress = false;
            });
        }
    }

    private void ShowEducation(Type type, UnityAction callback)
    {   
        _currentEducationPopupText = new string[0];
        Transform point = _viewPointsMapped[type].transform;
        switch (type)
        {
            case Type.NextLevel:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "нееееееееееееееет...." : "fuck american burgers",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ты прошел первый уровень" : "fuck trump fuck biden 9/11",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "но это еще не конец" : "fuck you ngiga",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "все еще впредеи" : "i heard you like kids nigga little kids boys and girls nigga",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "соси хуй" : "fuck you ngiga",
                    };
                break;
            case Type.Pohui:
                _currentEducationPopupText = new string[] 
                    {
                        "это трасса",
                        "иди пизданись на нее",
                        "4mo..."
                    };
                break;
            case Type.Ahui:
                _currentEducationPopupText = new string[] 
                    {
                        "это ты",
                        "pedik",
                        "4mo..."
                    };
                break;
        }

        _cameraSpan.Span(point,
            () => {
                _popUp.Show(_currentEducationPopupText, () => {
                    callback();
                });
            }
        );
    }
}
