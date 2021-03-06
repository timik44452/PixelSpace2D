﻿using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    private Ship currentShip;
    private Camera m_camera;
    private Transform blockPrototype;

    private int blockID = 0;
    private float rotation = 0;
    private Vector3 scale = Vector3.one;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_camera = Camera.main;
    }

    private void Update()
    {
        if (currentShip == null || GameService.managerMode != ManagerMode.BuildManager)
        {
            EndBuild();

            return;
        }

        bool selectorActive = false;
        Vector2 worldPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int localPoint = currentShip.transform.worldToLocalMatrix.MultiplyPoint(worldPoint).ToVector2Int();
        ShipBlock block = currentShip.currentData.GetBlock(localPoint.x, localPoint.y);

        if ((blockID == 0 && block == null) || (block != null && block.ID == 0))
        {
            foreach (Vector2Int offset in ServiceData.offsets)
            {
                block = currentShip.currentData.GetBlock(localPoint.x + offset.x, localPoint.y + offset.y);

                if (block != null)
                {
                    selectorActive = true;
                    break;
                }
            }
        }

        blockPrototype.gameObject.SetActive(selectorActive);

        if (selectorActive && Input.GetKeyDown(KeyCode.R))
        {
            rotation = Mathf.Repeat(rotation - 90, 360);
        }

        if (selectorActive && Input.GetMouseButtonDown(0))
        {
            Build(Mathf.RoundToInt(localPoint.x), Mathf.RoundToInt(localPoint.y));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndBuild();
        }

        Vector2 position = currentShip.transform.localToWorldMatrix.MultiplyPoint((Vector2)localPoint);
        
        blockPrototype.position = new Vector3(position.x, position.y, m_camera.transform.position.z + 1);
        blockPrototype.rotation = Quaternion.Euler(0, 0, currentShip.transform.eulerAngles.z + rotation);
        blockPrototype.localScale = Vector3.Scale(scale, currentShip.transform.lossyScale);
    }

    public void BeginBuild(int blockID, Ship ship)
    {
        GameService.managerMode = ManagerMode.BuildManager;
        this.blockID = blockID;
        currentShip = ship;

        blockPrototype = CreatePrototype();
    }

    public void EndBuild()
    {
        GameService.managerMode = ManagerMode.StrategyManager;

        if (blockPrototype)
        {
            Destroy(blockPrototype.gameObject);
        }
    }

    public void Build(int x, int y)
    {
        GameObject prebuildedBlockGO = new GameObject($"Prebuilded block {blockID}");

        prebuildedBlockGO.transform.parent = currentShip.transform;
        prebuildedBlockGO.transform.localRotation = Quaternion.identity;
        prebuildedBlockGO.transform.localPosition = new Vector3(x, y, -1);
        prebuildedBlockGO.transform.localScale = Vector3.Scale(scale, currentShip.transform.lossyScale);

        var spriteRenderer = prebuildedBlockGO.AddComponent<SpriteRenderer>();
        var prebuildBlock = prebuildedBlockGO.AddComponent<PrebuildBlock>();

        spriteRenderer.sprite = ResourceUtility.PrebuildBlockSprite;
        prebuildBlock.blockID = blockID;
        prebuildBlock.currentShip = currentShip;
        prebuildBlock.rotation = rotation;

        prebuildedBlockGO.AddComponent<BoxCollider2D>();
    }

    private Transform CreatePrototype()
    {
        var block = ResourceUtility.Blocks.GetBlock(blockID);
        var target = new GameObject("Selector");

        if (block != null)
        {
            scale = new Vector3(block.Width, block.Height, 1);

            if (block.useAtlas == false && block.usePrefab == true)
            {
                return CreateViewDublicate(block.prefab);
            }

            var renderer = target.AddComponent<MeshRenderer>();
            var meshFilter = target.AddComponent<MeshFilter>();
            var mesh = new Mesh()
            {
                vertices = new Vector3[]
                {
                    new Vector3(-0.5F, -0.5F),
                    new Vector3(-0.5F, 0.5F),
                    new Vector3(0.5F, 0.5F),
                    new Vector3(0.5F, -0.5F)
                },

                triangles = new int[]
                {
                    0,
                    1,
                    2,

                    0,
                    2,
                    3
                },

                uv = GetUV(block)
            };

            mesh.RecalculateNormals();

            if (block.useAtlas == true)
            {
                renderer.material = ResourceUtility.shipMaterial;
            }

            meshFilter.sharedMesh = mesh;
        }

        return target.transform;
    }

    private Transform CreateViewDublicate(GameObject prefab)
    {
        GameObject view = Instantiate(prefab);

        foreach (Component component in view.GetComponentsInChildren<Component>())
        {
            if (component is Transform ||
                component is Renderer ||
                component is MeshFilter)
            {
                continue;
            }

            Destroy(component);
        }

        return view.transform;
    }

    private Vector2[] GetUV(BlockResourceItem item)
    {
        if (item.useAtlas == true)
        {
            return new Vector2[]
                {
                    new Vector2(item.uv.xMin, item.uv.yMin),
                    new Vector2(item.uv.xMin, item.uv.yMax),
                    new Vector2(item.uv.xMax, item.uv.yMax),
                    new Vector2(item.uv.xMax, item.uv.yMin)
                };
        }

        return ServiceData.defaultUV;
    }
}
