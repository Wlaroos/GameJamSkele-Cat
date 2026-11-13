using UnityEngine;

public class SetGeometry : MonoBehaviour
{
    private float dist = 0.01f;
    private float centerX;
    private float centerY;
    private float offsetX;
    private float offsetY;

    void Start()
    {
        var trigger = GetComponent<BoxCollider2D>();
        var collider = GetComponent<PolygonCollider2D>();
        centerX = 0;
        centerY = 0;
        offsetX = (trigger.size.x / 2);
        offsetY = (trigger.size.y / 2);

        var path0 = new Vector2[]
        {
            new Vector2(centerX - offsetX, centerY - offsetY),
            new Vector2(centerX + offsetX, centerY - offsetY),
            new Vector2(centerX + offsetX, centerY + offsetY),
            new Vector2(centerX - offsetX, centerY + offsetY)
        };

        var path1 = new Vector2[]
       {
            new Vector2(centerX - offsetX + dist, centerY - offsetY + dist),
            new Vector2(centerX + offsetX - dist, centerY - offsetY + dist),
            new Vector2(centerX + offsetX - dist, centerY + offsetY - dist),
            new Vector2(centerX - offsetX + dist, centerY + offsetY - dist)
       };

        collider.pathCount = 2;

        collider.SetPath(0, path0);
        collider.SetPath(1, path1);
    }
}