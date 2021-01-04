using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharpExampleNetCore.Handler
{
    public class TangResourceRequestHandler : ResourceRequestHandler
    {
        // 过滤
        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }
        // 加载资源请求之前调用
        // 要重定向或改变资源加载选择修改请求 修改请求的URL将被视为重定向
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //Console.WriteLine("++++++" + request.ReferrerUrl);

            /*
            if (request.ReferrerUrl != refUrl && request.ReferrerUrl != "")
            {
                var bc = (ChromiumWebBrowser)browserControl;
                
                bc.Dispatcher.Invoke(() =>{
                    var tt = bc.Tag as TabTag;
                    refUrl = tt.RefererURL;
                });
                request.SetReferrer(refUrl, ReferrerPolicy.Always);
            }*/

            if (request.ReferrerUrl == "" && MainWindow.RefererUrl != "")
            {
                // 在导航过程中如何发送Referrer HTTP头值的策略。
                // 默认值 如果头值是HTTPS，但请求目标是HTTP，则清除referrer头。
                // 等同于 ClearReferrerOnTransitionFromSecureToInsecure
                request.SetReferrer(MainWindow.RefererUrl, ReferrerPolicy.Default);
            }

            return CefSharp.CefReturnValue.Continue;
        }

        //当资源加载完成后，在CEF IO线程上调用该方法。
        //该方法将被调用于所有请求，包括由于CEF关闭或相关浏览器被破坏而中止的请求。
        //在关联浏览器被销毁的情况下，这个回调可能会在该浏览器的OnBeforeClose(IWebBrowser, IBrowser)回调之后到达。
        //IsValid方法可以用来测试这种情况，如果框架无效，应注意不要调用修改状态的浏览器或框架方法（如LoadURL、SendProcessMessage等）。
        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            //base.OnResourceLoadComplete(chromiumWebBrowser, browser, frame, request, response, status, receivedContentLength);
        }

        // 在CEF UI线程上调用，处理对未知协议组件的URL的请求。
        // /安全警告：在允许操作系统执行之前，你应该使用这个方法来执行基于场景、主机或其他URL分析的限制。
        protected override bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return true;
        }
        // 当资源负载被重定向时，在CEF IO线程上被调用。
        //request参数将包含旧的URL和其他与请求相关的信息。
        // response参数将包含导致重定向的响应。newUrl参数将包含新的URL，如果需要，可以更改。
        protected override void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            //base.OnResourceRedirect(chromiumWebBrowser, browser, frame, request, response, ref newUrl);
        }

        // 当收到资源响应时，在CEF IO线程上调用。
        // 要允许资源加载不做任何修改，返回false.如果要重定向或重试资源加载，可选择修改请求并返回true.
        // 要重定向或重试资源加载，需要修改请求并返回true。对请求URL的修改将被视为重定向。
        // 使用默认网络加载器处理的请求不能在此回调中重定向。
        // 警告：使用该方法重定向已被废弃。使用OnBeforeResourceLoad或GetResourceHandler来执行重定向。
        // 允许资源加载不做任何修改，返回false.否则返回一个有效流的IResourceHandler实例。
        // 要重定向或重试资源加载，可选择修改请求并返回true。对请求URL的修改将被视为重定向。
        // 使用默认网络加载器处理的请求不能在此回调中重定向。
        protected override bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            int code = response.StatusCode;
            if (code == 404)
            {
                return true;
            }
            return false;
        }

    }
}
