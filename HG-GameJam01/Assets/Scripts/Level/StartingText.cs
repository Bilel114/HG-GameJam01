using System.Collections;
using System.Text;
using UnityEngine;
using TMPro;

public class StartingText : MonoBehaviour
{
    public PlayerCharacter Player;
    [TextArea] public string TextString;
    public string[] DialogueStrings;
    StringBuilder stringBuilder = new StringBuilder();
    public TextMeshPro TextComponent;
    public float Speed = 0.02f;
    public AudioSource AudioSource;
    int AudioTimer;

    public void StartTextScroll ()
    {
        StartCoroutine(ScrollTextCoroutine());
    }

    IEnumerator ScrollTextCoroutine()
    {
        for (int i = 0; i < DialogueStrings.Length; i++)
        {
            if (i % 2 == 0)
            {
                stringBuilder.Append(DialogueStrings[i]);
                TextComponent.text = stringBuilder.ToString();
                DialogueAudioTick();
                yield return new WaitForSeconds(Speed);
            }
            else
            {
                for (int j = 0; j < DialogueStrings[i].Length; j++)
                {
                    stringBuilder.Append(DialogueStrings[i][j]);
                    TextComponent.text = stringBuilder.ToString();
                    DialogueAudioTick();
                    yield return new WaitForSeconds(Speed);
                }
                stringBuilder.Append('\n');
            }
        }

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
        Player.StateMachine.SwitchToFrozenState = false;
    }

    void DialogueAudioTick ()
    {
        AudioTimer--;

        if (AudioTimer <= 0)
        {
            AudioSource.Play();
            AudioTimer = Random.Range(2, 4);
        }
    }
}
