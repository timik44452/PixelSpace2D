using UnityEngine.UI;

public class ShipManagmentWidget : UIWidget
{
    public Ship currentShip;

    private void Start()
    {
        Button closeButton = gameObject.GetElement<Button>("close");
        Button buildButton = gameObject.GetElement<Button>("build");
        Button statisticButton = gameObject.GetElement<Button>("statistic");

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }

        if (buildButton != null)
        {
            buildButton.onClick.AddListener(OpenBuildWindow);
        }

        //if (statisticButton != null)
        //{
        //    statisticButton.onClick.AddListener();
        //}
    }

    private void OpenBuildWindow()
    {
        BuildingWindow window = UIManager.Instance.ShowWindow<BuildingWindow>();
        window.currentShip = currentShip;
    }
}
