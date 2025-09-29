using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    [TextArea] public string TextString;
    public TextMeshProUGUI TextComponent;
    public float Speed = 0.1f;
    public CanvasGroup RetryButton;

    private void Start()
    {
        StartCoroutine(ScrollTextCoroutine());
    }

    IEnumerator ScrollTextCoroutine ()
    {
        for (int i = 0; i < TextString.Length + 1; i++)
        {
            TextComponent.text = TextString.Substring(0, i);
            yield return new WaitForSeconds(Speed);
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
