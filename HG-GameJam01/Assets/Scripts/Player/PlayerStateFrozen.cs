using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFrozen : PlayerStateBase
{
    public PlayerStateFrozen(PlayerStateMachine stateMachine) : base(stateMachine, PlayerStateIds.Frozen)
    {
    }

    public override void CheckSwitchState()
    {
        if (!_sm.SwitchToFrozenState)
        {
            _sm.SwitchState(PlayerStateIds.Normal);
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
