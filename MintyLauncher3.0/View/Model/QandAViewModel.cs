using GalaSoft.MvvmLight.Command;
using MintyLauncher3._0.View.Pages.Main;
using MintyLauncher3._0.View.Pages.QandA;
using System.Windows.Controls;
using System.Windows.Input;

namespace MintyLauncher3._0.View.Model
{
    internal class QandAViewModel : ViewModelBase
    {
            private Page QaA = new MainQandA();
            private Page Inf = new Develop_info();
            private Page _CurPage = new MainQandA();
            public Page CurPage
            {
                get => _CurPage;
                set => Set(ref _CurPage, value);
            }
            public ICommand OpenQandA
            {
                get
                {
                    return new RelayCommand(() => CurPage = QaA);
                }
            }
        public ICommand OpenInf
        {
            get
            {
                return new RelayCommand(() => CurPage = Inf);
            }
        }
    }
}
