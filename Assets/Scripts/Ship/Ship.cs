using Game;
using System;
using System.Collections;
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

    private Texture2D mask;
    private GameObject shipBody;
    private PolygonCollider2D polygonCollider2D;
    private int resolution = 5;

    private void Awake()
    {
        currentRigidbody2D = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
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
        if (shipBody != null)
        {
            Destroy(shipBody);
        }

        CreateMask();

        shipBody = ShipBuilder.Build(currentData);

        shipBody.GetComponent<MeshRenderer>().material.SetTexture("_Mask", mask);

        transform.localScale = Vector3.one * 0.5F;

        shipBody.transform.parent = transform;
        shipBody.transform.localRotation = Quaternion.identity;
        shipBody.transform.localPosition = Vector3.zero;
        shipBody.transform.localScale = Vector3.one;

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

    private void CreateMask()
    {
        if (mask != null)
        {
            return;
        }

        mask = new Texture2D(currentData.Bounds.width * resolution, currentData.Bounds.height * resolution);
        mask.wrapMode = ResourceUtility.atlas.wrapMode;
        mask.filterMode = ResourceUtility.atlas.filterMode;

        for (int x = 0; x < mask.width; x++)
            for (int y = 0; y < mask.height; y++)
            {
                mask.SetPixel(x, y, Color.white);
            }

        mask.Apply();
    }

    public void Explosion(float radius, Vector2 point)
    {
        float burnRadiusMult = 0.21F;

        for (float x = point.x - radius; x < point.x + radius; x += transform.localScale.x / resolution)
            for (float y = point.y - radius; y < point.y + radius; y += transform.localScale.y / resolution)
            {
                Vector2 localPoint = transform.worldToLocalMatrix.MultiplyPoint(new Vector2(x, y));
                float distance = Vector2.Distance(new Vector2(x, y), point);

                if (distance < radius - 0.5F)
                {
                    int block_x = Mathf.RoundToInt(localPoint.x);
                    int block_y = Mathf.RoundToInt(localPoint.y);

                    currentData.RemoveBlock(block_x, block_y);
                }

                int idx = Mathf.RoundToInt((localPoint.x - currentData.Bounds.x) * resolution);
                int idy = Mathf.RoundToInt((localPoint.y - currentData.Bounds.y) * resolution);

                if (idx >= 0 && idx < mask.width && idy >= 0 && idy < mask.height)
                {
                    float burn = (distance - radius) / (UnityEngine.Random.value * burnRadiusMult);

                    Color color = mask.GetPixel(idx, idy) * burn;
                    mask.SetPixel(idx, idy, color);
                }
            }

        mask.Apply();
        StartCoroutine(CheckSegmentation());
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

    private IEnumerator CheckSegmentation()
    {
        int iteration = 0;

        List<List<ShipBlock>> segments = new List<List<ShipBlock>>();

        foreach (var block in currentData.GetBlocks())
        {
            List<int> segmentIndexes = new List<int>();

            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i].Find(x =>
                    (x.X == block.X + 1 && x.Y == block.Y) ||
                    (x.X == block.X - 1 && x.Y == block.Y) ||
                    (x.X == block.X && x.Y == block.Y + 1) ||
                    (x.X == block.X && x.Y == block.Y - 1)) != null)
                {
                    segmentIndexes.Add(i);
                }
            }

            if (segmentIndexes.Count == 0)
            {
                segments.Add(new List<ShipBlock>());
                segments[segments.Count - 1].Add(block);
            }
            else if (segmentIndexes.Count == 1)
            {
                segments[segmentIndexes[0]].Add(block);
            }
            else
            {
                List<ShipBlock> newSegment = new List<ShipBlock>();

                segmentIndexes = segmentIndexes.OrderByDescending(x => x).ToList();

                while (segmentIndexes.Count > 0)
                {
                    int index = segmentIndexes[0];
                    segmentIndexes.RemoveAt(0);

                    newSegment.AddRange(segments[index]);
                    segments.RemoveAt(index);
                }

                newSegment.Add(block);

                segments.Add(newSegment);
            }

            if (iteration % 50 == 0)
            {
                yield return null;
            }

            iteration++;
        }

        if (segments.Count > 1)
        {
            segments = segments.OrderByDescending(x => x.Count).ToList();

            for (int i = 1; i < segments.Count; i++)
            {
                var newShipGO = new GameObject();

                newShipGO.transform.position = transform.position;
                newShipGO.transform.rotation = transform.rotation;

                var newShip = newShipGO.AddComponent<Ship>();

                newShip.currentRigidbody2D.gravityScale = currentRigidbody2D.gravityScale;
                newShip.currentRigidbody2D.velocity = currentRigidbody2D.velocity;

                foreach (var block in segments[i])
                {
                    newShip.currentData.AddBlock(block);
                    currentData.RemoveBlock(block);
                }
            }

            BuildShip();
        }
    }
}