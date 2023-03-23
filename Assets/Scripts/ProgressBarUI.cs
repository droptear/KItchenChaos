using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnCuttingProgressChanged += CuttingCounter_OnCuttingProgressChanged;

        barImage.fillAmount = 0.0f;

        HideVisual();
    }

    private void CuttingCounter_OnCuttingProgressChanged(object sender, CuttingCounter.OnCuttingProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.cuttingProgressNormalized;

        if (barImage.fillAmount == 0 || barImage.fillAmount == 1)
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
