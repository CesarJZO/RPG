﻿using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI aiText;
        [SerializeField] private Button nextButton;

        [SerializeField] private GameObject aIResponse;
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private GameObject choicePrefab;

        private PlayerConversant _playerConversant;

        private void Awake()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        private void Start()
        {
            nextButton.onClick.AddListener(Next);

            UpdateUI();
        }

        private void Next()
        {
            _playerConversant.Next();

            UpdateUI();
        }

        private void UpdateUI()
        {
            aIResponse.SetActive(!_playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(_playerConversant.IsChoosing());

            if (_playerConversant.IsChoosing())
            {
                foreach (Transform item in choiceRoot)
                {
                    Destroy(item.gameObject);
                }
                foreach (DialogueNode dialogueNode in _playerConversant.GetChoices())
                {
                    GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                    choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = dialogueNode.Text;
                }
            }
            else
            {
                aiText.text = _playerConversant.GetText();
                nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }
        }
    }
}
