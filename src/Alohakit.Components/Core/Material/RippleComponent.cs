namespace Alohakit.Components.Core.Material
{
    internal static class RippleComponent
    {
        public static readonly BindableProperty RippleColorProperty = BindableProperty.Create(
            nameof(IRippleComponent.RippleColor),
            typeof(Color),
            typeof(IRippleComponent),
            null);
    }
}
