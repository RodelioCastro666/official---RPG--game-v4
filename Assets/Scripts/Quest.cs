using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{

    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

  

    public QuestScript MyQuestScript { get; set; }

    public string MyTitle { get => title; set => title = value; }


    public string MyDescription { get => description; set => description = value; }

    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    public bool IsComplete
    {
        get
        {
            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                    Debug.Log("4");
                }
            }

            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {

    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;


    [SerializeField]
    private string type;

    private int currentAmount;


    public int MyAmount { get => amount; }

    public int MyCurrenAmount { get => currentAmount; set => currentAmount = value; }

    public string MyType { get => type; }

    public bool IsComplete
    {
        get
        {
            return MyCurrenAmount >= MyAmount;
            Debug.Log("3");

        }
    }
}

[System.Serializable]
public class CollectObjective : Objective 
{ 
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrenAmount = InventoryScripts.MyInstance.GetItemCount(item.MyTitle);
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }
}


