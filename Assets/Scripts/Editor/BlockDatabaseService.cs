using UnityEditor;

public static class BlockDatabaseService
{
    [MenuItem("Tools/Blockdatabase/Check database")]
    private static void CheckDatabase()
    {
        BlockDatabase database = ResourceUtility.Blocks;
        BlockDatabase.CheckDatabase(database);
    }
}
