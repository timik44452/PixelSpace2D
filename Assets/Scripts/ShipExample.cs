using Game;
using System.Collections.Generic;
using UnityEngine;

public class ShipExample : MonoBehaviour
{
    public Texture2D shipTexture;

    private Dictionary<Color, Blocks> blockDictionary = new Dictionary<Color, Blocks>()
    {
        { new Color(1, 1, 1), Blocks.Floor },
        { new Color(0, 0, 0), Blocks.Wall },
        { new Color(0, 0, 1), Blocks.Engine },
    };

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
                    foreach(var keyPair in blockDictionary)
                    {
                        if (IsColor(pixel, keyPair.Key))
                        {
                            shipData.AddBlock(_x, _y, (int)keyPair.Value);

                            break;
                        }
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
