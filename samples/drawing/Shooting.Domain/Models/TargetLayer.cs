namespace Shooting.Domain.Models;

public class TargetLayer
{
    public TargetLayer(string layer, int index)
    {
        Layer = layer;
        Index = index;
    }

    public string Layer { get; }
    public int Index { get; }
}