using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private const float _minMoveDistance = 0.001f;
    private const float _skinWidth = 0.01f;

    private Rigidbody2D _rigidbody;
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _raycastHitBuffer = new RaycastHit2D[16];

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(0)); // TODO add player layer
    }

    public void Move (Vector2 moveVector)
    {
        float distance = moveVector.magnitude;

        if (distance > _minMoveDistance)
        {
            int hitCount = _rigidbody.Cast(moveVector, _contactFilter, _raycastHitBuffer, distance + _skinWidth);
            
            for (int i = 0; i < hitCount; i++)
            {
                distance = Mathf.Min(distance, _raycastHitBuffer[i].distance - _skinWidth);
            }

            _rigidbody.position += distance * moveVector.normalized;
        }
    }
}
