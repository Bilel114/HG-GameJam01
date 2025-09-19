using UnityEngine;

public class PlayerStateNormal : PlayerStateBase
{
    public PlayerStateNormal(PlayerStateMachine stateMachine) : base(stateMachine, PlayerStateIds.Normal)
    {
    }

    public override void CheckSwitchState()
    {
        
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        _sm.MoveVector = _sm.MoveSpeed * Time.deltaTime * _sm.InputMove.normalized;
    }
}
