using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    List<GameObject> bulletArray;
    Dictionary<GameObject, Vector2> bulletDirections = new Dictionary<GameObject, Vector2>();

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float maxDistance = 15f;

    void Start()
    {
        Vector2 spawnLocation = new Vector2(-1000, -1000);
        bulletArray = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnLocation, Quaternion.identity);
            bulletArray.Add(bullet);
            bulletArray[i].SetActive(false);
        }
        Debug.Log(bulletArray);
    }
    void Update()
    {   
        Fly();
    }

    public GameObject GetFirstActive()
    {
        foreach (GameObject bul in bulletArray)
        {
            //find first bullet not being used
            if (!bul.activeSelf)
            return bul;
        }
        return null;
    }

    public void Launch(GameObject bul, Vector2 direction)
    {
        bulletDirections[bul] = direction.normalized;

        //rotate bullet to face its travel direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        bul.transform.rotation = Quaternion.Euler(0, 0, angle);

        bul.SetActive(true);
    }
    
    private void Fly()
    {
        foreach (GameObject bul in bulletArray)
        {
            if (!bul.activeSelf) continue;

            Vector2 dir = bulletDirections[bul];
            //move bullet based on speed and elapsed time
            bul.transform.position += (Vector3)(dir * bulletSpeed * Time.deltaTime);

            if (bul.transform.position.magnitude > maxDistance)
            {
                bul.SetActive(false);
            }
        }
    }
}