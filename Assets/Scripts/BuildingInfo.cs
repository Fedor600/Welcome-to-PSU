using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BuildingInfo : MonoBehaviour
{
    [Header("Информация о корпусе")]
    public string buildingName = "Корпус ПГНИУ";

    [TextArea(3, 8)]
    public string description = "Описание корпуса";

    private BoxCollider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
        triggerCollider.size = new Vector2(15f, 15f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BuildingInfoPanel panel = BuildingInfoPanel.Instance;
            if (panel != null)
                panel.Show(buildingName, description);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BuildingInfoPanel panel = BuildingInfoPanel.Instance;
            if (panel != null)
                panel.Hide();
        }
    }
}
