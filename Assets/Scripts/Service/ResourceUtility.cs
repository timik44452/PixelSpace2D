using UnityEngine;

public static class ResourceUtility
{
    public static Texture atlas
    {
        get
        {
            if (s_textureAtlas == null)
            {
                s_textureAtlas = Resources.Load<Texture>(atlasResourcePath);
            }

            return s_textureAtlas;
        }
    }
    public static Material shipMaterial
    {
        get
        {
            if(s_shipMaterial == null)
            {
                s_shipMaterial = Resources.Load<Material>(shipMaterialResourcePath);
            }

            return s_shipMaterial;
        }
    }
    public static BlockDatabase Blocks
    {
        get
        {
            if (s_blockDatabaseResource == null)
            {
                s_blockDatabaseResource = BlockDatabase.LoadDatabase(blockDatabaseResourcePath);
            }

            return s_blockDatabaseResource;
        }
    }

    public static Sprite PrebuildBlockSprite
    {
        get
        {
            if (s_prebuildBlockSprite == null)
            {
                s_prebuildBlockSprite = Resources.Load<Sprite>(prebuildBlockSpritePath);
            }

            return s_prebuildBlockSprite;
        }
    }

    private static Sprite s_prebuildBlockSprite;
    private static Texture s_textureAtlas;
    private static Material s_shipMaterial;
    private static BlockDatabase s_blockDatabaseResource;

    
    private const string atlasResourcePath = "Atlas";
    private const string prebuildBlockSpritePath = "Prebuild";
    private const string shipMaterialResourcePath = "ShipMaterial";
    private const string blockDatabaseResourcePath = "Blocks";
}
