using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStonePuzzle : MonoBehaviour
{
    public LevelManager LevelManager;
    public RuneStone[] RuneStones = new RuneStone[4];
    public RuneOrb[] RuneOrbs = new RuneOrb[4];
    public int NumRuneStonesActivated;

    private void Awake()
    {
        for (int i = 0; i < RuneStones.Length; i++)
        {
            RuneStones[i].Id = i;
            RuneOrbs[i].Id = i;
            RuneStones[i].Puzzle = this;
            RuneOrbs[i].Puzzle = this;
        }
    }

    public void OnRuneStoneActivated (int id)
    {
        RuneOrbs[id].OnSymbolUsed();
        if (++NumRuneStonesActivated == RuneStones.Length)
        {
            PuzzleWon();
        }
    }

    public void PuzzleWon()
    {
        LevelManager.SecondFightEnd();
    }
}
