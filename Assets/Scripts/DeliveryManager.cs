using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private MenuSO _menuSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4.0f;
    private int waitingRecipeMax = 4;

    private void Awake()
    {
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
}