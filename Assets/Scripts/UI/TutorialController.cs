using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoomBloxDemo.UI
{
    public class TutorialController : MonoBehaviour
    {
        public Action OnTutorialEnd;
        [SerializeField]
        private List<TextMeshProUGUI> _tutorialTexts;
        [SerializeField]
        private Button _previousButton;
        [SerializeField]
        private Button _nextButtonButton;
        [SerializeField]
        private TextMeshProUGUI _nextButtonText;
        private int _tutorialStep;

        private void Awake()
        {
            _tutorialStep = 0;
            foreach (var step in _tutorialTexts)
            {
                step.gameObject.SetActive(false);
            }
            _tutorialTexts[_tutorialStep].gameObject.SetActive(true);
            _previousButton.gameObject.SetActive(false);

            _previousButton.onClick.AddListener(PreviousStep);
            _nextButtonButton.onClick.AddListener(NextStep);
        }

        private void NextStep()
        {
            if (_tutorialStep < _tutorialTexts.Count - 1)
            {
                _tutorialStep++;
                SetTutorialState();
            }
            else
            {
                OnTutorialEnd?.Invoke();
            }
        }

        private void PreviousStep()
        {
            if (_tutorialStep > 0)
                _tutorialStep--;
            SetTutorialState();
        }

        private void SetTutorialState()
        {
            foreach (var step in _tutorialTexts)
            {
                step.gameObject.SetActive(false);
            }
            _tutorialTexts[_tutorialStep].gameObject.SetActive(true);

            _previousButton.gameObject.SetActive(_tutorialStep > 0);
            if (_tutorialStep >= _tutorialTexts.Count - 1)
                _nextButtonText.text = "Start";
            else
                _nextButtonText.text = "Next";
        }
    }
}
