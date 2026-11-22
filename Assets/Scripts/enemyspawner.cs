using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Projectile projectileManager;
    [SerializeField] float spawnInterval = 3f;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] float enemySize = 1f;

    [Header("Spawner Block")]
    [SerializeField] float blockSize = 2f;
    [SerializeField] int blockHealth = 3;
    [SerializeField] float blockDistance = 15f;
    [SerializeField] float blockYOffset = 0f;  
    [SerializeField] Sprite gunSprite;

    [Header("Enemy Colors")]
    [SerializeField] Color firstEnemyColor = Color.red;
    [SerializeField] Color secondEnemyColor = Color.yellow;

    Camera cam;
    Sprite squareSprite;
    float timer;
    Transform playerTransform;

    GameObject block;
    SpriteRenderer[] segments;
    int health;
    int spawnerIndex;
    Color currentEnemyColor;
    float centerX;

    void Start()
    {
        cam = Camera.main;
        centerX = cam.transform.position.x;

        //find the player
        GameObject player = GameObject.Find("Player");
        if (player != null)
            playerTransform = player.transform;

        //projectile component 
        if (projectileManager == null)
            projectileManager = GetComponent<Projectile>();

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        squareSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);

        SpawnBlock();
    }

    void SpawnBlock()
    {
        float x = (spawnerIndex == 0) ? centerX + blockDistance : centerX - blockDistance;
        currentEnemyColor = (spawnerIndex == 0) ? firstEnemyColor : secondEnemyColor;

        health = blockHealth;
        timer = 0f;
        BuildBlock(x);
    }

    void BuildBlock(float x)
    {
        block = new GameObject("SpawnerBlock");
        block.transform.position = new Vector3(x, 0f, 0f);

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
            
            float segX = -blockSize / 2f + segWidth / 2f + i * (segWidth + gap);
            
            seg.transform.localPosition = new Vector3(segX, barY, 0f);   
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
            spawnerIndex++;

            if (spawnerIndex == 1)
                SpawnBlock();
        }
    }

    void SpawnEnemy()
    {
        //pick a random side: 0=left, 1=right, 2=top, 3=bottom
        int side = Random.Range(0, 4);
        float x = block.transform.position.x;
        float y = block.transform.position.y;
        float offset = blockSize / 2f + enemySize / 2f;

        switch (side)
        {
            case 0: // left
                x -= offset;
                break;
            case 1: // right
                x += offset;
                break;
            case 2: // topBuildBlock
                y += offset;
                break;
            case 3: // bottom
                y -= offset;
                break;
        }

        GameObject e = new GameObject("Enemy");
        e.transform.position = new Vector3(x, y, 0f);

        //gun
        Enemy enemy = e.AddComponent<Enemy>();
        enemy.Init(projectileManager, squareSprite, enemySpeed, enemySize, gunSprite, playerTransform, currentEnemyColor);
    }
}
