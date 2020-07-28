﻿using Game;
using UnityEngine;

public class PrebuildBlock : InterractiveObject, ISelectable
{
    public int blockID = 0;
    public float rotation = 0;
    public Ship currentShip;

    private float progress = 0.0F;
    private float endProgress = 10F;
    

    private void FixedUpdate()
    {
        if(currentShip == null)
        {
            Destroy(gameObject);
        }

        progress += involvedUnits.Count * Time.fixedDeltaTime;

        if (progress >= endProgress)
        {
            int localPositionX = Mathf.RoundToInt(transform.localPosition.x);
            int localPositionY = Mathf.RoundToInt(transform.localPosition.y);

            var block = currentShip.currentData.AddBlock(localPositionX, localPositionY, blockID);

            block.Rotation = rotation;

            currentShip.BuildShip();

            Destroy(gameObject);
        }
    }

    public void OnSelected()
    {
    }

    public void OnDeselected()
    {
    }
}
