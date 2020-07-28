using UnityEngine;

public class ShipExample : MonoBehaviour
{
    public Texture2D shipTexture;

    public void Build(IShipDataContainer shipData)
    {
        for (int x = 0; x < shipTexture.width; x++)
            for (int y = 0; y < shipTexture.height; y++)
            {
                Color pixel = shipTexture.GetPixel(x, y);

                int _x = x - shipTexture.width / 2;
                int _y = y - shipTexture.height / 2;

                if (pixel.a > 0)
                {
                    if (IsColor(pixel, new Color(0, 0, 1)))
                    {
                        shipData.AddBlock(_x, _y, 2);
                    }
                    else if (IsColor(pixel, new Color(1, 1, 1)))
                    {
                        shipData.AddBlock(_x, _y, 0);
                    }
                    else if (IsColor(pixel, new Color(0, 0, 0)))
                    {
                        shipData.AddBlock(_x, _y, 1);
                    }
                }
            }
    }

    private bool IsColor(Color a, Color b)
    {
        float r2 = Mathf.Pow(a.r - b.r, 2);
        float g2 = Mathf.Pow(a.g - b.g, 2);
        float b2 = Mathf.Pow(a.b - b.b, 2);

        return Mathf.Sqrt(r2 + g2 + b2) < 0.1F;
    }
}
