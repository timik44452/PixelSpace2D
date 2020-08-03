using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game;

public class PathFinder : MonoBehaviour
{
    public class PathTracingItem
    {
        public float G;
        public float H;
        public Vector2Int point;
        public PathTracingItem root;

        public static PathTracingItem Create(Vector2Int point, Vector2Int from, Vector2Int to, PathTracingItem root = null)
        {
            PathTracingItem currentCell = new PathTracingItem();

            currentCell.H = Vector2.Distance(point, to);

            if (root == null)
            {
                currentCell.G = Vector2.Distance(from, point);
            }
            else
            {
                currentCell.G = Vector2.Distance(from, root.point) + Vector2.Distance(root.point, point);
            }

            currentCell.point = point;
            currentCell.root = root;

            return currentCell;
        }

        #region Overrides
        public override bool Equals(object obj)
        {
            return obj is PathTracingItem item &&
                   point.Equals(item.point);
        }

        public override int GetHashCode()
        {
            return 1595967545 + EqualityComparer<Vector2Int>.Default.GetHashCode(point);
        }
        #endregion
    }

    private static Vector2Int[] offsets = new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
        };


    public List<Vector2Int> FindPath(Vector2 from, Vector2 to, IShipDataContainer data)
    {
        Vector2Int _from = new Vector2Int(Mathf.RoundToInt(from.x), Mathf.RoundToInt(from.y));
        Vector2Int _to = new Vector2Int(Mathf.RoundToInt(to.x), Mathf.RoundToInt(to.y));

        return FindPath(_from, _to, data);
    }

    public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to, IShipDataContainer data)
    {
        PathTracingItem currentCell;
        List<Vector2Int> path = new List<Vector2Int>();
        List<PathTracingItem> viewed = new List<PathTracingItem>();
        List<PathTracingItem> viewing = new List<PathTracingItem>();

        if (from == to)
        {
            return path;
        }

        viewing.Add(PathTracingItem.Create(from, from, to));

        while (viewing.Count > 0)
        {
            currentCell = FindPathTracingItem(viewing);

            foreach (Vector2Int offset in offsets)
            {
                var isValid = false;
                var point = offset + currentCell.point;
                var blocks = data.GetBlocks(point.x, point.y);

                foreach (var block in blocks)
                {
                    if (block.ID == (int)Blocks.Floor)
                    {
                        isValid = true;
                    }

                    if (block.ID != (int)Blocks.Floor)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    viewing.Add(PathTracingItem.Create(point, from, to, currentCell));
                }

                if (point == to)
                {
                    path = Trace(currentCell);

                    return path;
                }
            }

            viewed.Add(currentCell);
            viewing.RemoveAll(x => viewed.Contains(x));

            if (viewed.Count > 1000)
            {
                return path;
            }
        }

        return path;
    }

    private PathTracingItem FindPathTracingItem(IEnumerable<PathTracingItem> items)
    {
        if(items.Count() == 0)
        {
            return null;
        }

        PathTracingItem tracingItem = items.First();

        foreach (PathTracingItem item in items)
        {
            if (item.H + item.G < tracingItem.H + tracingItem.G)
            {
                tracingItem = item;
            }
        }

        return tracingItem;
    }

    private List<Vector2Int> Trace(PathTracingItem item)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        PathTracingItem currentItem = item;

        while (currentItem != null)
        {
            path.Add(currentItem.point);
            currentItem = currentItem.root;
        }

        path.Reverse();

        return path;
    }
}
