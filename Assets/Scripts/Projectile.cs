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
    return false;
    } 
}