using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    public void Update()
    {

        
    }
    
    public void PlayButton()
    {

        SceneManager.LoadScene("select");
    }

    public void ExitButon()
    {

        Application.Quit();

    }
}
