using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public Transform _pointToSpawn;
    private KitchenObject _kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.Log("BaseClass.Interact();");
    }

    public virtual void InteractAlternate()
    {
        Debug.Log("BaseClass.InteractAlternate();");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _pointToSpawn;
    }

    public void SetKitchenObject(KitchenObject kitchenObjectToSet)
    {
        _kitchenObject = kitchenObjectToSet;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return (_kitchenObject != null);
    }
}