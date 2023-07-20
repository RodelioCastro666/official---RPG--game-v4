using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private QuestGiver questGiver;

    [SerializeField]
    private Transform questArea;

    [SerializeField]
    private GameObject questPrefab;

    public void ShowQuest(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (Quest quest in questGiver.MyQuest)
        {
            GameObject go = Instantiate(questPrefab, questArea);
            go.GetComponent<Text>().text = quest.MyTitle;
        }
    }

    public override void Open(Npc npc)
    {
        ShowQuest((npc as QuestGiver));
        base.Open(npc);
    }
}
