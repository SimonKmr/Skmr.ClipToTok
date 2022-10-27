using ReactiveUI;
using Skmr.ClipToTok.Avalonia.Views;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Avalonia.Utils
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            if (viewModel is ClipToTokViewModel)
            {
                return new ClipToTokView()
                {
                    DataContext = viewModel,
                    
                };
            }

            else if (viewModel is AnalyzerViewModel) throw new NotImplementedException();
            throw new ArgumentOutOfRangeException();
        }
    }
}
