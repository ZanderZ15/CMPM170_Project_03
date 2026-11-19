using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Projectile projectileManager;
    [SerializeField] float spawnInterval = 1.5f;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] float enemySize = 1f;

    [Header("Spawner Block")]
    [SerializeField] float blockSize = 2f;
    [SerializeField] int blockHealth = 3;
    [SerializeField] float blockYOffset = 0f;  
    [SerializeField] Sprite gunSprite;

    Camera cam;
    Sprite squareSprite;
    float timer;

    GameObject block;
    SpriteRenderer[] segments;
    int health;

    void Start()
    {
        cam = Camera.main;

        //projectile component 
        if (projectileManager == null)
            projectileManager = GetComponent<Projectile>();

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        squareSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);

        //spawner block spawn and block health
        health = blockHealth;
        BuildBlock();
    }

    void BuildBlock()
    {
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 c = cam.transform.position;

        block = new GameObject("SpawnerBlock");
        block.transform.position = new Vector3(15f, 0f, 0f);

        GameObject body = new GameObject("Body");
        body.transform.SetParent(block.transform, false);
        body.transform.localScale = Vector3.one * blockSize;
        SpriteRenderer bodySr = body.AddComponent<SpriteRenderer>();
        bodySr.sprite = squareSprite;
        bodySr.color = new Color(0.6f, 0.1f, 0.7f);   
        bodySr.sortingOrder = 0;

        // Health bar
        float barY = blockSize / 2f + 0.3f;
        float gap = 0.05f;
        float segWidth = (blockSize - gap * (blockHealth - 1)) / blockHealth;

        segments = new SpriteRenderer[blockHealth];
        for (int i = 0; i < blockHealth; i++)
        {
            GameObject seg = new GameObject("HealthSegment" + i);
            seg.transform.SetParent(block.transform, false);
            float x = -blockSize / 2f + segWidth / 2f + i * (segWidth + gap);

            seg.transform.localPosition = new Vector3(x, barY, 0f);
            seg.transform.localScale = new Vector3(segWidth, 0.22f, 1f);
            SpriteRenderer sr = seg.AddComponent<SpriteRenderer>();
            sr.sprite = squareSprite;
            sr.color = Color.green;
            sr.sortingOrder = 2;
            segments[i] = sr;
        }
    }

    void Update()
    {
        if (block == null) return;

        if (projectileManager != null &&
            projectileManager.TryHit(block.transform.position, blockSize / 2f))
        {
            //apply damage
            TakeBlockHit();
            if (block == null) return;
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            //spawn enemies
            timer = 0f;
            SpawnEnemy();
        }
    }

    void TakeBlockHit()
    {
        health--;

        if (health >= 0 && health < segments.Length)
            segments[health].enabled = false;

        if (health <= 0)
        {
            //destroy spawner
            Destroy(block);
            block = null;
        }
    }

    void SpawnEnemy()
    {
        //despawn boundary
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 c = cam.transform.position;

        //enemy spawn
        float x = block.transform.position.x - blockSize / 2f - enemySize / 2f;
        float y = block.transform.position.y;

        GameObject e = new GameObject("Enemy");
        e.transform.position = new Vector3(x, y, 0f);

        //gun
        Enemy enemy = e.AddComponent<Enemy>();
        enemy.Init(projectileManager, squareSprite, enemySpeed, enemySize,
                   c.x - halfW - enemySize * 2f, gunSprite);
    }
}