using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Vector2 point;

    public abstract void Shot();
}
