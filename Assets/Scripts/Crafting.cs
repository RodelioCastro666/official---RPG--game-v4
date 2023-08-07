using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    private Recipe selectedRecipe;

    public void ShowDescription(Recipe recipe)
    {
        if(selectedRecipe != null)
        {
            selectedRecipe.Deselect();
        }

        this.selectedRecipe = recipe;

        this.selectedRecipe.Select();
    }
}
