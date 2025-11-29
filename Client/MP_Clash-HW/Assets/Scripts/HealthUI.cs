using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image _filledImage;
    [SerializeField] private Gradient _healthGradient;

    public void UpdateHealth(float max, float current)
    {        
        float percent = current / max;
        _filledImage.fillAmount = percent;
        
        _filledImage.color = _healthGradient.Evaluate(percent);
    }
}
