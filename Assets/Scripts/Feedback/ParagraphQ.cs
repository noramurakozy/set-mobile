using TMPro;
using UnityEngine;

namespace Feedback
{
    public class ParagraphQ : MonoBehaviour
    {
        [SerializeField] private string question;
        private TMP_Text _qLabel;
        private TMP_InputField _inputField;
        private GFormQuestion _gFormQuestion;
        
        private void Awake()
        {
            _qLabel = GetComponentInChildren<TMP_Text>();
            _inputField = GetComponentInChildren<TMP_InputField>();
            _gFormQuestion = GetComponent<GFormQuestion>();
        }
        
        private void Start()
        {
            _qLabel.text = question;
            _inputField.onValueChanged.AddListener(newValue => _gFormQuestion.Answer = newValue);
            
            // Set default value
            _gFormQuestion.Answer = _inputField.text;
        }
    }
}