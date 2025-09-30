using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1IronGates : MonoBehaviour
{
    public Collider2D Collider;
    public AudioClip OpenSound, CloseSound;
    AudioSource _audioSource;
    bool _closedGates = false;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void OpenGates()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, true);
        }
        Invoke(nameof(PlayeOpenSound), 0.4f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.PlayerLayer && other.transform.position.y > transform.position.y + 0.15f && !_closedGates)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Collider2D>().enabled = true;
                transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, false);
            }
            _closedGates = true;
            Invoke(nameof(PlayCloseSound), 0.4f);
        }
    }

    void PlayCloseSound ()
    {
        _audioSource.PlayOneShot(CloseSound);
    }

    void PlayeOpenSound ()
    {
        _audioSource.PlayOneShot(OpenSound);
    }
}
