using UnityEngine;

public class WeaponControlTerminal : MonoBehaviour
{
    private Camera _camera;
    private Weapon _weapon;

    private void Start()
    {
        _camera = Camera.main;
        _weapon = GetComponentInParent<Ship>().GetComponentInChildren<Weapon>();
    }
    private void Update()
    {
        if(_weapon == null)
        {
            _weapon = GetComponentInParent<Ship>().GetComponentInChildren<Weapon>();
            return;
        }

        _weapon.point = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            _weapon.Shot();
        }
    }
}
