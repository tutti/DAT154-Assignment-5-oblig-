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

        private SpaceObject sun;
        private SpaceTime st;
        public event PropertyChangedEventHandler PropertyChanged;

        public int days { get; set; }
        public int rate {
            get {
                return st.rate;
            }
            set {
                st.rate = value;
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
                return -(int)(Canvas.GetLeft(focus) + (focus.Width / 2) - 400);
            }
        }
        public int transY {
            get {
                return -(int)(Canvas.GetTop(focus) + (focus.Height / 2) - 300);
            }
        }


        private List<SpaceObject> objects = new List<SpaceObject>();

        public MainWindow()
        {
            InitializeComponent();

            st = new SpaceTime();
            st.tick += dayTick;

            sun = new SpaceObject("Sun", "FFFFFF00", 696000, null, 0, 0);
            addSpaceObject(sun);
            st.tick += sun.daysPass;

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
                st.tick += o.daysPass;
                loaded[name] = o;
            }

            days = 0;
            rate = 1;
            zoom = 1;

            focus = sun.ellipse;

            DataContext = this;

            MainCanvas.MouseWheel += scroll;

            //updateSpaceObjects(days);
        }

        private void addSpaceObject(SpaceObject o) {
            o.ellipse.MouseUp += planetClick;
            MainCanvas.Children.Add(o.ellipse);
            objects.Add(o);
        }

        /*
        private void updateSpaceObjects(int daysPassed) {
            foreach(SpaceObject obj in objects) {
                obj.days += rate;
            }
        }
        */

        private void dayTick(int daysPassed) {
            days += daysPassed;
            //updateSpaceObjects(daysPassed);
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
