using System.Collections;
using UnityEngine;
using TMPro;

public class StartingText : MonoBehaviour
{
    [TextArea] public string TextString;
    public TextMeshPro TextComponent;
    public float Speed = 0.02f;

    public void StartTextScroll ()
    {
        StartCoroutine(ScrollTextCoroutine());
    }

    IEnumerator ScrollTextCoroutine()
    {
        for (int i = 0; i < TextString.Length + 1; i++)
        {
            TextComponent.text = TextString.Substring(0, i);
            yield return new WaitForSeconds(Speed);
        }
    }
}
