using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [Tooltip("How fast the door moves. Higher = snappier.")]
    [SerializeField] private float openSpeed = 8f;

    [Tooltip("How far the door slides when opened.")]
    [SerializeField] private float openDistance = 3f;

    [Tooltip("If true, the door opens downward instead of upward.")]
    [SerializeField] private bool openDownward = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPosition;

    private bool wantsToClose;
    private Vector2 doorSize;

    private void Awake()
    {
        CachePositions();
        CacheDoorSize();
        targetPosition = closedPosition;
    }

    private void Update()
    {
        TryClose();
        MoveTowardsTarget();
    }

    // --- Button Callbacks ---

    public void OnButtonPressed()
    {
        wantsToClose = false;
        SetTarget(openPosition);
    }

    public void OnButtonReleased()
    {
        wantsToClose = true;
    }

    // --- Movement ---

    private void CachePositions()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + GetOpenDirection() * openDistance;
    }

    private void CacheDoorSize()
    {
        Collider2D col = GetComponent<Collider2D>();
        doorSize = col != null ? col.bounds.size : (Vector2)transform.localScale;
    }

    private Vector3 GetOpenDirection()
    {
        return openDownward ? Vector3.down : Vector3.up;
    }

    // --- Close Safety ---

    private void TryClose()
    {
        if (!wantsToClose) return;
        if (IsPlayerBlocking()) return;

        wantsToClose = false;
        SetTarget(closedPosition);
    }

    private bool IsPlayerBlocking()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(closedPosition, doorSize, 0f);

        foreach (Collider2D hit in hits)
        {
            if (IsPlayer(hit)) return true;
        }

        return false;
    }

    private bool IsPlayer(Collider2D col)
    {
        return col.CompareTag("Player1") || col.CompareTag("Player2");
    }

    private void SetTarget(Vector3 newTarget)
    {
        targetPosition = newTarget;
    }

    private void MoveTowardsTarget()
    {
        if (HasReachedTarget()) return;

        transform.position = SmoothStep(transform.position, targetPosition);

        if (HasReachedTarget())
            SnapToTarget();
    }

    private Vector3 SmoothStep(Vector3 current, Vector3 target)
    {
        return Vector3.Lerp(current, target, openSpeed * Time.deltaTime);
    }

    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.001f;
    }

    private void SnapToTarget()
    {
        transform.position = targetPosition;
    }

    // --- Editor Visualization ---

    private void OnDrawGizmosSelected()
    {
        Vector3 gizmoClosed = Application.isPlaying ? closedPosition : transform.position;
        Vector3 gizmoOpen = gizmoClosed + GetOpenDirection() * openDistance;

        DrawOpenPositionGizmo(gizmoOpen);
        DrawPathGizmo(gizmoClosed, gizmoOpen);
    }

    private void DrawOpenPositionGizmo(Vector3 position)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(position, transform.localScale);
    }

    private void DrawPathGizmo(Vector3 from, Vector3 to)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(from, to);
    }
}
