using UnityEngine;

public abstract class CarBossStateBase
{
    public readonly CarBossStateIds StateId;

    protected CarBossStateMachine _sm;

    public CarBossStateBase(CarBossStateMachine stateMachine, CarBossStateIds stateId)
    {
        _sm = stateMachine;
        StateId = stateId;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void UpdateState();
}
