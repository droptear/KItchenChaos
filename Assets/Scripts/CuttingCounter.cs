using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    //[SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(kitchenObjectSOforCheck: player.GetKitchenObject().GetKitchenObjectSO()))
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                Debug.Log("Player doesn't carring anything.");
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                Debug.Log("Player already carring something.");
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate()
    {
        if (HasKitchenObject() && HasRecipeWithInput(kitchenObjectSOforCheck: GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(inputKitchenObjectSO: GetKitchenObject().GetKitchenObjectSO());

            GetKitchenObject().DestroySelf();
            Debug.Log("Destroy.");

            KitchenObject.SpawnKitchenObject(kitchenObjectSO: outputKitchenObjectSO, kitchenObjectParent: this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSOforCheck)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == kitchenObjectSOforCheck)
            {
                return true;
            }

        }

        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }

        }

        return null;
    }
}
