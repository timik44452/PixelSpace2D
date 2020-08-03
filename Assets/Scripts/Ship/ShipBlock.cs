using UnityEngine;

public class ShipBlock
{
    public int ID = 0;
    
    public int X
    {
        get => Mathf.RoundToInt(x);
        set => x = value;
    }
    public int Y
    {
        get => Mathf.RoundToInt(y);
        set => y = value;
    }

    public float Rotation
    {
        get => rotation;
        set => rotation = value;
    }

    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private float rotation;

    public override bool Equals(object obj)
    {
        return obj is ShipBlock block &&
               ID == block.ID &&
               x == block.x &&
               y == block.y;
    }

    public override int GetHashCode()
    {
        int hashCode = 2107267315;
        hashCode = hashCode * -1521134295 + ID.GetHashCode();
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }
}
