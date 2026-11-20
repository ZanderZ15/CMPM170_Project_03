using UnityEngine;

public class Enemy : MonoBehaviour
{
    Projectile projectiles;
    PlayerHealth player;         
    float speed;
    float halfSize;
    float despawnX;

    //debounce for enemy damage
    bool hasHit;

    Transform target;

    public void Init(Projectile projectileManager, Sprite sprite, float moveSpeed, float size, float leftEdgeX, Sprite gunSprite, Transform playerTransform)
    {
        projectiles = projectileManager;
        player = FindFirstObjectByType<PlayerHealth>();
        speed = moveSpeed;
        halfSize = size / 2f;
        target = playerTransform;

        //enemy sprite
        transform.localScale = Vector3.one * size;
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Color.red;
        sr.sortingOrder = 0;

        if (gunSprite != null)
        {
            const float gunWidth = 0.9f; 

            GameObject gun = new GameObject("Gun");
            gun.transform.SetParent(transform, false);
            gun.transform.localPosition = new Vector3(-0.6f, -0.1f, 0f);
            gun.transform.localScale = Vector3.one * (gunWidth / gunSprite.bounds.size.x);

            SpriteRenderer gsr = gun.AddComponent<SpriteRenderer>();
            gsr.sprite = gunSprite;
            gsr.sortingOrder = 1;  
        }
    }

    void Update()
    {
        //track the player
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        if (projectiles != null && projectiles.TryHit(transform.position, halfSize))
        {
            Destroy(gameObject);
            return;
        }

        //damaging player
        if (!hasHit && player != null && player.Overlaps(transform.position, halfSize))
        {
            hasHit = true;
            player.TakeDamage(1);
        }

        if (transform.position.x < despawnX)
        {
            hasHit = true;
            player.TakeDamage(1);
        }
    }
}
