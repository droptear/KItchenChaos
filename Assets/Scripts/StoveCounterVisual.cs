using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    [SerializeField] private GameObject _stoveOnVisual;
    [SerializeField] private GameObject _sizzlingParticles;

    private void Start()
    {
        _stoveCounter.OnStoveStateChanged += _stoveCounter_OnStoveStateChanged;
    }

    private void _stoveCounter_OnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedEventArgs e)
    {
        bool _showVisual = (e.currentStoveState == StoveCounter.State.Frying);
        _stoveOnVisual.SetActive(_showVisual);
        _sizzlingParticles.SetActive(_showVisual);
    }
}