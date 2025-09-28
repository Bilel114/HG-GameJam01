using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1IronGates : MonoBehaviour
{
    public void OpenGates()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, true);
        }
    }
}
