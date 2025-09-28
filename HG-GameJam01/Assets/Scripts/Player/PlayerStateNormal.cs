using UnityEngine;

public class PlayerStateNormal : PlayerStateBase
{
    public PlayerStateNormal(PlayerStateMachine stateMachine) : base(stateMachine, PlayerStateIds.Normal)
    {
    }

    public override void CheckSwitchState()
    {
        if (_sm.IsInputDodgePressed && _sm.DodgeCooldownTimer <= 0 && _sm.InputMove != Vector2.zero)
        {
            _sm.IsInputDodgePressed = false;
            _sm.SwitchState(PlayerStateIds.Dodge);
        }
        else if (_sm.IsInputShieldHeld && _sm.ShieldCooldownTimer < 0)
        {
            _sm.SwitchState(PlayerStateIds.Shield);
        }
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        _sm.MoveVector = Vector2.zero;
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

        _sm.PlayerCharacter.Animator.SetInteger(AnimatorHash.Player_MoveDirectionX, (int)_sm.InputMove.x);
        _sm.PlayerCharacter.Animator.SetInteger(AnimatorHash.Player_MoveDirectionY, (int)_sm.InputMove.y);
    }
}
