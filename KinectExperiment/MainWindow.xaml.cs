using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.Kinect.Nui;
using System.Diagnostics;

namespace KinectExperiment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse pointer = new Ellipse();

        public MainWindow()
        {
            InitializeComponent();
            this.MenuLoad += new RoutedEventHandler(changeMenu);

            pointer.Width = 40;
            pointer.Height = 40;
            pointer.Fill = Brushes.Purple;
            pointer.Opacity = .5;
        }

        Runtime nui;
        DateTime lastTime = DateTime.MaxValue;

        const int RED_IDX = 2;
        const int GREEN_IDX = 1;
        const int BLUE_IDX = 0;
        byte[] depthFrame32 = new byte[320 * 240 * 4];

        public static readonly RoutedEvent MenuLoadEvent = EventManager.RegisterRoutedEvent(
            "MenuLoad", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        List<TouchButton> tbs = new List<TouchButton>();

        private void drawBoxes()
        {
            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
            grid.RowDefinitions.Add(rowDef3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TouchButton tb = new TouchButton();

                    Grid.SetColumn(tb, j);
                    Grid.SetRow(tb, i);
                    if (i != 2 || j != 2) 
                    {
                        tb.content = i + " " + j;
                        grid.Children.Add(tb);
                        tbs.Add(tb);
                    }
                }
            }
            return;
        }

        public event RoutedEventHandler MenuLoad
        {
            add { AddHandler(MenuLoadEvent, value); } 
            remove { RemoveHandler(MenuLoadEvent, value); }
        }

        private void changeMenu(object sender, RoutedEventArgs e)
        {
            grid.Children.Clear();
            return;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            canvas.Children.Add(pointer);
            drawBoxes();
            nui = new Runtime();
            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex |
                    RuntimeOptions.UseSkeletalTracking |
                    RuntimeOptions.UseColor);
            }

            catch (InvalidOperationException)
            {
                return;
            }

            try
            {
                nui.VideoStream.Open(ImageStreamType.Video, 2,
                    ImageResolution.Resolution640x480, ImageType.Color);
                nui.DepthStream.Open(ImageStreamType.Depth, 2,
                    ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }

            catch (InvalidOperationException)
            {
                return;
            }
            lastTime = DateTime.Now;

            nui.SkeletonEngine.TransformSmooth = true;

            //Use to transform and reduce jitter
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.75f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            };

            nui.SkeletonEngine.SmoothParameters = parameters;

            nui.SkeletonFrameReady +=
                new EventHandler<SkeletonFrameReadyEventArgs>
                (nui_SkeletonFrameReady);
            nui.VideoFrameReady +=
                new EventHandler<ImageFrameReadyEventArgs>
                (nui_ColorFrameReady);
        }

        void nui_ColorFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            PlanarImage Image = e.ImageFrame.Image;
            video.Source = BitmapSource.Create(
                Image.Width, Image.Height, 96, 96, PixelFormats.Bgr32, null,
                Image.Bits, Image.Width * Image.BytesPerPixel);
        }

        TouchButton currentlySelected = null;

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;

            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    Joint hand = data.Joints[JointID.HandRight];
                    int xpos = (int)(2.0 * ((hand.Position.X + 1.0) * 400.0 - 200.0));
                    if (xpos > 780) { xpos = 780; }
                    else if (xpos < 20) { xpos = 20; }
                    int ypos = (int)(600.0-2.0 * ((hand.Position.Y + 1.0) * 300.0 - 150.0));
                    if (ypos > 580) { ypos = 580; }
                    else if (ypos < 20) { ypos = 20; }
                    
                    Canvas.SetLeft(pointer, xpos-20);
                    Canvas.SetTop(pointer, ypos-20);

                    Point handPoint = new Point(xpos, ypos);
                    HitTestResult htr = VisualTreeHelper.HitTest(canvas, handPoint);

                    Visual hit = null;
                    if (htr != null)
                    {
                        hit = (Visual)htr.VisualHit;
                    }

                    while (hit != null && !(hit is TouchButton))
                    {
                        hit = (Visual)VisualTreeHelper.GetParent(hit);
                    }

                    if (hit is TouchButton)
                    {
                        TouchButton hitTB = (TouchButton)hit;
                        if (hitTB != currentlySelected)
                        {
                            if (currentlySelected != null)
                            {
                                currentlySelected.RaiseEvent(new RoutedEventArgs(TouchButton.HandLeaveEvent));
                            }
                            hitTB.RaiseEvent(new RoutedEventArgs(TouchButton.HandEnterEvent));
                            currentlySelected = hitTB;
                        }
                    }
                    else if (hit == null)
                    {
                        Trace.WriteLine(handPoint);
                        if (currentlySelected != null) { currentlySelected.RaiseEvent(new RoutedEventArgs(TouchButton.HandLeaveEvent)); }
                        currentlySelected = null;
                    }

                }
            }
        }
    }
}
