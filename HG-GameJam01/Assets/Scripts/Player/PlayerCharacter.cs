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
}
