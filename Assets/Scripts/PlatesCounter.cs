using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateAdd;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4.0f;
    private int _spawnedPlatesAmount;
    private int _spawnedPlatesAmountMax = 4;

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;

        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0.0f;

            if(_spawnedPlatesAmount < _spawnedPlatesAmountMax)
            {
                _spawnedPlatesAmount++;

                OnPlateAdd?.Invoke(sender: this, e: EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (_spawnedPlatesAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSO: _kitchenObjectSO, kitchenObjectParent: player);
                _spawnedPlatesAmount--;

                OnPlateRemoved?.Invoke(sender: this, e: EventArgs.Empty);
            }
        }
    }
}
