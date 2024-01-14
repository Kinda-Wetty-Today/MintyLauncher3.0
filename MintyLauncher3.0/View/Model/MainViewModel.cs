using GalaSoft.MvvmLight.Command;
using MintyLauncher3._0.View.Pages.Main;
using System.Windows.Controls;
using System.Windows.Input;

namespace MintyLauncher3._0.View.Model
{
    internal class MainViewModel : ViewModelBase
    {
            private Page GI = new Genshin_Impact();
            private Page HSR = new Honkai_Star_Rail();
            private Page SE = new Settings();
            private Page _CurPage = new Genshin_Impact();
            public Page CurPage
            {
                get => _CurPage;
                set => Set(ref _CurPage, value);
            }
            public ICommand OpenGI
            {
                get
                {
                    return new RelayCommand(() => CurPage = GI);
                }
            }
            public ICommand OpenHsr
            {
               get
               {
                    return new RelayCommand(() => CurPage = HSR);
               }
            }
            public ICommand OpenSE
            {
              get
              {
                return new RelayCommand(() => CurPage = SE);
              }
            }
    }
}
