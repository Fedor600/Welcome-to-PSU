using UnityEngine;

[System.Serializable]
public class CampusPoint
{
    public string buildingName;
    [TextArea(3, 8)]
    public string curatorDialogue;
    public bool isCompleted;
}