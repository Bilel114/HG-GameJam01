using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public CanvasGroup TitleScreen;
    public Image TitleImage;
    public float TitleSpeed, TitleAlphaSpeed, TitleScreenFadeSpeed;
    public GameObject TitleScreenText;
    public Image WhiteScreen;
    public GameTimer GameTimer;
    public CanvasGroup GameOverScreen;
    public CarBossStateMachine Boss;
    public PlayerCharacter Player;
    public Transform Room1GatePoint, Room2StartPoint, Room2Center;
    public Transform[] Room2PatrolPoints = new Transform[2];
    public Room1IronGates Room1IronGates;
    public RuneStonePuzzle RuneStonePuzzle;
    public SymbolsPuzzle SymbolsPuzzle;
    public float SymbolSpeed = 1;
    public GameObject CameraRoom2Center;
    public StartingText StartingText;

    private void Awake()
    {
        GameTimer = GetComponent<GameTimer>();
    }

    private void Start()
    {
        StartCoroutine(TitleScreenCoroutine());
    }

    IEnumerator TitleScreenCoroutine()
    {
        while (TitleImage.rectTransform.localPosition.y < 0)
        {
            TitleImage.rectTransform.localPosition += TitleSpeed * Time.deltaTime * Vector3.up;
            TitleImage.color = new Color(1, 1, 1, TitleImage.color.a + TitleAlphaSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        TitleScreenText.SetActive(true);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        while (TitleScreen.alpha > 0)
        {
            TitleScreen.alpha -= TitleScreenFadeSpeed * Time.deltaTime;
            yield return null;
        }

        StartingText.StartTextScroll();
        Player.StateMachine.SwitchToFrozenState = false;
    }

    public void RetryLevel ()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver(bool isGoodEnding)
    {
        if (isGoodEnding)
        {
            WhiteScreen.gameObject.SetActive(true); 
        }
        else
        {
            Player.StateMachine.SwitchToFrozenState = true;
            Boss.SwitchToFrozenState = true;
            GameOverScreen.gameObject.SetActive(true);
        }
        StartCoroutine(FadeInCoroutine(isGoodEnding));
    }

    IEnumerator FadeInCoroutine(bool isGoodEnding)
    {
        if (isGoodEnding)
        {
            while (WhiteScreen.color.a < 1)
            {
                WhiteScreen.color = new Color(1, 1, 1, WhiteScreen.color.a + TitleAlphaSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(1);
        }
        else
        {
            while (GameOverScreen.alpha < 1)
            {
                GameOverScreen.alpha += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(2);
        }
    }

    public void StartFirstFight()
    {
        Invoke(nameof(UnfreezeBoss), 3);
        GameTimer.StartTimer();
    }

    void UnfreezeBoss()
    {
        Boss.SwitchToFrozenState = false;
        Boss.IsSecondFight = false;
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

        while (Vector2.Distance(Boss.transform.position, Room1GatePoint.position) > 0.01f)
        {
            Boss.transform.position = Vector2.MoveTowards(Boss.transform.position, Room1GatePoint.position, Boss.MoveSpeed * Time.deltaTime);
            yield return null;
        }

        // animate symbols
        for (int i = 0; i < SymbolsPuzzle.SymbolSequenceSprites.Length; i++)
        {
            SymbolsPuzzle.SymbolSequenceSprites[i].sortingOrder = 11;
        }

        while (Vector2.Distance(Boss.transform.position + 0.64f * Vector3.up, SymbolsPuzzle.SymbolSequenceSprites[0].transform.position) > 0.01f)
        {
            for (int i = 0; i < SymbolsPuzzle.SymbolSequenceSprites.Length; i++)
            {
                SymbolsPuzzle.SymbolSequenceSprites[i].transform.position = Vector2.MoveTowards(SymbolsPuzzle.SymbolSequenceSprites[i].transform.position,
                    Boss.transform.position + 0.64f * Vector3.up, SymbolSpeed * Time.deltaTime);
            }
            yield return null;
        }

        for (int i = 0; i < SymbolsPuzzle.SymbolSequenceSprites.Length; i++)
        {
            SymbolsPuzzle.SymbolSequenceSprites[i].gameObject.SetActive(false);
        }

        Boss.SpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        Boss.Animator.Play(AnimatorHash.Boss_Attack1Anticipation);
        Boss.SpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);

        Player.StateMachine.SwitchToFrozenState = false;
        GameTimer.EnableTimer = true;
        Boss.Animator.Play(AnimatorHash.Boss_Attack1Charge);

        while (Vector2.Distance(Boss.transform.position, Room2StartPoint.position) > 0.01f)
        {
            Boss.transform.position = Vector2.MoveTowards(Boss.transform.position, Room2StartPoint.position, Boss.RamAttackSpeed * Time.deltaTime);
            yield return null;
        }
        
        Boss.PatrolPoints = Room2PatrolPoints;
        Boss.SwitchToFrozenState = false;
        Boss.IsSecondFight = true;
    }

    public void SecondFightEnd ()
    {
        StartCoroutine(SecondFightEndCoroutine());
    }

    IEnumerator SecondFightEndCoroutine ()
    {
        Player.StateMachine.SwitchToFrozenState = true;
        Boss.SwitchToFrozenState = true;
        GameTimer.EnableTimer = false;
        CameraRoom2Center.SetActive(true);

        while (Vector2.Distance(Boss.transform.position, Room2Center.position) > 0.01f)
        {
            Boss.transform.position = Vector2.MoveTowards(Boss.transform.position, Room2Center.position, Boss.MoveSpeed * Time.deltaTime);
            yield return null;
        }
        Boss.Animator.Play(AnimatorHash.Boss_Attack1Anticipation);

        for (int i = 0; i < RuneStonePuzzle.RuneStones.Length; i++)
        {
            RuneStonePuzzle.RuneStones[i].SpriteRenderer.sortingOrder = 11;
        }

        while (Vector2.Distance(Boss.transform.position + 0.64f * Vector3.up, RuneStonePuzzle.RuneStones[0].RuneStoneSymbol.transform.position) > 0.01f)
        {
            for (int i = 0; i < RuneStonePuzzle.RuneStones.Length; i++)
            {
                RuneStonePuzzle.RuneStones[i].RuneStoneSymbol.transform.position = Vector2.MoveTowards(RuneStonePuzzle.RuneStones[i].RuneStoneSymbol.transform.position, 
                    Boss.transform.position + 0.64f * Vector3.up, SymbolSpeed * Time.deltaTime);
            }
            yield return null;
        }

        for (int i = 0; i < RuneStonePuzzle.RuneStones.Length; i++)
        {
            RuneStonePuzzle.RuneStones[i].RuneStoneSymbol.gameObject.SetActive(false);
        }

        Boss.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);

        while (Boss.transform.localScale.x > 0.05f)
        {
            Boss.transform.localScale *= 0.9f;
            yield return null;
        }
        Boss.gameObject.SetActive(false);

        GameOver(true);
    }
}
