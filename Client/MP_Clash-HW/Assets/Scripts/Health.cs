using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<float> UpdateHealth;
    [field: SerializeField] public float max { get; private set; } = 10f;
    [field: SerializeField] public bool isHealthful { get; private set; } = true;
    private float _current;

    private void Start()
    {
        _current = max;
    }

    public void ApplyDamage(float value)
    {
        _current -= value;
        if (_current < 0) _current = 0;

        isHealthful = false;

        UpdateHealth?.Invoke(_current);
    }

    public void ApplyDelayDamage(float delay, float damage)
    {
        StartCoroutine(DelayDamage(delay, damage));
    }

    private IEnumerator DelayDamage(float delay, float damage)
    {
        yield return new WaitForSeconds(delay);
        ApplyDamage(damage);
    }

    public void Recover(float value)
    {
        _current = Mathf.Clamp(_current + value, 0f, max);
        if (_current == max) isHealthful = true;
        UpdateHealth?.Invoke(_current);
    }
}

interface IHealth
{
    Health health { get; }
}
