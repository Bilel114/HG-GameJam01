using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBossStateRamAttack : CarBossStateBase
{


    public CarBossStateRamAttack(CarBossStateMachine stateMachine) : base(stateMachine, CarBossStateIds.RamAttack)
    {
    }

    public override void CheckSwitchState()
    {
        
    }

    public override void EnterState()
    {
        _sm.RamAttackPoint = _sm.transform.position;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (Vector2.Distance(_sm.transform.position, _sm.RamAttackPoint) < _sm.RamAttackDistance)
        {
            _sm.transform.position += _sm.RamAttackSpeed * Time.deltaTime * Vector3.down;
        }
    }
}
