using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogFixed : MonoBehaviour
{
    public GameObject windowDialog;
    public TextMeshProUGUI textDialog;
    [TextArea(3, 5)] public string[] message;
    public Button nextButton;
    
    private int currentIndex = 0;
    private bool isInDialog = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isInDialog)
        {
            StartDialog();
        }
    }

    private void StartDialog()
    {
        isInDialog = true;
        currentIndex = 0;
        windowDialog.SetActive(true);
        
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextDialog);
        
        ShowCurrentMessage();
    }

    private void ShowCurrentMessage()
    {
        if (currentIndex < message.Length)
        {
            textDialog.text = message[currentIndex];
            
            nextButton.gameObject.SetActive(currentIndex < message.Length - 1);
        }
        else
        {
            EndDialog();
        }
    }

    public void NextDialog()
    {
        currentIndex++;
        
        if (currentIndex < message.Length)
        {
            ShowCurrentMessage();
        }
        else
        {
            EndDialog();
        }
    }

    private void EndDialog()
    {
        windowDialog.SetActive(false);
        isInDialog = false;
        nextButton.onClick.RemoveAllListeners();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInDialog)
        {
            EndDialog();
        }
    }
}