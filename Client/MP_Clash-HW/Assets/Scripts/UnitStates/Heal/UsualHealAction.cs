using UnityEngine;

[CreateAssetMenu(fileName = "_UsualHealAction", menuName = "UnitState/UsualHealAction")]
public class UsualHealAction : UnitStateAttack
{
    private Unit _cachedTargetUnit;
    public override void Init()
    {
        if (TryFindTarget(out _stopAttackDistance) == false)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        _cachedTargetUnit = _target.GetComponent<Unit>();
        _time = 0f;
        _unit.transform.LookAt(_target.transform.position);
    }

    public override void Run()
    {
        if (_target == null || _cachedTargetUnit == null)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        float distanceToTarget = Vector3.Distance(_target.transform.position, _unit.transform.position);
        if (distanceToTarget > _stopAttackDistance)
        {
            _unit.SetState(UnitStateType.Chase);
            return;
        }

        _time += Time.deltaTime;
        if (_time < _delay) return;
        _time -= _delay;

        Attack();
    }

    protected override bool TryFindTarget(out float stopAttackDistance)
    {
        Vector3 unitPosition = _unit.transform.position;

        bool hasAllyDamaged = MapInfo.Instance.TryGetNearestWalkingAllyDamagedUnit(
            _unit, unitPosition, _unit.isEnemy, out Unit ally, out float distance);

        if (hasAllyDamaged && distance - ally.parameters.modelRadius <= _unit.parameters.startAttackDistance)
        {
            _target = ally.health;
            stopAttackDistance = _unit.parameters.stopAttackDistance + ally.parameters.modelRadius;
            return true;
        }

        stopAttackDistance = 0;
        return false;
    }

    protected override void Attack()
    {
        if (_target != null)
        {
            _target.Recover(_actionValue);
            Debug.Log($"Здоровье восстановлено у {_target.name}");
        }
    }
}
