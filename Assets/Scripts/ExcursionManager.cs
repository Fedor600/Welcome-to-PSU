using UnityEngine;
using TMPro;

public class ExcursionManager : MonoBehaviour
{
    public CampusPoint[] campusPoints;

    public TextMeshProUGUI progressText;
    public GameObject finalPanel;

    private void Start()
    {
        UpdateProgressUI();

        if (finalPanel != null)
            finalPanel.SetActive(false);
    }

    public void CompletePoint(string buildingName)
    {
        foreach (CampusPoint point in campusPoints)
        {
            if (point.buildingName == buildingName && !point.isCompleted)
            {
                point.isCompleted = true;
                UpdateProgressUI();
                CheckFinish();
                return;
            }
        }
    }

    private void UpdateProgressUI()
    {
        int completed = 0;

        foreach (CampusPoint point in campusPoints)
        {
            if (point.isCompleted)
                completed++;
        }

        progressText.text = $"Посещено: {completed}/{campusPoints.Length}";
    }

    private void CheckFinish()
    {
        foreach (CampusPoint point in campusPoints)
        {
            if (!point.isCompleted)
                return;
        }

        finalPanel.SetActive(true);
    }
}