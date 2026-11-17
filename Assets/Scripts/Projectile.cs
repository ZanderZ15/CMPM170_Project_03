using System.Diagnostics;
using System.Numerics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Array<GameObject> bulletArray;
    GameObject bulletPrefab;
    void Start()
    {
        Vector2 spawnLocation = new Vector2(-1000, -1000);
        bulletArray = new Array[5];
        for (int i = 0; i < 5; i++)
        {
            bulletArray[i] = Instantiate(bulletPrefab, spawnLocation);
            bulletArray[i].SetActive(false);
        }
        Debug.Log(bulletArray);
    }
}