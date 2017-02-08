
using System;
using System.Windows.Forms;

namespace SpaceSim {
    public delegate void SpaceTimeTickHandler(int daysPassed);

    public class SpaceTime {
        private Timer timer;
        public event SpaceTimeTickHandler tick;
        public int rate { get; set; }

        public SpaceTime() {
            timer = new Timer();
            timer.Interval = 1000/30;
            timer.Tick += onTick;
            timer.Start();
        }

        private void onTick(object sender, EventArgs e) {
            tick(rate);
        }
    }
}