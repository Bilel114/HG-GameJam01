using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameTimer GameTimer;
    public CanvasGroup GameOverScreen;

    private void Awake()
    {
        GameTimer = GetComponent<GameTimer>();
    }

    public void RetryLevel ()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        while (GameOverScreen.alpha < 1)
        {
            GameOverScreen.alpha += Time.deltaTime;
            yield return null;
        }
    }
}
