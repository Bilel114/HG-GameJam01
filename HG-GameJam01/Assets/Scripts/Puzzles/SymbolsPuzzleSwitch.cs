using UnityEngine;

public class SymbolsPuzzleSwitch : MonoBehaviour
{
    public int Id;
    public SymbolsPuzzle SymbolsPuzzle;
    public SpriteRenderer SpriteRenderer;
    public Sprite ButtonUpSprite, ButtonDownSprite;


    private void Awake ()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.sprite = ButtonDownSprite;
            // play button press sound?
            if (!SymbolsPuzzle.IsPuzzleWon)
            {
                SymbolsPuzzle.OnSwitchPressed(Id); 
            }
        }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.layer == PhysicsLayerIds.PlayerLayer)
        {
            SpriteRenderer.sprite = ButtonUpSprite;
            // play button release sound?
        }
    }
}
