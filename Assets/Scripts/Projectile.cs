using System.Collections.Generic;
using System.Linq;
//using Unity.VisualScripting;
//using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    List<ProjClass> bulletArray;
    //Dictionary<GameObject, Vector2> bulletDirections = new Dictionary<GameObject, Vector2>();

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletScale = 1.5f;   

    //SHOTGUN: pellet pool and settings
    List<ProjClass> pelletArray;
    Sprite pelletSprite;

    [Header("Shotgun")]
    [SerializeField] int pelletsPerShot = 10;
    [SerializeField] int pelletPoolSize = 50;
    [SerializeField] float coneAngle = 40f;      
    [SerializeField] float pelletSpeed = 14f;
    [SerializeField] float pelletRange = 5f;  
    [SerializeField] float pelletSize = 0.12f;
    

    void Start()
    {
        Vector2 spawnLocation = new Vector2(-1000, -1000);
        bulletArray = new List<ProjClass>();
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnLocation, Quaternion.identity);
            ProjClass pClass = new ProjClass(bullet);
            pClass.SetSpeed(bulletSpeed);
            bullet.transform.localScale *= bulletScale;

            bulletArray.Add(pClass);
            bulletArray[i].SetActive(false);
        }
        //Debug.Log(bulletArray);

        //shotgun - build pellet pool
        BuildPelletPool();
    }

    //shotgun - creates a pool of tiny white square pellets
    void BuildPelletPool()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        pelletSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);

        pelletArray = new List<ProjClass>();
        for (int i = 0; i < pelletPoolSize; i++)
        {
            GameObject go = new GameObject("Pellet");
            go.transform.position = new Vector2(-1000, -1000);
            go.transform.localScale = Vector3.one * pelletSize;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = pelletSprite;
            sr.color = Color.white;
            sr.sortingOrder = 3;

            ProjClass p = new ProjClass(go);
            p.SetMaxDist(pelletRange);
            p.SetActive(false);
            pelletArray.Add(p);
        }
    }

    void Update()
    {   
        Fly();
    }

    public ProjClass GetFirstActive()
    {
        foreach (ProjClass bul in bulletArray)
        {
            //find first bullet not being used
            if (!bul.IsActive())
                return bul;
        }
        return null;
    }

    public void Launch(ProjClass bul, Vector2 direction)
    {
        
        bul.SetStartPos(bul.GetProj().transform.position);
        bul.SetDir(direction.normalized);

        //rotate bullet to face its travel direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        bul.GetProj().transform.rotation = Quaternion.Euler(0, 0, angle);

        bul.SetActive(true);
    }

    //shotgun - fires a cone of small pellets around the facing direction
    public void FireShotgun(Vector2 origin, Vector2 facing)
    {
        float baseAngle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pelletsPerShot; i++)
        {
            ProjClass p = null;
            foreach (ProjClass pel in pelletArray)
            {
                //find first pellet not being used
                if (!pel.IsActive())
                {
                    p = pel;
                    break;
                }
            }
            if (p == null) return;   // pool exhausted

            float spread = Random.Range(-coneAngle / 2f, coneAngle / 2f);
            Vector2 dir = Quaternion.Euler(0, 0, spread) * facing;

            p.SetSpeed(pelletSpeed * Random.Range(0.8f, 1.2f));   
            p.GetProj().transform.position = origin;
            p.SetStartPos(origin);
            p.SetDir(dir);
            p.SetActive(true);
        }
    }
    
    private void Fly()
    {
        foreach (ProjClass bul in bulletArray)
        {
            if (!bul.IsActive()) continue;

            Vector2 dir = bul.GetDir();

            //move bullet based on speed and elapsed time
            bul.GetProj().transform.position += (Vector3)(dir * bul.GetSpeed() * Time.deltaTime);

            if (Vector2.Distance(bul.GetStartPos(), bul.GetProj().transform.position) > bul.GetMaxDist())
            {
                bul.SetActive(false);
            }
        }

        foreach (ProjClass bul in pelletArray)
        {
            if (!bul.IsActive()) continue;

            Vector2 dir = bul.GetDir();

            bul.GetProj().transform.position += (Vector3)(dir * bul.GetSpeed() * Time.deltaTime);

            if (Vector2.Distance(bul.GetStartPos(), bul.GetProj().transform.position) > bul.GetMaxDist())
            {
                bul.SetActive(false);
            }
        }
    }
    
    public bool TryHit(Vector2 center, float halfSize)
    //collision check, makes one bullet count as one hit
    {
    const float bulletHalf = 0.25f; 

    foreach (ProjClass bul in bulletArray)
    {
        if (!bul.IsActive()) continue;

        Vector2 p = bul.GetProj().transform.position;
        if (Mathf.Abs(p.x - center.x) < halfSize + bulletHalf &&
            Mathf.Abs(p.y - center.y) < halfSize + bulletHalf)
        {
            //hit response
            bul.SetActive(false);
            return true;
        }
    }

    //shotgun - smaller hitbox per pellet
    float pelletHalf = pelletSize / 2f;

    foreach (ProjClass bul in pelletArray)
    {
        if (!bul.IsActive()) continue;

        Vector2 p = bul.GetProj().transform.position;
        if (Mathf.Abs(p.x - center.x) < halfSize + pelletHalf &&
            Mathf.Abs(p.y - center.y) < halfSize + pelletHalf)
        {
            bul.SetActive(false);
            return true;
        }
    }
    return false;
    } 
}