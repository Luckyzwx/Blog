using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayPlane_01.BLL
{
    class Enemy:Entity
    {
        public int HP { get; set; }
        public Enemy(string name, int x, int y, int width, int height, int speedX, int speedY, int hp, System.Drawing.Bitmap bmp)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.SpeedX = speedX;
            this.SpeedY = speedY;
            this.HP = hp;
            this.Image = bmp;
        }
    }
}
