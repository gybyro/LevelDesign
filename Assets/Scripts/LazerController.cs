using UnityEngine;
using AGDDPlatformer;

public class LazerController : MonoBehaviour
{
    private bool isActive = true;
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void OnButtonPressed()
    {
        Deactivate();
    }

    public void OnButtonReleased()
    {
        Activate();
    }

    private void Activate()
    {
        isActive = true;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    private void Deactivate()
    {
        isActive = false;
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    // --- Player Collision ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;
        if (!IsPlayer(collision.collider)) return;

        KillPlayer();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isActive) return;
        if (!IsPlayer(collision.collider)) return;

        KillPlayer();
    }

    private bool IsPlayer(Collider2D col)
    {
        return col.CompareTag("Player1") || col.CompareTag("Player2");
    }

    private void KillPlayer()
    {
        GameManager.instance.OnPlayerDeath();
    }
}
