using Game;
using UnityEngine;

public class StrategyManager : MonoBehaviour
{
    public GameObject currentObject { get; private set; }

    public Sprite targetSprite;

    private RaycastHit2D raycastHit2D;
    private Transform target;
    private Camera m_camera;

    private void Start()
    {
        m_camera = Camera.main;
        target = CreateSelector();
    }

    private void Update()
    {
        if(GameService.managerMode != ManagerMode.StrategyManager)
        {
            target.gameObject.SetActive(false);

            return;
        }

        Vector2 worldPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);

        raycastHit2D = Physics2D.Raycast(worldPoint, Vector2.zero);

        bool isValid = raycastHit2D.collider != null && raycastHit2D.collider.GetComponent<ISelectable>() != null;

        target.gameObject.SetActive(raycastHit2D.collider != null);

        if (isValid)
        {
            Vector2 localPoint = raycastHit2D.transform.worldToLocalMatrix.MultiplyPoint(worldPoint);

            localPoint.x = Mathf.Round(localPoint.x);
            localPoint.y = Mathf.Round(localPoint.y);
            localPoint.y = Mathf.Round(localPoint.y);

            target.position = raycastHit2D.transform.localToWorldMatrix.MultiplyPoint(localPoint);
            target.rotation = Quaternion.Euler(raycastHit2D.collider.transform.eulerAngles);
            target.localScale = raycastHit2D.transform.lossyScale;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Active();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Deselect();

            if (isValid)
            {
                Select(raycastHit2D.collider.gameObject);
            }
        }
    }

    private void Active()
    {
        if(currentObject == null || raycastHit2D.collider == null)
        {
            return;
        }

        foreach (IStrategicUnit interractiveComponent in currentObject.GetComponents<IStrategicUnit>())
        {
            interractiveComponent.OnInvoke(raycastHit2D.point, raycastHit2D.collider.gameObject);
        }
    }

    private void Select(GameObject unit)
    {
        if (unit == null)
        {
            return;
        }

        foreach (ISelectable selectable in unit.GetComponents<ISelectable>())
        {
            selectable.OnSelected();
        }

        currentObject = unit;
    }

    private void Deselect()
    {
        if (currentObject == null)
        {
            return;
        }

        foreach (ISelectable selectable in currentObject.GetComponents<ISelectable>())
        {
            selectable.OnDeselected();
        }

        currentObject = null;
    }

    private Transform CreateSelector()
    {
        var target = new GameObject("Selector");
        var spriteRenderer = target.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = targetSprite;
        spriteRenderer.sortingOrder = 99;

        return target.transform;
    }
}