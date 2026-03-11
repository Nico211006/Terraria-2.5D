using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CraftingIngredient
{
    public ItemData item;
    public int amount = 1;
}

[System.Serializable]
public class CraftingRecipe
{
    public string recipeName;
    public CraftingStationType stationType = CraftingStationType.Inventory;
    public List<CraftingIngredient> ingredients = new List<CraftingIngredient>();
    public ItemData resultItem;
    public int resultAmount = 1;
}

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [Header("References")]
    public InventoryManager inventoryManager;
    public TextMeshProUGUI recipeText;
    public TextMeshProUGUI stationText;

    [Header("Recipes")]
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    [Header("Current Station")]
    public CraftingStationType currentStation = CraftingStationType.Inventory;

    private int currentRecipeIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (inventoryManager == null)
            inventoryManager = InventoryManager.Instance;

        SetStation(CraftingStationType.Inventory);
    }

    public void TestButtonClick()
    {
        Debug.Log("BUTTON WURDE GEKLICKT");
    }

    public void SetStation(CraftingStationType newStation)
    {
        currentStation = newStation;
        currentRecipeIndex = 0;
        UpdateRecipeText();
        UpdateStationText();
    }

    public void SetInventoryStation()
    {
        SetStation(CraftingStationType.Inventory);
    }

    public void SetWorkbenchStation()
    {
        SetStation(CraftingStationType.Workbench);
    }

    public void SetFurnaceStation()
    {
        SetStation(CraftingStationType.Furnace);
    }

    public void NextRecipe()
    {
        List<int> validRecipes = GetValidRecipeIndicesForCurrentStation();

        if (validRecipes.Count == 0)
        {
            UpdateRecipeText();
            return;
        }

        currentRecipeIndex++;

        if (currentRecipeIndex >= validRecipes.Count)
            currentRecipeIndex = 0;

        UpdateRecipeText();
    }

    public void PreviousRecipe()
    {
        List<int> validRecipes = GetValidRecipeIndicesForCurrentStation();

        if (validRecipes.Count == 0)
        {
            UpdateRecipeText();
            return;
        }

        currentRecipeIndex--;

        if (currentRecipeIndex < 0)
            currentRecipeIndex = validRecipes.Count - 1;

        UpdateRecipeText();
    }

    public void CraftCurrentRecipe()
    {
        List<int> validRecipes = GetValidRecipeIndicesForCurrentStation();

        if (validRecipes.Count == 0)
            return;

        if (currentRecipeIndex < 0 || currentRecipeIndex >= validRecipes.Count)
            return;

        CraftingRecipe recipe = recipes[validRecipes[currentRecipeIndex]];

        if (!CanCraft(recipe))
            return;

        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            CraftingIngredient ingredient = recipe.ingredients[i];

            if (ingredient.item != null && ingredient.amount > 0)
            {
                inventoryManager.RemoveItem(ingredient.item, ingredient.amount);
            }
        }

        inventoryManager.AddItem(recipe.resultItem, recipe.resultAmount);
        inventoryManager.RefreshUI();

        if (HotbarSelector.Instance != null)
            HotbarSelector.Instance.RefreshHotbarVisual();

        UpdateRecipeText();
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        if (recipe == null || inventoryManager == null)
            return false;

        if (recipe.resultItem == null)
            return false;

        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            CraftingIngredient ingredient = recipe.ingredients[i];

            if (ingredient.item == null || ingredient.amount <= 0)
                return false;

            if (!inventoryManager.HasItem(ingredient.item, ingredient.amount))
                return false;
        }

        return true;
    }

    public void UpdateRecipeText()
    {
        if (recipeText == null)
            return;

        List<int> validRecipes = GetValidRecipeIndicesForCurrentStation();

        if (validRecipes.Count == 0)
        {
            recipeText.text = "Keine Rezepte verfügbar";
            return;
        }

        if (currentRecipeIndex < 0 || currentRecipeIndex >= validRecipes.Count)
            currentRecipeIndex = 0;

        CraftingRecipe recipe = recipes[validRecipes[currentRecipeIndex]];

        string text = "Rezept: " + recipe.recipeName + "\n";

        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            CraftingIngredient ingredient = recipe.ingredients[i];

            if (ingredient.item == null)
                continue;

            int currentAmount = inventoryManager != null ? inventoryManager.GetItemCount(ingredient.item) : 0;

            text += ingredient.amount + "x " + ingredient.item.itemName + " (" + currentAmount + ")";

            if (i < recipe.ingredients.Count - 1)
                text += " + ";
        }

        text += "\n-> " + recipe.resultAmount + "x " + recipe.resultItem.itemName;

        recipeText.text = text;
    }

    public void UpdateStationText()
    {
        if (stationText == null)
            return;

        stationText.text = "Station\n" + currentStation.ToString();
    }

    private List<int> GetValidRecipeIndicesForCurrentStation()
    {
        List<int> result = new List<int>();

        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i] != null && recipes[i].stationType == currentStation)
            {
                result.Add(i);
            }
        }

        return result;
    }
}