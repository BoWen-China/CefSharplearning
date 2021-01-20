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
            // 添加右键菜单功能
            if(model.Count >0)
            {
                model.AddSeparator();
            }
            // 开发者工具设置为第一位显示
            //model.AddItem(CefMenuCommand.UserFirst, "Show DevTools");
            //model.AddItem(CefMenuCommand.Reload, "Reload");
            //model.AddItem(CefMenuCommand.UserLast, "Close DevTools");

        }

        // 调用该函数用来执行从上下文菜单中选择的命令
        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            //throw new NotImplementedException();
            //false 默认执行
            //true  命令被处理
            // 如果命令是 UserFirst 打开开发者工具
            if(commandId == CefMenuCommand.UserFirst)
            {
                browser.GetHost().ShowDevTools();
                return true;
            }
            if (commandId == (CefMenuCommand)26502)
            {
                browser.GetHost().CloseDevTools();
                return true;
            }
            return false;
        }
        // 当上下文菜单被取消时调用，不管菜单是否为空或命令是否被选择。
        public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
           // throw new NotImplementedException();
        }

        // 调用来允许自定义显示上下文菜单。
        // 自定义显示返回true 并于选定的命令ID同步或异步执行回调。
        // 默认显示返回false
        public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            //throw new NotImplementedException();
            return false;
        }
    }
}
