using UnityEngine;

public class Enemy : MonoBehaviour
{
    Projectile projectiles;
    float speed;
    float halfSize;
    float despawnX;

    public void Init(Projectile projectileManager, Sprite sprite, float moveSpeed, float size, float leftEdgeX)
    {
        projectiles = projectileManager;
        speed = moveSpeed;
        halfSize = size / 2f;
        despawnX = leftEdgeX;

        //enemy sprite
        transform.localScale = Vector3.one * size;
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Color.red;
        sr.sortingOrder = 0;
    }

    void Update()
    {
        //enemy sprite logic
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (projectiles != null && projectiles.TryHit(transform.position, halfSize))
        {
            Destroy(gameObject);
            return;
        }

        if (transform.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}