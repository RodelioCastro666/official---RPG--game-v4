using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour  , IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text quantity;

    private VendorItem vendorItem;

    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;

        if (vendorItem.MyQuantity > 0 || (vendorItem.MyQuantity == 0 && vendorItem.Unlimited))
        {
            icon.sprite = vendorItem.MyItem.MyIcon;
            title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[vendorItem.MyItem.MyQuality], vendorItem.MyItem.MyTitle);
            


            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.MyQuantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            if (vendorItem.MyItem.MyPrice > 0)
            {
                price.text = "Price: " + vendorItem.MyItem.MyPrice.ToString();
            }
            else
            {
                price.text = string.Empty;
            }

            gameObject.SetActive(true);
        }

       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player.MyInstance.MyGold >= vendorItem.MyItem.MyPrice && InventoryScripts.MyInstance.AddItem(vendorItem.MyItem))
        {
            SellItem();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UiManager.MyInstance.ShowToolTip(vendorItem.MyItem);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
        UiManager.MyInstance.HideToolTip();
    }

    private void SellItem()
    {
        Player.MyInstance.MyGold -= vendorItem.MyItem.MyPrice;

        if (!vendorItem.Unlimited)
        {
            vendorItem.MyQuantity--;
            quantity.text = vendorItem.MyQuantity.ToString();

            if (vendorItem.MyQuantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

  
}
