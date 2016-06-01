using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayPlane_01.BLL
{
    class Reward:Entity
    {
        public int StnTimes { get; set; }
        public int Counter { get; set; }

        public Reward(string name, int x, int y, int width, int height, int speedX, int speedY, int stnTimes, System.Drawing.Bitmap bmp)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.SpeedX = speedX;
            this.SpeedY = speedY;
            this.StnTimes = stnTimes;
            this.Image = bmp;
            this.Counter = 0;
        }
    }
}
