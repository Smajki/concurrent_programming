using System;

namespace Logic;

public sealed class BallStateChangedEventArgs : EventArgs
{
    public int Id { get; }
    public double CenterX { get; }
    public double CenterY { get; }
    public double Diameter { get; }

    public BallStateChangedEventArgs(int id, double centerX, double centerY, double diameter)
    {
        Id = id;
        CenterX = centerX;
        CenterY = centerY;
        Diameter = diameter;
    }
}