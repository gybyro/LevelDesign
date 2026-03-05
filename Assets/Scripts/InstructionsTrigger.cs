using UnityEngine;

public class InstructionsTrigger : MonoBehaviour
{

    [SerializeField] private Animator instructionsAnimator;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        instructionsAnimator.SetBool("apyr", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        instructionsAnimator.SetBool("apyr", false);
    }

    private bool IsPlayer(Collider2D col)
    {
        return col.CompareTag("Player1") || col.CompareTag("Player2");
    }
}