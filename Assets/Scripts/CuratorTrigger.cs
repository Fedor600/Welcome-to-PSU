using UnityEngine;

public class CuratorTrigger : MonoBehaviour
{
    [Header("Информация о корпусе")]
    public string buildingName;

    [TextArea(3, 8)]
    public string dialogueText;

    [Header("Ссылки")]
    public DialogueUI dialogueUI;
    public ExcursionManager excursionManager;

    private bool playerInside;

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueUI != null)
            {
                dialogueUI.ShowDialogue(buildingName, dialogueText, this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (dialogueUI != null)
        {
            dialogueUI.ShowInteractionHint("Нажмите E, чтобы поговорить с куратором");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (dialogueUI != null)
        {
            dialogueUI.HideInteractionHint();
            dialogueUI.CloseDialogue();
        }
    }

    public void CompleteDialogue()
    {
        if (excursionManager != null)
        {
            excursionManager.CompletePoint(buildingName);
        }
    }
}