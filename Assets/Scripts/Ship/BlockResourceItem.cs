using System;
using UnityEngine;

[CreateAssetMenu]
public class BlockResourceItem : ScriptableObject
{
    public bool useAtlas;
    public bool usePreview;
    public bool usePrefab;

    public Rect uv;
    public Texture preview;
    public GameObject prefab;

    [NonSerialized, HideInInspector]
    public int ID;
}