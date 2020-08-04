using UnityEngine;

[CreateAssetMenu]
public class BlockResourceItem : ScriptableObject
{
    public Blocks type;
    
    public int Width = 1;
    public int Height = 1;

    public bool useAtlas;
    public bool usePreview;
    public bool usePrefab;

    public Rect uv;
    public Texture preview;
    public GameObject prefab;
}