using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private int outputCount;

    [SerializeField]
    private string description;

    [SerializeField]
    private Image highLight;

    public Item OutPut { get => output;  }

    public int MyOutputCount { get => outputCount; set => outputCount = value; }

    public string MyDescription { get => description;  }

    public CraftingMaterial[] Materials { get => materials; }

    void Start()
    {
        GetComponent<Text>().text = output.MyTitle;
    }

   
   public void Select()
    {
        Color c = highLight.color;
        c.a = .3f;
        highLight.color = c;
    }

    public void Deselect()
    {
        Color c = highLight.color;
        c.a = 0f;
        highLight.color = c;
    }
}
