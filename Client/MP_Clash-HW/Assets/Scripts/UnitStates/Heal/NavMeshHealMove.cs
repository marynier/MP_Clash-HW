using UnityEngine;

[CreateAssetMenu(fileName = "_NavMeshHealMove", menuName = "UnitState/NavMeshHealMove")]
public class NavMeshHealMove : UnitStateNavMeshMove
{
    public override void Init()
    {
        Vector3 unitPosition = _unit.transform.position;
        _nearestTower = MapInfo.Instance.GetNearestTower(in unitPosition, _unit.isEnemy);
        _agent.SetDestination(_nearestTower.transform.position);
    }

    protected override bool TryFindTarget(out UnitStateType changeType)
    {
        bool hasDamagedAlly = MapInfo.Instance.TryGetNearestWalkingAllyDamagedUnit(
        _unit, _unit.transform.position, _unit.isEnemy, out Unit ally, out float distance);

        if (hasDamagedAlly)
        {            
            changeType = UnitStateType.Chase;
            return true;
        }

        bool hasAlly = MapInfo.Instance.TryGetNearestWalkingUnit(
        _unit.transform.position, _unit.isEnemy, out ally, out distance, _unit);

        if (hasAlly)
        {
            changeType = UnitStateType.Chase;
            return true;
        }

        changeType = UnitStateType.None;
        return false;
    } 
}
