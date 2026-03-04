using Assets.UI.HUD.Inventory;
using System;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    #region Attributes
    [SerializeField] private SlotView[] slots;
    [SerializeField] private SlotItemView slotItemPrefab;

    private int selectedIndex = 0;
    #endregion

    #region Properties
    public event Action<int> OnSelectedItemChanged;
    #endregion

    #region Methods
    void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init(i);
            slots[i].OnClicked += SelectIndex;
        }

        UpdateSelectionVisual();
    }

    void Update()
    {
        HandleScrollInput();
    }

    void HandleScrollInput()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll == 0)
            return;

        int direction = scroll > 0 ? -1 : 1;
        int nextIndex = selectedIndex;

        for (int i = 0; i < slots.Length; i++)
        {
            nextIndex += direction;

            if (nextIndex < 0)
                nextIndex = slots.Length - 1;

            if (nextIndex >= slots.Length)
                nextIndex = 0;

            if (slots[nextIndex].HasItem())
            {
                SelectIndex(nextIndex);
                return;
            }
        }
    }

    public void Bind(InventoryItemViewModel[] items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Clear();

            if (items == null || i >= items.Length || items[i] == null)
                continue;

            var itemView = Instantiate(slotItemPrefab, slots[i].transform, false);
            itemView.Bind(items[i], i);
            slots[i].SetItemView(itemView);
        }
    }

    public void SubscribeToDrop(System.Action<int, int> callback)
    {
        foreach (var slot in slots)
            slot.OnItemDropped += callback;
    }

    public void UnsubscribeToDrop(System.Action<int, int> callback)
    {
        foreach (var slot in slots)
            slot.OnItemDropped -= callback;
    }

    private void UpdateSelectionVisual()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(i == selectedIndex);
        }
    }

    private void SelectIndex(int index)
    {
        selectedIndex = index;

        UpdateSelectionVisual();
        OnSelectedItemChanged?.Invoke(selectedIndex);
    }
    #endregion
}