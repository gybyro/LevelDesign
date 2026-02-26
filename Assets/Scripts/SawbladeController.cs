using UnityEngine;
using AGDDPlatformer;

public class SawbladeController : MonoBehaviour
{
    [Header("Sawblade Settings")]
    [Tooltip("Degrees per second the sawblade rotates.")]
    [SerializeField] private float spinSpeed = 360f;

    private bool isActive = true;

    private void Update()
    {
        if (isActive)
            Spin();
    }

    // --- Spinning ---

    private void Spin()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

    // --- Button Callbacks ---

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
    }

    private void Deactivate()
    {
        isActive = false;
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
