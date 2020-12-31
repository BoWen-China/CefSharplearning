using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharpExampleNetCore.Handler
{
    // 浏览器生命持续时间操作类
    public class LifeSpanHandler : ILifeSpanHandler
    {
        MainWindow mainWindow;
        // Eventhandler 事件处理者
        // 表示将用于处理不具有事件数据的事件的方法。
        public event EventHandler OpenInNewTab;

        // 构造函数
        public LifeSpanHandler(MainWindow mw)
        {
            mainWindow = mw;
        }
        // 默认行为false
        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        // 在弹出窗口之前创建
        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            var bc = (ChromiumWebBrowser)chromiumWebBrowser;
            newBrowser = null;
            if(OpenInNewTab != null)
            {
                bc.Dispatcher.Invoke(() =>
                {
                    MainWindow.RefererUrl = chromiumWebBrowser.Address;
                });
                OpenInNewTab.Invoke(this, new NewTabEventArgs(targetUrl));
            }
            return true;
        }
    }
}
