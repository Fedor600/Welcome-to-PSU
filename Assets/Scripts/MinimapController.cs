using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform playerTransform;
    public RectTransform playerDot;
    public RectTransform minimapRect;

    [Header("Границы игрового мира (Unity units)")]
    public float worldMinX = -8f;
    public float worldMaxX = 8f;
    public float worldMinY = -5f;
    public float worldMaxY = 5f;

    [Header("Настройки")]
    public bool showMinimap = true;
    public KeyCode toggleKey = KeyCode.M;

    private GameObject minimapContainer;

    private void Awake()
    {
        minimapContainer = transform.parent != null ? transform.parent.gameObject : gameObject;
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }

        UpdateVisibility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            showMinimap = !showMinimap;
            UpdateVisibility();
        }

        if (showMinimap && playerTransform != null)
            UpdatePlayerDot();
    }

    private void UpdatePlayerDot()
    {
        if (playerDot == null || minimapRect == null) return;

        float normX = Mathf.InverseLerp(worldMinX, worldMaxX, playerTransform.position.x);
        float normY = Mathf.InverseLerp(worldMinY, worldMaxY, playerTransform.position.y);

        normX = Mathf.Clamp01(normX);
        normY = Mathf.Clamp01(normY);

        float mapW = minimapRect.rect.width;
        float mapH = minimapRect.rect.height;

        playerDot.anchoredPosition = new Vector2(
            (normX - 0.5f) * mapW,
            (normY - 0.5f) * mapH
        );
    }

    private void UpdateVisibility()
    {
        if (minimapContainer != null)
            minimapContainer.SetActive(showMinimap);
    }
}
