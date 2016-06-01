using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayPlane_01.BLL
{
    class Plane :Entity
    {
        public int Level { get; set; }
        public Plane(string name, int x, int y, int width, int height, int speedX,int speedY, System.Drawing.Bitmap bmp)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.SpeedX = speedX;
            this.SpeedY = speedY;
            this.Image = bmp;
            this.Level = 1;
        }

        public void LevelUp()
        {
            Level++;
        }
    }
}
