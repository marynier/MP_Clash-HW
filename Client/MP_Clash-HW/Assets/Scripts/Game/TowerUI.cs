using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private Tower _tower;
    [SerializeField] private GameObject _healthbar;
    [SerializeField] private Image _fillHealthImage;
    [SerializeField] private Gradient _healthGradient;
    private float _maxHealth;

    private void Start()
    {
        _healthbar.SetActive(false);
        _maxHealth = _tower.health.max;

        _tower.health.UpdateHealth += UpdateHealth;
    }

    private void OnDestroy()
    {
        _tower.health.UpdateHealth -= UpdateHealth;
    }

    private void UpdateHealth(float currentValue)
    {
        _healthbar.SetActive(true);
        float percent = currentValue / _maxHealth;
        _fillHealthImage.fillAmount = percent;
        _fillHealthImage.color = _healthGradient.Evaluate(percent);
    }
}
