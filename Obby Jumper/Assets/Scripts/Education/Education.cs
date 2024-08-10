using System.Collections;
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
        AccessoriesStore

    }
    public bool EducationInProgress
    {
        get => _educationInProgress;
    }


    [SerializeField] private LanguageTranslator _language;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerProfile _profile;
    [SerializeField] private CameraSpan _cameraBrain;
    [SerializeField] private EducationPopup _popUp;
    [SerializeField] private EducationViewPoint[] _viewPoints;
    [SerializeField] private GameObject _teacher;
    [SerializeField] private int _moneyGift;
    [Header("———————  Windows  ———————")]
    [SerializeField] private Inventory _eggInventory;
    [SerializeField] private CustomizationStore _skinStore;
    [SerializeField] private CustomizationStore _accessoriesStore;

    private Dictionary<Type, EducationViewPoint> _viewPointsMapped;
    private string[] _currentEducationPopupText;

    private bool _educationInProgress;

    private bool _isSpinWheelShowed = false;

    private void Start()
    {
        _teacher.SetActive(false);
        
        _viewPointsMapped = new Dictionary<Type, EducationViewPoint>();
        foreach (EducationViewPoint p in _viewPoints)
        {
            _viewPointsMapped[p.Type] = p;
        }

        _educationInProgress = true;
        _movement.Lock();
        _teacher.SetActive(true);
        ShowEducation(Type.Start, () => {
            ShowEducation(Type.Ramp, () => {
                _teacher.SetActive(false);
                ShowEducation(Type.Trace, () => {
                    _cameraBrain.ReturnBack();
                    _movement.Unlock();
                    _educationInProgress = false;
                });
            });
        });
    }   
    
    private IEnumerator WaitSkinStoreEducation()
    {

        yield return new WaitForSeconds(2f);
        _skinStore.Show();
        _skinStore.HideCloseButton();
        _profile.IncreaseMoney(_moneyGift);
        yield return new WaitForSeconds(7f);
        _skinStore.Close();
        StartCoroutine(WaitAccessoriesStoreEducation());
        ShowEducation(Type.AccessoriesStore, () => {
            
        });
    }
    private IEnumerator WaitAccessoriesStoreEducation()
    {
        yield return new WaitForSeconds(2f);
        _accessoriesStore.Show();
        _accessoriesStore.HideCloseButton();
        yield return new WaitForSeconds(7f);
        _accessoriesStore.Close();
        _cameraBrain.ReturnBack();
        _movement.Unlock();
        _educationInProgress = false;
        
    }
    public void SpinwheelEducation()
    {
        if (!_isSpinWheelShowed && _educationInProgress)
        {
            ShowEducation(Type.Spinwheel, () => {
                _cameraBrain.ReturnBack();
                _movement.Unlock();
                _educationInProgress = false;
                _isSpinWheelShowed = true;
            });
        }
    }
    public void OnChangeLevel(int level)
    {
        if (level == 2)
        {
            _educationInProgress = true;
            _movement.Lock();
            _teacher.SetActive(true);
            ShowEducation(Type.SecondLevel, () => {
                ShowEducation(Type.PetOpening, () => {
                    _teacher.SetActive(false);
                    _eggInventory.Show();
                    _eggInventory.HideCloseButton();
                });
            });
        }

        if (level == 3)
        {
            _educationInProgress = true;
            _movement.Lock();
            _teacher.SetActive(true);
            ShowEducation(Type.Memes, () => {
                StartCoroutine(WaitSkinStoreEducation());
                ShowEducation(Type.SkinStore, () => {
                _teacher.SetActive(false);
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
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ТЫ НИКОГДА НЕ СМОЖЕШЬ СТАТЬ ТАКИМ, КАК Я!" : "YOU CAN NEVER BE LIKE ME!",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ХОТЯ МОЖЕШЬ ПОПРОБОВАТЬ..." : "ALTHOUGH YOU CAN TRY...",

                    };
                break;
            case Type.Ramp:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЕСЛИ ТЫ СИГМА, ТО РАЗБЕЖИСЬ ПО ЭТОЙ РАМПЕ И..." : "IF YOU ARE SIGMA, THEN ACCELERATE...",
                    };
                break;
            case Type.Trace:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПОПРОБУЙ ДОПРЫГНУТЬ ДО КОНЦА" : "TRY TO JUMP TO THE END",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "КОГДА ПРЫГАЕШЬ, УВЕЛИЧИВАЕТСЯ ТВОЯ СИЛА" : "WHEN YOU JUMP, YOUR POWER INCREASES, OINK",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПРЫГАЙ, ПОКА НЕ ДОБЕРЕШЬСЯ ДО КОНЦА" : "JUMP AS MUCH UNTIL YOU JUMP TO THE END",
                    };

                break;
            case Type.SecondLevel:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "О НЕЕЕТ... ТЫ РЕАЛЬНО ДОПРЫГНУЛ ДО КОНЦА???" : "OH NOOO...DID YOU REALLY JUMP TO THE END???",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ВОЗМОЖНО ТЫ ПРАВДА В БУДУЩЕМ СТАНЕШЬ СИГМОЙ" : "MAYBE YOU WILL ACTUALLY BECOME SIGMA IN THE FUTURE",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "НО ТРАССЫ БУДУТ ДЛИННЕЕ С КАЖДЫМ УРОВНЕМ..." : "BUT THAT'S NOT ALL, NOW THE TRAILS WILL BE LONGER",
                    };
                break;
            case Type.PetOpening:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "Я БУДУ ДАВАТЬ ТЕБЕ ЯЙЦО С РАЗНЫМИ ПИТОМЦАМИ КАЖДЫЙ УРОВЕНЬ" : "MOVE ON, AND AFTER EACH LEVEL, I WILL GIVE YOU AN EGG WITH A PET",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ОНИ БУДУТ ПОМОГАТЬ ТЕБЕ" : "DIFFERENT PETS THAT WILL HELP YOU CAN FALL FROM THE EGG",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ОТКРОЙ ЯЙЦО И ПОЛУЧИ ЕГО ВПЕРВЫЕ!" : "OPEN AND GET YOUR FIRST PET",
                    };
                break;
            case Type.Spinwheel:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ХОЧЕШЬ БЫСТРЕЕ ПОЛУЧИТЬ ДОПОЛНИТЕЛЬНОГО ПИТОМЦА ИЛИ МОНЕТКИ?" : "IF YOU WANT TO GET AN ADDITIONAL PET OR COINS FASTER...",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "КРУТИ КОЛЕСО И ПОЛУЧАЙ ПРИЗЫ!" : "SPIN THE WHEEL AND EARN PRIZES!",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "НЕ БУДУ ЗАДЕРЖИВАТЬ ТЕБЯ, ТРЕНИРУЙСЯ" : "I WILL NOT DELAY YOU, TRAIN",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "СТАНОВИСЬ СИГМОЙ!" : "BECOME SIGMA!",

                    };
                break;

            case Type.Memes:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ТЫ НЕВЕОРЯТЕН! НИ У КОГО ЕЩЕ НЕ ПОЛУЧАЛОСЬ ВЫЗВАТЬ ОСТРОВОК ТАЙН" : "YOU ARE INCREDIBLE! NO ONE HAS BEEN ABLE TO CALL THE CHAMBER OF SECRETS",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПО СЛУХАМ, НА НЕМ РАЗНЫЕ ПАСХАЛКИ И МОНЕТКИ" : "ACCORDING TO RUMOR, IT CONTAINS MEMES",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "КАЖДЫЙ УРОВЕНЬ ОСТРОВОК МЕНЯЕТСЯ, НО ЗАПАРКУРИТЬ ТУДА СТАНОВИТСЯ СЛОЖНЕЕ!" : "EACH LEVEL ROOM CHANGES, BUT PARKOURING THERE BECOMES MORE DIFFICULT",
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "В СВОБОДНОЕ ВРЕМЯ ЗАБИРАЙСЯ ТУДА, ЗАРАБАТЫВАЙ ДЕНЬГИ" : "IN YOUR FREE TIME, GO THERE AND MAKE MONEY",
                    };
                break;  
            case Type.SkinStore:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ЧТО У ТЕБЯ ЗА СКИН? ДЕРЖИ ДЕНЕГ, КУПИ СЕБЕ НОРМАЛЬНЫЙ В МАГАЗИНЕ СКИНОВ" : "WHAT IS YOUR SKIN? BUY YOURSELF A NORMAL ONE IN THE SKIN STORE TO LOOK COOL",
                    };
                break;
            case Type.AccessoriesStore:
                _currentEducationPopupText = new string[] 
                    {
                        _language.CurrentLangunage == LanguageTranslator.Languages.Russian ? "ПОКУПАЙ ШЛЯПЫ, КРЫЛЬЯ И МНОГО ЧЕГО ЕЩЕ В МАГАЗИНЕ АКСЕССУАРОВ" : "YOU CAN ALSO BUY YOURSELF HATS, WINGS AND MUCH MORE IN THE ACCESSORIES STORE",
                    };
                break;  
        }

        _cameraBrain.Span(point,
            () => {
                _popUp.Show(_currentEducationPopupText, () => {
                    callback();
                });
            }
        );
    }
}
