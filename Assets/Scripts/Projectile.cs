using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    List<GameObject> bulletArray;
    [SerializeField] GameObject bulletPrefab;
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
        for (int i = 0; i < 5; i++)
        {
            if (!bulletArray[i].activeSelf)
            {
                return bulletArray[i];
            }
        }
        return null;
    }
    private void Fly()
    {
        foreach (GameObject bul in bulletArray)
        {
            if (bul.activeSelf)
            {
                //Replace with acc fly code
                Vector2 newPos = new Vector2(bul.transform.position.x + 0.01f, bul.transform.position.y);
                bul.transform.position = newPos;
                if (bul.transform.position.x > 10)
                {
                    bul.SetActive(false);
                }
            }
        }
    }
}