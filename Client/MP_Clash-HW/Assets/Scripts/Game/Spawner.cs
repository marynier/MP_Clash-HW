using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void Spawn(string id, in Vector3 spawnPoint, bool isEnemy)
    {
        Unit unitPrefab;
        Quaternion rotation = Quaternion.identity;
        if (isEnemy)
        {
            unitPrefab = CardsInGame.Instance._enemyDeck[id].unit;
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            unitPrefab = CardsInGame.Instance._playerDeck[id].unit;
        }

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);
        unit.Init(isEnemy);
        MapInfo.Instance.AddUnit(unit);
    }
}
