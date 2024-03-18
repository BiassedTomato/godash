using System;
using System.Diagnostics;
using Godot;

public static class GodashExts
{
    public static T FindChild<T>(this Node self, string pattern, bool recursive = true, bool owned = true) where T : class
    {
        return self.FindChild(pattern, recursive, owned) as T;
    }

    public static Godash Q => Godash.Instance;

    public static Vector3I RoundToInt(this Vector3 self)
    {
        return new Vector3I(
            Mathf.RoundToInt(self.X),
            Mathf.RoundToInt(self.Y),
            Mathf.RoundToInt(self.Z));
    }

    public static Vector2I Flatten(this Vector3I self)
    {
        return new Vector2I(self.X, self.Z);
    }

    /// Identical to the built-in FindChild functions, but is generic and does not require a pattern.
    public static T FindChild<T>(this Node self) where T : Node
    {
        foreach (var child in self.GetChildren())
            if (child is T)
                return child as T;

        return default(T);
    }

    public static void ToGround(this Node3D self, GridMap gridMap)
    {
        if (gridMap == null)
            return;

        var scale = Mathf.RoundToInt(gridMap.CellSize.Y); // TODO: по хорошему надо учитывать скейлинг для всех трех осей. Пока работает, т.к. скейл = 1.

        var cellPosition = self.GlobalPosition.RoundToInt() / scale;

        int y = 256;// cellPosition.Y;
        var cell = gridMap.GetCellItem(cellPosition);

        while (cell == GridMap.InvalidCellItem && y > -1)
        {
            y -= 1;

            cell = gridMap.GetCellItem(new Vector3I(cellPosition.X, y, cellPosition.Z));
        }
        if (y > -1)
        {
            self.GlobalPosition = new Vector3(cellPosition.X, y + 1, cellPosition.Z) * scale;
        }
    }

    public static void CallAfter(this Callable self, float time, bool deferred = false)
    {
        var timer = Q.GetTree().CreateTimer(time);
        timer.Timeout += () =>
        {
            if (deferred)
                self.CallDeferred();
            else self.Call();

        };
    }

    public static void CallAfter<T>(this Action<T> self, float time, bool deferred = false)
    {
        Callable.From(self).CallAfter(time, deferred);
    }

    public static void CallAfter(this Action self, float time, bool deferred = false)
    {
        Callable.From(self).CallAfter(time, deferred);
    }

    public static T Require<T>(this Node self) where T : Node
    {
        var fch = self.FindChild<T>();
#if DEBUG
        Debug.Assert(fch != default(T), $"The node {self.Name} ({self.GetType()}) does not have a child node of type '{typeof(T)}.'");
#endif

        return fch;
    }
}