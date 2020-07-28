using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShipCameraFollow : MonoBehaviour
{
    [System.Serializable]
    public class Limit
    {
        public float minOrthographicSize = 1.0F;
        public float maxOrthographicSize = 5.0F;
    }

    [System.Serializable]
    public class SpeedParams
    {
        public float followSpeed = 1.0F;
        public float zoomSpeed = 1.0F;
        public float zoomDamping = 1.0F;
    }

    public Ship target;

    [Space]
    public Vector3 offset;

    [Space]
    public Limit orthographicLimit;
    public SpeedParams speedParams;

    #region Service
    private Camera m_camera;
    private Vector3 _offset;
    private float _minOrthographicSize;
    private float _maxOrthographicSize;
    private float _zoomDelta;
    #endregion

    private void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        UpdateOrthographicLimits();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float zoom = Input.GetAxis("Mouse ScrollWheel") * (_maxOrthographicSize - _minOrthographicSize);

            if (Input.GetMouseButton(2))
            {
                _offset -= new Vector3(mouseX * target.transform.localScale.x, mouseY * target.transform.localScale.y);
                _offset.x = Mathf.Clamp(_offset.x, -target.Bounds.width * target.transform.localScale.x, target.Bounds.width * target.transform.localScale.x);
                _offset.y = Mathf.Clamp(_offset.y, -target.Bounds.width * target.transform.localScale.y, target.Bounds.width * target.transform.localScale.y);
            }
            
            if (Mathf.Abs(zoom) < 0.1F)
            {
                _zoomDelta = Mathf.Lerp(_zoomDelta, zoom, speedParams.zoomDamping * 5 * Time.deltaTime);
            }
            else
            {
                _zoomDelta = Mathf.Lerp(_zoomDelta, zoom, speedParams.zoomDamping * Time.deltaTime);
            }

            m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize - _zoomDelta * speedParams.zoomSpeed, _minOrthographicSize, _maxOrthographicSize);
            m_camera.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (speedParams.followSpeed == 0)
            {
                transform.position = target.transform.position + offset + _offset;
            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, target.transform.position + offset + _offset, speedParams.followSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdateOrthographicLimits() 
    {
        _minOrthographicSize = Mathf.Min(target.Bounds.width, target.Bounds.height) * orthographicLimit.minOrthographicSize;
        _maxOrthographicSize = Mathf.Max(target.Bounds.width, target.Bounds.height) * orthographicLimit.maxOrthographicSize;
    }
}
