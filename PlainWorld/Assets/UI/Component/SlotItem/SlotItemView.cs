using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    public void Bind(ItemSO item, int quantity)
    {
        icon.sprite = item.Icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : "";
    }
}