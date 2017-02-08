using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceSim {
    public class SpaceObject : INotifyPropertyChanged {
        const double SIZE_BASE = 0.3;
        const double DIST_FACTOR = 50;
        const double DIST_BASE = 0.5;

        private int _x;
        private int _y;
        private int _days;
        private bool hasPosition;
        private Ellipse el;

        public event PropertyChangedEventHandler PropertyChanged;

        public string name { get; set; }
        public SpaceObject parent { get; set; }
        public int radius { get; set; } // In km
        public int distance { get; set; } // Distance from parent object in 1000 km
        public double period { get; set; } // Earth days to complete 1 orbit

        public SpaceObject(string _name, string _colour, int _radius, SpaceObject _parent, int _distance, double _period) {
            name = _name;
            radius = _radius;
            parent = _parent;
            distance = _distance;
            period = _period;

            _x = 0;
            _y = 0;
            hasPosition = false;

            el = new Ellipse();
            el.Width = Math.Pow(radius, SIZE_BASE);
            el.Height = el.Width;
            el.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#" + _colour);
            Binding bindingX = new Binding("drawX");
            Binding bindingY = new Binding("drawY");
            bindingX.Source = this;
            bindingY.Source = this;
            el.SetBinding(Canvas.LeftProperty, bindingX);
            el.SetBinding(Canvas.TopProperty, bindingY);
        }

        private void calculatePosition() {
            if (hasPosition) return;

            if (parent == null) {
                _x = 0;
                _y = 0;
            } else {
                double _distance = Math.Pow(distance / DIST_FACTOR, DIST_BASE);
                double rotation = (days / period) % 1;
                double angle = rotation * Math.PI * 2;

                int tx = (int)(Math.Cos(angle) * _distance);
                int ty = (int)(Math.Sin(angle) * _distance);

                _x = tx + parent.x;
                _y = ty + parent.y;
            }

            hasPosition = true;
        }

        public int x {
            get {
                calculatePosition();
                return _x;
            }
        }

        public int y {
            get {
                calculatePosition();
                return _y;
            }
        }

        public int drawX {
            get {
                return 400 + x - (int)el.Width / 2;
            }
        }

        public int drawY {
            get {
                return 300 + y - (int)el.Height / 2;
            }
        }

        public Ellipse ellipse {
            get {
                return el;
            }
        }

        public int days {
            get {
                return _days;
            }
            set {
                _days = value;
                hasPosition = false;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("drawX"));
                    PropertyChanged(this, new PropertyChangedEventArgs("drawY"));
                }
            }
        }
    }
}
