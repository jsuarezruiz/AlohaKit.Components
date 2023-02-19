namespace Alohakit.Components.Core
{
    internal static class SwitchComponent
    {
        public static readonly BindableProperty TrackColorProperty = BindableProperty.Create(
            nameof(ISwitch.TrackColor),
            typeof(Color),
            typeof(ISwitch),
            null); 
                
        public static readonly BindableProperty TrackOpacityProperty = BindableProperty.Create(
            nameof(ISwitch.TrackOpacity),
            typeof(double),
            typeof(ISwitch),
            1.0d);

        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(
            nameof(ISwitch.ThumbColor),
            typeof(Color),
            typeof(ISwitch),
            null); 
        
        public static readonly BindableProperty ThumbOpacityProperty = BindableProperty.Create(
            nameof(ISwitch.ThumbOpacity),
            typeof(double),
            typeof(ISwitch),
            1.0d);
    }
}