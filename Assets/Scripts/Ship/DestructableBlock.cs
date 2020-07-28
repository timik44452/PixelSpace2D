using UnityEngine;

public class DestructableBlock : MonoBehaviour
{
    public int resolution = 8;
    private float burnRadiusMult = 0.25F;

    private Material material;
    private Color transparent = new Color(0, 0, 0, 0);
    private Texture2D mask;

    private void Start()
    {
        mask = new Texture2D(resolution, resolution);
        mask.filterMode = FilterMode.Point;
        material = GetComponent<SpriteRenderer>().material;
    }

    public void Explosion(float radius, Vector2 point)
    {
        float burnRadius = radius + burnRadiusMult;

        if (Vector2.Distance(point, transform.position) - 0.5F > burnRadius)
        {
            return;
        }

        for (int index = 0; index < resolution * resolution; index++)
        {
            int idx = index % resolution;
            int idy = index / resolution;

            float x = idx / (float)resolution - 0.5F;
            float y = idy / (float)resolution - 0.5F;

            float distance = Vector2.Distance(new Vector2(x + transform.position.x, y + transform.position.y), point);

            float burn = (distance - radius) / (burnRadius - radius) + (Random.value * burnRadiusMult * 2);

            mask.SetPixel(idx, idy, (distance < radius) ? transparent : new Color(burn, burn, burn));
        }

        mask.Apply();

        material.SetTexture("_Mask", mask);
    }
}
