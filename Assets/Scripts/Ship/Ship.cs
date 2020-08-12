using Game;
using Game.ShipService;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D), typeof(Rigidbody2D))]
public class Ship : MonoBehaviour, ISelectable
{
    [HideInInspector]
    public RectInt Bounds
    {
        get => currentData.Bounds;
    }

    public IShipDataContainer currentData { get; } = new ShipData();
    public Rigidbody2D currentRigidbody2D { get; private set; }
    public Material material { get; private set; }

    private GameObject shipBody;
    private PolygonCollider2D polygonCollider2D;
    private ShipDestructionService destructionService;
    
    private void Awake()
    {
        shipBody = CreateBody();
        currentRigidbody2D = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        material = shipBody.GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        if (GetComponent<ShipExample>())
        {
            GetComponent<ShipExample>().Build(currentData);
        }

        if (currentData == null)
        {
            return;
        }

        BuildShip();
    }


    public void BuildShip()
    {
        if (destructionService == null)
        {
            destructionService = new ShipDestructionService(this);
        }

        ShipBuilder.Build(shipBody, currentData);
        
        transform.localScale = Vector3.one * 0.5F;

        UpdateCollider();
    }

    public T GetInterractiveObject<T>() where T : InterractiveObject
    {
        return GetComponentInChildren<T>();
    }

    public T[] GetInterractiveObjects<T>() where T : InterractiveObject
    {
        return GetComponentsInChildren<T>(); ;
    }

    private void UpdateCollider()
    {
        List<Vector2> points = new List<Vector2>();
        Vector2Int[] offsets = new Vector2Int[]
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
        };

        Vector2Int dir = Vector2Int.zero;
        Vector2Int pos = Vector2Int.zero;

        #region 0
        foreach (var block in currentData.GetBlocks())
        {
            bool isBreak = false;

            for (int i = 0; i < ServiceData.offsets.Length; i++)
            {
                dir = ServiceData.offsets[i];

                if (GetEdges(block.X, block.Y) < 15 && GetEdges(block.X + dir.x, block.Y + dir.y) < 15)
                {
                    pos = new Vector2Int(block.X, block.Y);
                    isBreak = true;
                    break;
                }
            }

            if (isBreak)
            {
                break;
            }
        }
        #endregion
        #region 1
        int iteration = 0;

        while (++iteration < currentData.GetBlocks().Count())
        {
            ShipBlock nextBlock = null;

            foreach(Vector2Int _offset in offsets)
            {
                if (_offset == -dir)
                {
                    continue;
                }

                var point = pos + _offset;
                var tempBlock = currentData.GetBlock(point.x, point.y);
                
                if (tempBlock == null || points.Contains(point))
                {
                    continue;
                }

                var edges = GetEdges(point.x, point.y);

                if (edges >= 15)
                {
                    continue;
                }

                nextBlock = tempBlock;
            }

            if (nextBlock == null)
            {
                break;
            }

            points.Add(pos);
            dir = new Vector2Int(nextBlock.X, nextBlock.Y) - pos;
            pos += dir;
        }
        #endregion

        polygonCollider2D.points = points.ToArray();
    }

    public void OnSelected()
    {
        var widget = UIManager.Instance.ShowWidget<ShipManagmentWidget>();
        widget.currentShip = this;
    }

    public void OnDeselected()
    {
        UIManager.Instance.HideWidget<ShipManagmentWidget>();
    }

    private int GetEdges(int x, int y)
    {
        int edges = 0;

        for (int i = 0; i < ServiceData.offsets.Length; i++)
        {
            Vector2Int offset = ServiceData.offsets[i];

            if (currentData.GetBlock(x + offset.x, y + offset.y) != null)
            {
                edges |= 1 << i;
            }
        }

        return edges;
    }

    private GameObject CreateBody()
    {
        GameObject shipBody = new GameObject("Ship mesh");

        shipBody.transform.parent = transform;
        shipBody.transform.localRotation = Quaternion.identity;
        shipBody.transform.localPosition = Vector3.zero;
        shipBody.transform.localScale = Vector3.one;

        shipBody.AddComponent<MeshFilter>();
        shipBody.AddComponent<MeshRenderer>().material = ResourceUtility.shipMaterial;

        return shipBody;
    }
}