using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBossStateLightAttack : CarBossStateBase
{
    enum LightAttackPhases { GoingToPosition, Aniticipation, Firing, }
    LightAttackPhases _attackPhase;
    Vector2 _nextDestination;
    float _anticipationTimer;
    float _endTimer;
    bool _attackComplete;

    public CarBossStateLightAttack(CarBossStateMachine stateMachine) : base(stateMachine, CarBossStateIds.LightAttack)
    {
    }

    public override void CheckSwitchState()
    {
        if (_sm.SwitchToFrozenState)
        {
            _sm.SwitchState(CarBossStateIds.Frozen);
        }
        else if (_attackComplete)
        {
            _sm.SwitchState(CarBossStateIds.Normal);
        }
    }

    public override void EnterState()
    {
        _attackComplete = false;
        _attackPhase = LightAttackPhases.GoingToPosition;
        _nextDestination = _sm.PatrolPoints[0].position + (_sm.PatrolPoints[1].position - _sm.PatrolPoints[0].position) * 0.3f;
    }

    public override void ExitState()
    {
        _sm.LightAttackHitBox.SetActive(false);
        _sm.Animator.Play(AnimatorHash.Boss_Move);
    }

    public override void UpdateState()
    {
        switch (_attackPhase)
        {
            case LightAttackPhases.GoingToPosition:

                if (Vector2.Distance(_sm.transform.position, _nextDestination) > 0.01f)
                {
                    _sm.transform.position = Vector2.MoveTowards(_sm.transform.position, _nextDestination, _sm.MoveSpeed * Time.deltaTime);
                }
                else
                {
                    _attackPhase = LightAttackPhases.Aniticipation;
                    _sm.Animator.Play(AnimatorHash.Boss_Attack2Anticipation);
                    _anticipationTimer = _sm.LightAttackAnticipationDuration;
                }
                break;

            case LightAttackPhases.Aniticipation:

                if (_anticipationTimer > 0)
                {
                    _anticipationTimer -= Time.deltaTime;
                }
                else
                {
                    _attackPhase = LightAttackPhases.Firing;
                    _sm.Animator.Play(AnimatorHash.Boss_BossAttack2Beam);
                    _endTimer = _sm.LightAttackDuration;
                    _sm.SpriteRenderer.sortingOrder = 31;
                }
                break;

            case LightAttackPhases.Firing:

                if (_endTimer > 0)
                {
                    _endTimer -= Time.deltaTime;
                    if (_endTimer < _sm.LightAttackHitBoxOnTime && _endTimer > _sm.LightAttackHitBoxOffTime)
                    {
                        _sm.LightAttackHitBox.SetActive(true);
                    }
                    else
                    {
                        _sm.LightAttackHitBox.SetActive(false);
                    }
                }
                else
                {
                    _sm.SpriteRenderer.sortingOrder = 10;
                    _attackComplete = true;
                }
                break;
        }
    }
}
