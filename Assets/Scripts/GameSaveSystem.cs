using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;

public class GameSaveSystem : MonoBehaviour
{
    [Header("Ссылки на игрока")]
    [SerializeField] private Transform playerTransform;


    [Header("Опционально: для сохранения прогресса экскурсии")]
    [SerializeField] private ExcursionManager excursionManager;


    private static GameSaveSystem instance;
    private bool isLoading;

    private string SavePath => Path.Combine(Application.persistentDataPath, "game_save.json");

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame()
    {
        FindSceneObjectsIfNeeded();

        SaveData data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name
        };

        if (playerTransform != null)
        {
            Vector3 position = playerTransform.position;

            data.playerPosX = position.x;
            data.playerPosY = position.y;
            data.playerPosZ = position.z;
        }

        if (excursionManager != null && excursionManager.campusPoints != null)
        {
            data.completedPoints = new bool[excursionManager.campusPoints.Length];

            for (int i = 0; i < excursionManager.campusPoints.Length; i++)
            {
                data.completedPoints[i] = excursionManager.campusPoints[i].isCompleted;
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("Игра сохранена: " + SavePath);
    }

    public void LoadGame()
    {
        if (isLoading)
            return;

        if (!File.Exists(SavePath))
        {
            Debug.Log("Файл сохранения не найден");
            return;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
            {
                Debug.LogError("Не удалось прочитать сохранение");
                return;
            }

            StartCoroutine(LoadGameRoutine(data));
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка загрузки сохранения: " + e.Message);
        }
    }

    private IEnumerator LoadGameRoutine(SaveData data)
    {
        isLoading = true;

        if (!string.IsNullOrEmpty(data.sceneName) &&
            SceneManager.GetActiveScene().name != data.sceneName)
        {
            SceneManager.LoadScene(data.sceneName);
        }

        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);

        FindSceneObjectsIfNeeded();

        if (playerTransform != null)
        {
            playerTransform.position = new Vector3(
                data.playerPosX,
                data.playerPosY,
                data.playerPosZ
            );
        }

        if (excursionManager != null &&
            excursionManager.campusPoints != null &&
            data.completedPoints != null)
        {
            int count = Mathf.Min(
                excursionManager.campusPoints.Length,
                data.completedPoints.Length
            );

            for (int i = 0; i < count; i++)
            {
                excursionManager.campusPoints[i].isCompleted = data.completedPoints[i];
            }

            excursionManager.UpdateUI();
        }

        isLoading = false;

        Debug.Log("Игра загружена");
    }

    public void SaveAndExit()
    {
        SaveGame();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public bool HasSaveFile()
    {
        return File.Exists(SavePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Сохранение удалено");
        }
    }

    private void FindSceneObjectsIfNeeded()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerTransform = player.transform;
        }

        if (excursionManager == null)
        {
            excursionManager = FindObjectOfType<ExcursionManager>();
        }
    }
}

[Serializable]
public class SaveData
{
    public string sceneName;

    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public bool[] completedPoints;
}