using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace PlayPlane_01.BLL
{
    class EntityFactory
    {
        enum ImgItem { boom_add = 1, bomb_icon = 2, bullet_0 = 3, bullet_1 = 4, bullet_add = 5, enemy_b = 6, enemy_m = 7, enemy_s = 8, explosion_01 = 9, explosion_02 = 10, explosion_03 = 11, hero_1 = 12, hero_2 = 13, pause_button = 14, resume_button = 15, smoke_01 = 16, smoke_02 = 17 };
        static Bitmap[] image_item = new Bitmap[18];
        public static void InitFactory(string xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNode parent = xmlDoc.SelectSingleNode("TextureAtlas");
            Bitmap bmp = new Bitmap(Bitmap.FromFile(System.IO.Path.GetDirectoryName(xmlPath) + "/" + ((XmlElement)parent).GetAttribute("imagePath")));
            XmlNodeList nodes = parent.ChildNodes;
            int i = 1;

            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                string name = xe.GetAttribute("name");
                int x = int.Parse(xe.GetAttribute("x"));
                int y = int.Parse(xe.GetAttribute("y"));
                int width = int.Parse(xe.GetAttribute("width"));
                int height = int.Parse(xe.GetAttribute("height"));
                Bitmap subBmp = new Bitmap(width, height);

                Graphics g = Graphics.FromImage(subBmp);
                g.DrawImage(bmp, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                image_item[i] = subBmp;
                i++;
            }
        }

        public static Plane GenPlane(string style)
        {
            if ("normal".Equals(style))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.hero_1];
                return new Plane("small", 250,500, tempBmp.Width, tempBmp.Height,0, 0, tempBmp);
            }
            else if ("super".Equals(style))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.hero_2];
                return new Plane("mid", 350, 700, tempBmp.Width, tempBmp.Height, 0,0, tempBmp);
            }
            return null;
        }

        public static Enemy GenEnemy(string size,int speedBase)
        {
            if ("small".Equals(size))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.enemy_s];
                return new Enemy("small", new Random().Next(450)+50, 0, tempBmp.Width, tempBmp.Height,new Random().Next(10000)%5-2, new Random().Next(10000)%4+2+speedBase,1,tempBmp);
            }
            else if ("mid".Equals(size))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.enemy_m];
                return new Enemy("mid", new Random().Next(450) + 50, 0, tempBmp.Width, tempBmp.Height, new Random().Next(10000) % 5 - 2, new Random().Next(10000) % 4 + 1+speedBase, 5, tempBmp);
            }
            else if ("big".Equals(size))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.enemy_b];
                return new Enemy("big", new Random().Next(450) + 50, 0, tempBmp.Width, tempBmp.Height, new Random().Next(10000) % 3 - 1, new Random().Next(10000) % 3 + 1+speedBase, 20, tempBmp);
            }
            return null;
        }

        public static Bullet GenBullet(string style,int p_x,int p_y)
        {
            if ("red".Equals(style))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.bullet_0];
                return new Bullet("small", p_x, p_y, tempBmp.Width, tempBmp.Height,0, 20, tempBmp);
            }
            else if ("blue".Equals(style))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.bullet_1];
                return new Bullet("mid", p_x, p_y, tempBmp.Width, tempBmp.Height,0, 20, tempBmp);
            }
            return null;
        }

        public static Reward GenReward(string style, int p_x, int p_y)
        {
            if ("bullet_add".Equals(style))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.bullet_add];
                return new Reward("bullet_add", p_x, p_y, tempBmp.Width, tempBmp.Height, new Random().Next(10000) % 5 - 2, 3, 5000, tempBmp);
            }
            else if ("boom_add".Equals(style))
            {
                Bitmap tempBmp = image_item[(int)ImgItem.boom_add];
                return new Reward("boom_add", p_x, p_y, tempBmp.Width, tempBmp.Height, new Random().Next(10000) % 5 - 2, 3, 5000, tempBmp);
            }
            return null;
        }

        public static Bitmap GetBoomIcon()
        {
            return image_item[(int)ImgItem.bomb_icon];
        }

        public static Explosion GenExplosion(string style, int p_x, int p_y)
        {
            if ("small".Equals(style))
            {
                Bitmap[] tempBmp = { image_item[(int)ImgItem.explosion_01], 
                  image_item[(int)ImgItem.explosion_02], image_item[(int)ImgItem.explosion_03] ,
                  image_item[(int)ImgItem.explosion_02],image_item[(int)ImgItem.explosion_01]};
                return new Explosion(p_x, p_y, 300, tempBmp);
            }
            else if ("mid".Equals(style))
            {
                Bitmap[] tempBmp = { image_item[(int)ImgItem.explosion_01] };
                return new Explosion(p_x, p_y, 500, tempBmp);
            }
            else if ("big".Equals(style))
            {
                Bitmap[] tempBmp = { image_item[(int)ImgItem.explosion_01] };
                return new Explosion(p_x, p_y, 500, tempBmp);
            }
            return null;
        }
    }
}
