using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DialogHostDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            //DialogHost.DialogHost;


            Button theButton = this.Find<Button>("TheButton");
            theButton.Click += OpenDialogWithView;
        }

        private void OpenDialogWithView(object? sender, RoutedEventArgs e)
        {
            /*DialogHost.DialogHost dialogHost = this.Find<DialogHost.DialogHost>("mDialogHost");
            dialogHost.IsOpen = true;*/
            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock() { Text = "Hello" });
            stackPanel.Children.Add(new TextBlock() { Text = "World" });
            stackPanel.Children.Add(new Button() { Content = "OK" });
            stackPanel.Children.Add(new Button() { Content = "Close" });

            ShowDialogSync<object>(stackPanel);
        }

        public static T ShowDialogSync<T>(object content)
        {
                using var source = new CancellationTokenSource();
                var result = default(T);
                var dialogTask = DialogHost.DialogHost.Show(content).ContinueWith(
                    t =>
                    {
                        if (t.IsCompletedSuccessfully)
                            result = (T)t.Result;
                        source.Cancel();
                    },
                    TaskScheduler.FromCurrentSynchronizationContext());
                Dispatcher.UIThread.MainLoop(source.Token);
                return result;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}