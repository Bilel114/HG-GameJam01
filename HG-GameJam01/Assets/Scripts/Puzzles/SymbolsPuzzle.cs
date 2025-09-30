using System.Collections.Generic;
using UnityEngine;

public class SymbolsPuzzle : MonoBehaviour
{
    public LevelManager LevelManager;
    public bool IsPuzzleWon;
    public int CurrentSequenceIndex = 0;
    public SymbolsPuzzleSwitch[] Switches = new SymbolsPuzzleSwitch[6];
    int[] SwitchSequence = new int[] { 0, 1, 3, 0, 2, 3};
    public SpriteRenderer[] SymbolSequenceSprites = new SpriteRenderer[6];
    public Color SymbolOffColor, SymbolOnColor;
    public AudioSource AudioSource;
    public AudioClip WrongSymbolSound, SwitchPressedSound;

    private void Awake ()
    {
        AudioSource = GetComponent<AudioSource>();
        for (int i = 0; i < Switches.Length; i++)
        {
            Switches[i].Id = i;
            Switches[i].SymbolsPuzzle = this;
        }
    }

    public void OnSwitchPressed (int switchId)
    {
        if (switchId == SwitchSequence[CurrentSequenceIndex])
        {
            SymbolSequenceSprites[CurrentSequenceIndex++].color = SymbolOnColor;
            
            AudioSource.PlayOneShot(SwitchPressedSound);

            if (CurrentSequenceIndex == SwitchSequence.Length)
            {
                PuzzleWon();
            }
        }
        else
        {
            CurrentSequenceIndex = 0;
            for (int i = 0; i < SymbolSequenceSprites.Length; i++)
            {
                SymbolSequenceSprites[i].color = SymbolOffColor;
            }
            AudioSource.PlayOneShot(WrongSymbolSound);
        }
    }

    public void PuzzleWon ()
    {
        IsPuzzleWon = true;
        LevelManager.TransitionToSecondFight();
    }
}
