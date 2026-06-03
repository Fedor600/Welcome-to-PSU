using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject interactionHint;

    private CuratorTrigger currentCurator;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        interactionHint.SetActive(false);
    }

    public void ShowDialogue(string buildingName, string text, CuratorTrigger curator)
    {
        currentCurator = curator;

        buildingNameText.text = buildingName;
        dialogueText.text = text;

        dialoguePanel.SetActive(true);
        interactionHint.SetActive(false);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);

        if (currentCurator != null)
        {
            currentCurator.CompleteDialogue();
            currentCurator = null;
        }
    }

    public void ShowInteractionHint(string text)
    {
        interactionHint.SetActive(true);
        interactionHint.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void HideInteractionHint()
    {
        interactionHint.SetActive(false);
    }
}