using UnityEngine;

public class ShipBlock
{
    public int ID;
    
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public Rect Rect { get; }
    
    public float Rotation { get; }

    public ShipBlock(int id, float rotation, int x, int y, int width, int height)
    {
        ID = id;
        X = x;
        Y = y;
        Width = width;
        Height = height;

        Rotation = rotation;
        
        Rect = new Rect(x - width * 0.5F, y - height * 0.5F, width, height);
    }


    public static ShipBlock Create(Blocks type, int x, int y)
    {
        return Create((int)type, x, y);
    }

    public static ShipBlock Create(int id, int x, int y, float rotation = 0)
    {
        var blockResource = ResourceUtility.Blocks.GetBlock((Blocks)id);
        var blockPrototype = new ShipBlock(id, rotation, x, y, blockResource.Width, blockResource.Height);

        return blockPrototype;
    }

    public override bool Equals(object obj)
    {
        return obj is ShipBlock block &&
               ID == block.ID &&
               X == block.X &&
               Y == block.Y &&
               Width == block.Width &&
               Height == block.Height;
    }

    public override int GetHashCode()
    {
        int hashCode = 2107267315;

        hashCode = hashCode * -1521134295 + ID.GetHashCode();
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        hashCode = hashCode * -1521134295 + Width.GetHashCode();
        hashCode = hashCode * -1521134295 + Height.GetHashCode();

        return hashCode;
    }
}
