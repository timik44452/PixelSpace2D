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

    public static Vector2[] blockVertices = new Vector2[]
        {
            new Vector2(-0.5F, -0.5F),
            new Vector2(-0.5F,  0.5F),
            new Vector2( 0.5F,  0.5F),
            new Vector2( 0.5F, -0.5F)
        };

    public static Vector2[][] colliderEdgePoints = new Vector2[][]
        {
            blockVertices, //0
            new Vector2[]{ blockVertices[0], blockVertices[1] }, //1
            new Vector2[]{ blockVertices[1], blockVertices[2] }, //2
            new Vector2[]{ blockVertices[0], blockVertices[1], blockVertices[2] }, //3
            new Vector2[]{ blockVertices[2], blockVertices[3] }, //4
            blockVertices, //5
            new Vector2[]{ blockVertices[1], blockVertices[2], blockVertices[3] }, //6
            blockVertices, //7
            new Vector2[]{ blockVertices[3], blockVertices[0] }, //8
            new Vector2[]{ blockVertices[3], blockVertices[0], blockVertices[1] }, //9
            blockVertices, //10
            blockVertices, //11
            new Vector2[]{ blockVertices[2], blockVertices[3], blockVertices[0] }, //12
            blockVertices, //13
            blockVertices, //14
        };
}