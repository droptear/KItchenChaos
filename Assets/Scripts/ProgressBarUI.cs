using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _hasProgressGameObject;
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;

    private void Start()
    {
        _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress == null)
        {
            Debug.LogError($"Game Object {_hasProgressGameObject} doesn't implement IHasProgress interface.");
        }

        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        _barImage.fillAmount = 0.0f;

        HideVisual();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        _barImage.fillAmount = e.progressNormalized;

        if (_barImage.fillAmount == 0 || _barImage.fillAmount == 1)
        {
            HideVisual();
        } else
        {
            ShowVisual();
        }
    }

    private void ShowVisual()
    {
        gameObject.SetActive(true);
    }

    private void HideVisual()
    {
        gameObject.SetActive(false);
    }
}
