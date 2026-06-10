using UnityEngine;
using TMPro;

public class ExcursionManager : MonoBehaviour
{
    [Header("Точки кампуса")]
    public CampusPoint[] campusPoints;

    [Header("UI")]
    public TextMeshProUGUI progressText;
    public GameObject finalPanel;

    private void Start()
    {
        UpdateProgressUI();
        CheckFinish();

        if (finalPanel != null)
            finalPanel.SetActive(false);
    }

    public void CompletePoint(string buildingName)
    {
        if (campusPoints == null)
            return;

        foreach (CampusPoint point in campusPoints)
        {
            if (point == null)
                continue;

            if (point.buildingName == buildingName && !point.isCompleted)
            {
                point.isCompleted = true;

                UpdateProgressUI();
                CheckFinish();

                return;
            }
        }
    }

    public void UpdateProgressUI()
    {
        if (campusPoints == null)
            return;

        int completed = 0;

        foreach (CampusPoint point in campusPoints)
        {
            if (point != null && point.isCompleted)
                completed++;
        }

        if (progressText != null)
        {
            progressText.text = $"Посещено: {completed}/{campusPoints.Length}";
        }
    }

    public void CheckFinish()
    {
        if (campusPoints == null || campusPoints.Length == 0)
            return;

        foreach (CampusPoint point in campusPoints)
        {
            if (point == null || !point.isCompleted)
            {
                if (finalPanel != null)
                    finalPanel.SetActive(false);

                return;
            }
        }

        if (finalPanel != null)
            finalPanel.SetActive(true);
    }
}