using UnityEngine;
using UnityEngine.UI;
public class BuildingWindow : MonoBehaviour, IUIWindow
{
    public Ship currentShip;

    private Transform container;
    private Button itemPrototype;

    private bool initialized = false;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Redraw()
    {
        if (initialized == false)
        {
            FillWindow();

            initialized = true;
        }
    }

    private void FillWindow()
    {
        //  Hierarchy
        //  
        //   -- Window
        //  |  -- container
        //    |  -- prototype  
        //
        
        container = gameObject.GetElement<Transform>("container");
        itemPrototype = gameObject.GetElement<Button>("prototype");
        itemPrototype.gameObject.SetActive(ResourceUtility.Blocks.blocks.Count != 0);

        for (int i = 0; i < ResourceUtility.Blocks.blocks.Count; i++)
        {
            BlockResourceItem block = ResourceUtility.Blocks.blocks[i];
            GameObject blockGameObject = (i == 0) ? itemPrototype.gameObject : Instantiate(itemPrototype.gameObject);

            Button itemButton = blockGameObject.GetComponent<Button>();
            RawImage iconImage = blockGameObject.GetElement<RawImage>("icon");
            Text nameText = blockGameObject.GetElement<Text>("name");

            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => { BuildManager.Instance.BeginBuild(block.ID, currentShip); Hide(); });
            }

            if (iconImage != null)
            {
                iconImage.texture = ResourceUtility.atlas;
                iconImage.uvRect = block.uv;
            }

            if (nameText != null)
            {
                nameText.text = ((Blocks)block.ID).ToString();
            }

            blockGameObject.transform.SetParent(container, false);
            //blockGameObject
        }
    }
}
