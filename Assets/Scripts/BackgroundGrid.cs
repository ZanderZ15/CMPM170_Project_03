using UnityEngine;

public class BackgroundGrid : MonoBehaviour
{
    [SerializeField] float spacing = 2f;      
    [SerializeField] float lineWidth = 0.06f;
    [SerializeField] Color lineColor = new Color(1f, 1f, 1f, 0.15f);

    Camera cam;
    Transform[] vLines;   
    Transform[] hLines;   
    int vCount, hCount;

    void Start()
    {
        cam = Camera.main;

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        vCount = Mathf.CeilToInt(halfW * 2f / spacing) + 3;
        hCount = Mathf.CeilToInt(halfH * 2f / spacing) + 3;

        vLines = new Transform[vCount];
        hLines = new Transform[hCount];

        for (int i = 0; i < vCount; i++)
            vLines[i] = MakeLine(sprite, new Vector3(lineWidth, halfH * 2f + spacing * 2f, 1f));

        for (int i = 0; i < hCount; i++)
            hLines[i] = MakeLine(sprite, new Vector3(halfW * 2f + spacing * 2f, lineWidth, 1f));
    }

    Transform MakeLine(Sprite sprite, Vector3 scale)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(transform, false);
        line.transform.localScale = scale;

        SpriteRenderer sr = line.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = lineColor;
        sr.sortingOrder = -10;   
        return line.transform;
    }

    void LateUpdate()
    {
        Vector3 c = cam.transform.position;

        // Snap to the nearest grid multiple so lines stay fixed in the world
        float startX = Mathf.Floor(c.x / spacing) * spacing - (vCount / 2) * spacing;
        float startY = Mathf.Floor(c.y / spacing) * spacing - (hCount / 2) * spacing;

        for (int i = 0; i < vCount; i++)
            vLines[i].position = new Vector3(startX + i * spacing, c.y, 0f);

        for (int i = 0; i < hCount; i++)
            hLines[i].position = new Vector3(c.x, startY + i * spacing, 0f);
    }
}