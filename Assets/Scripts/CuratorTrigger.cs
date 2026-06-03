using UnityEngine;

public class CuratorTrigger : MonoBehaviour
{
    public string buildingName;
    [TextArea(3, 8)]
    public string dialogueText;

    public DialogueUI dialogueUI;
    public ExcursionManager excursionManager;

    private bool playerInside;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            dialogueUI.ShowDialogue(buildingName, dialogueText, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            dialogueUI.ShowInteractionHint("Нажмите E, чтобы поговорить с куратором");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            dialogueUI.HideInteractionHint();
        }
    }

    public void CompleteDialogue()
    {
        excursionManager.CompletePoint(buildingName);
    }
}