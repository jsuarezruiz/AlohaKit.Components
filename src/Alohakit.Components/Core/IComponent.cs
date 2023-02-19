namespace Alohakit.Components.Core
{
    public interface IComponent
    {
        bool IsPointerOver { get; set; }
        ComponentState ComponentState { get; set; }
        Visual Visual { get; set; }
        void ChangeVisual();
    }
}