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
using System.Windows.Media.Animation;
using System.Threading;

namespace KinectExperiment
{
    /// <summary>
    /// Interaction logic for TouchButton.xaml
    /// </summary>

    public partial class TouchButton : UserControl
    {
        bool pointOn = false; // true if pointer is over this box
        public String content;

        public static readonly RoutedEvent HandEnterEvent = EventManager.RegisterRoutedEvent(
             "HandEnter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchButton));
        public static readonly RoutedEvent HandLeaveEvent = EventManager.RegisterRoutedEvent(
             "HandLeave", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchButton));

        public TouchButton()
        {
            InitializeComponent();
            content = "test";
        }

        public event RoutedEventHandler HandEnter
        {
            add { AddHandler(HandEnterEvent, value); }
            remove { RemoveHandler(HandEnterEvent, value); }
        }

        public event RoutedEventHandler HandLeave
        {
            add { AddHandler(HandLeaveEvent, value); }
            remove { RemoveHandler(HandLeaveEvent, value); }
        }

        public void hoverOn(object sender, RoutedEventArgs e)
        {
            border.Fill = Brushes.Gray;
            pointOn = true;
            fade();
        }

        private Storyboard myStoryBoard;

        private void fade()
        {

            myStoryBoard.Begin(this, true);

            //border.BeginAnimation(Rectangle.StrokeThicknessProperty, anima);
        }

        private void select(Object sender, EventArgs e)
        {
            if (pointOn)
            {
                rect.Fill = Brushes.Brown;
                Window parentWindow = Window.GetWindow(this);

                parentWindow.RaiseEvent(new RoutedEventArgs(MainWindow.MenuLoadEvent, content));
            }
        }

        public void hoverOff(object sender, RoutedEventArgs e)
        {
            myStoryBoard.Stop(this);
            pointOn = false;
            border.Fill = Brushes.White;
        }

        private void TouchButton_Loaded(object sender, RoutedEventArgs e)
        {
            this.HandEnter += new RoutedEventHandler(hoverOn);
            this.HandLeave += new RoutedEventHandler(hoverOff);

            double w1 = 0;
            double w2 = 18;
            DoubleAnimation anima = new DoubleAnimation();
            anima.Duration = new Duration(TimeSpan.FromSeconds(3.0));
            anima.From = w1;
            anima.To = w2;
            anima.FillBehavior = FillBehavior.HoldEnd;
            anima.Completed += new EventHandler(select);

            
            Storyboard.SetTargetName(anima, "border");
            Storyboard.SetTargetProperty(anima, new PropertyPath(Rectangle.StrokeThicknessProperty));
            myStoryBoard = new Storyboard();
            myStoryBoard.Children.Add(anima);
        }
    }
}
