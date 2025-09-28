using UnityEngine;

public class PlayerStateShield : PlayerStateBase
{
    float _shieldCreationTimer;

    public PlayerStateShield(PlayerStateMachine stateMachine) : base(stateMachine, PlayerStateIds.Shield)
    {
    }

    public override void CheckSwitchState()
    {
        if (_sm.SwitchToFrozenState)
        {
            _sm.SwitchState(PlayerStateIds.Frozen);
        }
        else if (_sm.IsShieldInterrupted || !_sm.IsInputShieldHeld)
        {
            _sm.SwitchState(PlayerStateIds.Normal);
        }
    }

    public override void EnterState()
    {
        _sm.PlayerCharacter.ShieldEffectAnimator.Play(AnimatorHash.Player_ShieldForming);
        _shieldCreationTimer = _sm.ShieldCreationTime;
        _sm.IsShieldInterrupted = false;
    }

    public override void ExitState()
    {
        if (_sm.IsShieldBarrierUp)
        {
            _sm.IsShieldBarrierUp = false;
            _sm.PlayerCharacter.ShieldBarrier.SetActive(false);

            if (_sm.IsShieldInterrupted)
            {
                _sm.PlayerCharacter.ShieldEffectAnimator.Play(AnimatorHash.Player_ShieldPopping);
            }
        }
        _sm.ShieldCooldownTimer = _sm.ShieldCooldownDuration;
    }

    public override void UpdateState()
    {
        if (_shieldCreationTimer > 0)
        {
            _shieldCreationTimer -= Time.deltaTime;
        }
        else
        {
            if (!_sm.IsShieldBarrierUp)
            {
                _sm.IsShieldBarrierUp = true;
                _sm.PlayerCharacter.ShieldBarrier.SetActive(true);
            }
        }
    }
}
