using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PulsarRemoteAPI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var button = this.FindControl<Button>("button_Search");
        if (button == null) {
            Console.WriteLine("Button not found??");
            return;
        }
        button.Click += Button_Click;
    }

    private async void Button_Click(object? sender, RoutedEventArgs e)
    {
        var queryBox = this.FindControl<TextBox>("textBox_Query");
        if (queryBox == null) {
            Console.WriteLine("QueryBox not found??");
            return;
        }
        var resultBox = this.FindControl<TextBox>("textBox_Result");
        if (resultBox == null) {
            Console.WriteLine("ResultBox not found??");
            return;
        }
        var requestBox = this.FindControl<TextBox>("textBox_Request");
        if (requestBox == null) {
            Console.WriteLine("RequestBox not found??");
            return;
        }

        var dataQuery = new Dictionary<string,object> {
            {"query",$@"select * from Account where Name LIKE '%{queryBox.Text}%'"}
        };
        var results = await RemoteAPICall.MakeCall("select","Account",dataQuery);
        requestBox.Text = results?["request"];
        resultBox.Text = results?["result"];
    }
}