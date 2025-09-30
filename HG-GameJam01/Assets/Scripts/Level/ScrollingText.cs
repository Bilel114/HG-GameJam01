using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    [TextArea] public string TextString;
    public string[] DialogueStrings;
    StringBuilder stringBuilder = new StringBuilder();
    public TextMeshProUGUI TextComponent;
    public float Speed = 0.02f;
    public CanvasGroup RetryButton;
    public CanvasGroup FadeInImage;

    private void Start()
    {
        StartCoroutine(ScrollTextCoroutine());
    }

    IEnumerator ScrollTextCoroutine ()
    {
        while (FadeInImage.alpha > 0)
        {
            FadeInImage.alpha -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < DialogueStrings.Length; i++)
        {
            if (i % 2 == 0)
            {
                stringBuilder.Append(DialogueStrings[i]);
                TextComponent.text = stringBuilder.ToString();
                yield return new WaitForSeconds(Speed);
            }
            else
            {
                for (int j = 0; j < DialogueStrings[i].Length; j++)
                {
                    stringBuilder.Append(DialogueStrings[i][j]);
                    TextComponent.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(Speed);
                }
                stringBuilder.Append('\n');
            }
        }

        RetryButton.gameObject.SetActive(true);
        while (RetryButton.alpha < 1)
        {
            RetryButton.alpha += Time.deltaTime;
            yield return null;
        }

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        Retry();
    }

    public void Retry ()
    {
        SceneManager.LoadScene(0);
    }
}
