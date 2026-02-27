using UnityEngine;
using AGDDPlatformer;

public class ButtonController : MonoBehaviour
{
    public enum ActivationMode { Player1, Player2, Both }

    [Header("Sprites")]
    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;

    [Header("Activation")]
    [SerializeField] private ActivationMode activationMode = ActivationMode.Both;

    [Tooltip("If true, pressing this button reverses gravity for all KinematicObjects.")]
    [SerializeField] private bool reversesGravity = false;

    [Tooltip("If true, the button acts as a toggle switch: entering activates it, entering again deactivates it.")]
    [SerializeField] private bool isSwitch = false;

    [Header("Targets")]
    [Tooltip("The GameObjects to notify when this button is pressed/released (e.g. doors, sawblades, spawners, lazers).")]
    [SerializeField] private GameObject[] targetObjects;

    private SpriteRenderer spriteRenderer;
    private bool isPlayer1Pressing;
    private bool isPlayer2Pressing;
    private bool isActivated;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetSprite(unpressedSprite);
    }

    // --- Trigger Detection ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isSwitch)
        {
            if (IsRelevantPlayer(other))
                ToggleSwitch();
        }
        else
        {
            RegisterPlayerEnter(other);
            UpdateSprite();
            TryActivate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isSwitch) return;

        RegisterPlayerExit(other);
        UpdateSprite();
        TryDeactivate();
    }

    // --- Player Tracking ---

    private void RegisterPlayerEnter(Collider2D other)
    {
        if (IsPlayer1(other)) isPlayer1Pressing = true;
        if (IsPlayer2(other)) isPlayer2Pressing = true;
    }

    private void RegisterPlayerExit(Collider2D other)
    {
        if (IsPlayer1(other)) isPlayer1Pressing = false;
        if (IsPlayer2(other)) isPlayer2Pressing = false;
    }

    private bool IsPlayer1(Collider2D other) => other.CompareTag("Player1");
    private bool IsPlayer2(Collider2D other) => other.CompareTag("Player2");

    private bool IsRelevantPlayer(Collider2D other)
    {
        return activationMode switch
        {
            ActivationMode.Player1 => IsPlayer1(other),
            ActivationMode.Player2 => IsPlayer2(other),
            ActivationMode.Both    => IsPlayer1(other) || IsPlayer2(other),
            _ => false
        };
    }

    // --- Switch Toggle ---

    private void ToggleSwitch()
    {
        if (isActivated)
        {
            isActivated = false;
            SetSprite(unpressedSprite);
            NotifyTarget("OnButtonReleased");
        }
        else
        {
            isActivated = true;
            SetSprite(pressedSprite);
            NotifyTarget("OnButtonPressed");

            if (reversesGravity)
                ReverseGravity();
        }
    }

    // --- Sprite ---

    private void UpdateSprite()
    {
        SetSprite(IsAnyPlayerPressing() ? pressedSprite : unpressedSprite);
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    // --- Activation Logic ---

    private bool IsAnyPlayerPressing() => isPlayer1Pressing || isPlayer2Pressing;

    private bool IsActivationConditionMet()
    {
        return activationMode switch
        {
            ActivationMode.Player1 => isPlayer1Pressing,
            ActivationMode.Player2 => isPlayer2Pressing,
            ActivationMode.Both    => isPlayer1Pressing || isPlayer2Pressing,
            _ => false
        };
    }

    private void TryActivate()
    {
        if (isActivated || !IsActivationConditionMet()) return;

        isActivated = true;
        NotifyTarget("OnButtonPressed");

        if (reversesGravity)
            ReverseGravity();
    }

    private void TryDeactivate()
    {
        if (!isActivated || IsActivationConditionMet()) return;

        isActivated = false;
        NotifyTarget("OnButtonReleased");
    }

    // --- Target Notification ---

    private void NotifyTarget(string methodName)
    {
        if (targetObjects == null) return;

        foreach (var target in targetObjects)
        {
            if (target != null)
                target.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }
    }

    // --- Gravity Reversal ---

    private void ReverseGravity()
    {
        GameManager.instance.ReverseAllGravity();
    }
}
