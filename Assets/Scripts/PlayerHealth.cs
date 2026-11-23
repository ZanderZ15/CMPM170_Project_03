using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] float heartSize = 0.4f;
    [SerializeField] float heartSpacing = 0.45f;
    [SerializeField] float heightAbovePlayer = 1f;
    
    const float hitHalfSize = 0.2f;

    int health;
    Transform barRoot;
    SpriteRenderer[] hearts;

    void Start()
    {
        health = maxHealth;
        BuildHearts(MakeHeartSprite());
    }

    void BuildHearts(Sprite heartSprite)
    {
        barRoot = new GameObject("HealthBar").transform;
        hearts = new SpriteRenderer[maxHealth];

        float totalWidth = (maxHealth - 1) * heartSpacing;
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject h = new GameObject("Heart" + i);
            h.transform.SetParent(barRoot, false);
            h.transform.localPosition = new Vector3(-totalWidth / 2f + i * heartSpacing, 0f, 0f);
            h.transform.localScale = Vector3.one * heartSize;

            SpriteRenderer sr = h.AddComponent<SpriteRenderer>();
            sr.sprite = heartSprite;
            sr.color = new Color(1f, 0.2f, 0.6f);
            sr.sortingOrder = 5;
            hearts[i] = sr;
        }
    }

    void LateUpdate()
    {
        if (barRoot != null)
            barRoot.position = transform.position + Vector3.up * heightAbovePlayer;
    }

    public bool Overlaps(Vector2 center, float halfSize)
    {
        Vector2 p = transform.position;
        return Mathf.Abs(p.x - center.x) < halfSize + hitHalfSize &&
               Mathf.Abs(p.y - center.y) < halfSize + hitHalfSize;
    }

    public void TakeDamage(int amount)
    {
        for (int i = 0; i < amount && health > 0; i++)
        {
            health--;
            hearts[health].enabled = false;   
            // remove the rightmost remaining heart
        }

        if (health <= 0)
        {
            Debug.Log("Player is out of health!");
            gameObject.SetActive(false);
        }

    }

    void OnDestroy()
    {
        if (barRoot != null) Destroy(barRoot.gameObject);
    }

    // Draws a heart shape into a texture
    Sprite MakeHeartSprite()
    {
        const int size = 64;
        Texture2D tex = new Texture2D(size, size);
        tex.wrapMode = TextureWrapMode.Clamp;

        for (int py = 0; py < size; py++)
        {
            for (int px = 0; px < size; px++)
            {
                float x = (px / (size - 1f) - 0.5f) * 2.6f;
                float y = (py / (size - 1f) - 0.5f) * 2.6f;
                float a = x * x + y * y - 1f;
                bool inside = a * a * a - x * x * y * y * y <= 0f;
                tex.SetPixel(px, py, inside ? Color.white : Color.clear);
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}