using System.Collections.Generic;
using UnityEngine;

public class SymbolsPuzzle : MonoBehaviour
{
    public SymbolsPuzzleSwitch[] Switches = new SymbolsPuzzleSwitch[4];
    public List<int> SwitchSequence = new List<int>() { 0, 1, 3, 0, 2, 3};
    public int CurrentSequenceIndex = 0;

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
        if (switchId == SwitchSequence[CurrentSequenceIndex])
        {
            CurrentSequenceIndex++;
            // light symbol sprite?
            // play correct sound?

            if (CurrentSequenceIndex == SwitchSequence.Count)
            {
                PuzzleWon();
            }
        }
        else
        {
            CurrentSequenceIndex = 0;
            // play mistake sound?
        }
    }

    public void PuzzleWon ()
    {
        // damage boss
    }
}
