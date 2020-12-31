using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CefSharpExampleNetCore.Handler
{
    public class TangRequestHandler : RequestHandler
    {
        MainWindow mw;
        public TangRequestHandler(MainWindow mainWindow)
        {
            mw = mainWindow;
        }
        
        // 浏览器需要用户凭证时候调用
        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            // 用于异步继续认证请求的回调接口释放资源
            callback.Dispose();
            return false;
        }
        // 在浏览器导航之前调用
        protected override bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            // true 取消浏览器导航
            // false 允许浏览器继续导航
            return false;
        }
    
        // 在某些有限的情况下，在OnBeforeBrowse之前在UI线程上被调用，
        //在这些情况下，导航一个新的或不同的浏览器可能是可取的。
        //这包括用户发起的可能以特殊方式打开的导航（例如通过中间点击或ctrl+左键点击的链接）
        //和某些类型的由渲染器进程发起的跨源导航（例如导航顶层框架到/从文件URL）。
        protected override bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            // 返回true以取消导航，或者返回false以允许在源浏览器的顶层框架中继续导航。
            return false;
        }

        // 当插件崩溃时候调用
        protected override void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
        }
        //调用该方法来处理SSL证书无效的URL请求。
        //返回true，并在本方法中或在以后调用Continue(Boolean)来继续或取消请求。
        //如果CefSettings.IgnoreCertificateErrors被设置，所有无效证书将被接受，而无需调用本方法。
        // IRequestCallback 用于异步延续url请求的回调接口。
        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            // false 立即取消请求
            // true 使用IRequestCallback 以异步方式执行
            return true;
        }
        //当 JavaScript 通过 webkitStorageInfo.requestQuota 函数请求特定的存储配额大小时调用。
        //对于异步处理，返回true，并在稍后执行Continue(Boolean)以批准或拒绝请求，或执行Cancel()以取消请求
        protected override bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            // 稍后处理
            callback.Continue(true);
            // 异步处理 返回true 
            return true ;
        }
        // 渲染过程意外中止时候调用
        protected override void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            //base.OnRenderProcessTerminated(chromiumWebBrowser, browser, status);
        }

        // 当与浏览器相关联的渲染视图准备好在渲染过程中接收/处理IPC(进程间通信)消息时，在CEF UI线程上被调用。
        protected override void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //base.OnRenderViewReady(chromiumWebBrowser, browser);
        }

        // 当浏览器需要用户选择客户端证书进行认证请求时调用（例如：PKI认证）。
        protected override bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            //return base.OnSelectClientCertificate(chromiumWebBrowser, browser, isProxy, host, port, certificates, callback);
            throw new System.NotImplementedException();
            // 返回true，继续请求，并调用ISelectClientCertificateCallback.Select()，选择证书进行认证。返回false，使用默认行为，即浏览器从列表中选择第一个证书。
        }

        // 在启动资源请求之前，在CEF IO线程上调用。
        // 要允许资源加载以默认的处理方式进行，返回null.要指定资源的处理方式，
        // 返回一个IResourceRequestHandler对象。
        // 要为资源指定一个处理方法，返回一个IResourceRequestHandler对象。
        // 如果这个回调返回null，将在相关联的IRequestContextHandler（如果有的话）上调用相同的方法。
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            TangResourceRequestHandler tangResourceRequestHandle = new TangResourceRequestHandler();
            return tangResourceRequestHandle;
        }
    }
}
