using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image _filledImage;

    public void UpdateHealth(float max, float current)
    {
        Color color;
        float percent = current / max;

        if (percent > 0.7f) color = Color.green;
        else if (percent > 0.4f) color = Color.yellow;
        else color = Color.red;

        _filledImage.fillAmount = percent;
        _filledImage.color = color;
    }
}
