using System.Collections.Generic;
using UnityEngine;

public class GUIGraph
{
    private bool graphChanged = false;
    private float minValue = float.MaxValue;
    private float maxValue = float.MinValue;
    private Rect oldRect;
    private Texture2D texture;

    private Dictionary<int, List<float>> values = new Dictionary<int, List<float>>();


    public GUIGraph()
    {

    }

    public void AddValue(float value, int index = 0)
    {
        if (value < minValue)
        {
            minValue = value;
        }

        if (value > maxValue)
        {
            maxValue = value;
        }

        if(!values.ContainsKey(index))
        {
            values.Add(index, new List<float>());
        }

        values[index].Add(value);
        
        if(values[index].Count > 100)
        {
            values[index].RemoveAt(0);
        }

        graphChanged = true;
    }

    /// <summary>
    /// Invoke from OnGUI
    /// </summary>
    public void Draw(Rect rect)
    {
        if (texture == null || rect != oldRect)
        {
            CreateTexture2D(Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height));
            graphChanged = true;
            oldRect = rect;
        }

        if (graphChanged == true)
        {
            DrawData();
            graphChanged = false;
        }

        GUI.Box(rect, texture);
    }

    private void CreateTexture2D(int width, int height)
    {
        if (texture == null || texture.width != width || texture.height != height)
        {
            if (texture == null)
            {
                Object.Destroy(texture);
            }

            texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
        }
    }

    private void DrawData()
    {
        for (int x = 0; x < texture.width; x++)
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }

        int i = 0;

        foreach(var keyPair in values)
        {
            float alpha = (i + 1F) / values.Count;
            Color color = Color.Lerp(Color.green, Color.red, alpha);
            List<float> valueList = keyPair.Value;

            for (int index = 1; index < valueList.Count; index++)
            {
                float value0 = Map(valueList[index - 1]);
                float value1 = Map(valueList[index]);

                float x0 = (index - 1) / (valueList.Count - 1.0F);
                float x1 = index / (valueList.Count - 1.0F);

                Vector2 a = new Vector2(x0 * texture.width, value0 * texture.height);
                Vector2 b = new Vector2(x1 * texture.width, value1 * texture.height);

                DrawLine(a, b, color);
            }

            i++;
        }

        texture.Apply();
    }

    private void DrawLine(Vector2 a, Vector2 b, Color color)
    {
        float step = 1F / Vector2.Distance(a, b);

        for (float alpha = 0F; alpha < 1.0F + step; alpha += step)
        {
            Vector2 point = Vector2.Lerp(a, b, alpha);

            texture.SetPixel(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), color);
        }
    }

    private float Map(float value)
    {
        return (value - minValue) / (maxValue - minValue);
    }
}
