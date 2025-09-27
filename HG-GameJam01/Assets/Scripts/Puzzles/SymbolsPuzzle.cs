using System.Collections.Generic;
using UnityEngine;

public class SymbolsPuzzle : MonoBehaviour
{
    public bool IsPuzzleWon;
    public int CurrentSequenceIndex = 0;
    public SymbolsPuzzleSwitch[] Switches = new SymbolsPuzzleSwitch[6];
    public int[] SwitchSequence = new int[] { 0, 1, 3, 0, 2, 3};
    public SpriteRenderer[] SymbolSequenceSprites = new SpriteRenderer[6];
    public Color SymbolOffColor, SymbolOnColor;

    private void Awake ()
    {
        for (int i = 0; i < Switches.Length; i++)
        {
            Switches[i].Id = i;
            Switches[i].SymbolsPuzzle = this;
        }
    }

    public void OnSwitchPressed (int switchId)
    {
        Debug.Log($"Switch Pressed ({switchId})");
        if (switchId == SwitchSequence[CurrentSequenceIndex])
        {
            SymbolSequenceSprites[CurrentSequenceIndex++].color = SymbolOnColor;
            // play correct sound?

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
            // play mistake sound?
        }
    }

    public void PuzzleWon ()
    {
        // damage boss
        Debug.Log("Symbols Puzzle Won!");
        IsPuzzleWon = true;
    }
}
