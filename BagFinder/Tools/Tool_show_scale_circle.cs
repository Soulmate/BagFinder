using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolShowScaleCircle : Tool
    {
        private bool _showScaleCircle = false;

        private float tention; //притяжение 
        private float visc; // 0-1, 1 - нет вязкости
        private List<float> vx = new List<float>(); //текущая скорость
        private List<float> vy = new List<float>(); //текущая скорость
        private List<PointF> p      = new List<PointF>(); //текущие позиции
        private List<PointF> prev_p = new List<PointF>(); //предыдущая позиция
        private PointF target_pos; // положение цели
        private bool target_hit; // попали в цель
        private Random rand = new Random();

        public ToolShowScaleCircle(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "sc";
        }

        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.O)
            {
                if (_showScaleCircle == false)
                {
                    PointF mp = Program.ViewerImage.Pb.PointToClient(Cursor.Position);
                    var rWcf = Program.ProgramSettings.ScaleCircle * (float)Program.ViewerImage.Ct.Icm;
                    mp.Y -= rWcf;
                    _showScaleCircle = true;

                    vx.Clear();
                    vy.Clear();
                    p.Clear();
                    prev_p.Clear();
                    p.Add(mp);
                    prev_p.Add(mp);
                    vx.Add(0);
                    vy.Add(0);


                    tention = 10; //притяжение 
                    visc = 1.01F; // 1-inf, 1 - нет вязкости
                   


                    float target_x_w = rand.Next(Program.ViewerImage.Pb.Width);
                    float target_y_w = rand.Next(Program.ViewerImage.Pb.Height);
                    target_pos = new PointF(target_x_w, target_y_w);
                    Program.ViewerImage.Invalidate();
                }
            }

            return false;
        }

        public override bool KeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.O)
            {
                _showScaleCircle = false;
                Program.ViewerImage.Invalidate();
            }

            return false;
        }

        public override bool MouseMove(MouseEventArgs e)
        {
            if (_showScaleCircle)
            {
                PointF mp = e.Location;
                var rWcf = Program.ProgramSettings.ScaleCircle * (float)Program.ViewerImage.Ct.Icm;
                mp.Y -= rWcf;
                float dt = 0.01F; // (DateTime.Now - prev_t).TotalSeconds;
                for (int i = 0; i<vx.Count; i++)
                {
                    if (i == 0)
                    {
                        float tention_i = tention;// / (i * 0.01F + 1);
                        float visc_i = visc;
                        vx[i] = vx[i] + dt * tention_i * (mp.X - prev_p[i].X);
                        vy[i] = vy[i] + dt * tention_i * (mp.Y - prev_p[i].Y);
                        //float dist = (float)Math.Sqrt((mp.X - prev_p[i].X) * (mp.X - prev_p[i].X) + (mp.Y - prev_p[i].Y) * (mp.Y - prev_p[i].Y)) + 10;
                        //vx[i] = vx[i] + dt * 1e4f * tention_i * (mp.X - p[0].X) / dist / dist  ;
                        //vy[i] = vy[i] + dt * 1e4f * tention_i * (mp.Y - p[0].Y) / dist / dist ;
                        vx[i] /= visc_i;
                        vy[i] /= visc_i;
                        if (p[0].X < 0 && vx[0] < 0) {
                            vx[0] *= -1;
                            p[0] = new PointF(rWcf + 1, p[0].Y);
                        }
                        if (p[0].Y < 0 && vy[0] < 0) {
                            vy[0] *= -1;
                            p[0] = new PointF(p[0].X, rWcf + 1);
                        }
                        if (p[0].X > Program.ViewerImage.Pb.Width  && vx[0] > 0) {
                            vx[0] *= -1;
                            p[0] = new PointF(Program.ViewerImage.Pb.Width - rWcf - 1, p[0].Y);
                        }
                        if (p[0].Y > Program.ViewerImage.Pb.Height && vy[0] > 0) {
                            vy[0] *= -1;
                            p[0] = new PointF(p[0].X, Program.ViewerImage.Pb.Height - rWcf - 1);
                        }
                    }
                    else
                    {
                        float tention_i = tention * 50;// / ((i - 1) * 0.05F + 1);
                        float visc_i = visc * 2;//* ((i-1) * 0.05F + 1);
                        float fluct = (float)Math.Pow(rand.NextDouble(),10);
                        vx[i] = vx[i] + dt * tention_i * (p[i - 1].X - prev_p[i].X) * (1 + fluct);
                        vy[i] = vy[i] + dt * tention_i * (p[i - 1].Y - prev_p[i].Y) * (1 + fluct);
                        vx[i] /= visc_i;
                        vy[i] /= visc_i;
                    }

                    
                    
                    p[i] = new PointF(prev_p[i].X + vx[i] * dt, prev_p[i].Y + vy[i] * dt);
                }
                
                prev_p = new List<PointF>(p);

                
                target_hit = (Math.Abs(p[0].X  - target_pos.X) < rWcf && Math.Abs(p[0].Y - target_pos.Y) < rWcf);
                if (target_hit)
                {
                    float target_x_w = rand.Next(Program.ViewerImage.Pb.Width);
                    float target_y_w = rand.Next(Program.ViewerImage.Pb.Height);
                    target_pos = new PointF(target_x_w, target_y_w);

                    float Vabs0 = (float)Math.Sqrt(vx[0] * vx[0] + vy[0] * vy[0]);
                    var newP = new PointF(
                        p[0].X - vx[0] / Vabs0 * rWcf * 2,
                        p[0].Y - vy[0] / Vabs0 * rWcf * 2);
                    p.Add(newP);
                    prev_p.Add(newP);
                    vx.Add(0);
                    vy.Add(0);
                }

                bool gameOver = false;
                for (int i = 1; i < p.Count; i++)
                {
                    gameOver |= Math.Abs(p[0].X - p[i].X) < rWcf && Math.Abs(p[0].Y - p[i].Y) < rWcf;
                }
                if (gameOver)
                {
                    vx.Clear();
                    vy.Clear();
                    p.Clear();
                    prev_p.Clear();
                    p.Add(mp);
                    prev_p.Add(mp);
                    vx.Add(0);
                    vy.Add(0);
                }
                Program.ViewerImage.Invalidate();
            }
            return false;
        }

        public override bool Paint(Graphics g)
        {
            if (_showScaleCircle)
            {
                var rWcf = Program.ProgramSettings.ScaleCircle * (float) Program.ViewerImage.Ct.Icm;
                g.DrawEllipse(new Pen(Program.ProgramSettings.ScaleCircleColor, 2), p[0].X - rWcf, p[0].Y - rWcf, 2 * rWcf, 2 * rWcf);
                //g.DrawLine(Pens.Red, p[0].X, p[0].Y, p[0].X + vx[0]/10, p[0].Y + vy[0] / 10);

                //target
                g.DrawLine(Pens.Red, target_pos.X - 5, target_pos.Y, target_pos.X + 5, target_pos.Y);
                g.DrawLine(Pens.Red, target_pos.X, target_pos.Y - 5, target_pos.X, target_pos.Y + 5);

                for (int i = 1; i < p.Count; i++)
                {
                    g.DrawLine(Pens.Red, p[i].X - 5, p[i].Y, p[i].X + 5, p[i].Y);
                    g.DrawLine(Pens.Red, p[i].X, p[i].Y - 5, p[i].X, p[i].Y + 5);
                }

                if (p.Count > 1)
                {
                    g.DrawString((p.Count - 1).ToString(), new Font(FontFamily.GenericSerif, 14),Brushes.Red,p[0],
                        new StringFormat{LineAlignment = StringAlignment.Center,Alignment = StringAlignment.Center});
                }
            }

            return false;
        }
    }
}
