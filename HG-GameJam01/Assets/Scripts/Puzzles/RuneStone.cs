using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStone : MonoBehaviour
{
    public Transform RuneStoneSymbol;
    public SpriteRenderer SpriteRenderer;
    public Color RuneOnColor;
    public bool IsRuneOn;
    public int Id;
    public RuneStonePuzzle Puzzle;

    private void Awake()
    {
        SpriteRenderer = RuneStoneSymbol.GetComponent<SpriteRenderer>();
        SpriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsRuneOn && other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsRuneOn && other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.enabled = false;
        }
    }

    public void ActivateRuneStone()
    {
        SpriteRenderer.color = RuneOnColor;
        IsRuneOn = true;
        Puzzle.OnRuneStoneActivated(Id);
    }
}
