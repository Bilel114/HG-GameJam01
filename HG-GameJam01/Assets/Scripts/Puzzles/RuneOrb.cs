using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneOrb : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Color SymbolOffColor, SymbolOnColor;
    public bool IsSymbolUsed;
    public int Id;
    public RuneStonePuzzle Puzzle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsSymbolUsed && other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.color = Color.white;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsSymbolUsed && other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.color = SymbolOffColor;
        }
    }

    public void OnSymbolUsed ()
    {
        IsSymbolUsed = true;
        SpriteRenderer.color = SymbolOnColor;
    }
}
