using System.Windows.Input;

namespace Alohakit.Components.Core
{
    public interface ICommandComponent
    {
        ICommand Command { get; set; }
        object CommandParameter { get; set; }
    }
}
