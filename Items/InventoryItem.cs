using Godot;

[GlobalClass]
public partial class InventoryItem : Node
{
    [Export]
    public ItemData ItemData { get; set; }

    int count = 1;
    [Export]
    public int Count
    {
        get => count;

        set
        {
            var _prev = count;
            count = Mathf.Clamp(value, 0, ItemData?.MaxStack ?? 0);

            EmitSignal(SignalName.CountChanged, _prev, Count);
        }
    }

    // Used to place items in a dict-like fashion, as opposed to simple list
    [Export]
    public int Index = -1;

    [Signal]
    public delegate void CountChangedEventHandler(int prev, int cur);

    public void Consume(int count = 1)
    {
        Count--;

        if (Count == 0)
        {
           // QueueFree();
        }
    }

    public virtual void Use(UseContext ctx)
    {
        if (ItemData.ConsumeOnUse)
        {
            Consume();
        }
    }
}