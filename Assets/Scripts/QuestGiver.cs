using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Npc
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionSilver, exclamation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    public Quest[] MyQuests { get => quests;  }


    private void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }
                else if (!QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    break;
                }
                else if (!quest.IsComplete &&   QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                }
            }
        }
    }
}
