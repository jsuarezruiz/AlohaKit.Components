namespace Alohakit.Components.Core.Material
{
    public interface IRippleComponent
    {
        Point TouchPoint { get; set; }
        Color RippleColor { get; }
        float RippleProgress { get; set; }

        void StartRippleEffect();
    }
}