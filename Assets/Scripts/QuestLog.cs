using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    [SerializeField]
    private Text questDescription;

    private Quest selected;

    private List<QuestScript> questScripts = new List<QuestScript>();


    private static QuestLog instance;

    public static QuestLog MyInstance 
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }

            return instance;

        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void AcceptQuest(Quest quest)
    {
        foreach (CollectObjective o in quest.MyCollectObjectives)
        {
            InventoryScripts.MyInstance.itemCountChanged += new ItemCountChanged(o.UpdateItemCount);
            
        }
        

        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>();
        quest.MyQuestScript = qs;
        qs.MyQuest = quest;
        questScripts.Add(qs);
        go.GetComponent<Text>().text = quest.MyTitle;

      

    }
    
    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.Deselect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrenAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("{0} \n<size=10>{1}</size>\nObjectives\n<size=10>{2}</size>", title, quest.MyDescription, objectives);
        }

        
    }

    public void CheckCompletion()
    {
        Debug.Log("2");
        foreach (QuestScript qs in questScripts)
        {
            qs.IsComplete();
            Debug.Log("1");
        }
    }
}
