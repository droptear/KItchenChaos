using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;
    public class OnCuttingProgressChangedEventArgs: EventArgs {
        public float cuttingProgressNormalized;
    }

    public event EventHandler OnCut;

    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(kitchenObjectSOforCheck: player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO: GetKitchenObject().GetKitchenObjectSO());

                    cuttingProgress = 0;

                    OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs
                    {
                        cuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
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
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO: GetKitchenObject().GetKitchenObjectSO());

            OnCut?.Invoke(this, EventArgs.Empty);

            cuttingProgress++;

            OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs
            {
                cuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(inputKitchenObjectSOforGetOutput: GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();
                Debug.Log("Destroy.");

                KitchenObject.SpawnKitchenObject(kitchenObjectSO: outputKitchenObjectSO, kitchenObjectParent: this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSOforCheck)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO: kitchenObjectSOforCheck);

        return (cuttingRecipeSO != null);
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSOforGetOutput)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO: inputKitchenObjectSOforGetOutput);
                
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        } else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
