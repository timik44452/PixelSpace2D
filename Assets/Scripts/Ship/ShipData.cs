using UnityEngine;
using System.Collections.Generic;

public class ShipData : IShipDataContainer
{
    public RectInt Bounds
    {
        get => m_bounds;
    }

    private List<ShipBlock> m_blocks = new List<ShipBlock>();
    private RectInt m_bounds = new RectInt(0, 0, 1, 1);

    public ShipBlock GetBlock(int x, int y)
    {
        return m_blocks.Find(_block => _block.X == x && _block.Y == y); ;
    }

    public ShipBlock GetBlockByID(int x, int y, int id)
    {
        return m_blocks.Find(_block => _block.ID == id && _block.X == x && _block.Y == y); ;
    }

    public void AddBlock(ShipBlock block)
    {
        m_blocks.Add(block);
    }

    public void RemoveBlock(ShipBlock block)
    {
        m_blocks.Remove(block);
    }

    public ShipBlock AddBlock(int x, int y, int blockID)
    {
        if (x < Bounds.xMin)
        {
            m_bounds.xMin = x;
        }
        else if (x > Bounds.xMax)
        {
            m_bounds.xMax = x;
        }

        if (y < Bounds.yMin)
        {
            m_bounds.yMin = y;
        }
        else if (y > Bounds.yMax)
        {
            m_bounds.yMax = y;
        }

        ShipBlock shipBlock = new ShipBlock();

        shipBlock.X = x;
        shipBlock.Y = y;
        shipBlock.ID = blockID;

        m_blocks.Add(shipBlock);

        return shipBlock;
    }

    public ShipBlock RemoveBlock(int x, int y)
    {
        ShipBlock block = GetBlock(x, y);

        if (block == null)
        {
            return null;
        }

        m_blocks.Remove(block);

        return block;
    }

    public IEnumerable<ShipBlock> GetBlocks()
    {
        return m_blocks;
    }

    public IEnumerable<ShipBlock> GetBlocksBytID(int id)
    {
        return m_blocks.FindAll(x => x.ID == id);
    }
}