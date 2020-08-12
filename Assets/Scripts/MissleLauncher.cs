using UnityEngine;

public class MissleLauncher : Weapon
{
    public Transform launcher;

    public override void Shot()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(point, Vector2.zero);

        launcher.transform.rotation = Quaternion.LookRotation((Vector3)point - transform.position);

        raycastHit2D.collider?.SendMessage("Explosion", null, SendMessageOptions.DontRequireReceiver);
    }
}
