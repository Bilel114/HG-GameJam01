

public abstract class PlayerStateBase
{
    public readonly PlayerStateIds StateId;

    protected PlayerStateMachine _sm;

    public PlayerStateBase(PlayerStateMachine stateMachine, PlayerStateIds stateId)
    {
        _sm = stateMachine;
        StateId = stateId;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void UpdateState();

}
