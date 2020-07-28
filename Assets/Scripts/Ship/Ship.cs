using Game;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D), typeof(Rigidbody2D))]
public class Ship : MonoBehaviour, ISelectable
{
    class ColliderBuildingItem
    {
        public float angle;
        public Vector2 point;

        public ColliderBuildingItem(Vector2 point)
        {
            this.point = point;
            angle = Vector2.Angle(Vector2.up, point.normalized) * Mathf.Sign(-point.x);
        }
    }

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

        currentRigidbody2D = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        

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

    private void CreateMask()
    {
        if(mask != null)
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
                    float burn = (distance - radius) / (Random.value * burnRadiusMult);

                    Color color = mask.GetPixel(idx, idy) * burn;
                    mask.SetPixel(idx, idy,  color);
                }
            }
        
        mask.Apply();
    }

    private void UpdateCollider()
    {
        List<Vector2> points = new List<Vector2>();
        List<ColliderBuildingItem> prePoints = new List<ColliderBuildingItem>();

        foreach (var block in currentData.GetBlocks())
        {
            if (currentData.GetBlock(block.X + 1, block.Y) != null &&
                currentData.GetBlock(block.X - 1, block.Y) != null &&
                currentData.GetBlock(block.X, block.Y + 1) != null &&
                currentData.GetBlock(block.X, block.Y - 1) != null)
            {
                continue;
            }

            var item = new ColliderBuildingItem(new Vector2(block.X, block.Y));

            if (prePoints.Find(x => x.point == item.point) == null)
            {
                prePoints.Add(item);
            }
        }

        if (prePoints.Count == 0)
        {
            return;
        }

        points = (from item in prePoints 
                  orderby item.angle 
                  select item.point).ToList();

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

    public int snapshot = 0;
    private List<List<ShipBlock[]>> segmentsnapshots = new List<List<ShipBlock[]>>();
    private IEnumerator CheckSegmentation()
    {
        int iteration = 0;
        segmentsnapshots.Clear();

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

            var tempBuffer = new List<ShipBlock[]>();

            foreach (var item in segments)
                tempBuffer.Add(item.ToArray());

            segmentsnapshots.Add(tempBuffer);

            if (iteration % 50 == 0)
            {
                yield return null;
            }

            iteration++;
        }

        if (segments.Count > 1)
        {
            for (int i = 1; i < segments.Count; i++)
            {
                var newShipGO = new GameObject();
                var newShip = newShipGO.AddComponent<Ship>();

                foreach (var block in segments[i])
                {
                    newShip.currentData.AddBlock(block);
                    currentData.RemoveBlock(block);
                }
            }

            BuildShip();
        }
    }
    private void OnDrawGizmos()
    {
        if (snapshot >= 0 && snapshot < segmentsnapshots.Count)
        {
            List<ShipBlock[]> currentSnapshot = segmentsnapshots[snapshot];

            for (int i = 0; i < currentSnapshot.Count; i++)
            {
                float alpha = (i + 1.0F) / currentSnapshot.Count;
                Color color = Color.Lerp(Color.red, Color.green, alpha);

                Gizmos.color = color;

                foreach (var block in currentSnapshot[i])
                {
                    Vector3 point = new Vector3(block.X, block.Y);
                    Gizmos.DrawCube(point, Vector3.one);
                }
            }
        }
    }
}
