using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private MenuSO _menuSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4.0f;
    private int waitingRecipeMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer < 0.0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = _menuSO.recipeSOList[Random.Range(0, _menuSO.recipeSOList.Count)];

                Debug.Log($"{waitingRecipeSO.recipeName}!");

                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            //Has the same number of ingredients
            {
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)
                //Cycling through all indredients in each ordered recipe
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    //Cycling through all ingredients in the Plate to be delivered
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        //Ingredient matches!
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    //This Recipe ingredient was not found on the Plate
                    {
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe)
                //Player delivered the correct recipe!
                {
                    RecipeSO correctlyDeliveredRecipe = waitingRecipeSOList[i];
                    Debug.Log($"{correctlyDeliveredRecipe.recipeName} delivered to customer.");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        //Player didn't deliver the correct recipe.
        Debug.Log("Incorrect recipe.");
    }
}