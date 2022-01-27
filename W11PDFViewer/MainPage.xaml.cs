using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace W11PDFViewer
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

        private PdfDocument _myDocument { get; set; }



        private async Task OpenPDFAsync(Windows.Storage.StorageFile file)
        {
            if (file == null) throw new ArgumentNullException();

            _myDocument = await PdfDocument.LoadFromFileAsync(file);
        }


        private async Task DisplayPage(uint pageIndex)
        {
            if (_myDocument == null)
            {
                throw new Exception("No document open.");
            }

            if (pageIndex >= _myDocument.PageCount)
            {
                throw new ArgumentOutOfRangeException($"Document has only {_myDocument.PageCount} pages.");
            }

            // Get the page you want to render.
            var page = _myDocument.GetPage(pageIndex);

            // Create an image to render into.
            var image = new BitmapImage();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await page.RenderToStreamAsync(stream);
                await image.SetSourceAsync(stream);

                // Set the XAML `Image` control to display the rendered image.
                PdfImage.Source = image;
            }
        }


        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
            filePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            filePicker.FileTypeFilter.Add(".pdf");

            Windows.Storage.StorageFile file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                this.FileName.Text = "File Name: " + file.Name;
                this.Path.Text = "Picked folder: " + file.Path;
            }
            else
            {
                this.Path.Text = "Operation cancelled.";
            }

            //Open and display pdf
            await OpenPDFAsync(file);
            await DisplayPage(0);
        }
    }
}
