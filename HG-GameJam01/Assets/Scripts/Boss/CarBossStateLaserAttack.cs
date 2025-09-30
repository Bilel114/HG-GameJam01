using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBossStateLaserAttack : CarBossStateBase
{
    enum LaserAttackPhases { GoingToPosition, FollowingPlayer, Aniticipation, Firing, }
    LaserAttackPhases _attackPhase;
    Vector2 _nextDestination;
    Vector3 _firingTarget;
    float _anticipationTimer;
    float _endTimer;
    bool _attackComplete, _firedBeam;

    public CarBossStateLaserAttack(CarBossStateMachine stateMachine) : base(stateMachine, CarBossStateIds.LaserAttack)
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
        _firedBeam = false;
        _attackPhase = LaserAttackPhases.GoingToPosition;
        _nextDestination = (_sm.PatrolPoints[0].position + _sm.PatrolPoints[1].position) * 0.5f;
    }

    public override void ExitState()
    {
        _sm.LaserBeamPointer.SetActive(false);
        _sm.Animator.Play(AnimatorHash.Boss_Move);
    }

    public override void UpdateState()
    {
        switch (_attackPhase)
        {
            case LaserAttackPhases.GoingToPosition:

                if (Vector2.Distance(_sm.transform.position, _nextDestination) > 0.01f)
                {
                    _sm.transform.position = Vector2.MoveTowards(_sm.transform.position, _nextDestination, _sm.MoveSpeed * Time.deltaTime);
                }
                else
                {
                    _attackPhase = LaserAttackPhases.FollowingPlayer;
                    _anticipationTimer = _sm.LaserAttackFollowingDuration;
                    _sm.LaserBeamPointer.transform.up = _sm.LaserBeamPoint.position - _sm.Player.transform.position;
                    _sm.LaserBeamPointer.SetActive(true);
                }
                break;

            case LaserAttackPhases.FollowingPlayer:

                Debug.DrawRay(_sm.LaserBeamPoint.position, _sm.Player.transform.position - _sm.LaserBeamPoint.position, Color.red, 0, false);
                _sm.LaserBeamPointer.transform.up = _sm.LaserBeamPoint.position - _sm.Player.transform.position;

                if (_anticipationTimer > 0)
                {
                    _anticipationTimer -= Time.deltaTime;
                }
                else
                {
                    _attackPhase = LaserAttackPhases.Aniticipation;
                    _firingTarget = _sm.Player.transform.position;
                    _anticipationTimer = _sm.LaserAttackPauseDuration;
                    _sm.Animator.Play(AnimatorHash.Boss_Attack1Anticipation);
                    _sm.AudioSource.PlayOneShot(_sm.AnticipationSound);
                }
                break;

            case LaserAttackPhases.Aniticipation:

                Debug.DrawRay(_sm.LaserBeamPoint.position, _firingTarget - _sm.LaserBeamPoint.position, Color.red, 0, false);

                if (_anticipationTimer > 0)
                {
                    _anticipationTimer -= Time.deltaTime;
                }
                else
                {
                    _attackPhase = LaserAttackPhases.Firing;
                    _endTimer = _sm.LaserAttackPauseDuration;
                    _sm.LaserBeamPointer.SetActive(false);
                }
                break;

            case LaserAttackPhases.Firing:

                if (!_firedBeam)
                {
                    GameObject go = GameObject.Instantiate(_sm.LaserBeamPrefab, _sm.LaserBeamPoint.position, Quaternion.identity);
                    go.transform.up = _sm.LaserBeamPoint.position - _firingTarget;
                    go.transform.position -= _sm.BeamCenterOffset * go.transform.up;
                    _firedBeam = true;
                }

                if (_endTimer > 0)
                {
                    _endTimer -= Time.deltaTime;
                }
                else
                {
                    _attackComplete = true;
                }
                break;
        }
    }
}
