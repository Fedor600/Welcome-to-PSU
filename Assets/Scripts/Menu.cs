using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    
    public void PlayButton()
    {

        SceneManager.LoadScene("select");
    }

    public void ExitButon()
    {

        Debug.Log("Выход");


        Application.Quit();

    }
}
