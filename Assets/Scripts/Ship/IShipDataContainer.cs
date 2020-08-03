using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public interface IShipDataContainer
    {
        RectInt Bounds { get; }

        void AddBlock(ShipBlock block);
        void RemoveBlock(ShipBlock block);

        ShipBlock AddBlock(int x, int y, int id);
        ShipBlock RemoveBlock(int x, int y);

        ShipBlock GetBlock(int x, int y);
        ShipBlock GetBlockByID(int x, int y, int id);

        IEnumerable<ShipBlock> GetBlocks();
        IEnumerable<ShipBlock> GetBlocks(int x, int y);
        IEnumerable<ShipBlock> GetBlocksBytID(int id);
    }
}