using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        OnHide(); 
    }
    public void Redraw()
    {
        OnRedraw();
    }

    protected virtual void OnShow() 
    {
    }

    protected virtual void OnHide()
    {
    }

    protected virtual void OnRedraw()
    {
    }
}
