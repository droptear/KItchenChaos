using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStoveStateChangedEventArgs> OnStoveStateChanged;
    public class OnStoveStateChangedEventArgs : EventArgs
    {
        public State currentStoveState;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] FryingRecipeSO[] _fryingRecipeSOArray;

    private State _currentState;
    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;

    public enum State
    {
        Idle,
        Frying
    }

    private void Start()
    {
        _currentState = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_currentState)
            {
                case State.Idle:
                    break;

                case State.Frying:
                    
                     if (HasRecipeWithInput(kitchenObjectSOforCheck: GetKitchenObject().GetKitchenObjectSO()))
                     {
                        _fryingTimer += Time.deltaTime;

                        OnProgressChanged?.Invoke(sender: this, e: new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = _fryingTimer/_fryingRecipeSO.fryingTimerMax
                        });

                        if (_fryingTimer >= _fryingRecipeSO.fryingTimerMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                            _fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO: _fryingRecipeSO.output);
                            _fryingTimer = 0.0f;
                        }
                     } else
                     {
                            _currentState = State.Idle;

                            OnStoveStateChanged?.Invoke(sender: this, e: new OnStoveStateChangedEventArgs
                            {
                                currentStoveState = _currentState
                            }); 
                     }
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(kitchenObjectSOforCheck: player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(KitchenObjectParentToSet: this);
                    _fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO: GetKitchenObject().GetKitchenObjectSO());

                    _fryingTimer = 0.0f;
                    _currentState = State.Frying;

                    OnStoveStateChanged?.Invoke(sender: this, e: new OnStoveStateChangedEventArgs
                    {
                        currentStoveState = _currentState
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
                _currentState = State.Idle;

                OnStoveStateChanged?.Invoke(sender: this, e: new OnStoveStateChangedEventArgs
                {
                    currentStoveState = _currentState
                });

                OnProgressChanged?.Invoke(sender: this, e: new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0.0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSOforCheck)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO: kitchenObjectSOforCheck);

        return (fryingRecipeSO != null);
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSOforGetOutput)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO: inputKitchenObjectSOforGetOutput);

        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
}