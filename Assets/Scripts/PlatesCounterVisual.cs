using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _platesCounter;
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _plateVisualPrefab;

    private List<GameObject> _plateVisualGameObjectList;
    private float _plateVisualYOffset = 0.1f;

    private void Awake()
    {
        _plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        _platesCounter.OnPlateAdd += _platesCounter_OnPlateAdd;
        _platesCounter.OnPlateRemoved += _platesCounter_OnPlateRemoved;
    }


    private void _platesCounter_OnPlateAdd(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(_plateVisualPrefab, _counterTopPoint);
        plateVisualTransform.localPosition = new Vector3(0, _plateVisualYOffset * _plateVisualGameObjectList.Count , 0);

        _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    private void _platesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = _plateVisualGameObjectList[_plateVisualGameObjectList.Count - 1];
        _plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
}