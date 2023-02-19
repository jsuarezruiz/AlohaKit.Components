namespace Alohakit.Components.Core
{
    internal static class OutlineComponent
    {
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create(
            nameof(IOutlineComponent.OutlineWidth),
            typeof(int),
            typeof(IOutlineComponent),
            0);

        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create(
            nameof(IOutlineComponent.OutlineColor),
            typeof(Color),
            typeof(IOutlineComponent),
            null);
    }
}