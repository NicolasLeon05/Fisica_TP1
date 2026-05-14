using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public OBB OBB
    {
        get
        {
            Vector2 size = spriteRenderer.sprite.bounds.size;

            size.x *= transform.lossyScale.x;
            size.y *= transform.lossyScale.y;

            return new OBB(
                transform.position,
                size,
                transform.eulerAngles.z);
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}