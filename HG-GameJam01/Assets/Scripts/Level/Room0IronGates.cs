using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room0IronGates : MonoBehaviour
{
    public LevelManager LevelManager;
    public Collider2D Collider;
    public AudioClip CloseSound;
    AudioSource _audioSource;
    bool _closedGates = false;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.PlayerLayer && other.transform.position.y > transform.position.y && !_closedGates)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Collider2D>().enabled = true;
                transform.GetChild(i).GetComponent<Animator>().SetBool(AnimatorHash.Level_IronGateIsOpen, false);
            }
            _closedGates = true;
            Invoke(nameof(PlaySound), 0.4f);
            LevelManager.StartFirstFight();
        }
    }

    void PlaySound ()
    {
        _audioSource.PlayOneShot(CloseSound);
    }
}
