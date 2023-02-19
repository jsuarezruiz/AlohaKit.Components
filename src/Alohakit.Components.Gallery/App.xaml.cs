using Alohakit.Components.Extensions;

namespace Alohakit.Components.Gallery
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //this.UseAlohaKitComponents();

            MainPage = new AppShell();
        }
    }
}