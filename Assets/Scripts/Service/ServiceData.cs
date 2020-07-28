using UnityEngine;

public static class ServiceData
{
    public enum Side
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
    }

    public static Vector2Int[] offsets = new Vector2Int[]
            {
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1),
            };
}