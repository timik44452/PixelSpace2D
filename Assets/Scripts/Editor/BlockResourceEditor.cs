using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockResourceItem))]
public class BlockResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BlockResourceItem item = target as BlockResourceItem;

        item.useAtlas = EditorGUILayout.Toggle("Use atlas", item.useAtlas);

        if (item.useAtlas == true)
        {
            item.uv = EditorGUILayout.RectField(item.uv);
        }

        item.usePreview = EditorGUILayout.Toggle("Use custom prewiew", item.usePreview);

        if (item.usePreview == true)
        {
            item.preview = (Texture)EditorGUILayout.ObjectField("Preview", item.preview, typeof(Texture), false);
        }

        item.usePrefab = EditorGUILayout.Toggle("Use prefab", item.usePrefab);

        if (item.usePrefab == true)
        {
            item.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", item.prefab, typeof(GameObject), false);
        }
    }
}
