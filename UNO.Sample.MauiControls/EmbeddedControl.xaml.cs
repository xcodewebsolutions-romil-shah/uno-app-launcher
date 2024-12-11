using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace UNO.Sample.MauiControls;

public partial class EmbeddedControl : ContentView
{
    private IDispatcherTimer _dispatcherTimer;

    public EmbeddedControl()
    {
        InitializeComponent();
    }

    #region EVENTS
    protected override async void OnParentSet()
    {
        base.OnParentSet();
        await LoadControlsInBackground();
    }

    private void btnScanQR_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool IsBarcodeDetected = false;

            gridQR.IsVisible = true;
            CameraBarcodeReaderView cameraBarcodeReaderView = new CameraBarcodeReaderView
            {
                IsDetecting = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = false,
            };
            cameraBarcodeReaderView.BarcodesDetected += async (object? sender, BarcodeDetectionEventArgs e) =>
            {
                if (!IsBarcodeDetected)
                {
                    IsBarcodeDetected = true;
                    await Dispatcher.DispatchAsync(async () =>
                    {
                        gridQR.IsVisible = false;
                        gridQR.Children.Clear();
                        await Application.Current?.MainPage?.DisplayAlert("Alert", Convert.ToString(e.Results[0].Value), "OK");
                        await Launcher.OpenAsync(new Uri(Convert.ToString(e.Results[0].Value)));
                    });
                }
            };
            gridQR.Children.Add(cameraBarcodeReaderView);
            btnScanQR.IsVisible = false;
            btnCloseQR.IsVisible = true;
        }
        catch (Exception ex)
        {
        }
    }

    private async void btnRequestCameraPermission_Clicked(object sender, EventArgs e)
    {
        try
        {
            PermissionStatus cameraPermissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
        }
        catch
        {
        }
    }

    private async void btnCloseQR_Clicked(object sender, EventArgs e)
    {
        btnScanQR.IsVisible = true;
        btnCloseQR.IsVisible = false;
        gridQR.IsVisible = false;
        gridQR.Children.Clear();
        await LoadControls();
    }

    #endregion

    #region METHODS
    private async Task LoadControlsInBackground()
    {
        Task.Run(async () =>
        {
            _dispatcherTimer = Dispatcher.CreateTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dispatcherTimer.Tick += async (s, e) =>
            {
                await LoadControls();
            };
            _dispatcherTimer.Start();
        });
    }

    private async Task LoadControls()
    {
        try
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                PermissionStatus cameraPermissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraPermissionStatus == PermissionStatus.Granted)
                {
                    if (gridQR.Children != null && gridQR.Children.Count > 0)
                    {
                        btnRequestCameraPermission.IsVisible = false;
                        btnScanQR.IsVisible = false;
                        btnCloseQR.IsVisible = true;
                        gridQR.IsVisible = true;
                    }
                    else
                    {
                        btnRequestCameraPermission.IsVisible = false;
                        btnScanQR.IsVisible = true;
                        btnCloseQR.IsVisible = false;
                        gridQR.IsVisible = false;
                    }
                }
                else
                {
                    btnRequestCameraPermission.IsVisible = true;
                    btnScanQR.IsVisible = false;
                    btnCloseQR.IsVisible = false;
                    gridQR.IsVisible = false;
                    gridQR.Children.Clear();
                }
            });
        }
        catch
        {
            await Dispatcher.DispatchAsync(() =>
            {
                btnRequestCameraPermission.IsVisible = true;
                btnScanQR.IsVisible = false;
                btnCloseQR.IsVisible = false;
                gridQR.IsVisible = false;
                gridQR.Children.Clear();
            });
        }
    }
    #endregion
}

public class ScannedResult
{
    public string Text { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
}
