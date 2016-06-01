using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayPlane_01.BLL
{
    class Explosion
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int StnTimes { get; set; }
        public int Counter { get; set; }
        public System.Drawing.Bitmap[] Images { get; set; }

        public Explosion(int x, int y,  int stnTimes, System.Drawing.Bitmap[] bmp)
        {
            this.X = x;
            this.Y = y;
            this.StnTimes = stnTimes;
            this.Images = bmp;
            this.Counter = 0;
        }
    }
}
