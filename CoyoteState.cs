using System;

public class CoyoteState
{
    float _time;
    public float CoyoteTime
    {
        get => _time;
        private set => _time = value;
    }

    Func<bool> _floorCallback;

    public CoyoteState(float time, Func<bool> _floorCallback)
    {
        CoyoteTime = time;
        this._floorCallback = _floorCallback;
    }
}
