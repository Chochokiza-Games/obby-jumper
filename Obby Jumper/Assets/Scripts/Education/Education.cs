using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Education : MonoBehaviour
{
    public enum Type
    {
        Start,
        Ramp,
        Trace,

        SecondLevel,
        PetOpening,
        Spinwheel,
        
        Memes,
        SkinStore,
        Store

    }

    [SerializeField] private LanguageTranslator _language;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private CameraSpan _cameraSpan;
    [SerializeField] private EducationPopup _popUp;
    [SerializeField] private EducationViewPoint[] _viewPoints;

    private Dictionary<Type, EducationViewPoint> _viewPointsMapped;
    private string[] _currentEducationPopupText;


    private void Start()
    {
        _viewPointsMapped = new Dictionary<Type, EducationViewPoint>();
        foreach (EducationViewPoint p in _viewPoints)
        {
            _viewPointsMapped[p.Type] = p;
        }
        
        _movement.Lock();
        ShowEducation(Type.Start, () => {
            ShowEducation(Type.Ramp, () => {
                ShowEducation(Type.Trace, () => {
                    _cameraSpan.ReturnBack();
                    _movement.Unlock();
                });
            });
        });
    }   

    public void OnChangeLevel(int level)
    {
        if (level == 2)
        {
            _movement.Lock();
            ShowEducation(Type.SecondLevel, () => {
                ShowEducation(Type.PetOpening, () => {
                    ShowEducation(Type.Spinwheel, () => {
                        _cameraSpan.ReturnBack();
                        _movement.Unlock();
                    });
                });
            });
        }
        
        if (level == 3)
        {
            _movement.Lock();
            ShowEducation(Type.Memes, () => {
                ShowEducation(Type.SkinStore, () => {
                    ShowEducation(Type.Store, () => {
                        _cameraSpan.ReturnBack();
                        _movement.Unlock();
                    });
                });
            });
        }

        
    }

    private void ShowEducation(Type type, UnityAction callback)
    {   
        _currentEducationPopupText = new string[0];
        Transform point = _viewPointsMapped[type].transform;
        switch (type)
        {
            case Type.Start:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "Я ВСЕМОГУЩИЙ СИГМА-ПРЫГУН! ХРЮ" : "I AM THE ALMIGHTY SIGMA JUMPER! OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ТЫ НИКОГДА НЕ СМОЖЕШЬ СТАТЬ ТАКИМ КАК Я! ХРЮ" : "YOU CAN NEVER BE LIKE ME! OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ХОТЯ МОЖЕШЬ ПОПРОБОВАТЬ..." : "ALTHOUGH YOU CAN TRY...",

                    };
                break;
            case Type.Ramp:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЭТО РАМПА, ХРЮ" : "IT'S A RAMP, OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЕСЛИ ТЫ СИГМА, ТО РАЗБЕЖИСЬ..." : "IF YOU ARE SIGMA, THEN ACCELERATE...",
                    };
                break;
            case Type.Trace:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПОПРОБУЙ ДОПРЫГНУТЬ ДО КОНЦА" : "TRY TO JUMP TO THE END",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "КОГДА ПРЫГАЕШЬ, УВЕЛИЧИВАЕТСЯ ТВОЯ СИЛА, ХРЮ" : "WHEN YOU JUMP, YOUR POWER INCREASES, OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПРЫГАЙ СТОЛЬКО, ПОКА НЕ ДОПРЫГНЕШЬ ДО КОНЦА, ХРЮ" : "JUMP AS MUCH UNTIL YOU JUMP TO THE END, OINK",
                    };

                break;
            case Type.SecondLevel:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "О НЕЕЕТ... ТЫ РЕАЛЬНО ДОПРЫГНУЛ ДО КОНЦА???" : "OH NOOO...DID YOU REALLY JUMP TO THE END???",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ВОЗМОЖНО ТЫ ПРАВДА В БУДУЩЕМ СТАНЕШЬ СИГМОЙ, ХРЮ" : "MAYBE YOU WILL ACTUALLY BECOME SIGMA IN THE FUTURE, OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "НО ЭТО ЕЩЕ НЕ ВСЕ, ТЕПЕРЬ ТРАССЫ БУДУТ СТАНОВИТСЯ ДЛИННЕЕ" : "BUT THAT'S NOT ALL, NOW THE TRAILS WILL BE LONGER",
                    };
                break;
            case Type.PetOpening:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПРОХОДИ ДАЛЬШЕ, А ПОСЛЕ КАЖДОГО ЛЕВЕЛА, Я БУДУ ДАВАТЬ ТЕБЕ ЯЙЦО С ПИТОМЦЕМ" : "MOVE ON, AND AFTER EACH LEVEL, I WILL GIVE YOU AN EGG WITH A PET",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ИЗ ЯЙЦА МОГУТ ВЫПАСТЬ РАЗНЫЕ ПИТОМЦЫ, КОТОРЫЕ БУДУТ ПОМОГАТЬ ТЕБЕ" : "DIFFERENT PETS THAT WILL HELP YOU CAN FALL FROM THE EGG",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ОТКРОЙ И ПОЛУЧИ СВОЕГО ПЕРВОГО ПИТОМЦА, ХРЮ" : "OPEN AND GET YOUR FIRST PET, OINK",
                    };
                break;
            case Type.Spinwheel:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЕСЛИ ТЫ ХОЧЕШЬ БЫСТРЕЕ ПОЛУЧИТЬ ДОПОЛНИТЕЛЬНОГО ПИТОМЦА ИЛИ МОНЕТОК..." : "IF YOU WANT TO GET AN ADDITIONAL PET OR COINS FASTER...",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "КРУТИ КОЛЕСО И ПОЛУЧАЙ ПРИЗЫ, ХРЮ!" : "SPIN THE WHEEL AND EARN PRIZES, OINK!",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "НЕ БУДУ ЗАДЕРЖИВАТЬ ТЕБЯ, ТРЕНИРУЙСЯ" : "I WILL NOT DELAY YOU, TRAIN",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "СТАНОВИСЬ СИГМОЙ, ХРЮ!" : "BECOME SIGMA, OINK!",

                    };
                break;

            case Type.Memes:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ТЫ НЕВЕОРЯТЕН! НИ У КОГО ЕЩЕ НЕ ПОЛУЧАЛОСЬ ВЫЗВАТЬ ТАЙНУЮ КОМНАТУ" : "YOU ARE INCREDIBLE! NO ONE HAS BEEN ABLE TO CALL THE CHAMBER OF SECRETS",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПО СЛУХАМ, В НЕЙ МЕМЫ, ХРЮ" : "ACCORDING TO RUMOR, IT CONTAINS MEMES, OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "КАЖДЫЙ ЛЕВЕЛ КОМНАТА МЕНЯЕТСЯ, НО ЗАПАРКУРИТЬ ТУДА СТАНОВИТСЯ СЛОЖНЕЕ!" : "EACH LEVEL ROOM CHANGES, BUT PARKOURING THERE BECOMES MORE DIFFICULT",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "В СВОБОДНОЕ ВРЕМЯ ЗАБЕРАЙСЯ ТУДА, ЗАРАБОТАЙ ДЕНЕГ" : "IN YOUR FREE TIME, GO THERE AND MAKE MONEY",
                    };
                break;  
            case Type.SkinStore:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЧТО У ТЕБЯ ЗА СКИН? КУПИ СЕБЕ НОРМАЛЬНЫЙ В МАГАЗИНЕ СКИНОВ, ЧТОБЫ ВЫГЛЯДЕТЬ КРУТО, ХРЮ" : "WHAT IS YOUR SKIN? BUY YOURSELF A NORMAL ONE IN THE SKIN STORE TO LOOK COOL, OINK",
                    };
                break;
            case Type.Store:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЕЩЕ МОЖЕШЬ КУПИТЬ СЕБЕ ШЛЯПЫ, КРЫЛЬЯ И МНОГО ЧЕГО ЕЩЕ В МАГАЗИНЕ АКСЕССУАРОВ" : "YOU CAN ALSO BUY YOURSELF HATS, WINGS AND MUCH MORE IN THE ACCESSORIES STORE",
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
