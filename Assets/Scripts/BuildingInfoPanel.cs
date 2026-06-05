using UnityEngine;
using TMPro;

public class BuildingInfoPanel : MonoBehaviour
{
    public static BuildingInfoPanel Instance { get; private set; }

    [Header("UI элементы")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Show(string buildingName, string description)
    {
        if (panel == null) return;

        if (titleText != null)
            titleText.text = buildingName;

        if (descriptionText != null)
            descriptionText.text = description;

        panel.SetActive(true);
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
