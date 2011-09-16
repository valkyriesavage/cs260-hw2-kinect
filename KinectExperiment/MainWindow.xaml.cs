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
            Canvas.SetZIndex(pointer, 5);
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

        private void drawMenu(string menuName)
        {
            Menu menuLoaded = new Menu(menuName);
            foreach (MenuItem mi in menuLoaded.menuItems) {
                TouchButton tb = new TouchButton(mi);
                if (mi.name == "back")
                {
                    //tb.rect.Fill = Brushes.HotPink;
                }
                Canvas.SetLeft(tb, mi.getUpperLeft().X);
                Canvas.SetTop(tb, mi.getUpperLeft().Y);
                canvas.Children.Add(tb);
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
            Trace.WriteLine(e.Source.GetType());
            if (e.Source is TouchButton)
            {
                List<UIElement> toRemove = new List<UIElement>();
                foreach (UIElement uie in canvas.Children)
                {
                    if (uie is TouchButton)
                    {
                        toRemove.Add(uie);
                    }
                }
                foreach (UIElement uie in toRemove)
                {
                    canvas.Children.Remove(uie);
                }
                TouchButton tb = (TouchButton)e.Source;
                string menuToDraw;
                if (tb.menuItem.name == "back")
                {
                    menuToDraw = tb.menuItem.previousMenu;
                }
                else
                {
                    menuToDraw = tb.menuItem.name;
                }
                drawMenu(menuToDraw);
            }

            return;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            canvas.Children.Add(pointer);
            drawMenu("root");
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
                        if (!Object.ReferenceEquals(hitTB,currentlySelected))
                        {
                            if (currentlySelected != null)
                            {
                                currentlySelected.RaiseEvent(new RoutedEventArgs(TouchButton.HandLeaveEvent));
                            }
                            hitTB.RaiseEvent(new RoutedEventArgs(TouchButton.HandEnterEvent));
                            currentlySelected = hitTB;
                        }
                    }
                    else
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
