using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharpExampleNetCore.Handler
{
    class MenuHandler : IContextMenuHandler
    {
        // 在上下文功能菜单调用前
        // IMenuModel 创建和修改菜单
        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            // 清除菜单
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            //throw new NotImplementedException();
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
           // throw new NotImplementedException();
        }

        public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            //throw new NotImplementedException();
            return false;
        }
    }
}
