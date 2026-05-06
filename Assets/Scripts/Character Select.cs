using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    private GameObject[] characters;
    private int index;

    private void Start()
    {
        // 1. Собираем детей только один раз
        int childCount = transform.childCount;
        characters = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;
        }

        // 2. Безопасно читаем PlayerPrefs и ограничиваем индекс
        int savedIndex = PlayerPrefs.GetInt("SelectPlayer", 0);
        index = Mathf.Clamp(savedIndex, 0, characters.Length - 1);

        // 3. Выключаем всех, включаем выбранного
        foreach (var go in characters)
        {
            if (go != null) go.SetActive(false);
        }

        if (characters[index] != null)
        {
            characters[index].SetActive(true);
        }
    }

    public void ButtonNext()
    {
        if (characters == null || characters.Length == 0) return;

        characters[index].SetActive(false);
        index = (index + 1) % characters.Length; // циклический переход без if
        characters[index].SetActive(true);
    }

    public void ButtonPrev()
    {
        if (characters == null || characters.Length == 0) return;

        characters[index].SetActive(false);
        index = (index - 1 + characters.Length) % characters.Length; // безопасно для отрицательных
        characters[index].SetActive(true);
    }

    public void ButtonPlay()
    {
        PlayerPrefs.SetInt("SelectPlayer", index);
        PlayerPrefs.Save(); // рекомендуется вызывать явно
        SceneManager.LoadScene("game");
    }
}