using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private Transform _pointToSpawn;
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public void Interact()
    {
        Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab, _pointToSpawn);
        kitchenObjectTransform.localPosition = Vector3.zero;

        Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
    }
}