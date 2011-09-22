/* (c) 2011 - Valkyrie Savage and Steve Rubin
 * Kinect 2D menu interface
 * Navigate the menu using your hand.
 * Rest your hand over a menu item for 1.5 seconds to select it.
 */

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
using System.Windows.Threading;

namespace KinectExperiment
{

    public partial class MainWindow : Window
    {
        // represents the user's hand on-screen
        Ellipse pointer = new Ellipse();

        TouchButton currentlySelected = null;

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

        public static readonly RoutedEvent MenuLoadEvent = EventManager.RegisterRoutedEvent(
            "MenuLoad", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        List<TouchButton> tbs = new List<TouchButton>();

        // Draw a menu given by menuName
        private void drawMenu(string menuName)
        {
            Menu menuLoaded = new Menu(menuName);

            if (menuLoaded.isLeaf())
            {
                TextBlock successText = new TextBlock();
                successText.Text = "Successfully selected " + menuName;
                successText.FontSize = 36;
                successText.MaxWidth = 800;
                Canvas.SetTop(successText, 250);
                canvas.Children.Add(successText);

                DispatcherTimer dispTimer = new DispatcherTimer();
                dispTimer.Tick += new EventHandler(dispTimer_Tick);
                dispTimer.Interval = new TimeSpan(0, 0, 2);
                dispTimer.Start();
            }
            else
            {
                foreach (MenuItem mi in menuLoaded.menuItems)
                {
                    TouchButton tb = new TouchButton(mi);
                    Canvas.SetLeft(tb, mi.getUpperLeft().X);
                    Canvas.SetTop(tb, mi.getUpperLeft().Y);
                    canvas.Children.Add(tb);
                }
            }
            return;
        }

        // Show selected leaves in the menu tree
        private void dispTimer_Tick(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            (sender as DispatcherTimer).Stop();
            canvas.Children.Remove(canvas.Children.OfType<TextBlock>().First<TextBlock>());
            drawMenu("root");
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
                // First, clear the existing TouchButtons
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

        // This code is based off of code in the Kinect SDK skeletal tracking
        // tutorial/documentation
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            canvas.Children.Add(pointer);
            drawMenu("root");
            nui = new Runtime();
            try
            {
                nui.Initialize(RuntimeOptions.UseSkeletalTracking | 
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
            }

            catch (InvalidOperationException)
            {
                return;
            }
            lastTime = DateTime.Now;

            nui.SkeletonEngine.TransformSmooth = true;

            // Use to transform and reduce jitter
            // (this code is from an online tutorial)
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

        TouchButton getHit(Point handPoint)
        {
            TouchButton hit = null;

            foreach (TouchButton tb in canvas.Children.OfType<TouchButton>())
            {
                if (tb.isIntersecting(handPoint))
                {
                    hit = tb;
                }
            }
            return hit;
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;

            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    Joint hand = data.Joints[JointID.HandRight];

                    // Adjust the coordinates so we don't have to stretch to the
                    // extremes of the Kinect's view in order to reach the extreme
                    // menu items.
                    int xpos = (int)(2.0 * ((hand.Position.X + 1.0) * 400.0 - 200.0));
                    if (xpos > 780) { xpos = 780; }
                    else if (xpos < 20) { xpos = 20; }
                    int ypos = (int)(600.0 - 2.0 * ((hand.Position.Y + 1.0) * 300.0 - 150.0));
                    if (ypos > 580) { ypos = 580; }
                    else if (ypos < 20) { ypos = 20; }

                    Canvas.SetLeft(pointer, xpos - 20);
                    Canvas.SetTop(pointer, ypos - 20);

                    Point handPoint = new Point(xpos, ypos);
                    TouchButton hit = getHit(handPoint);

                    if (hit != null)
                    {
                        TouchButton hitTB = (TouchButton)hit;
                        if (!Object.ReferenceEquals(hitTB, currentlySelected))
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
                        if (currentlySelected != null) { currentlySelected.RaiseEvent(new RoutedEventArgs(TouchButton.HandLeaveEvent)); }
                        currentlySelected = null;
                    }
                }
            }
        }
    }   
}
