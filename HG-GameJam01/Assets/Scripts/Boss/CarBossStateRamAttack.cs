using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBossStateRamAttack : CarBossStateBase
{
    enum RamAttackPhases { LookingForPlayer, Aniticipation, Ramming, End, Resetting }
    RamAttackPhases _attackPhase;
    Vector2 _nextDestination;
    float _anticipationTimer;
    float _endTimer;
    bool _attackComplete;

    public CarBossStateRamAttack(CarBossStateMachine stateMachine) : base(stateMachine, CarBossStateIds.RamAttack)
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
        _attackPhase = RamAttackPhases.LookingForPlayer;
        _attackComplete = false;
        _nextDestination = _sm.PatrolPoints[_sm.CurrentPatrolPoint].position;
    }

    public override void ExitState()
    {
        _sm.Animator.Play(AnimatorHash.Boss_Move);
    }

    public override void UpdateState()
    {
        switch (_attackPhase)
        {
            case RamAttackPhases.LookingForPlayer:

                if (Vector2.Distance(_sm.transform.position, _nextDestination) > 0.01f)
                {
                    _sm.transform.position = Vector2.MoveTowards(_sm.transform.position, _nextDestination, _sm.MoveSpeed * Time.deltaTime);
                }
                else
                {
                    _sm.CurrentPatrolPoint = ++_sm.CurrentPatrolPoint % _sm.PatrolPoints.Length;
                    _nextDestination = _sm.PatrolPoints[_sm.CurrentPatrolPoint].position;
                }

                if (Mathf.Abs(_sm.transform.position.x - _sm.Player.transform.position.x) < _sm.Player.Collider.size.x * 0.5f)
                {
                    _attackPhase = RamAttackPhases.Aniticipation;
                    _anticipationTimer = _sm.RamAttackAnticipationDuration;
                    _sm.RamAttackPoint = _sm.transform.position;
                    _sm.Animator.Play(AnimatorHash.Boss_Attack1Anticipation);
                }
                break;

            case RamAttackPhases.Aniticipation:

                if (_anticipationTimer > 0)
                {
                    _anticipationTimer -= Time.deltaTime;
                }
                else
                {
                    _attackPhase = RamAttackPhases.Ramming;
                    _sm.Animator.Play(AnimatorHash.Boss_Attack1Charge);
                }
                break;

            case RamAttackPhases.Ramming:

                if (Vector2.Distance(_sm.transform.position, _sm.RamAttackPoint) < _sm.RamAttackDistance)
                {
                    _sm.transform.position += _sm.RamAttackSpeed * Time.deltaTime * Vector3.down;
                }
                else
                {
                    _attackPhase = RamAttackPhases.End;
                    _endTimer = _sm.RamAttackEndDuration;
                    _sm.Animator.Play(AnimatorHash.Boss_Attack1End);
                }
                break;

            case RamAttackPhases.End:

                if (_endTimer > 0)
                {
                    _endTimer -= Time.deltaTime;
                }
                else
                {
                    _attackPhase = RamAttackPhases.Resetting;
                    _sm.Animator.Play(AnimatorHash.Boss_Move);
                }
                break;

            case RamAttackPhases.Resetting:

                if (Vector2.Distance(_sm.transform.position, _sm.RamAttackPoint) > 0.01f)
                {
                    _sm.transform.position = Vector2.MoveTowards(_sm.transform.position, _sm.RamAttackPoint, _sm.MoveSpeed * Time.deltaTime);
                }
                else
                {
                    _attackComplete = true;
                }
                break;
        }
    }
}
