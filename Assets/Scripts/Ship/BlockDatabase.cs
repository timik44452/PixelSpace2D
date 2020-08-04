using System;
using System.Collections.Generic;
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
        
        foreach (BlockResourceItem resourceItem in Resources.LoadAll<BlockResourceItem>(path))
        {
            blockDatabase.blocks.Add(resourceItem);
        }

        return blockDatabase;
    }

    public static void CheckDatabase(BlockDatabase database)
    {
        foreach (string name in Enum.GetNames(typeof(Blocks)))
        {
            Blocks block = (Blocks)Enum.Parse(typeof(Blocks), name);

            int count = database.blocks.FindAll(x => x.type == block).Count;

            if(count == 0)
            {
                Debug.LogWarning($"{name} resource hasn't found");
            }
            else if (count > 1)
            {
                Debug.LogWarning($"Found more that one block {name}");
            }
        }
    }

    public BlockResourceItem GetBlock(Blocks block)
    {
        return blocks.Find(x => x.type == block);
    }

    public BlockResourceItem GetBlock(int ID)
    {
        return blocks.Find(x => x.type == (Blocks)ID);
    }
}
