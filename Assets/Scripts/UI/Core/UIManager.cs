using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform widgetContainer;
    public GameObject widgetPrototype;
    public List<UIWidget> definedWidgets;

    private void Awake()
    {
        Instance = this;
    }

    #region Window methods
    public T GetWindow<T>() where T : IUIWindow
    {
        return transform.GetComponentInChildren<T>(true);
    }
    public IUIWindow[] GetWindows()
    {
        return transform.GetComponentsInChildren<IUIWindow>(true);
    }
    public T ShowWindow<T>() where T : IUIWindow
    {
        T window = GetWindow<T>();

        if (window != null)
        {
            window.Redraw();
            window.Show();

            return window;
        }

        return default;
    }
    public void HideWindow<T>() where T : IUIWindow
    {
        GetWindow<T>()?.Hide();
    }
    public void RedrawAllWindows()
    {
        foreach (IUIWindow window in GetWindows())
        {
            window.Redraw();
        }
    }
    #endregion

    #region Widget methods
    public T GetWidget<T>() where T : UIWidget
    {
        return transform.GetComponentInChildren<T>(true);
    }
    public UIWidget[] GetWidgets()
    {
        return transform.GetComponentsInChildren<UIWidget>(true);
    }
    public T ShowWidget<T>() where T : UIWidget
    {
        T widget = GetWidget<T>();

        if (widget == null)
        {
            GameObject widgetPrototype = definedWidgets.Find(x => x.GetComponent<T>()).gameObject;

            if(widgetPrototype == null)
            {
                widgetPrototype = this.widgetPrototype;
            }

            GameObject widgetGameObject = Instantiate(widgetPrototype, widgetContainer);

            widget = widgetGameObject.AddComponent<T>();
        }

        widget.Redraw();
        widget.Show();

        return widget;
    }
    public void HideWidget<T>() where T : UIWidget
    {
        GetWidget<T>()?.Hide();
    }
    public void RedrawAllWidgets()
    {
        foreach (UIWidget window in GetWidgets())
        {
            window.Redraw();
        }
    }
    #endregion

    #region 
    public void ShowBar()
    {

    }
    #endregion
}
