using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für HighlighterView.xaml
    /// </summary>
    public partial class HighlighterView : ReactiveUserControl<HighlighterViewModel>
    {
        public HighlighterView()
        {
            InitializeComponent();
            Locator.CurrentMutable.Register(() => new HighlighterView(), typeof(IViewFor<HighlighterViewModel>));
        }
    }
}
