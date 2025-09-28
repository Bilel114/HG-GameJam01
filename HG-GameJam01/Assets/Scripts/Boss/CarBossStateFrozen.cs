using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBossStateFrozen : CarBossStateBase
{
    public CarBossStateFrozen(CarBossStateMachine stateMachine) : base(stateMachine, CarBossStateIds.Frozen)
    {
    }

    public override void CheckSwitchState()
    {
        if (!_sm.SwitchToFrozenState)
        {
            _sm.SwitchState(CarBossStateIds.Normal);
        }
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
