using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float MaxTimer = 60;
    [SerializeField] float _timer;
    private float _timerRatio;
    public float TimerRate = 1;
    public bool EnableTimer = false;
    public Image TimerUIMask;
    public Image TimerUISprite;
    public Animator HourglassAnimator;
    public Color[] TimerColors = new Color[4];
    public bool TimeEnded;

    private void Update ()
    {
        if (EnableTimer)
        {
            if (_timer > 0)
            {
                _timer -= TimerRate * Time.deltaTime;
                _timerRatio = _timer / MaxTimer;
                TimerUIMask.fillAmount = _timerRatio;
                if (_timerRatio > 0.75f)
                {
                    TimerUISprite.color = TimerColors[0];
                }
                else if (_timerRatio > 0.5f && _timerRatio <= 0.75f)
                {
                    TimerUISprite.color = TimerColors[1];
                }
                else if (_timerRatio > 0.25f && _timerRatio <= 0.5f)
                {
                    TimerUISprite.color = TimerColors[2];
                }
                else
                {
                    TimerUISprite.color = TimerColors[3];
                } 
            }
            else
            {
                if (!TimeEnded)
                {
                    TimeEnded = true;
                    OnTimeEnded();
                }
            }
        }
    }

    public void StartTimer ()
    {
        EnableTimer = true;
        _timer = MaxTimer;
        HourglassAnimator.enabled = true;
    }

    public void DecreaseTimer (float time)
    {
        _timer -= time;
    }

    public void OnTimeEnded ()
    {
        Debug.Log("Game Over!");
        GetComponent<LevelManager>().GameOver();
    }
}
