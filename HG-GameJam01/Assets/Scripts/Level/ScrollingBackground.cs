using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float Speed = 2;
    public Renderer Renderer;

    void Update()
    {
        Renderer.material.mainTextureOffset += Speed * Time.deltaTime * Vector2.right;
    }
}
