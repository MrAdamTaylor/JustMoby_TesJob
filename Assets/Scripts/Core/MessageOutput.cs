using System.Collections;
using Localization;
using TMPro;
using UnityEngine;

namespace Core
{
    public class MessageOutput : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextMeshProUGUI _text;
        [Range(1f,2f)]
        [SerializeField] private float _duration;

        public LocalizationData Data { get; set; }
    
        private string _lastKey = MessagesKey.DROP_IN_EMPTY;
        private Coroutine _currentDisableRoutine;
        private bool _isInitialized;

        public void Localize()
        {
            _text.text = Data.GetText(_lastKey);
        }

        public void SetDefaultKey()
        {
            _lastKey = MessagesKey.DROP_IN_EMPTY;
        }

        public void OutputByKey(string key, bool isChangedKey = true)
        {
            _text.text = Data.GetText(key);
            _lastKey = isChangedKey ? key : MessagesKey.DROP_IN_EMPTY;

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        
            if (_currentDisableRoutine != null)
            {
                StopCoroutine(_currentDisableRoutine);
            }
        
            _currentDisableRoutine = StartCoroutine(DisableAfterDelay( _duration));
        }
    
        private IEnumerator DisableAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        
            gameObject.SetActive(false);
        
            _currentDisableRoutine = null;
        }

        private void OnDisable()
        {
            if (_currentDisableRoutine != null)
            {
                StopCoroutine(_currentDisableRoutine);
                _currentDisableRoutine = null;
            }
        }

        public bool KeyIsNot(string key)
        {
            return _lastKey != key;
        }
    }
}