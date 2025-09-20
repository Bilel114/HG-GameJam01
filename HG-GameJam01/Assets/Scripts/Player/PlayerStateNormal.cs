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
        _sm.MoveVector = Vector2.zero;

        if (_sm.Debug_AutoMoveVector != Vector2.zero)
        {
            _sm.MoveVector = _sm.MoveSpeed * Time.deltaTime * _sm.Debug_AutoMoveVector.normalized;
        }
        if (_sm.InputMove != Vector2.zero)
        {
            _sm.MoveVector = _sm.MoveSpeed * Time.deltaTime * _sm.InputMove.normalized; 
        }
    }
}
