using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStone : MonoBehaviour
{
    public Transform RuneStoneSymbol;
    public SpriteRenderer SpriteRenderer;
    public Color RuneOnColor, RuneOffColor;
    public bool IsRuneOn;
    public int Id;
    public RuneStonePuzzle Puzzle;

    private void Awake()
    {
        SpriteRenderer = RuneStoneSymbol.GetComponent<SpriteRenderer>();
        RuneOffColor = SpriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsRuneOn && other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.color = Color.white;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsRuneOn && other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.color = RuneOffColor;
        }
    }

    public void ActivateRuneStone()
    {
        SpriteRenderer.color = RuneOnColor;
        IsRuneOn = true;
        Puzzle.OnRuneStoneActivated(Id);
    }
}
