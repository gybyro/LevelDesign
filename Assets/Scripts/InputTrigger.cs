using UnityEngine;
using AGDDPlatformer;

public class InputTrigger : MonoBehaviour
{

    // [Header("Sawblade Settings")]
    // [SerializeField] private float spinSpeed = 360f;

    public bool isActive = false;

    private void Update()
    {
 
    }

    // --- Player Collision ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;
        if (!IsPlayer(collision.collider)) return;

        if (Input.GetButtonDown("Space"))
        {
            isActive = !isActive;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isActive) return;
        if (!IsPlayer(collision.collider)) return;
        if (Input.GetButtonDown("Space"))
        {
            isActive = !isActive;
        }
    }

    private bool IsPlayer(Collider2D col)
    {
        return col.CompareTag("Player1") || col.CompareTag("Player2");
    }
}