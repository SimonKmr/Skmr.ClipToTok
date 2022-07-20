using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok
{
    public class TmpDirectoryManager
    {
        public List<string> TmpDirectoryList { get; set; }
        int count;
        string workspace;

        public TmpDirectoryManager()
        {
            TmpDirectoryList = new List<string>();
            count = 0;
            workspace = Path.GetTempPath();
        }
        public TmpDirectoryManager(string workspacePath)
        {
            TmpDirectoryList = new List<string>();
            count = 0;
            workspace = workspacePath;
        }


        public void Add()
        {
            TmpDirectoryList.Add($"{workspace}temp{count}");
            Directory.CreateDirectory(TmpDirectoryList[count]);
            count++;
        }
        public void Clear()
        {
            for (int i = 0; i < count; i++) Directory.Delete(TmpDirectoryList[i], true);
            count = 0;
            TmpDirectoryList.Clear();
        }
    }
}
