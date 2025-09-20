using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private const float _minMoveDistance = 0.001f;
    private const float _skinWidth = 0.01f;

    private Rigidbody2D _rigidbody;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _raycastHitBuffer = new RaycastHit2D[16];
    private int _hitCount;
    private float _distance;
    private Vector2 _tempVector = new Vector2();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // TODO add player layer
    }

    public void Move(Vector2 moveVector)
    {
        // start with horizontal component first
        _distance = Mathf.Abs(moveVector.x);
        if (_distance > _minMoveDistance)
        {
            _tempVector.Set(moveVector.x, 0);
            _hitCount = _rigidbody.Cast(_tempVector, _contactFilter, _raycastHitBuffer, _distance + _skinWidth);

            for (int i = 0; i < _hitCount; i++)
            {
                Debug.DrawLine(transform.position, _raycastHitBuffer[i].point, Color.green, 0, false);
                _distance = Mathf.Min(_distance, _raycastHitBuffer[i].distance - _skinWidth);
            }

            if (_distance > _minMoveDistance)
            {
                _tempVector.Set(Mathf.Sign(moveVector.x) * _distance, 0);
                _rigidbody.position += _tempVector;
            }
        }

        // then vertical component 
        _distance = Mathf.Abs(moveVector.y);
        if (_distance > _minMoveDistance)
        {
            _tempVector.Set(0, moveVector.y);
            _hitCount = _rigidbody.Cast(_tempVector, _contactFilter, _raycastHitBuffer, _distance + _skinWidth);

            for (int i = 0; i < _hitCount; i++)
            {
                Debug.DrawLine(transform.position, _raycastHitBuffer[i].point, Color.green, 0, false);
                _distance = Mathf.Min(_distance, _raycastHitBuffer[i].distance - _skinWidth);
            }

            if (_distance > _minMoveDistance)
            {
                _tempVector.Set(0, Mathf.Sign(moveVector.y) * _distance);
                _rigidbody.position += _tempVector;
            }
        }
    }
}
