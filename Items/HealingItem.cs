using Godot;
using System;

[GlobalClass]
public partial class HealingItem : InventoryItem
{
    [Export]
    public HealingItemData HealingData { get; set; }

    public AudioStream HealSound { get; private set; } = ResourceLoader.Load<AudioStream>("res://SFX/Sounds/General/heal.wav");

    public override void Use(UseContext ctx)
    {
        ctx.User.GetNode<Health>("Health").CurrentHealth += HealingData?.Power ?? 0;

        ctx.User.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = HealSound;
        ctx.User.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

        base.Use(ctx);
    }
}
