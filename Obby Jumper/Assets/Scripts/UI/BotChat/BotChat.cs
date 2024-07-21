using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotChat : MonoBehaviour
{
    [SerializeField] private GameObject _messagePrefab;
    [SerializeField] private int _messagesCap;
    [SerializeField] private string _ruNicksNotSeparated;
    [SerializeField] private string _ruMessagesNotSeparated;
    [SerializeField] private string _enNicksNotSeparated;
    [SerializeField] private string _enMessagesNotSeparated;
    [SerializeField] private float _minPause;
    [SerializeField] private float _maxPause;
    [SerializeField] private Color[] _nickColors;
    [SerializeField] private LanguageTranslator _language;

    private List<GameObject> _messagesObjects;
    private List<string> _messagesSeparated;
    private List<string> _nicksSeparated;

    private void OnEnable()
    {
        if (_messagesSeparated == null)
        {
            return;
        }
        StartCoroutine(ChatRoutine());
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Start()
    {
        if (_language.CurrentLangunage == LanguageTranslator.Languages.Russian) 
        {
            _messagesObjects = new List<GameObject>();
            _messagesSeparated = new List<string>();
            _nicksSeparated = new List<string>();   

            foreach (string msg in _ruMessagesNotSeparated.Split(";"))
            {
                _messagesSeparated.Add(msg);
            }

            foreach (string nick in _ruNicksNotSeparated.Split(";"))
            {
                _nicksSeparated.Add(nick);
            }                
        }
        else
        {
            _messagesObjects = new List<GameObject>();
            _messagesSeparated = new List<string>();
            _nicksSeparated = new List<string>();

            foreach (string msg in _enMessagesNotSeparated.Split(";"))
            {
                _messagesSeparated.Add(msg);
            }

            foreach (string nick in _enNicksNotSeparated.Split(";"))
            {
                _nicksSeparated.Add(nick);
            }
        }
        StartCoroutine(ChatRoutine());
    }

    private IEnumerator ChatRoutine()
    {
        while(true)
        {
            BotMessage message = Instantiate(_messagePrefab, transform).GetComponent<BotMessage>();
            message.Init(_nicksSeparated[Random.Range(0, _nicksSeparated.Count)],
                _messagesSeparated[Random.Range(0, _messagesSeparated.Count)],
                _nickColors[Random.Range(0, _nickColors.Length)]);
            _messagesObjects.Add(message.gameObject);
            if (_messagesObjects.Count > _messagesCap) 
            {
                Destroy(_messagesObjects[0]);
                _messagesObjects.RemoveAt(0);
            }
            yield return new WaitForSeconds(Random.Range(_minPause, _maxPause));
        }
    }
}
