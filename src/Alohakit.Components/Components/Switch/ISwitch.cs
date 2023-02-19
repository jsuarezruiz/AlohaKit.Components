using Alohakit.Components.Core;
using Alohakit.Components.Core.Material;

namespace Alohakit.Components
{
    public interface ISwitch :
        IComponent,
        IOutlineComponent,
        IRippleComponent
    {
        bool IsChecked { get; }
        Color TrackColor { get; }
        double TrackOpacity { get; }
        Color ThumbColor { get; }
        double ThumbOpacity { get; }
    }
}