using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteBin : BaseCounter
{
    private KitchenObject _objectToWaste;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            _objectToWaste = player.GetKitchenObject();
            
            _objectToWaste.SetKitchenObjectParent(KitchenObjectParentToSet: this);
            
            StartCoroutine(SelfDestruct(timeToDestruct: 0.8f));
        }
        else
        {
            Debug.Log("Player doesn't carring anything.");
        }
    }

    IEnumerator SelfDestruct(float timeToDestruct)
    {
        yield return new WaitForSeconds(timeToDestruct);

        _objectToWaste.DestroySelf();   
    }
}