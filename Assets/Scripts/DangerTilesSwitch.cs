using UnityEngine;
using AGDDPlatformer;

public class DangerTilesSwitch : MonoBehaviour
{
    public GameObject tilesheet;

    [Header("Sprites")]
    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;

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
        SetSprite(unpressedSprite);
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

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SwitchState()
    {
        if (!isActive)
        {
            isActive = !isActive;
            SetSprite(pressedSprite);
            tilesheet.SetActive(false);
        }
        else if (isActive)
        {
            isActive = !isActive;
            SetSprite(unpressedSprite);
            tilesheet.SetActive(true);
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
}
