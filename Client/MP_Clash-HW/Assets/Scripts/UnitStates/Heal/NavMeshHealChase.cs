using UnityEngine;

[CreateAssetMenu(fileName = "_NavMeshHealChase", menuName = "UnitState/NavMeshHealChase")]
public class NavMeshHealChase : UnitStateNavMeshChase
{
    private bool _targetIsDamaged;
    public override void Init()
    {
        base.Init();

        if (_targetUnit != null) _targetIsDamaged = !_targetUnit.health.isHealthful;

    }

    public override void Run()
    {
        if (_targetUnit == null)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        float distanceToTarget = Vector3.Distance(_unit.transform.position, _targetUnit.transform.position);

        if (distanceToTarget > _unit.parameters.stopChaseDistance)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        bool targetIsDamagedNow = !_targetUnit.health.isHealthful;
        if (targetIsDamagedNow && distanceToTarget <= _startAttackDistance)
        {
            Debug.Log($"HealChase → Attack: {targetIsDamagedNow}, dist={distanceToTarget}");
            _unit.SetState(UnitStateType.Attack);
        }

        else
        {           
            if (!targetIsDamagedNow)
            {
                FindTargetUnit(out _targetUnit);
            }
            _agent.SetDestination(_targetUnit.transform.position);
        }
    }

    protected override void FindTargetUnit(out Unit targetUnit)
    {
        bool hasDamagedAlly = MapInfo.Instance.TryGetNearestWalkingAllyDamagedUnit(
            _unit, _unit.transform.position, _unit.isEnemy, out targetUnit, out float distance);

        if (hasDamagedAlly && targetUnit != _unit)
        {
            _targetIsDamaged = true;
            return;
        }

        if (hasDamagedAlly == false)
        {
            bool hasAlly = MapInfo.Instance.TryGetNearestWalkingUnit(
                        _unit.transform.position, _unit.isEnemy, out targetUnit, out distance, _unit);

            _targetIsDamaged = false;
        }

        //Debug.Log($"HealChase: target = {targetUnit?.name}, damaged = {_targetIsDamaged}");
    }
}
