using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum PlayerStateIds { Normal, Dodge, Shield, Frozen, }

public class PlayerStateMachine : MonoBehaviour
{
    // dictionary for caching states
    private readonly Dictionary<PlayerStateIds, PlayerStateBase> _playerStates = new Dictionary<PlayerStateIds, PlayerStateBase>();
    // dictionary of all state types to make state creation easier (avoid big switch case statement)
    private readonly Dictionary<PlayerStateIds, System.Type> _playerStateTypes = new Dictionary<PlayerStateIds, System.Type>()
    {
        {PlayerStateIds.Normal, typeof(PlayerStateNormal) },
        {PlayerStateIds.Dodge, typeof(PlayerStateDodge) },
        {PlayerStateIds.Shield, typeof(PlayerStateShield) },
        {PlayerStateIds.Frozen, typeof(PlayerStateFrozen) },
    };
    private readonly object[] _thisInstanceAsArray = new object[1];
    private PlayerStateBase _currentState;
    public PlayerStateBase CurrentState { get => _currentState; set => _currentState = value; }
    [SerializeField] private PlayerStateIds _currentStateId;
    public PlayerStateIds CurrentStateId { get => _currentStateId; }

    private PlayerCharacter _playerCharacter;
    public PlayerCharacter PlayerCharacter { get => _playerCharacter; }
    private CharacterController2D _charController;

    // analog movement input values
    private Vector2 _normalizedInput;
    private const float _ANALOG_ANGLE_SIN_COS = 0.3826f;

    public bool SwitchToFrozenState;

    // Movement fields
    private Vector2 _moveVector;
    public Vector2 MoveVector { get => _moveVector; set => _moveVector = value; }
    [SerializeField] private float _moveSpeed = 1;
    public float MoveSpeed { get => _moveSpeed; }

    // Dodge fields
    [SerializeField] private float _dodgeDuration = 0.4f;
    public float DodgeDuration { get => _dodgeDuration; }
    [SerializeField] private float _dodgeSpeed = 2;
    public float DodgeSpeed { get => _dodgeSpeed; }
    [SerializeField] private float _dodgeCooldownDuration = 1.4f;
    public float DodgeCooldownDuration { get => _dodgeCooldownDuration; }
    private float _dodgeCooldownTimer;
    public float DodgeCooldownTimer { get => _dodgeCooldownTimer; set => _dodgeCooldownTimer = value; }
    [SerializeField] private GameObject _dustEffectPrefab;
    public GameObject DustEffectPrefab { get => _dustEffectPrefab; }
    
    public float ImmunityDuration = 1f;
    public float ImmunityTimer;
    public float DamageTimeCost = 5;

    // Shield fields
    public float ShieldCreationTime = 0.16f;
    public float ShieldCooldownDuration = 1;
    public float ShieldCooldownTimer;
    public bool IsShieldInterrupted;
    public bool IsShieldBarrierUp;

    public RuneStone _currentRuneStone;
    public RuneOrb _currentRuneOrb;
    public int CurrentRuneStoneId = -1;

    // input fields
    #region Input fields
    [SerializeField] private InputManagerSO _inputManager;
    private const float _INPUT_BUFFER_DURATION = 0.2f;
    private readonly Dictionary<Bool, float> _inputBuffer = new Dictionary<Bool, float>();
    private readonly List<Bool> _expiredInputList = new List<Bool>();

    private Vector2 _inputMove;
    private Bool _isInputInteractPressed = new Bool();
    private bool _isInputInteractHeld;
    private Bool _isInputDodgePressed = new Bool();
    private bool _isInputDodgeHeld;
    private bool _isInputShieldPressed;
    private bool _isInputShieldHeld;
    private bool _isInputPausePressed;

    public Vector2 InputMove { get => _inputMove; }
    public bool IsInputInteractPressed { get => _isInputInteractPressed.val; set => _isInputInteractPressed.val = value; }
    public bool IsInputInteractHeld { get => _isInputInteractHeld; }
    public bool IsInputDodgePressed { get => _isInputDodgePressed.val; set => _isInputDodgePressed.val = value; }
    public bool IsInputDodgeHeld { get => _isInputDodgeHeld; }
    public bool IsInputShieldPressed { get => _isInputShieldPressed; set => _isInputShieldPressed = value; }
    public bool IsInputShieldHeld { get => _isInputShieldHeld; }
    public bool IsInputPausePressed { get => _isInputPausePressed; set => _isInputPausePressed = value; }
    #endregion

    // debug fields
    public float Debug_TimeScale = 1;
    public Vector2 Debug_AutoMoveVector;


    private void Awake()
    {
        Application.targetFrameRate = 60; // TODO move to game manager
        _thisInstanceAsArray[0] = this;
        _playerCharacter = GetComponent<PlayerCharacter>();
        _charController = GetComponent<CharacterController2D>();

        _currentState = GetState(PlayerStateIds.Normal);
        _currentStateId = PlayerStateIds.Normal;
        _currentState.EnterState();
    }

    private void OnEnable()
    {
        _inputManager.Enable();
        InitializeInput();
    }

    private void OnDisable()
    {
        ReleaseInput();
    }

    private void Update()
    {
        Time.timeScale = Debug_TimeScale;

        _currentState.CheckSwitchState();
        _currentState.UpdateState();

        MoveCharacter();
        UpdateTimers();
        CheckInteraction();

        CheckInputBuffer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.EnemyLayer)
        {
            if (_currentStateId != PlayerStateIds.Dodge && _currentStateId != PlayerStateIds.Frozen && ImmunityTimer < 0)
            {
                if (IsShieldBarrierUp)
                {
                    IsShieldInterrupted = true;
                    ImmunityTimer = ImmunityDuration;
                }
                else
                {
                    GetHit(); 
                }
            }
        }
        else if (other.gameObject.layer == PhysicsLayerIds.InteractableLayer)
        {
            other.TryGetComponent<RuneStone>(out _currentRuneStone);
            other.TryGetComponent<RuneOrb>(out _currentRuneOrb);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.InteractableLayer)
        {
            _currentRuneStone = null;
            _currentRuneOrb = null;
        }
    }

    public PlayerStateBase GetState(PlayerStateIds stateId)
    {
        if (!_playerStates.ContainsKey(stateId))
        {
            // create a new instance of the proper state type (from _playerStateTypes)
            // and pass this PlayerStateMachine to its constructor
            PlayerStateBase newState = (PlayerStateBase)System.Activator.CreateInstance(_playerStateTypes[stateId], _thisInstanceAsArray);
            _playerStates.Add(stateId, newState);
        }

        return _playerStates[stateId];
    }

    public void SwitchState (PlayerStateIds newStateId)
    {
        _currentState.ExitState();
        _currentState = GetState(newStateId);
        _currentStateId = newStateId;
        _currentState.EnterState();
    }

    private void MoveCharacter ()
    {
        _charController.Move(_moveVector);
    }

    private void UpdateTimers ()
    {
        if (_dodgeCooldownTimer > 0)
        {
            _dodgeCooldownTimer -= Time.deltaTime;
        }
        if (ImmunityTimer >= 0)
        {
            ImmunityTimer -= Time.deltaTime;
        }
        if (ShieldCooldownTimer >= 0)
        {
            ShieldCooldownTimer -= Time.deltaTime;
        }
    }

    private void GetHit ()
    {
        PlayerCharacter.LevelManager.GameTimer.DecreaseTimer(DamageTimeCost);
        PlayerCharacter.DamageEffectAnimator.SetTrigger(AnimatorHash.Player_GetHit);
        // play sound
        StartCoroutine(GetHitColorCoroutine());
        ImmunityTimer = ImmunityDuration;
    }

    IEnumerator GetHitColorCoroutine()
    {
        PlayerCharacter.SpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        PlayerCharacter.SpriteRenderer.color = Color.white;
    }

    private void CheckInteraction ()
    {
        if (IsInputInteractPressed)
        {
            if (_currentRuneStone != null && !_currentRuneStone.IsRuneOn && _currentRuneStone.Id == CurrentRuneStoneId)
            {
                IsInputInteractPressed = false;
                _currentRuneStone.ActivateRuneStone();
                CurrentRuneStoneId = -1;
                PlayerCharacter.FollowingRuneSprite.enabled = false;
            }
            else if (_currentRuneOrb != null && !_currentRuneOrb.IsSymbolUsed)
            {
                IsInputInteractPressed = false;
                CurrentRuneStoneId = _currentRuneOrb.Id;
                PlayerCharacter.FollowingRuneSprite.enabled = true;
                PlayerCharacter.FollowingRuneSprite.sprite = _currentRuneOrb.SpriteRenderer.sprite;
            }
        }
    }

    #region Input Methods
    private void InitializeInput()
    {
        _inputManager.MoveEvent += OnInputMove;
        _inputManager.MoveAnalogEvent += OnInputMoveAnalog;
        _inputManager.InteractEvent += OnInputInteractPressed;
        _inputManager.InteractCancelledEvent += OnInputInteractReleased;
        _inputManager.DodgeEvent += OnInputDodgePressed;
        _inputManager.DodgeCancelledEvent += OnInputDodgeReleased;
        _inputManager.ShieldEvent += OnInputShieldPressed;
        _inputManager.ShieldCancelledEvent += OnInputShieldReleased;
        _inputManager.PauseEvent += OnInputPausePressed;
    }

    private void ReleaseInput()
    {
        _inputManager.MoveEvent -= OnInputMove;
        _inputManager.MoveAnalogEvent -= OnInputMoveAnalog;
        _inputManager.InteractEvent -= OnInputInteractPressed;
        _inputManager.InteractCancelledEvent -= OnInputInteractReleased;
        _inputManager.DodgeEvent -= OnInputDodgePressed;
        _inputManager.DodgeCancelledEvent -= OnInputDodgeReleased;
        _inputManager.ShieldEvent -= OnInputShieldPressed;
        _inputManager.ShieldCancelledEvent -= OnInputShieldReleased;
        _inputManager.PauseEvent -= OnInputPausePressed;
    }

    private void AddInputBuffer(Bool input)
    {
        if (_inputBuffer.ContainsKey(input))
        {
            _inputBuffer[input] = Time.time + _INPUT_BUFFER_DURATION;
        }
        else
        {
            _inputBuffer.Add(input, Time.time + _INPUT_BUFFER_DURATION);
        }
    }

    private void CheckInputBuffer()
    {
        _expiredInputList.Clear();
        foreach (Bool input in _inputBuffer.Keys)
        {
            if (Time.time > _inputBuffer[input])
            {
                _expiredInputList.Add(input);
            }
        }

        foreach (Bool input in _expiredInputList)
        {
            _inputBuffer.Remove(input);
            input.val = false;
        }
    }

    private void OnInputMove(Vector2 input)
    {
        _inputMove = input;
    }

    private void OnInputMoveAnalog(Vector2 input)
    {
        _inputMove = Vector2.zero;

        if (input.x == 0 && input.y == 0) // no key is pressed
        {
            return;
        }

        _normalizedInput = input.normalized;

        if (_normalizedInput.x > _ANALOG_ANGLE_SIN_COS) // right key is pressed
        {
            _inputMove.x = 1;
        }
        else if (_normalizedInput.x < -_ANALOG_ANGLE_SIN_COS) // left key is pressed
        {
            _inputMove.x = -1;
        }

        if (_normalizedInput.y > _ANALOG_ANGLE_SIN_COS) // up key is pressed
        {
            _inputMove.y = 1;
        }
        else if (_normalizedInput.y < -_ANALOG_ANGLE_SIN_COS) // down key is pressed
        {
            _inputMove.y = -1;
        }
    }

    private void OnInputInteractPressed()
    {
        _isInputInteractPressed.val = _isInputInteractHeld = true;
        AddInputBuffer(_isInputInteractPressed);
    }

    private void OnInputInteractReleased()
    {
        _isInputInteractPressed.val = _isInputInteractHeld = false;
    }

    private void OnInputDodgePressed()
    {
        _isInputDodgePressed.val = _isInputDodgeHeld = true;
        AddInputBuffer(_isInputDodgePressed);
    }

    private void OnInputDodgeReleased()
    {
        _isInputDodgePressed.val = _isInputDodgeHeld = false;
    }

    private void OnInputShieldPressed()
    {
        _isInputShieldPressed = _isInputShieldHeld = true;
    }

    private void OnInputShieldReleased()
    {
        _isInputShieldPressed = _isInputShieldHeld = false;
    }

    private void OnInputPausePressed()
    {
        _isInputPausePressed = true;
    }
    #endregion
}

/// <summary>
/// Class used to keep a refenrece of a bool variable
/// </summary>
public class Bool
{
    public bool val;
}
