using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayPlane_01.BLL
{
    class Entity
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SpeedY { get; set; }
        public int SpeedX { get; set; }
        public System.Drawing.Bitmap Image{get;set;}
        public override string ToString()
        {
            return Name;
            //return base.ToString();
        }
    }
}
