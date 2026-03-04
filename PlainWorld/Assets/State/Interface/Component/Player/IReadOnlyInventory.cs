using Assets.State.Component.Player;
using System;

namespace Assets.State.Interface.Component.Player
{
    public interface IReadOnlyInventory
    {
        InventoryItemSnapshot[] Items { get; }
        InventoryItemSnapshot SelectedItem { get; }

        event Action OnInventoryChanged;
        event Action<int> OnSelectedInventorySlotChanged;
    }
}
