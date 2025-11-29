using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float max { get; private set; } = 10f;
    [SerializeField] private HealthUI _ui;
    private float _current;
    public event Action OnDied;

    private void Start()
    {
        _current = max;
        _ui.UpdateHealth(max, _current);
    }

    public void ApplyDamage(float value)
    {
        _current -= value;
        if (_current <= 0)
        {
            _current = 0;
            OnDied?.Invoke();
        }                

        UpdateUI();

        Debug.Log($"Объект {name}: было {_current + value}, стало {_current}");
    }

    private void UpdateUI()
    {
        _ui.UpdateHealth(max, _current);
    }
}

interface IHealth
{
    Health health { get;}
}
