using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SpaceSim;

namespace assignment5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //private SpaceObject[] objects = new SpaceObject[10];
        //private int count = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        /*private int _days;
        public int days {
            get {
                return _days;
            }
            set {
                _days = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("days"));
                }
            }
        }*/
        public int days { get; set; }
        private int _rate;
        public int rate {
            get {
                return _rate;
            }
            set {
                _rate = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("rate"));
                }
            }
        }
        private double _zoom;
        public double zoom {
            get {
                return _zoom;
            }
            set {
                _zoom = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("zoom"));
                }
            }
        }
        private Ellipse focus;
        public int transX {
            get {
                return -(int)(Canvas.GetLeft(focus) + focus.Width / 2 - 400);
            }
        }
        public int transY {
            get {
                return -(int)(Canvas.GetTop(focus) + focus.Height / 2 - 300);
            }
        }

        private Timer timer;
        private SpaceObject sun;
        //private Ellipse sunEl;

        private List<SpaceObject> objects = new List<SpaceObject>();

        public MainWindow()
        {
            InitializeComponent();

            sun = new SpaceObject("Sun", "FFFFFF00", 696000, null, 0, 0);
            addSpaceObject(sun);
            //sunEl = addSpaceObject(sun, "FFFFFF00");

            Dictionary<string, SpaceObject> loaded = new Dictionary<string, SpaceObject>();
            loaded["Sun"] = sun;

            string[] lines = System.IO.File.ReadAllLines(@"../../planets.txt");

            char[] sep = {';'};

            foreach (string line in lines) {
                if (line == "") continue;
                string[] data = line.Split(sep);
                string name = data[0];
                int radius = Int32.Parse(data[1]);
                SpaceObject parent = loaded[data[2]];
                int distance = Int32.Parse(data[3]);
                double period = Double.Parse(data[4]);
                string colour = data[5];

                SpaceObject o = new SpaceObject(name, colour, radius, parent, distance, period);
                addSpaceObject(o);
                loaded[name] = o;
            }

            days = 0;
            rate = 1;
            zoom = 1;

            timer = new Timer();
            timer.Interval = 1000/30;
            timer.Tick += dayTick;
            timer.Start();

            //MainCanvas.MouseUp += planetClick;

            //focus = sunEl;
            focus = sun.ellipse;

            //(MainCanvas.RenderTransform as TransformGroup).ScaleTransform.ScaleX = 2;

            DataContext = this;

            //MainCanvas.MouseWheel += scroll;

            updateSpaceObjects(days);
        }

        private void addSpaceObject(SpaceObject o) {
            o.ellipse.MouseUp += planetClick;
            MainCanvas.Children.Add(o.ellipse);
            objects.Add(o);
        }

        private void updateSpaceObjects(int days) {
            foreach(SpaceObject obj in objects) {
                obj.days += rate;

                //Tuple<int, int> drawingPos = calculateDrawingCoordinates(obj, days);

                //Canvas.SetLeft(el, 400 + drawingPos.Item1 - el.Width / 2);
                //Canvas.SetTop(el, 300 + drawingPos.Item2 - el.Height / 2);
            }

            //DayDisplay.Content = "Days passed: " + days;


        }

        /*private Tuple<int, int> calculateDrawingCoordinates(SpaceObject o, int days) {
            if (o.parent == null) {
                // If the object is not orbiting anything, it's in the center of the solar system
                return new Tuple<int, int>(0, 0);
            } else {
                // Get the drawing position of the parent object
                Tuple<int, int> parentPos = calculateDrawingCoordinates(o.parent, days);

                // Calculate the distance at which to draw this object
                //double distance = Math.Log(o.distance, DIST_BASE);
                double distance = Math.Pow(o.distance / DIST_FACTOR, DIST_BASE);

                // Calculate the rotation around the parent object
                double rotation = (days / o.period) % 1;
                double angle = rotation * Math.PI * 2;

                // Calculate the difference in x and y.
                int x = (int)(Math.Cos(angle) * distance);
                int y = (int)(Math.Sin(angle) * distance);

                return new Tuple<int, int>(x + parentPos.Item1, y + parentPos.Item2);
            }
        }*/

        private void dayTick(object sender, EventArgs e) {
            days += rate;
            updateSpaceObjects(days);
            PropertyChanged(this, new PropertyChangedEventArgs("days"));
            PropertyChanged(this, new PropertyChangedEventArgs("transX"));
            PropertyChanged(this, new PropertyChangedEventArgs("transY"));
        }

        private void planetClick(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                focus = (Ellipse)sender;
                zoom = 2;
            } else {
                focus = sun.ellipse;
                zoom = 1;
            }
            e.Handled = true;
        }

        private void scroll(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                rate += 1;
            } else {
                rate -= 1;
                if (rate < 1) rate = 1;
            }
        }
    }
}
