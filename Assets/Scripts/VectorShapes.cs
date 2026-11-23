using UnityEngine;

public static class VectorShapes
{
    static Material mat;

    static Material GetMat()
    {
        if (mat == null) mat = new Material(Shader.Find("Sprites/Default"));
        return mat;
    }
    public static LineRenderer Loop(Transform parent, Vector2[] pts, Color color, float width, int order, bool closed = true)
    {
        GameObject go = new GameObject("Line");
        go.transform.SetParent(parent, false);

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.loop = closed;
        lr.positionCount = pts.Length;
        for (int i = 0; i < pts.Length; i++)
            lr.SetPosition(i, pts[i]);

        lr.startWidth = lr.endWidth = width;
        lr.startColor = lr.endColor = color;
        lr.material = GetMat();
        lr.sortingOrder = order;
        lr.numCornerVertices = 2;
        return lr;
    }

    public static Vector2[] Square(float half) => new[]
    {
        new Vector2(-half, -half), new Vector2(-half,  half),
        new Vector2( half,  half), new Vector2( half, -half)
    };

    public static Vector2[] Diamond(float half) => new[]
    {
        new Vector2(-half, 0), new Vector2(0,  half),
        new Vector2( half, 0), new Vector2(0, -half)
    };

    public static Vector2[] Triangle(float half) => new[]
    {
        new Vector2(-half, -half), new Vector2(half, 0), new Vector2(-half, half)
    };
}