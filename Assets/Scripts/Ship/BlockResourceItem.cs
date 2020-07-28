using System;
using UnityEngine;

[CreateAssetMenu]
public class BlockResourceItem : ScriptableObject
{
    public Rect uv;
    public GameObject prefab;

    

    [NonSerialized, HideInInspector]
    public int ID;
}