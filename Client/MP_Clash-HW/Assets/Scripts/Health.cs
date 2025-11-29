using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float max { get; private set; } = 10f;
    [SerializeField] private HealthUI _ui;
    private float _current;

    private void Start()
    {
        _current = max;
        _ui.UpdateHealth(max, _current);
    }

    public void ApplyDamage(float value)
    {
        _current -= value;
        if (_current < 0) _current = 0;

        UpdateHP();

        Debug.Log($"Объект {name}: было {_current + value}, стало {_current}");
    }

    private void UpdateHP()
    {
        _ui.UpdateHealth(max, _current);
    }
}

interface IHealth
{
    Health health { get;}
}
