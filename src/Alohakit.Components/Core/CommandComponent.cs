using System.Windows.Input;

namespace Alohakit.Components.Core
{
    internal static class CommandComponent
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(ICommandComponent.Command),
            typeof(ICommand),
            typeof(ICommandComponent),
            null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(ICommandComponent.CommandParameter),
            typeof(object),
            typeof(ICommandComponent),
            null);
    }
}