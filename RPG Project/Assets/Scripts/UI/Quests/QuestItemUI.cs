﻿using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;

        public QuestStatus QuestStatus { get; private set; }

        public void Setup(QuestStatus status)
        {
            QuestStatus = status;
            title.text = status.Quest.Title;
            progress.text = $"{status.CompletedCount}/{status.Quest.ObjectiveCount}";
        }
    }
}
