using CefSharp.Wpf;
using CefSharpExampleNetCore.Behaviours;
using CefSharpExampleNetCore.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CefSharpExampleNetCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 参照页网址
        public static string RefererUrl = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         *  初始化浏览器页面对象。
         */
        public void InitBrowser(ChromiumWebBrowser Browser)
        {
            // 浏览器菜单对象 
            // 实现上下文菜单功能（应该是右键出来的上下文菜单）
            Browser.MenuHandler = new MenuHandler();
            LifeSpanHandler lifeSpanHandler = new LifeSpanHandler(this);
            Browser.LifeSpanHandler = lifeSpanHandler;
        }

        private void life_OpenInNewTab(object sender,EventArgs e)
        {
            var ev = (NewTabEventArgs)e;
            //this.Dispatcher.Invoke(() => CreateNewTab(e.Url));
        }

        // 创建一个新的标签
        public ChromiumWebBrowser CreateNewTab(string url,string refererUrl = null)
        {
            // 声明一个新的浏览器对象
            ChromiumWebBrowser browser = new ChromiumWebBrowser()
            {
                Address = url
            };
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
            BrowserTabs.Items.Add(newTab);
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
            // 进度条绑定方法
            progressBar.SetBinding(ProgressBar.IsIndeterminateProperty, new Binding() { Path = new PropertyPath("IsLoading"), Source = browser });

            return browser;
        }
    }
}
