using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public PlayerController player;     // se ficar vazio, o script procura sozinho
    public Sprite iconSprite;           // sprite da nave (opcional)
    public Color iconColor = new Color(0.68f, 0.93f, 0.93f, 1f);

    [Header("Layout")]
    public int maxIcons = 3;
    public float iconSize = 70f;
    public float spacing = 10f;
    public Vector2 margin = new Vector2(30f, -30f);   // distância do canto superior esquerdo

    Image[] icons;
    GameObject canvasGO;

    void Awake()
    {
        // Canvas próprio, independente do resto da cena
        canvasGO = new GameObject("VidasCanvas", typeof(Canvas), typeof(CanvasScaler));

        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        Sprite sprite = iconSprite != null ? iconSprite : CreateWhiteSprite();

        icons = new Image[maxIcons];
        for (int i = 0; i < maxIcons; i++)
        {
            var go = new GameObject("Vida" + (i + 1), typeof(RectTransform), typeof(Image));
            go.transform.SetParent(canvasGO.transform, false);

            var r = (RectTransform)go.transform;
            r.anchorMin = new Vector2(0f, 1f);
            r.anchorMax = new Vector2(0f, 1f);
            r.pivot = new Vector2(0f, 1f);
            r.sizeDelta = new Vector2(iconSize, iconSize);
            r.anchoredPosition = new Vector2(margin.x + i * (iconSize + spacing), margin.y);

            var img = go.GetComponent<Image>();
            img.sprite = sprite;
            img.color = iconColor;
            img.preserveAspect = true;
            img.raycastTarget = false;
            icons[i] = img;
        }
    }

    void OnEnable()
    {
        if (player == null) player = FindFirstObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("LivesUI: não achei o Player na cena.", this);
            return;
        }

        player.OnLivesChanged += Refresh;
        Refresh(player.extraLives);   // mostra o valor atual logo ao ligar
        Debug.Log("LivesUI: iniciou com " + player.extraLives + " vidas extras.");
    }

    void OnDisable()
    {
        if (player != null) player.OnLivesChanged -= Refresh;
    }

    void OnDestroy()
    {
        if (canvasGO != null) Destroy(canvasGO);
    }

    void Refresh(int extraLives)
    {
        if (icons == null) return;
        for (int i = 0; i < icons.Length; i++)
            icons[i].enabled = i < extraLives;
    }

    static Sprite CreateWhiteSprite()
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }
}