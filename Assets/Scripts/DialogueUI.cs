using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Окно диалога")]
    [SerializeField] private GameObject dialoguePanel;

    [Header("Текст диалога")]
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Подсказка взаимодействия")]
    [SerializeField] private GameObject interactionHint;
    [SerializeField] private TextMeshProUGUI interactionHintText;

    private CuratorTrigger currentCurator;
    private bool isDialogueOpen;

    private void Start()
    {
        ClosePanelOnly();

        if (interactionHint != null)
        {
            interactionHint.SetActive(false);
        }
    }

    public void ShowDialogue(string buildingName, string text, CuratorTrigger curator)
    {
        if (isDialogueOpen)
            return;

        currentCurator = curator;
        isDialogueOpen = true;

        if (buildingNameText != null)
        {
            buildingNameText.text = buildingName;
        }

        if (dialogueText != null)
        {
            dialogueText.text = text;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        HideInteractionHint();
    }

    public void CloseDialogue()
    {
        if (!isDialogueOpen)
            return;

        ClosePanelOnly();

        if (currentCurator != null)
        {
            currentCurator.CompleteDialogue();
            currentCurator = null;
        }
    }

    public void ShowInteractionHint(string text)
    {
        if (interactionHint != null)
        {
            interactionHint.SetActive(true);
        }

        if (interactionHintText != null)
        {
            interactionHintText.text = text;
        }
    }

    public void HideInteractionHint()
    {
        if (interactionHint != null)
        {
            interactionHint.SetActive(false);
        }
    }

    private void ClosePanelOnly()
    {
        isDialogueOpen = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isDialogueOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDialogue();
        }
    }
}