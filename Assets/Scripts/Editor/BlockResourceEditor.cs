using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockResourceItem))]
public class BlockResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BlockResourceItem item = target as BlockResourceItem;
        Rect uv = item.uv;
        Texture preview = item.preview;
        GameObject prefab = item.prefab;

        var type = (Blocks)EditorGUILayout.EnumPopup(item.type);

        EditorGUILayout.Space();

        var width = EditorGUILayout.IntField("Width", item.Width);
        var height = EditorGUILayout.IntField("Height", item.Height);
        
        EditorGUILayout.Space();

        var useAtlas = EditorGUILayout.Toggle("Use atlas", item.useAtlas);
        var usePreview = EditorGUILayout.Toggle("Use custom prewiew", item.usePreview);
        var usePrefab = EditorGUILayout.Toggle("Use prefab", item.usePrefab);

        EditorGUILayout.Space();

        if (useAtlas == true)
        {
            uv = EditorGUILayout.RectField(item.uv);
        }

        if (usePreview == true)
        {
            preview = (Texture)EditorGUILayout.ObjectField("Preview", item.preview, typeof(Texture), false);
        }

        if (usePrefab == true)
        {
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", item.prefab, typeof(GameObject), false);
        }


        if (item.type != type)
        {
            item.type = type;

            EditorUtility.SetDirty(item);
        }

        if (item.Width != width || item.Height != height)
        {
            item.Width = width;
            item.Height = height;

            EditorUtility.SetDirty(item);
        }

        if (item.useAtlas != useAtlas || item.uv != uv)
        {
            item.useAtlas = useAtlas;
            item.uv = uv;

            EditorUtility.SetDirty(item);
        }

        if (item.usePrefab != usePrefab || item.prefab != prefab)
        {
            item.usePrefab = usePrefab;
            item.prefab = prefab;

            EditorUtility.SetDirty(item);
        }

        if (item.usePreview != usePreview || item.preview != preview)
        {
            item.usePreview = usePreview;
            item.preview = preview;

            EditorUtility.SetDirty(item);
        }
    }
}
