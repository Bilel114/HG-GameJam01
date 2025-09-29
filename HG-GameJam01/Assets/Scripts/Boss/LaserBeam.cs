using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Collider2D Collider;

    public void ActivateHitbox ()
    {
        Collider.enabled = true;
    }

    public void AttackEnd ()
    {
        Destroy(gameObject);
    }
}
