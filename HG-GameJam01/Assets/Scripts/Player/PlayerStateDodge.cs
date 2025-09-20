using UnityEngine;

public class PlayerStateDodge : PlayerStateBase
{
    private float _dodgeTimer;

    public PlayerStateDodge(PlayerStateMachine stateMachine) : base(stateMachine, PlayerStateIds.Dodge)
    {
    }

    public override void CheckSwitchState()
    {
        if (_dodgeTimer < 0)
        {
            _sm.SwitchState(PlayerStateIds.Normal);
        }
    }

    public override void EnterState()
    {
        _dodgeTimer = _sm.DodgeDuration;
        _sm.DodgeCooldownTimer = _sm.DodgeCooldownDuration;
        _sm.MoveVector = _sm.DodgeSpeed * Time.deltaTime * _sm.InputMove.normalized;
        _sm.PlayerCharacter.SpriteRenderer.color = Color.blue;
    }

    public override void ExitState()
    {
        _sm.PlayerCharacter.SpriteRenderer.color = Color.red;
    }

    public override void UpdateState()
    {
        _dodgeTimer -= Time.deltaTime;
    }
}
