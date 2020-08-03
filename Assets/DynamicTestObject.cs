using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTestObject : MonoBehaviour
{
    private class Rope
    {
        public float strong = 1.0F;
        public float distance;

        public Joint from;
        public Joint to;

        public Rope(Joint from, Joint to)
        {
            this.from = from;
            this.to = to;

            distance = Vector2.Distance(from.position, to.position);
        }
    }

    private class Joint
    {
        public Vector2 position;

        public Joint(Vector2 point)
        {
            position = point;
        }

        public static implicit operator Joint (Vector2 point)
        {
            return new Joint(point);
        }
    }


    private List<Rope> _ropes = new List<Rope>();
    private List<Joint> _joints = new List<Joint>();

    private void Start()
    {
        //       w
        //  +----+----+
        //  |    |    |
        //  +    +----+ h
        //  |         |
        //  +----+----+

        float half_w = 0.5F;
        float half_h = 0.5F;

        _joints.Add(new Vector2(-half_w, -half_h));
        _joints.Add(new Vector2(-half_w, 0));
        _joints.Add(new Vector2(-half_w, half_h));
        _joints.Add(new Vector2(0, half_h));
        _joints.Add(new Vector2(half_w, half_h));
        _joints.Add(new Vector2(half_w, 0));
        _joints.Add(new Vector2(half_w, -half_h));
        _joints.Add(new Vector2(0, -half_h));

        for (int i = 0; i < _joints.Count; i++)
        {
            int index0 = i;
            int index1 = (int)Mathf.Repeat(i + 1, _joints.Count);

            _ropes.Add(new Rope(_joints[index0], _joints[index1]));
        }
    }
}
