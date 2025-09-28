using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }
    private Animator _animator;
    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    private BoxCollider2D _collider;
    public BoxCollider2D Collider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider2D>();
            }
            return _collider;
        }
    }

    private Animator _damageEffectAnimator;
    public Animator DamageEffectAnimator
    {
        get
        {
            if (_damageEffectAnimator == null)
            {
                _damageEffectAnimator = transform.Find("DamageEffect").GetComponent<Animator>();
            }
            return _damageEffectAnimator;
        }
    }

    private Animator _shieldEffectAnimator;
    public Animator ShieldEffectAnimator
    {
        get
        {
            if (_shieldEffectAnimator == null)
            {
                _shieldEffectAnimator = transform.Find("ShieldEffect").GetComponent<Animator>();
            }
            return _shieldEffectAnimator;
        }
    }

    private GameObject _shieldBarrier;
    public GameObject ShieldBarrier
    {
        get
        {
            if (_shieldBarrier == null)
            {
                _shieldBarrier = transform.Find("ShieldBarrier").gameObject;
            }
            return _shieldBarrier;
        }
    }

    public LevelManager LevelManager;
}
