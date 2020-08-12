using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.ShipService
{
    public class ShipDestructionService
    {
        private Ship _ship;
        private Texture2D _mask;

        private const int resolution = 5;

        public ShipDestructionService(Ship ship)
        {
            _ship = ship;
            CreateMask();
        }

        public void Explosion(float radius, Vector2 point)
        {
            float centerX = RoundFunction(point.x, _ship.transform.lossyScale.x);
            float centerY = RoundFunction(point.y, _ship.transform.lossyScale.y);

            float roundedRadiusX = RoundFunction(radius * 0.5F, _ship.transform.lossyScale.x);
            float roundedRadiusY = RoundFunction(radius * 0.5F, _ship.transform.lossyScale.y);

            float fromX = centerX - roundedRadiusX;
            float fromY = centerY - roundedRadiusY;

            float toX = centerX + roundedRadiusX;
            float toY = centerY + roundedRadiusY;

            Vector2 localPoint = _ship.transform.worldToLocalMatrix.MultiplyPoint(new Vector2(centerX, centerY));

            for (float x = fromX; x <= toX; x += _ship.transform.lossyScale.x)
                for (float y = fromY; y <= toY; y += _ship.transform.lossyScale.y)
                {
                    Vector2 blockLocalPoint = _ship.transform.worldToLocalMatrix.MultiplyPoint(new Vector2(x, y));

                    float _x = RoundFunction(x, _ship.transform.lossyScale.x / resolution);
                    float _y = RoundFunction(y, _ship.transform.lossyScale.y / resolution);
                    float deltaX = RoundFunction(_ship.transform.lossyScale.x * 0.5F, _ship.transform.lossyScale.x / resolution);
                    float deltaY = RoundFunction(_ship.transform.lossyScale.y * 0.5F, _ship.transform.lossyScale.y / resolution);

                    for (float px = _x - deltaX; px <= _x + deltaX + _ship.transform.lossyScale.x / resolution; px += _ship.transform.lossyScale.x / resolution)
                        for (float py = _y - deltaY; py <= _y + deltaY + _ship.transform.lossyScale.y / resolution; py += _ship.transform.lossyScale.y / resolution)
                        {
                            Vector2 pixelPoint = _ship.transform.worldToLocalMatrix.MultiplyPoint(new Vector2(px, py));
                            float distance = Vector2.Distance(localPoint, pixelPoint);

                            int idx = Mathf.RoundToInt((pixelPoint.x - _ship.currentData.Bounds.x) * resolution) + resolution / 2;
                            int idy = Mathf.RoundToInt((pixelPoint.y - _ship.currentData.Bounds.y) * resolution) + resolution / 2;

                            _mask.SetPixel(idx, idy, _mask.GetPixel(idx, idy) * BurnFunction(distance, radius));
                        }

                    if (Vector2.Distance(localPoint, blockLocalPoint) > radius - 0.5F)
                    {
                        continue;
                    }

                    foreach (var block in _ship.currentData.GetBlocks(blockLocalPoint.x, blockLocalPoint.y))
                    {
                        _ship.currentData.RemoveBlock(block);
                    }
                }

            _mask.Apply();
            _ship.StartCoroutine(CheckSegmentation());
        }

        private void CreateMask()
        {
            if (_mask != null)
            {
                return;
            }

            _mask = new Texture2D(_ship.currentData.Bounds.width * resolution, _ship.currentData.Bounds.height * resolution);
            _mask.wrapMode = ResourceUtility.atlas.wrapMode;
            _mask.filterMode = ResourceUtility.atlas.filterMode;

            for (int x = 0; x < _mask.width; x++)
                for (int y = 0; y < _mask.height; y++)
                {
                    _mask.SetPixel(x, y, Color.white);
                }

            _mask.Apply();

            _ship.material.SetTexture("_Mask", _mask);
        }

        private IEnumerator CheckSegmentation()
        {
            int iteration = 0;
            var preBounds = _ship.currentData.Bounds;

            List<List<ShipBlock>> segments = new List<List<ShipBlock>>();

            foreach (var block in _ship.currentData.GetBlocks())
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

                    newShipGO.transform.position = _ship.transform.position;
                    newShipGO.transform.rotation = _ship.transform.rotation;

                    var newShip = newShipGO.AddComponent<Ship>();

                    newShip.currentRigidbody2D.gravityScale = _ship.currentRigidbody2D.gravityScale;
                    newShip.currentRigidbody2D.velocity = _ship.currentRigidbody2D.velocity;

                    foreach (var block in segments[i])
                    {
                        newShip.currentData.AddBlock(block);
                        _ship.currentData.RemoveBlock(block);
                    }

                    //newShip.CreateMask();
                    //newShip.mask.SetPixels(mask.GetPixels(
                    //    (newShip.currentData.Bounds.x - preBounds.x) * resolution,
                    //    (newShip.currentData.Bounds.y - preBounds.y) * resolution,
                    //    newShip.currentData.Bounds.width * resolution,
                    //    newShip.currentData.Bounds.height * resolution));

                    //newShip.mask.Apply();
                }
            }

            //BuildShip();
        }

        private float BurnFunction(float distance, float radius)
        {
            float noize = 0.2F;
            float burnRadius = radius * 0.5F;

            return Mathf.Clamp01((distance - burnRadius) / radius + noize * Random.value);
        }

        private float RoundFunction(float value, float step)
        {
            return Mathf.Round(value / step) * step;
        }
    }
}
