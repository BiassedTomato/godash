using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class Inventory : Node
{
    public List<InventoryItem> Items => new(GetChildren().Cast<InventoryItem>());

    public Dictionary<int, InventoryItem> ItemsGrid = new();

    [Export]
    public int MaxSize { get; set; }

    void RefreshChildList()
    {

    }

    public override void _Ready()
    {
        ChildOrderChanged += RefreshChildList;

        var freeze = Items;

        for (int i = 0; i < freeze.Count; i++)
        {
            freeze[i].Index = i;
            ItemsGrid.Add(i, freeze[i]);
        }

        Callable.From(() =>
        {
            GetTree().CallGroup("InventoryListener", "Accept", this);
        }).CallDeferred();
    }

    IResizeStrategy doOnResizeConflict = new ResizeDropStrategy();

    public void Resize(int newSize)
    {
        var itemsCount = Items.Where(x => x != null).Count();

        if (itemsCount > newSize)
        {
            // Conflict!

            doOnResizeConflict.TryResolve(this);

            EmitSignal(SignalName.ResizeConflict);
        }
    }

    public bool ConsumeOfType<T>() where T : InventoryItem
    {
        foreach (var x in Items)
        {
            if (x is T)
            {
                RemoveOrDecreaseItem(x);
                EmitSignal(SignalName.InventoryChanged);
                return true;
            }
        };

        return false;
    }

    public void Visit(InventoryBox invBox)
    {
        InventoryChanged += () =>
        {
            invBox.ForceRefresh(Items);
        };
    }
    /// <summary>
    /// Add item to the list and emit the <c>InventoryChanged</c> signal.
    ///</summary>
    public bool AddItem(InventoryItem item)
    {
        if (item.GetParent() == this)
            return false;

        if (Items.Count < MaxSize)
        {
            AddChild(item);
            EmitSignal(SignalName.InventoryChanged);
            return true;
        }
        else
        {
            // emit fail signal
            return false;
        }
    }

    public bool MoveItemsTo(Inventory to)
    {
        //Do checks

        if (to.MaxSize < Items.Count)
            return false;

        foreach (var item in Items)
        {
            RemoveChild(item);
            to.AddItem(item);
        }
        return true;
    }

    public InventoryItem RemoveOrDecreaseItem(InventoryItem item)
    {
        if (item.Count > 1)
            item.Count--;
        else
            RemoveItem(item);

        return item;
    }

    public InventoryItem RemoveItem(InventoryItem item)
    {
        RemoveChild(item);

        EmitSignal(SignalName.InventoryChanged);

        return item;
    }

    public InventoryItem RemoveItem(int idx)
    {
        return RemoveItem(Items[idx]);
    }

    [Signal]
    public delegate void InventoryChangedEventHandler();

    [Signal]
    public delegate void ResizeConflictEventHandler(); // Occurs when the new size is smaller than the amount of slots taken in the inventory
}