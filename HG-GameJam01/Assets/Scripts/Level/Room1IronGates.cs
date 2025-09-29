using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1IronGates : MonoBehaviour
{
    public Collider2D Collider;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }

    public void OpenGates()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.PlayerLayer && other.transform.position.y > transform.position.y + 0.15f)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Collider2D>().enabled = true;
                transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, false);
            }
        }
    }
}
