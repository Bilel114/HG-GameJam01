using UnityEngine;

public class PlayerStateDodge : PlayerStateBase
{
    private float _dodgeTimer;

    public PlayerStateDodge(PlayerStateMachine stateMachine) : base(stateMachine, PlayerStateIds.Dodge)
    {
    }

    public override void CheckSwitchState()
    {
        if (_sm.SwitchToFrozenState)
        {
            _sm.SwitchState(PlayerStateIds.Frozen);
        }
        else if (_dodgeTimer < 0)
        {
            _sm.SwitchState(PlayerStateIds.Normal);
        }
    }

    public override void EnterState()
    {
        _dodgeTimer = _sm.DodgeDuration;
        _sm.DodgeCooldownTimer = _sm.DodgeCooldownDuration;
        _sm.MoveVector = _sm.DodgeSpeed * Time.deltaTime * _sm.InputMove.normalized;
        _sm.PlayerCharacter.Animator.SetTrigger(AnimatorHash.Player_Dodge);
        _sm.PlayerCharacter.Animator.SetInteger(AnimatorHash.Player_MoveDirectionX, (int)_sm.InputMove.x);
        _sm.PlayerCharacter.Animator.SetInteger(AnimatorHash.Player_MoveDirectionY, (int)_sm.InputMove.y);
        GameObject.Instantiate(_sm.DustEffectPrefab, _sm.transform.position, Quaternion.identity);
    }

    public override void ExitState()
    {
        _sm.PlayerCharacter.Animator.SetInteger(AnimatorHash.Player_MoveDirectionX, (int)_sm.InputMove.x);
        _sm.PlayerCharacter.Animator.SetInteger(AnimatorHash.Player_MoveDirectionY, (int)_sm.InputMove.y);
        //_sm.PlayerCharacter.SpriteRenderer.color = Color.red;
    }

    public override void UpdateState()
    {
        _dodgeTimer -= Time.deltaTime;
    }
}
