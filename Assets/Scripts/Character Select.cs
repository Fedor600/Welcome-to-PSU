using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterSelect : MonoBehaviour
{
    private int current_char;

    private GameObject[] characters;

    private int index;

    private void Start()
    {

        index = PlayerPrefs.GetInt("SelectPlayer");

        characters = new GameObject[transform.childCount];

        for (int  i = 0; i < transform.childCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;

        }

        foreach (GameObject go in characters)
        {
            go.SetActive(false);
        }

        if (characters[index])
        {
            characters[index].SetActive(true);
        }
    }

    public void ButtonNext()
    {

        characters[index].SetActive(false);

        index++;

        if (index == characters.Length)
        {
            index = 0;
        }
        characters[index].SetActive(true);

    }

    public void ButtonPrev()
    {

        characters[index].SetActive(false);

        index--;

        if (index < 0)
        {
            index = characters.Length - 1;
        }
        characters[index].SetActive(true);
        
    }


    public void ButtonPlay()
    {

        PlayerPrefs.SetInt("SelectPlayer", index);

        SceneManager.LoadScene("Testing");


    }

    public void ButtonExit()
    {

        Application.Quit();

        Debug.Log("Вы вышли из игры");
    }
}
