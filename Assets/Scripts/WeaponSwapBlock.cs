using UnityEngine;

public class WeaponSwapBlock : MonoBehaviour
{
    [SerializeField] float distanceAbovePlayer = 4f;
    [SerializeField] Vector2 size = new Vector2(3f, 1.2f);
    [SerializeField] Color color = new Color(0f, 1f, 1f);

    const float playerHalf = 0.2f;   

    Transform player;
    Movement movement;
    Vector2 half;
    bool wasTouching;

    void Start()
    {
        GameObject p = GameObject.Find("Player");
        if (p == null)
        {
            Debug.LogWarning("WeaponSwapBlock: no object named 'Player' found.");
            enabled = false;
            return;
        }

        player = p.transform;
        movement = p.GetComponent<Movement>();
        half = size / 2f;

        // sit above where player starts
        transform.position = player.position + Vector3.up * distanceAbovePlayer;

        Vector2[] rect =
        {
            new Vector2(-half.x, -half.y), new Vector2(-half.x, half.y),
            new Vector2( half.x,  half.y), new Vector2( half.x, -half.y)
        };
        VectorShapes.Loop(transform, rect, color, 0.08f, 0);

        BuildLabel();
    }

    void BuildLabel()
    {
        GameObject label = new GameObject("Label");
        label.transform.SetParent(transform, false);

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        TextMesh tm = label.AddComponent<TextMesh>();
        tm.text = "SWAP WEAPON";
        tm.font = font;
        tm.fontSize = 64;
        tm.characterSize = 0.06f;   
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = color;

        MeshRenderer mr = label.GetComponent<MeshRenderer>();
        mr.material = font.material;
        mr.sortingOrder = 3;
    }

    void Update()
    {
        if (player == null || movement == null || !player.gameObject.activeInHierarchy) return;

        Vector2 c = transform.position;
        Vector2 p = player.position;
        bool touching = Mathf.Abs(p.x - c.x) < half.x + playerHalf &&
                        Mathf.Abs(p.y - c.y) < half.y + playerHalf;

        // swap once per touch
        if (touching && !wasTouching)
            movement.ToggleWeapon();

        wasTouching = touching;
    }
}