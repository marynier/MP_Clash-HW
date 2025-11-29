using UnityEngine;

[RequireComponent(typeof(Health))]
public class Tower : MonoBehaviour, IHealth
{
    [field: SerializeField] public Health health { get; private set; }
    [field: SerializeField] public float radius { get; private set; } = 2f;
    public float GetDistance(in Vector3 point) => Vector3.Distance(transform.position, point) - radius;

    private void Start()
    {
        health.OnDied += Die;
    }

    private void OnDestroy()
    {
        health.OnDied -= Die;
    }

    private void Die()
    {
        MapInfo.Instance.RemoveTower(this);
        Destroy(gameObject);
    }
}
