using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private GameSaveSystem saveSystem;
    
    void Start()
    {
        saveSystem = FindObjectOfType<GameSaveSystem>();
        
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnExitClick);
        }
    }
    
    public void OnExitClick()
    {
        if (saveSystem != null)
        {
            saveSystem.SaveAndExit();
        }
        else
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}