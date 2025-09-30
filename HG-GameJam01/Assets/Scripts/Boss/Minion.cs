using UnityEngine;

public class Minion : MonoBehaviour
{
    public PlayerCharacter Player;
    public float MoveSpeed = 1;
    public float AttackRadius = 1;
    Vector2 _startingPos;
    AudioSource _audioSource;

    private void Start()
    {
        _startingPos = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Vector2.Distance(_startingPos, Player.transform.position) < AttackRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _startingPos, MoveSpeed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, Player.transform.position) < 2 * AttackRadius)
        {
            _audioSource.enabled = true;
        }
        else
        {
            _audioSource.enabled = false;
        }
    }

    public void PlayBounceSound ()
    {
        if (_audioSource.enabled)
        {
            _audioSource.Play(); 
        }
    }
}
