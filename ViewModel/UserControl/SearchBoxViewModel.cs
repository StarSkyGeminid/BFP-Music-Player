using BFPMusicPlayer.Backend;
using BFPMusicPlayer.Command;
using BFPMusicPlayer.Model;
using System;
using System.Collections.ObjectModel;

namespace BFPMusicPlayer.ViewModel.UserControl
{
    public class SearchBoxViewModel : ViewModelBase
    {
        private Action delegateAction;
        private readonly MainControl mainControl;

        private string text;
        private int currentView = 1;

        public ObservableCollection<MusicModel> dataSearch = new ObservableCollection<MusicModel>();

        public ObservableCollection<MusicModel> Data { get => dataSearch; }

        public DefaultCommand Command { get; set; }

        public string TextSearchBox
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(TextSearchBox));
            }
        }

        public void SetCurrentView(int i)
        {
            currentView = i;
        }

        public void SetAction(Action action)
        {
            delegateAction = action;
        }

        public SearchBoxViewModel()
        {
            Command = new DefaultCommand(SearchData);
            mainControl = new MainControl();
        }

        private void SearchData()
        {
            dataSearch = mainControl.SearchMusic(text, currentView == 0);
            delegateAction();
        }
    }
}
