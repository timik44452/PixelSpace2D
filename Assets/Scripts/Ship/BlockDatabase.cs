using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BlockDatabase
{
    public List<BlockResourceItem> blocks;

    private BlockDatabase()
    {
        blocks = new List<BlockResourceItem>();
    }

    public static BlockDatabase LoadDatabase(string path)
    {
        BlockDatabase blockDatabase = new BlockDatabase();

        foreach (int blockID in Enum.GetValues(typeof(Blocks)))
        {
            string blockName = Enum.GetName(typeof(Blocks), blockID);
            string blockPath = Path.Combine(path, blockName);

            BlockResourceItem blockResourceItem = Resources.Load<BlockResourceItem>(blockPath);

            if (blockResourceItem == null)
            {
                Debug.LogWarning($"{blockName} resource hasn't found");
            }
            else
            {
                blockResourceItem.ID = blockID;
                blockDatabase.blocks.Add(blockResourceItem);
            }
        }

        return blockDatabase;
    }

    public BlockResourceItem GetBlock(int ID)
    {
        return blocks.Find(x => x.ID == ID);
    }
}
