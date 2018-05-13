using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ms.Graph.UwpSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void BtnSign_Click(object sender, RoutedEventArgs e)
        {
            await AuthenticationHelper.GetTokenForUserAsync();
        }

        private async void BtnGetFiles_Click(object sender, RoutedEventArgs e)
        {
            var recent = await GraphApiProxy.GetRecentFiles();

        }

        private async  void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");
            
            var file = await openPicker.PickSingleFileAsync();


            if (file != null)
            {
                var content = await Windows.Storage.FileIO.ReadTextAsync(file);
                await GraphApiProxy.UploadNewFile(content, file.Name);                
            }
            else
            {
                TxtFile.Text = "<select a file>";
            }
        }

        private async void BtnDownloadFile_OnClick(object sender, RoutedEventArgs e)
        {

            using (var remoteStream = await GraphApiProxy.DownloadFile(TxtFileName.Text))
            {
                FileSavePicker picker = new FileSavePicker();
                
                picker.DefaultFileExtension = ".txt";
                picker.FileTypeChoices.Add("Text", new string[] {".txt"});
                picker.FileTypeChoices.Add("Unknown", new List<string>() {"."});
                picker.SuggestedFileName = TxtFileName.Text;
                var target = await picker.PickSaveFileAsync();
                var targetStream = await target.OpenStreamForWriteAsync();
                await remoteStream.CopyToAsync(targetStream);
                await targetStream.FlushAsync();
            }
        }
        
        

    }
}
