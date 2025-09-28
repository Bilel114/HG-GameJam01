using System.Collections.Generic;
using UnityEngine;

public enum CarBossStateIds { Normal, RamAttack, Frozen, }

public class CarBossStateMachine : MonoBehaviour
{
    // dictionary for caching states
    private readonly Dictionary<CarBossStateIds, CarBossStateBase> _carBossStates = new Dictionary<CarBossStateIds, CarBossStateBase>();
    // dictionary of all state types to make state creation easier (avoid big switch case statement)
    private readonly Dictionary<CarBossStateIds, System.Type> _carBossStateTypes = new Dictionary<CarBossStateIds, System.Type>()
    {
        {CarBossStateIds.Normal, typeof(CarBossStateNormal) },
        {CarBossStateIds.RamAttack, typeof(CarBossStateRamAttack) },
        {CarBossStateIds.Frozen, typeof(CarBossStateFrozen) },
    };
    private readonly object[] _thisInstanceAsArray = new object[1];
    private CarBossStateBase _currentState;
    public CarBossStateBase CurrentState { get => _currentState; set => _currentState = value; }
    [SerializeField] private CarBossStateIds _currentStateId;
    public CarBossStateIds CurrentStateId { get => _currentStateId; }

    public PlayerCharacter Player;
    public Animator Animator;

    // movement fields
    public float MoveSpeed = 0.75f;
    public Transform[] PatrolPoints = new Transform[2];
    public int CurrentPatrolPoint;

    public bool SwitchToFrozenState;

    public float AttackCooldownMin = 3;
    public float AttackCooldownMax = 5;
    public float AttackTimer;

    public Vector2 RamAttackPoint;
    public float RamAttackSpeed = 1.5f;
    public float RamAttackDistance;
    public float RamAttackAnticipationDuration = 1;
    public float RamAttackEndDuration = 1;

    private void Awake()
    {
        _thisInstanceAsArray[0] = this;
        Animator = GetComponent<Animator>();

        _currentState = GetState(CarBossStateIds.Frozen);
        _currentStateId = CarBossStateIds.Frozen;
        SwitchToFrozenState = true;
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.CheckSwitchState();
        _currentState.UpdateState();
    }

    public CarBossStateBase GetState(CarBossStateIds stateId)
    {
        if (!_carBossStates.ContainsKey(stateId))
        {
            // create a new instance of the proper state type (from _playerStateTypes)
            // and pass this PlayerStateMachine to its constructor
            CarBossStateBase newState = (CarBossStateBase)System.Activator.CreateInstance(_carBossStateTypes[stateId], _thisInstanceAsArray);
            _carBossStates.Add(stateId, newState);
        }

        return _carBossStates[stateId];
    }

    public void SwitchState(CarBossStateIds newStateId)
    {
        _currentState.ExitState();
        _currentState = GetState(newStateId);
        _currentStateId = newStateId;
        _currentState.EnterState();
    }
}
