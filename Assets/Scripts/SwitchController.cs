using UnityEngine;
using AGDDPlatformer;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private GameObject[] targetObjects;

    [Header("Sprites")]
    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer extra;
    [SerializeField] private Sprite extra1; // same as default
    [SerializeField] private Sprite extra2;

    [SerializeField] private SwitchType aType;
    public enum SwitchType
    {
        DoorTrigger,
        DisableTrigger
    }

    [SerializeField] private Animator instructionsAnimator;
    [SerializeField] private bool doInstructions = false;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;
    private bool isPlayerIn = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetSprite(unpressedSprite, extra1);
    }

    private void Update()
    {
        if (Input.GetButtonDown("E"))
        {
            if (!isPlayerIn) return;

            SwitchState();
            if (doInstructions) {
                instructionsAnimator.SetTrigger("goaway");
                doInstructions = false;
                instructionsAnimator.SetBool("apyr", false);
            }
        }
    }

    private void SetSprite(Sprite sprite, Sprite extraSprite)
    {
        spriteRenderer.sprite = sprite;
        extra.sprite = extraSprite;
    }

    public void SwitchState()
    {
        
        if (!isActive)
        {
            isActive = !isActive;
            SetSprite(pressedSprite, extra2);
            if (aType == SwitchType.DisableTrigger) DisableObjects();
            else if (aType == SwitchType.DoorTrigger) NotifyTarget("OnButtonPressed");
        }
        else if (isActive)
        {
            isActive = !isActive;
            SetSprite(unpressedSprite, extra1);
            if (aType == SwitchType.DisableTrigger) EnableObjects();
            else if (aType == SwitchType.DoorTrigger) NotifyTarget("OnButtonReleased");
        }
 
    }

    private void DisableObjects()
    {
        if (targetObjects == null) return;

        foreach (var target in targetObjects)
        {
            if (target != null)
                target.SetActive(false);
        }
    }
    private void EnableObjects()
    {
        if (targetObjects == null) return;

        foreach (var target in targetObjects)
        {
            if (target != null)
                target.SetActive(true);
        }
    }

    private void NotifyTarget(string methodName)
    {
        if (targetObjects == null) return;

        foreach (var target in targetObjects)
        {
            if (target != null)
                target.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {  
        Debug.Log("enter" + doInstructions);
        if (!IsPlayer(collision)) return;
        if (doInstructions) instructionsAnimator.SetBool("apyr", true);
        isPlayerIn = true;
    }

    private void OnTriggerStay2D(Collider2D  collision)
    {
        if (!IsPlayer(collision)) return;
        isPlayerIn = true;
    }
     private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit" + doInstructions);
        // if (IsPlayer(collision)) return;
      
        if (doInstructions) instructionsAnimator.SetBool("apyr", false);
        isPlayerIn = false;
    }

    private bool IsPlayer(Collider2D col)
    {
        return col.CompareTag("Player1") || col.CompareTag("Player2");
    }


    public void ResetState()
    {
        isActive = true;

        if (aType == SwitchType.DisableTrigger) SwitchState();
        else if (aType == SwitchType.DoorTrigger) NotifyTarget("OnButtonReleased");
    }
}
