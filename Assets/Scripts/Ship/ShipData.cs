using UnityEngine;
using System.Collections.Generic;
using Game;

public class ShipData : IShipDataContainer
{
    private class ShipDataChunk
    {
        public RectInt rect;
        public List<ShipBlock> blocks;

        public ShipDataChunk(int x, int y, int width, int height)
        {
            rect = new RectInt(x, y, width, height);
            blocks = new List<ShipBlock>();
        }
    }

    public RectInt Bounds { get => _bounds; }

    private RectInt _bounds =  new RectInt(0, 0, 1, 1);
    private bool _blockCollectionChanged = true;
    private List<ShipDataChunk> _chunks = new List<ShipDataChunk>();
    private List<ShipBlock> _cachedBlockCollection = new List<ShipBlock>();

    private const int chunkSize = 5;

    public ShipBlock GetBlock(float x, float y)
    {
        var chunk = GetOrAddChunk(x, y);
        var point = new Vector2(x, y);

        return chunk.blocks.Find(_block => _block.Rect.Contains(point));
    }

    public ShipBlock GetBlockByID(float x, float y, int id)
    {
        ShipDataChunk chunk = GetOrAddChunk(x, y);

        var point = new Vector2(x, y);

        return chunk.blocks.Find(_block => _block.ID == id && _block.Rect.Contains(point));
    }

    public void AddBlock(ShipBlock block)
    {
        ShipDataChunk chunk = GetOrAddChunk(block.X, block.Y);

        chunk.blocks.Add(block);

        if (block.X < Bounds.xMin)
        {
            _bounds.xMin = block.X;
        }
        else if (block.X > Bounds.xMax)
        {
            _bounds.xMax = block.X;
        }

        if (block.Y < Bounds.yMin)
        {
            _bounds.yMin = block.Y;
        }
        else if (block.Y > Bounds.yMax)
        {
            _bounds.yMax = block.Y;
        }

        _blockCollectionChanged = true;
    }

    public void RemoveBlock(ShipBlock block)
    {
        ShipDataChunk chunk = GetOrAddChunk(block.X, block.Y);

        _blockCollectionChanged |= chunk.blocks.Remove(block);
    }

    public IEnumerable<ShipBlock> GetBlocks()
    {
        if (_blockCollectionChanged == true)
        {
            _cachedBlockCollection.Clear();

            foreach (ShipDataChunk chunk in _chunks)
            {
                _cachedBlockCollection.AddRange(chunk.blocks);
            }

            _blockCollectionChanged = false;
        }

        return _cachedBlockCollection;
    }

    public IEnumerable<ShipBlock> GetBlocksBytID(int id)
    {
        List<ShipBlock> tempCollection = new List<ShipBlock>();

        foreach (ShipDataChunk chunk in _chunks)
        {
            tempCollection.AddRange(chunk.blocks.FindAll(_block => _block.ID == id));
        }

        return tempCollection;
    }

    public IEnumerable<ShipBlock> GetBlocks(float x, float y)
    {
        var chunk = GetOrAddChunk(x, y);
        var point = new Vector2(x, y);

        return chunk.blocks.FindAll(_block => _block.Rect.Contains(point));
    }

    private ShipDataChunk GetOrAddChunk(float x, float y)
    {
        int chunk_x = Mathf.RoundToInt(x / chunkSize) * chunkSize;
        int chunk_y = Mathf.RoundToInt(y / chunkSize) * chunkSize;

        Vector2Int chunkCenter = new Vector2Int(chunk_x + chunkSize / 2, chunk_y + chunkSize / 2);

        ShipDataChunk chunk = _chunks.Find(_chunk => _chunk.rect.Contains(chunkCenter));

        if (chunk == null)
        {
            chunk = new ShipDataChunk(
                chunk_x,
                chunk_y,
                chunkSize, 
                chunkSize);

            _chunks.Add(chunk);
        }

        return chunk;
    }
}