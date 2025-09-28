using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameTimer GameTimer;
    public CanvasGroup GameOverScreen;
    public CarBossStateMachine Boss;
    public PlayerCharacter Player;
    public Transform Room1GatePoint, Room2StartPoint;
    public Transform[] Room2PatrolPoints = new Transform[2];
    public Room1IronGates Room1IronGates;

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

    public void StartFirstFight()
    {
        Boss.SwitchToFrozenState = false;
        GameTimer.StartTimer();
    }

    public void TransitionToSecondFight ()
    {
        StartCoroutine(TransitionToSecondFightCoroutine());
    }

    IEnumerator TransitionToSecondFightCoroutine()
    {
        Player.StateMachine.SwitchToFrozenState = true;
        Boss.SwitchToFrozenState = true;
        GameTimer.EnableTimer = false;
        Room1IronGates.OpenGates();

        while (Vector2.Distance(Boss.transform.position, Room1GatePoint.position) > 0.1f)
        {
            Boss.transform.position = Vector2.MoveTowards(Boss.transform.position, Room1GatePoint.position, Boss.MoveSpeed * Time.deltaTime);
            yield return null;
        }

        // animate symbols?
        yield return new WaitForSeconds(1);

        while (Vector2.Distance(Boss.transform.position, Room2StartPoint.position) > 0.1f)
        {
            Boss.transform.position = Vector2.MoveTowards(Boss.transform.position, Room2StartPoint.position, Boss.RamAttackSpeed * Time.deltaTime);
            yield return null;
        }

        Player.StateMachine.SwitchToFrozenState = false;
        Boss.PatrolPoints = Room2PatrolPoints;
        Boss.SwitchToFrozenState = false;
        GameTimer.EnableTimer = true;
    }
}
