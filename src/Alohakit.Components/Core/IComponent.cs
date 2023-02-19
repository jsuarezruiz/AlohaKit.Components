namespace Alohakit.Components.Core
{
    public interface IComponent
    {
        ComponentState ComponentState { get; set; }
        Visual Visual { get; set; }
        void ChangeVisual();
    }
}