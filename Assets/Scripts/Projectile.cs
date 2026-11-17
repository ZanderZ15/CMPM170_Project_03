using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    List<GameObject> bulletArray;
    GameObject bulletPrefab;
    void Start()
    {
        Vector2 spawnLocation = new Vector2(-1000, -1000);
        bulletArray = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            bulletArray[i] = Instantiate(bulletPrefab, spawnLocation, Quaternion.identity);
            bulletArray[i].SetActive(false);
        }
        Debug.Log(bulletArray);
    }
}