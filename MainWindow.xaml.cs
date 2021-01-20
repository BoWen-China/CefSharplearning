using CefSharp;
using CefSharp.Wpf;
using CefSharpExampleNetCore.Behaviours;
using CefSharpExampleNetCore.Handler;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CefSharpExampleNetCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string DefaultUrl = "https://www.baidu.com";
        // 参照页网址
        public static string RefererUrl = "";

        public event EventHandler OpenInNewtab = delegate { };
        public MainWindow()
        {
          
            InitializeComponent();
            CreateNewTab(DefaultUrl);
        }

        /**
         *  初始化浏览器页面对象。
         */
        public void InitBrowser(ChromiumWebBrowser Browser)
        {
            // 浏览器菜单对象 
            // 实现上下文菜单功能（应该是右键出来的上下文菜单）
            Browser.MenuHandler = new MenuHandler();
            // this 指代 MainWindow 对象
            LifeSpanHandler lifeSpanHandler = new LifeSpanHandler(this);
            // 浏览器生命周期处理
            Browser.LifeSpanHandler = lifeSpanHandler;
            // 开启新的页面的生命周期
            lifeSpanHandler.OpenInNewTab += Life_OpenInNewTab;
            Browser.RequestHandler = new TangRequestHandler(this);

            Browser.PreviewTextInput += (o, e) =>
            {
                foreach (var character in e.Text)
                {
                    // 根据浏览器对象获取浏览器实例、
                    // 根据实例找到当前浏览器主机对象发送按键事件
                    // WM.CHAR,当一个WM_KEYDOWN消息被TranslateMessage函数翻译后，
                    // WM_CHAR消息会被发布到键盘焦点的窗口中。WM_CHAR消息包含了被按下的键的字符代码。
                    Browser.GetBrowser().GetHost().SendKeyEvent((int)WM.CHAR, (int)character, 0);
                }
                e.Handled = true;
            };
        }

        // 开启新的标签页生命周期
        private void Life_OpenInNewTab(object sender,EventArgs e)
        {
            var ev = (NewTabEventArgs)e;
            this.Dispatcher.Invoke(() => CreateNewTab(ev.Url));
        }

        // 创建一个新的标签
        public ChromiumWebBrowser CreateNewTab(string url,string refererUrl = null)
        {
            // 声明一个新的浏览器对象
            ChromiumWebBrowser browser = new ChromiumWebBrowser()
            {
                Address = url
            };
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.Plugins = CefState.Enabled;
            browser.BrowserSettings = browserSettings;
            InitBrowser(browser);
            // 初始化浏览器地址栏和导航栏
            DockPanel TopBarPanel = new DockPanel()
            {
                Style = Resources["TopBarPanel"] as Style
            };
            DockPanel.SetDock(TopBarPanel, Dock.Top);
            // 回退 前进按钮 以及地址栏的位置
            DockPanel TopBarPanelIn = new DockPanel();

            Border TopBarPanelBorder = new Border();
            TopBarPanel.Children.Add(TopBarPanelBorder);
            TopBarPanelBorder.Child = TopBarPanelIn;
            // 回退按钮
            Button BtnBack = new Button()
            {
                Content = "\xe6a4",
                Width = 30,
                Height = 24,
                FontSize = 17,
                Style = Resources["IconButton"] as Style
            };
            // 绑定命令 将浏览器回退功能命令绑定到回退按钮上
            BtnBack.SetBinding(Button.CommandProperty, new Binding() 
            {
                Source = browser.BackCommand
            });
            // 前进按钮
            Button BtnForward = new Button()
            {
                Content = "\xe6a5",
                Width = 30,
                Height = 24,
                Margin = new Thickness(5, 0, 0, 0),
                FontSize = 17,
                Style = Resources["IconButton"] as Style
            };
            // 将浏览器前进功能命令绑定到前进按钮上
            BtnForward.SetBinding(Button.CommandProperty, new Binding() 
            { 
                Source = browser.ForwardCommand 
            });
            // 文本地址栏
            TextBox TextAddress = new TextBox()
            {
                FontSize = 14,
                Height = 24,
                Margin = new Thickness(5, 0, 0, 0),
                Padding = new Thickness(5, 0, 5, 0),
                Style = Resources["TxtAddress"] as Style
            };

            TextBoxBindingUpdateOnEnterBehaviour behaviour = new TextBoxBindingUpdateOnEnterBehaviour();
            // 将行为附加到对象上
            behaviour.Attach(TextAddress);

            TextAddress.SetBinding(TextBox.TextProperty, new Binding()
            {
                Source = browser,
                Path = new PropertyPath("Address"),
                // 绑定中数据流的方向 自动更新另一方
                Mode = BindingMode.TwoWay
            }); ;
            // 地址栏中添加 回退前进按钮以及地址栏
            TopBarPanelIn.Children.Add(BtnBack);
            TopBarPanelIn.Children.Add(BtnForward);
            TopBarPanelIn.Children.Add(TextAddress);

            // 初始化Tab控件可选项
            // 选项卡
            TabItem newTab = new TabItem()
            {
                Header = "New Tab",
                Style = Resources["CloseTab"] as Style,
                // 边框粗细
                BorderThickness = new Thickness(0)
            };
           
            // 设置当前选中的第一项
            BrowserTabs.SelectedItem = newTab;
            newTab.Name = "Tab_" + newTab.TabIndex;
            browser.Name = "Browser_" + newTab.TabIndex;
            newTab.SetBinding(TabItem.HeaderProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Source = browser,
                Path = new PropertyPath("Title"),
                FallbackValue = "info.tangce.cn"
            });
            // 选项卡中的样式内容等
            DockPanel TabItemPanel = new DockPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            newTab.Content = TabItemPanel;

            // 浏览器栏
            DockPanel BrowserPanel = new DockPanel()
            {
                VerticalAlignment = VerticalAlignment.Stretch
            };
            BrowserPanel.Children.Add(browser);

            // 进度条
            ProgressBar progressBar = new ProgressBar()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 2,
                Style = Resources["AutoWidth"] as Style,
                BorderThickness = new Thickness(0)
            };
            // 进度条绑定方法 浏览器是否加载中
            progressBar.SetBinding(ProgressBar.IsIndeterminateProperty, new Binding() 
            { 
                Path = new PropertyPath("IsLoading"), Source = browser 
            });
            // 进度条嵌板 对象
            DockPanel ProgressPanel = new DockPanel()
            {
                Height = 2,
                // 元素显示状态 隐藏
                Visibility = Visibility.Hidden
            };
            // 进度条嵌板针对元素可见参数绑定 
            // 如果加载中显示进度条嵌板
            ProgressPanel.SetBinding(DockPanel.VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("IsLoading"),
                Source = browser,
                // Visibility枚举值布尔转换器
                Converter = new BooleanToVisibilityConverter()
            });
            // 将进度条元素设置在Dock 底部
            DockPanel.SetDock(ProgressPanel, Dock.Bottom);
            // 将进度条放置到进度条嵌板中
            ProgressPanel.Children.Add(progressBar);
            // 将进度条嵌板 浏览器工具嵌板以及浏览器页面嵌板添加到标签选项卡中
            TabItemPanel.Children.Add(ProgressPanel);
            TabItemPanel.Children.Add(TopBarPanel);
            TabItemPanel.Children.Add(BrowserPanel);

            TabTag tabtag = new TabTag
            {
                IsOpen = true,
                Browser = browser,
                OrigURL = url,
                CurURL = url,
                Title = browser.Title,
                RefererURL = refererUrl,
                DateCreated = DateTime.Now
            };
            // 新的标签自定义信息
            newTab.Tag = tabtag;
            // 显示页面
            BrowserTabs.Items.Add(newTab);
            return browser;
        }
        // 关闭窗口
        private void Window_Closed(object sender,EventArgs e)
        {
            Console.WriteLine(BrowserTabs.Items.Count);
            for (int i = 0; i < BrowserTabs.Items.Count; i++)
            {
                var item = BrowserTabs.Items[i] as TabItem;
                var tabTag = item.Tag as TabTag;
                var CurBrowser = tabTag.Browser;
                if (CurBrowser != null)
                {
                    // 浏览器对象释放资源
                    CurBrowser.Dispose();
                }
            }
            // 关闭Cef浏览器
            Cef.Shutdown();
        }
        // 关闭标签页面
        private void CloseTabItem(TabItem tabItem)
        {
            if (BrowserTabs.Items.Count > 1)
            {
                var tabTag = tabItem.Tag as TabTag;
                var CurBrowser = tabTag.Browser;
                if (CurBrowser != null)
                {
                    CurBrowser.Dispose();
                    BrowserTabs.Items.Remove(tabItem);
                }
            }
        }
        // 选项卡关闭事件
        private void BtnItemClose_Click(object sender,RoutedEventArgs e)
        {
            var CurrentItem = FindVisualParent<TabItem>(sender as Button);
            Console.WriteLine(CurrentItem);
            CloseTabItem(CurrentItem);
        }
        // 查找可见的父对象
        public static T FindVisualParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            T result = null;
            // 参与依赖属性系统的值
            DependencyObject dp = dependencyObject;
            while (result == null)
            {
                // 返回表示视觉对象的父对象的 DependencyObject 值。
                dp = VisualTreeHelper.GetParent(dp);
                result = dp as T;
                if (dp == null) return null;
            }
            return result;
        }
        // 查找可视化子元素对象的值
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}
 internal class TabTag
{
    public bool IsOpen;

    public string OrigURL;
    public string CurURL;
    public string Title;
    public string RefererURL;

    public DateTime DateCreated;
    public ChromiumWebBrowser Browser;
}