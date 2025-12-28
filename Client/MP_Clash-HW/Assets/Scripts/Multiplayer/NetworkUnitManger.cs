using UnityEngine;

public class NetworkUnitManger : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    private MultiplayerManager _multiplayerManager;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
        _multiplayerManager.OnEnemyUnitSpawned += SpawnEnemyUnit;
    }

    private void OnDestroy()
    {
        _multiplayerManager.OnEnemyUnitSpawned -= SpawnEnemyUnit;
    }

    private void SpawnEnemyUnit(string cardId, Vector3 position)
    {
        if (_spawner != null)
        {            
            Vector3 enemyPosition = new Vector3(position.x, 0, -position.z);
            _spawner.Spawn(cardId, enemyPosition, true);
        }
    }
}
