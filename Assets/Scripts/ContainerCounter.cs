using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject; 

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO: _kitchenObjectSO, kitchenObjectParent: player);

            //Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab);
            //kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(KitchenObjectParentToSet: player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        } else
        {
            Debug.Log($"{player} already has something.");
        }
    }
}