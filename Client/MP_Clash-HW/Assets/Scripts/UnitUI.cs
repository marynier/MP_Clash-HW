using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private GameObject _healthbar;
    [SerializeField] private Image _fillHealthImage;
    [SerializeField] private Gradient _healthGradient;
    private float _maxHealth;

    private void Start()
    {
        _healthbar.SetActive(false);
        _maxHealth = _unit.health.max;

        _unit.health.UpdateHealth += UpdateHealth;
    }

    private void OnDestroy()
    {
        _unit.health.UpdateHealth -= UpdateHealth;
    }

    private void UpdateHealth(float currentValue)
    {
        _healthbar.SetActive(true);
        float percent = currentValue / _maxHealth;
        _fillHealthImage.fillAmount = percent;
        _fillHealthImage.color = _healthGradient.Evaluate(percent);
    }
}
