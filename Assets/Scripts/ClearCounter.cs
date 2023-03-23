using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                Debug.Log("Player doesn't carring anything.");
            }
        } else
        {
            if (player.HasKitchenObject())
            {
                Debug.Log("Player already carring something.");
            } else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}