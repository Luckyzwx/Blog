using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Media;
using PlayPlane_01.BLL;

namespace PlayPlane_01
{
    public partial class Form1 : Form
    {
        Plane plane;

        Timer t_draw;

        List<Enemy> enemy_lsit = new List<Enemy>();
        List<Bullet> bullet_lsit = new List<Bullet>();
        List<Explosion> explosion_list = new List<Explosion>();
        List<Reward> reward_list = new List<Reward>();
        int score = 0;
        int boom_count = 5;
        bool pause = false;
        Bitmap background;

        public Form1()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;
            EntityFactory.InitFactory("resource/plane.xml");
            background = new Bitmap(Image.FromFile(@"resource/bg_02.jpg"));
            plane = EntityFactory.GenPlane("normal");
            this.Cursor.Dispose();
            Cursor.Position = new Point(plane.X + this.Location.X, plane.Y + this.Location.Y);
            t_draw = new Timer();
            t_draw.Interval = 20;
            send_interval = 100 / t_draw.Interval;
            block_interval = 260 / t_draw.Interval;
            reward_interval = 5000 / t_draw.Interval;
            t_draw.Tick += new EventHandler(t_draw_Tick);
            t_draw.Start();
        }

        void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!pause && e.Button == MouseButtons.Right)
            {
                if (boom_count > 0)
                {
                    boom_count--;
                    for (int i = 0; i < enemy_lsit.Count; i++)
                    {
                        //socre ++
                        if (enemy_lsit[i].Name == "small") score += 1000;
                        else if (enemy_lsit[i].Name == "mid") score += 6000;
                        else if (enemy_lsit[i].Name == "big") score += 25000;
                        //add to explosion
                        explosion_list.Add(EntityFactory.GenExplosion("small", enemy_lsit[i].X, enemy_lsit[i].Y));

                        new DXPlay(this, @"resource/BOMB3.wav").ThreadPlay();
                    }
                    enemy_lsit.Clear();
                }
            }
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!pause)
            {
                plane.X = e.X;
                plane.Y = e.Y;
            }
        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                pause = !pause;
                if (pause)
                {
                    this.Cursor = new Cursor (Cursors.Arrow.CopyHandle());
                }
                else
                {
                    this.Cursor.Dispose();
                    Cursor.Position = new Point(plane.X + this.Location.X, plane.Y + this.Location.Y);
                }
            }
            /*else if (e.KeyChar == 27)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (e.KeyChar == '\r')
            {
                this.WindowState = FormWindowState.Maximized;
            }*/
        }

        int block_time = 1;
        int block_interval = 0;
        int send_time = 0;
        int send_interval = 0;
        int reward_time = 1;
        int reward_interval = 0;
        int rwd_bullet_stnTime = 0;

        int backY = 800;

        //DateTime dis = DateTime.Now;

        private void t_draw_Tick(object sender, EventArgs e)
        {
            //Console.Write("\n   ALLTime--->" + (DateTime.Now - dis));
            if (pause)
            {
                this.CreateGraphics().DrawString("暂 停", new Font("微软雅黑", 22), Brushes.Red, new PointF(this.Width / 2 - 30, this.Height / 2 - 50));
                return;
            }

            //////////////////////////////////////////////////////////////////
            ///////////////////                  /////////////////////////////
            //////////////////       Create     //////////////////////////////
            /////////////////                  ///////////////////////////////
            //////////////////////////////////////////////////////////////////

            /*------send bullets-----*/
            if (send_time > send_interval)
            {
                if (rwd_bullet_stnTime > 0)
                {
                    bullet_lsit.Add(EntityFactory.GenBullet("blue", plane.X - 6, plane.Y - 50));
                    bullet_lsit.Add(EntityFactory.GenBullet("blue", plane.X + 6, plane.Y - 50));
                    rwd_bullet_stnTime -= t_draw.Interval * send_interval;
                }
                else
                {
                    bullet_lsit.Add(EntityFactory.GenBullet("red", plane.X, plane.Y - 50));
                }
                new DXPlay(this, @"resource/shoot.wav").ThreadPlay();
                send_time = 0;
            }

            /*------generate enemy-----*/
            if (block_time % block_interval == 0)
            {
                int speedBase = 0;
                if (block_interval < 2)
                    speedBase = 1;
                if (block_interval < 5)
                    speedBase = 2;
                else if (block_interval < 10)
                    speedBase = 1;
             
                if (block_time % (block_interval * 20) == 0)
                {
                    enemy_lsit.Add(EntityFactory.GenEnemy("big",speedBase));
                }
                else if (block_time % (block_interval * 10) == 0)
                {
                    enemy_lsit.Add(EntityFactory.GenEnemy("mid", speedBase));
                }
                else
                {
                    enemy_lsit.Add(EntityFactory.GenEnemy("small", speedBase));
                }
            }

            /*-----reward-----*/
            if (reward_time == reward_interval)
            {
                if (new Random().Next(10000) % 2 == 0)
                {
                    reward_list.Add(EntityFactory.GenReward("bullet_add", new Random().Next(50, this.Width - 50), 0));
                }
                else
                {
                    reward_list.Add(EntityFactory.GenReward("boom_add", new Random().Next(50, this.Width - 50), 0));
                }
                reward_time = 0;
            }

            send_time++;
            block_time++;
            reward_time++;

            //////////////////////////////////////////////////////////////////
            ///////////////////                  /////////////////////////////
            //////////////////       Judge      //////////////////////////////
            /////////////////                  ///////////////////////////////
            //////////////////////////////////////////////////////////////////

            /*-----plane level up-----*/
            if (send_interval>0&&score > plane.Level * plane.Level * 50000)
            {
                plane.LevelUp();
                send_interval--;
            }

            /*-----enemy lv up-----*/
            if (block_interval > 1 && block_time % 300 == 300-1)
            {
                block_interval--;
            }

            /*-----enemy crash-----*/
            for (int i = 0; i < enemy_lsit.Count; i++)
            {
                for (int j = 0; j < bullet_lsit.Count; j++)
                {
                    if (Math.Abs(bullet_lsit[j].X - enemy_lsit[i].X) < (bullet_lsit[j].Width + enemy_lsit[i].Width) / 2 && Math.Abs(bullet_lsit[j].Y - enemy_lsit[i].Y) < (bullet_lsit[j].Height + enemy_lsit[i].Height) / 2)
                    {
                        enemy_lsit[i].HP--;

                        if (enemy_lsit[i].HP == 0)//explose
                        {
                            //socre ++
                            if (enemy_lsit[i].Name == "small") score += 1000;
                            else if (enemy_lsit[i].Name == "mid") score += 6000;
                            else if (enemy_lsit[i].Name == "big") score += 25000;
                            //add to explosion
                            explosion_list.Add(EntityFactory.GenExplosion("small", enemy_lsit[i].X, enemy_lsit[i].Y));

                            new DXPlay(this, @"resource/explosion.wav").ThreadPlay();

                            //remove both
                            enemy_lsit.Remove(enemy_lsit[i]);
                            bullet_lsit.Remove(bullet_lsit[j]);
                        }
                        else
                        {
                            //g.FillRectangle(Brushes.Red,new Rectangle(bullet_lsit[j].X,bullet_lsit[j].Y-bullet_lsit[j].Width/2,30,5));
                            bullet_lsit.Remove(bullet_lsit[j]);
                        }
                        break;
                    }
                }
            }

            /*-----get reward-----*/
            for (int i = 0; i < reward_list.Count; i++)
            {
                if (Math.Abs(plane.X - reward_list[i].X) < (plane.Width + reward_list[i].Width) / 2 && Math.Abs(plane.Y - reward_list[i].Y) < (plane.Height + reward_list[i].Height) / 2)
                {
                    if (reward_list[i].Name == "bullet_add")
                    {
                        rwd_bullet_stnTime += reward_list[i].StnTimes;
                    }
                    else if (reward_list[i].Name == "boom_add")
                    {
                        boom_count++;
                    }
                    reward_list.Remove(reward_list[i]);
                }
            }

            /*-----plane crash-----*/
            for (int i = 0; i < enemy_lsit.Count; i++)
            {
                bool isCrashed = false;
                if (Math.Abs(plane.X - enemy_lsit[i].X) < (plane.Width / 4 + enemy_lsit[i].Width) / 2 && Math.Abs(plane.Y - enemy_lsit[i].Y) < (plane.Height - 30 + enemy_lsit[i].Height) / 2)
                {
                    isCrashed = true;
                }
                if (isCrashed)
                {
                    t_draw.Stop();
                    this.CreateGraphics().DrawString("Game Over", new Font("微软雅黑", 22), Brushes.Red, new PointF(this.Width / 2 - 100, this.Height / 2 - 50));
                    //enemy_lsit.Remove(enemy_lsit[i]);
                    return;
                }
            }

            //////////////////////////////////////////////////////////////////
            ///////////////////                  /////////////////////////////
            //////////////////       Draw       //////////////////////////////
            /////////////////                  ///////////////////////////////
            //////////////////////////////////////////////////////////////////
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bmp);
            /*-----clear panel-----*/
            g.Clear(this.BackColor);

            /*-----background-----*/
            int img_count = 0;
            if (background.Width < this.Width)
            {
                Bitmap tempBg = new Bitmap(this.Width, 1600);
                while (background.Width * (img_count) < this.Width)
                {
                    Graphics g_tempBg = Graphics.FromImage(tempBg);
                    g_tempBg.DrawImage(background, background.Width * img_count, 0);
                    g_tempBg.DrawImage(background, background.Width * img_count, 800);
                    img_count++;
                }
                background = tempBg;
            }
            g.DrawImage(background, new Rectangle(0, 0, this.Width, this.Height), new Rectangle(0, backY, this.Width, this.Height), GraphicsUnit.Pixel);
            backY -= 2;
            if (backY < 0)
                backY = 800;

            /*------plane------*/

            g.DrawImage(plane.Image, new Point(plane.X - plane.Width / 2, plane.Y - plane.Height / 2));

            /*-----bullets-----*/
            for (int i = 0; i < bullet_lsit.Count; i++)
            {
                g.DrawImage(bullet_lsit[i].Image, new Point(bullet_lsit[i].X - bullet_lsit[i].Width / 2, bullet_lsit[i].Y - bullet_lsit[i].Height / 2));
                bullet_lsit[i].Y -= bullet_lsit[i].SpeedY;
                if (bullet_lsit[i].Y < -40)
                {
                    bullet_lsit.Remove(bullet_lsit[i]);
                }
            }

            /*-----draw reward-----*/
            for (int i = 0; i < reward_list.Count; i++)
            {
                g.DrawImage(reward_list[i].Image, new Point(reward_list[i].X - reward_list[i].Width / 2, reward_list[i].Y - reward_list[i].Height / 2));
                reward_list[i].Y += reward_list[i].SpeedY;
                reward_list[i].X += reward_list[i].SpeedX;
                if (reward_list[i].Y > this.Height + 20)
                {
                    reward_list.Remove(reward_list[i]);
                }
            }

            /*-----draw boom icon-----*/
            Bitmap boom_icon = EntityFactory.GetBoomIcon();
            if (boom_count > 0)
            {
                g.DrawImage(boom_icon, new Point(10, this.Height - 40 - boom_icon.Height));
                g.DrawString("×" + boom_count, new Font("微软雅黑", 18), Brushes.RosyBrown, new Point(10 + boom_icon.Width, this.Height - 40 - boom_icon.Height));
            }

            /*-----enemy-----*/
            for (int i = 0; i < enemy_lsit.Count; i++)
            {
                g.DrawImage(enemy_lsit[i].Image, new Point(enemy_lsit[i].X - enemy_lsit[i].Width / 2, enemy_lsit[i].Y - enemy_lsit[i].Height / 2));

                enemy_lsit[i].Y += enemy_lsit[i].SpeedY;
                enemy_lsit[i].X += enemy_lsit[i].SpeedX;
                if (enemy_lsit[i].X > this.Width || enemy_lsit[i].X < 0)
                {
                    enemy_lsit[i].SpeedX = -enemy_lsit[i].SpeedX;
                }
                if (enemy_lsit[i].Y > this.Width + 20)
                {
                    enemy_lsit.Remove(enemy_lsit[i]);
                }
            }

            /*-----draw explose-----*/
            for (int i = 0; i < explosion_list.Count; i++)
            {
                Bitmap temp_explose = explosion_list[i].Images[explosion_list[i].Counter / (explosion_list[i].StnTimes / explosion_list[i].Images.Length)];
                g.DrawImage(temp_explose, new Point(explosion_list[i].X - temp_explose.Width / 2, explosion_list[i].Y - temp_explose.Height / 2));
                explosion_list[i].Counter += 24;
                if (explosion_list[i].Counter > explosion_list[i].StnTimes)
                    explosion_list.Remove(explosion_list[i]);
            }

            /*-----score panel-----*/
            g.DrawString("分数：" + score, new Font("微软雅黑", 14), Brushes.Green, new PointF(10, 10));
            /*-----level panel-----*/
            g.DrawString("等级：" + (send_interval == 1 ? "满级" : plane.Level.ToString()), new Font("微软雅黑", 14), Brushes.Green, new PointF(this.Width - 120, 10));
            
            g.Dispose();
            this.CreateGraphics().DrawImage(bmp, 0, 0);
            bmp.Dispose();
            //dis = DateTime.Now;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
