using BFPMusicPlayer.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BFPMusicPlayer.ViewModel.UserControl
{
    internal class SideMenuViewModel
    {
        private readonly ResourceDictionary dict = Application.LoadComponent(new Uri("/BFPMusicPlayer;component/Assets/IconPack.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;

        public List<MenuItemsData> MenuList => new List<MenuItemsData>
                {
                    new MenuItemsData(){ IconData= (PathGeometry)dict["icon_dashboard"], MenuText="Dashboard", MenuGroupName="Discover"},
                    new MenuItemsData(){ IconData= (PathGeometry)dict["icon_youtube"],MenuText="Youtube", MenuGroupName="Discover"},
                    new MenuItemsData(){ IconData= (PathGeometry)dict["icon_music"],MenuText="Music", MenuGroupName="Discover"},
                };
    }

    public class MenuItemsData
    {
        public PathGeometry IconData { get; set; }
        public string MenuText { get; set; }
        public string MenuGroupName { get; set; }

        public MenuItemsData()
        {
            Command = new CommandViewModel(Execute);
        }

        public ICommand Command { get; }

        private void Execute()
        {
            string MT = MenuText.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(MT))
            {
                navigateToPage(MT);
            }
        }

        private void navigateToPage(string Menu)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    _ = (window as MainWindow).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "View/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
}
