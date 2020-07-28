using UnityEngine;

public static class TypeConvertExtension
{
    #region Vector2Int
    public static Vector2Int ToVector2Int(this Vector2 value)
    {
        return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
    }
    public static Vector2Int ToVector2Int(this Vector3 value)
    {
        return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
    }
    #endregion
}