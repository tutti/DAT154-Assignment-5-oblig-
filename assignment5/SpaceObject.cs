using System;

namespace SpaceSim {
    public class SpaceObject {
        public string name { get; set; }
        public SpaceObject parent { get; set; }
        public int radius { get; set; } // In km
        public int distance { get; set; } // Distance from parent object in 1000 km
        public double period { get; set; } // Earth days to complete 1 orbit

        public SpaceObject(String _name, int _radius, SpaceObject _parent, int _distance, double _period) {
            name = _name;
            radius = _radius;
            parent = _parent;
            distance = _distance;
            period = _period;
        }
    }
}
