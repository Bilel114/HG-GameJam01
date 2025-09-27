using UnityEngine;

public class CarBossStateNormal : CarBossStateBase
{
    Vector2 _nextDestination;
    bool _switchState;
    CarBossStateIds _nextState;

    public CarBossStateNormal(CarBossStateMachine stateMachine) : base(stateMachine, CarBossStateIds.Normal)
    {
    }

    public override void CheckSwitchState()
    {
        if (_switchState)
        {
            _sm.SwitchState(_nextState);
        }
    }

    public override void EnterState()
    {
        _nextDestination = _sm.PatrolPoints[_sm.CurrentPatrolPoint].position;
        _sm.AttackTimer = Random.Range(_sm.AttackCooldownMin, _sm.AttackCooldownMax);
        _switchState = false;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
        if (Vector2.Distance(_sm.transform.position, _nextDestination) > 0.01f)
        {
            _sm.transform.position = Vector2.MoveTowards(_sm.transform.position, _nextDestination, _sm.MoveSpeed * Time.deltaTime);
        }
        else
        {
            _sm.CurrentPatrolPoint = ++_sm.CurrentPatrolPoint % _sm.PatrolPoints.Length;
            _nextDestination = _sm.PatrolPoints[_sm.CurrentPatrolPoint].position;
        }

        if (_sm.AttackTimer > 0)
        {
            _sm.AttackTimer -= Time.deltaTime;
        }
        else
        {
            Attack();
        }
    }

    void Attack ()
    {
        _switchState = true;
        _nextState = CarBossStateIds.RamAttack;
    }
}
