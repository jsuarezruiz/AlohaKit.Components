namespace Alohakit.Components.Core
{
    internal static class VisualComponent
    {
        public static readonly BindableProperty VisualProperty = BindableProperty.Create(
            nameof(IComponent.Visual),
            typeof(Visual),
            typeof(IComponent),
            Visual.Material,
            propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                ((IComponent)bindableObject).ChangeVisual();
            });
    }
}