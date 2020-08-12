using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public interface IShipDataContainer
    {
        RectInt Bounds { get; }

        void AddBlock(ShipBlock block);
        void RemoveBlock(ShipBlock block);

        ShipBlock GetBlock(float x, float y);
        ShipBlock GetBlockByID(float x, float y, int id);

        IEnumerable<ShipBlock> GetBlocks();
        IEnumerable<ShipBlock> GetBlocks(float x, float y);
        IEnumerable<ShipBlock> GetBlocksBytID(int id);
    }
}