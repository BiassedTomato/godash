using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource
{

    [Export]
    public string Name { get; set; }

    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; }

    [Export]
    public int MaxStack { get; set; } = 1;

    [Export]
    public Texture2D Texture { get; set; }

    [Export]
    public bool ConsumeOnUse { get; set; }

    [Export]
    public int Power { get; set; }

    [Export]
    public bool CanBeUsed { get; set; }
}
