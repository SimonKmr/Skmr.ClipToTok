using Skmr.ClipToTok.ViewModels;
using Skmr.ClipToTok.ViewModels.ClipToTok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Utility
{
    public static class ViewModelBus
    {
        public static PlayerViewModel PlayerViewModel { get; set; }
        public static ProjectViewModel SettingsViewModel { get; set; }
        public static VideoViewModel VideoViewModel { get; set; }
    }
}
