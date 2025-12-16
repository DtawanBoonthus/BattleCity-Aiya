namespace BC.Gameplay.Tanks;

public interface IIFrame
{
    void IFrame(float iframeTime);
    bool IsIFrame { get; }
}