using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float _moveSpeed = 7.0f;
    [SerializeField] private float _rotateSpeed = 10.0f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    [SerializeField] LayerMask _countersLayerMask;

    private float _playerRadius = 0.7f;
    private float _playerHeight = 2.0f;
    private float _interactDistance = 2.0f;

    private float _moveDistance;
    private float _rotation;

    private Vector3 _lastInteractDirection;

    private bool _canMove;
    private bool _isWalking;

    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance.");
        }

        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _gameInput.OnInteractActionAlternate += GameInput_OnInteractActionAlternate;
    }


    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(player: this);
        }
    }

    private void GameInput_OnInteractActionAlternate(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate();
        }
    }

    private void Update()
    {
        CalculateMovement();
        HandleInteractions();
    }

    private void CalculateMovement()
    {
        Vector2 inputVector = _gameInput.GetInputVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        _moveDistance = _moveSpeed * Time.deltaTime;
        _rotation = _rotateSpeed * Time.deltaTime;

        _canMove = !Physics.CapsuleCast(
            point1: transform.position,
            point2: transform.position + (Vector3.up * _playerHeight),
            radius: _playerRadius,
            direction: moveDirection,
            maxDistance: _moveDistance);

        if (!_canMove)
        {
            //Can't move towards moveDirection
            //Check if can move towards only X part

            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            _canMove = CheckDirection(direction: moveDirectionX);

            if (_canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                //or if can move towards only Z part

                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                _canMove = CheckDirection(direction: moveDirectionZ);
                if (_canMove)
                {
                    moveDirection = moveDirectionZ;
                }
                else
                {
                    //Can't move in any direction.
                    Debug.Log("Can't move in any direction.");
                }
            }
        }

        if (_canMove)
        {
            transform.position += moveDirection * _moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, _rotation);

        }

        _isWalking = (moveDirection != Vector3.zero);
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetInputVectorNormalized();
        Vector3 currentInteractionDirection = new Vector3(inputVector.x, 0, inputVector.y);
        RaycastHit raycastHit;

        //Tracking of last direction when Player is not moving
        if (currentInteractionDirection != Vector3.zero)
        {
            _lastInteractDirection = currentInteractionDirection;
        }

        if (Physics.Raycast(origin: transform.position,
                            direction: _lastInteractDirection,
                            hitInfo: out raycastHit,
                            maxDistance: _interactDistance,
                            layerMask: _countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(counterToSelect: baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(counterToSelect: null);
            }
        }
        else
        {
            SetSelectedCounter(counterToSelect: null);
        }
    }

    public bool CheckDirection(Vector3 direction)
    {
        return !Physics.CapsuleCast(
            point1: transform.position,
            point2: transform.position + (Vector3.up * _playerHeight),
            radius: _playerRadius,
            direction: direction,
            maxDistance: _moveDistance);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void SetSelectedCounter(BaseCounter counterToSelect)
    {
        _selectedCounter = counterToSelect;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
            { selectedCounter = counterToSelect }
        );
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObjectToSet)
    {
        _kitchenObject = kitchenObjectToSet;

        if (_kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
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