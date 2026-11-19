using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 height = new Vector3(0, 0, -20);
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + height;
    }
}
