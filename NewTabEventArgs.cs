using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharpExampleNetCore
{
    class NewTabEventArgs : EventArgs
    {
        public string Url { get; private set; }
        public NewTabEventArgs(string url)
        {
            Url = url;
        }

    }
}
