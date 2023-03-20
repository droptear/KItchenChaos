using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private Transform _pointToSpawn;
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private KitchenObject _kitchenObject;

    public void Interact(Player player)
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab, _pointToSpawn);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(KitchenObjectParentToSet: this);
        } else
        {
            _kitchenObject.SetKitchenObjectParent(KitchenObjectParentToSet: player);
        }
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