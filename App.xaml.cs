using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharpExampleNetCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitBrowser();
        }
        private static void InitBrowser()
        {
            // 获取启动应用程序可执行文件的路径
            string startUpPath = System.Windows.Forms.Application.StartupPath;
            // 初始化设置
            CefSettings cefSettings = new CefSettings();
            cefSettings.Locale = "zh-CN";
            cefSettings.CefCommandLineArgs.Add("enable-npapi", "1");
            cefSettings.CefCommandLineArgs.Add("enable-system-flash", "1"); //启用flash
            cefSettings.CefCommandLineArgs.Add("enable-media-stream", "1"); //启用媒体流
            cefSettings.CefCommandLineArgs.Add("ppapi-flash-version", "99.0.0.999"); //设置flash插件版本
            cefSettings.CefCommandLineArgs.Add("ppapi-flash-path", startUpPath + @"Plugins\pepflashplayer.dll");
            cefSettings.CefCommandLineArgs.Add("plugin-policy", "allow");
            // 要在浏览器对象初始化运行前使用
            // 用用户提供的设置初始化CefSharp 
            // initialize 和Shutdown 必须在你的主应用程序线程（通常UI 线程）调用
            Cef.Initialize(cefSettings, true);
            Cef.EnableHighDPISupport();
            var contx = Cef.GetGlobalRequestContext();
            Cef.UIThreadTaskFactory.StartNew(() => contx.SetPreference("profile.default_content_setting_values.plugins", 1, out string err));
        }
    }
}
