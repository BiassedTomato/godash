using Godot;
using System.Collections.Generic;



public partial class Godash : Node
{
    public override void _Ready()
    {
        Instance = this;
        //TranslationServer.SetLocale("ru");
    }

    public Dictionary<string, string> GlobalVars = new();


    public static Godash Instance { get; private set; }

    public override void _Process(double delta)
    {
    }

    public GodashNodeQuery<T> Query<T>(IEnumerable<T> dataSource)
    {
        return null;
    }

    public Basis GetCameraBasis(bool globalTransform = true)
    {
        var cam = GetViewport().GetCamera3D();

        if (globalTransform)
            return cam.GlobalTransform.Basis;
        return cam.Transform.Basis;
    }

    public void FaceCamera(Node3D node, bool globalCamTransform = true)
    {
        node.LookAt(GetCameraBasis(globalCamTransform).Z + node.Transform.Origin);
    }

    public T Near<T>()
    {
        return default(T);
    }


}