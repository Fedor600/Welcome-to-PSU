using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private int current_char;

    [SerializeField] GameObject[] characters;

    public void ButtonNext()
    {

        if (current_char == characters.Length - 1)
        {

            return;
        }

        characters[current_char].SetActive(false);
   
        current_char++;

        characters[current_char].SetActive(true);

        PlayerPrefs.SetInt("SelectedCharacter", current_char);


    }

    public void ButtonPrev()
    {

        if (current_char == 0)
        {
            return;
        }

        characters[current_char].SetActive(false);

        current_char--;

        characters[current_char].SetActive(true);

        PlayerPrefs.SetInt("SelectedCharacter", current_char);
    }
}
