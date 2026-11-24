using UnityEngine;

public class ProjClass
{
    private GameObject bullet;
    private Vector2 startingPos;
    private Vector2 direction;
    float bulletSpeed = 10f;
    float maxDistance = 15f;
    float hitHalf = 0.25f;
    
    public ProjClass(GameObject bul)
    {
        bullet = bul;
    }

    //StartPos
    public void SetStartPos(Vector2 startPos)
    {
        startingPos = startPos;
    }
    public Vector2 GetStartPos()
    {
        return startingPos;
    }
    //MaxDist
    public void SetMaxDist(float max)
    {
        maxDistance = max;
    }
    public float GetMaxDist()
    {
        return maxDistance;
    }

    //Active
    public void SetActive(bool cond)
    {
        bullet.SetActive(cond);
    }
    public bool IsActive()
    {
        return bullet.activeSelf;
    }
    //GameObj
    public GameObject GetProj()
    {
        return bullet;
    }
    //Direction
    public void SetDir(Vector2 dir)
    {
        direction = dir;
    }
    public Vector2 GetDir()
    {
        return direction;
    }
    //Speed
    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }
    public float GetSpeed()
    {
        return bulletSpeed;
    }

    public void SetHitHalf(float h)
    {
        hitHalf = h;
    }

    public float GetHitHalf()
    {
        return hitHalf;
    }

}
