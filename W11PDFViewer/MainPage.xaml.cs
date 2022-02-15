using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class MainPage : Page , INotifyPropertyChanged
    {
        public MainPage()
        {
            this.InitializeComponent();
           
        }



        //private PdfDocument _myDocument { get; set; }

        public ObservableCollection<BitmapImage> PdfPages { get; set; } = new ObservableCollection<BitmapImage>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Uri Source { get; set; }



        //private async Task OpenPDFAsync(Windows.Storage.StorageFile file)
        //{
        //    if (file == null) throw new ArgumentNullException();

        //    _myDocument = await PdfDocument.LoadFromFileAsync(file);
        //}


        //private async Task DisplayPage(uint pageIndex)
        //{
        //    if (_myDocument == null)
        //    {
        //        throw new Exception("No document open.");
        //    }

        //    if (pageIndex >= _myDocument.PageCount)
        //    {
        //        throw new ArgumentOutOfRangeException($"Document has only {_myDocument.PageCount} pages.");
        //    }

        //    // Get the page you want to render.
        //    var page = _myDocument.GetPage(pageIndex);

        //    // Create an image to render into.
        //    var image = new BitmapImage();

        //    using (var stream = new InMemoryRandomAccessStream())
        //    {
        //        await page.RenderToStreamAsync(stream);
        //        await image.SetSourceAsync(stream);

        //        // Set the XAML `Image` control to display the rendered image.
        //        //PdfImage.Source = image;
        //    }
        //}


        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            filePicker.FileTypeFilter.Add(".pdf");

            StorageFile file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                SetupPDFDisplay(file);
            }
        }
        private async void SetupPDFDisplay(StorageFile file)
        {


            var uri = new Uri(file.Path);

            //set window title
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            appView.Title = file.Name;

            //this.FileName.Text = "File Name: " + file.Name;
            //this.Path.Text = "Picked folder: " + file.Path + "|||" + uri.ToString();

            //this.Source = uri;
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));\

            PdfDocument pdfDoc = await PdfDocument.LoadFromFileAsync(file);

            Load(pdfDoc);

        }

        async void Load(PdfDocument pdfDoc)
        {
            PdfPages.Clear();

            for (uint i = 0; i < pdfDoc.PageCount; i++)
            {
                BitmapImage image = new BitmapImage();

                var page = pdfDoc.GetPage(i);

                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await page.RenderToStreamAsync(stream);
                    await image.SetSourceAsync(stream);
                }

                PdfPages.Add(image);
            }
        }

        
    }
}
