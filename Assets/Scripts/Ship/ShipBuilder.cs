using UnityEngine;
using System.Collections.Generic;
using Game;

public static class ShipBuilder
{
    public static GameObject Build(IShipDataContainer data)
    {
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector2> uvs2 = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();

        var ship = new GameObject("Ship mesh");

        var meshFilter = ship.AddComponent<MeshFilter>();
        var meshRenderer = ship.AddComponent<MeshRenderer>();

        float delta = 0.5F;
        float uv2Width = 1F / data.Bounds.width;
        float uv2Height = 1F / data.Bounds.height;

        foreach (ShipBlock block in data.GetBlocks())
        {
            BlockResourceItem blockResource = ResourceUtility.Blocks.GetBlock(block.ID);

            if (blockResource == null)
            {
                continue;
            }

            Vector3 localPosition = new Vector3(block.X, block.Y);
            Quaternion localRotation = Quaternion.Euler(0, 0, block.Rotation);
            
            if (blockResource.prefab != null)
            {
                GameObject prefab = Object.Instantiate(blockResource.prefab);

                prefab.transform.parent = ship.transform;
                prefab.transform.localPosition = localPosition;
                prefab.transform.localRotation = localRotation; 
            }

            int verticesIndex = vertices.Count;
            Matrix4x4 rotateMatrix = Matrix4x4.Rotate(localRotation);

            Rect uv = blockResource.uv;
            Rect uv2 = new Rect((block.X - data.Bounds.x) / (float)data.Bounds.width, (block.Y - data.Bounds.y) / (float)data.Bounds.height, uv2Width, uv2Height);

            Vector2 uv_center = new Vector2(uv.x + uv.width * 0.5F, uv.y + uv.height * 0.5F);
            Vector2 uv_up = rotateMatrix.MultiplyPoint(new Vector2(0, uv.height * 0.5F));
            Vector2 uv_right = rotateMatrix.MultiplyPoint(new Vector2(uv.width * 0.5F, 0));
            

            vertices.Add(localPosition + new Vector3(-delta, -delta));
            vertices.Add(localPosition + new Vector3(-delta, delta));
            vertices.Add(localPosition + new Vector3(delta, delta));
            vertices.Add(localPosition + new Vector3(delta, -delta));


            triangles.Add(verticesIndex);
            triangles.Add(verticesIndex + 1);
            triangles.Add(verticesIndex + 2);
            triangles.Add(verticesIndex);
            triangles.Add(verticesIndex + 2);
            triangles.Add(verticesIndex + 3);


            uvs.Add(uv_center - uv_up - uv_right);
            uvs.Add(uv_center + uv_up - uv_right);
            uvs.Add(uv_center + uv_up + uv_right);
            uvs.Add(uv_center - uv_up + uv_right);

            uvs2.Add(new Vector2(uv2.xMin, uv2.yMin));
            uvs2.Add(new Vector2(uv2.xMin, uv2.yMax));
            uvs2.Add(new Vector2(uv2.xMax, uv2.yMax));
            uvs2.Add(new Vector2(uv2.xMax, uv2.yMin));
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.uv2 = uvs2.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;
        meshRenderer.material = ResourceUtility.shipMaterial;

        return ship;
    }
    public static GameObject Build(IShipDataContainer data, IEnumerable<ShipBlock> shipBlocks)
    {
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector2> uvs2 = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();

        var ship = new GameObject("Ship mesh");

        var meshFilter = ship.AddComponent<MeshFilter>();
        var meshRenderer = ship.AddComponent<MeshRenderer>();

        float delta = 0.5F;
        float uv2Width = 1F / data.Bounds.width;
        float uv2Height = 1F / data.Bounds.height;

        foreach (ShipBlock block in shipBlocks)
        {
            BlockResourceItem blockResource = ResourceUtility.Blocks.GetBlock(block.ID);

            if (blockResource == null)
            {
                continue;
            }

            Vector3 localPosition = new Vector3(block.X, block.Y);
            Quaternion localRotation = Quaternion.Euler(0, 0, block.Rotation);

            if (blockResource.prefab != null)
            {
                GameObject prefab = Object.Instantiate(blockResource.prefab);

                prefab.transform.parent = ship.transform;
                prefab.transform.localPosition = localPosition;
                prefab.transform.localRotation = localRotation;
            }

            int verticesIndex = vertices.Count;
            Matrix4x4 rotateMatrix = Matrix4x4.Rotate(localRotation);

            Rect uv = blockResource.uv;
            Rect uv2 = new Rect((block.X - data.Bounds.x) / (float)data.Bounds.width, (block.Y - data.Bounds.y) / (float)data.Bounds.height, uv2Width, uv2Height);

            Vector2 uv_center = new Vector2(uv.x + uv.width * 0.5F, uv.y + uv.height * 0.5F);
            Vector2 uv_up = rotateMatrix.MultiplyPoint(new Vector2(0, uv.height * 0.5F));
            Vector2 uv_right = rotateMatrix.MultiplyPoint(new Vector2(uv.width * 0.5F, 0));


            vertices.Add(localPosition + new Vector3(-delta, -delta));
            vertices.Add(localPosition + new Vector3(-delta, delta));
            vertices.Add(localPosition + new Vector3(delta, delta));
            vertices.Add(localPosition + new Vector3(delta, -delta));


            triangles.Add(verticesIndex);
            triangles.Add(verticesIndex + 1);
            triangles.Add(verticesIndex + 2);
            triangles.Add(verticesIndex);
            triangles.Add(verticesIndex + 2);
            triangles.Add(verticesIndex + 3);


            uvs.Add(uv_center - uv_up - uv_right);
            uvs.Add(uv_center + uv_up - uv_right);
            uvs.Add(uv_center + uv_up + uv_right);
            uvs.Add(uv_center - uv_up + uv_right);

            uvs2.Add(new Vector2(uv2.xMin, uv2.yMin));
            uvs2.Add(new Vector2(uv2.xMin, uv2.yMax));
            uvs2.Add(new Vector2(uv2.xMax, uv2.yMax));
            uvs2.Add(new Vector2(uv2.xMax, uv2.yMin));
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.uv2 = uvs2.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;
        meshRenderer.material = ResourceUtility.shipMaterial;

        return ship;
    }
}
