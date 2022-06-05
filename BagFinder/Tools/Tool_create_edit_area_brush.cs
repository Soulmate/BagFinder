using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Tools
{
    internal class Tool_create_edit_area_brush : Tool
    {
        double _brush_size = 32;
        Brush brush_paint;
        //Color color_to_erase;
        Brush brush_erase;

        PointF pos = new PointF(0, 0);
        PointF previous_pos;

        enum CurrentStatus { None, Painting, Erasing };
        CurrentStatus currentStatus = CurrentStatus.None;
        Marker_area_brush _curremt_marker;

        Bitmap _temp_bitmap;

        public Tool_create_edit_area_brush(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "Brush";

            brush_paint =       new SolidBrush(Color.FromArgb(100, 255, 0, 0));
            //color_to_erase =    Color.FromArgb(100, 0, 255, 0);
            brush_erase = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
        }
        public override bool MouseDown(MouseEventArgs e)
        {
            if (OwnerToolSet.ToolEditMarkers.IsEditing) return false; //если попали в маркер и началось его редактирование
            int currentFrame = Program.Rewinder.ImNum;
            pos = e.Location;
            previous_pos = pos;
            var pos_Ic = Program.ViewerImage.Ct.Wc2Ic(pos);
            //найдем есть ли уже маркер такого типа на этом кадре
            var m_list = Program.Record.MarkersList.FindAll(
                m => m.FrameStart == currentFrame &&
                m.TypeText == "area_brush");
            Marker_area_brush marker;
            if (m_list.Count == 0)
            {
                marker = new Marker_area_brush() { F = currentFrame };
                Program.Record.MarkersList.Add(marker);
            }
            else
                marker = (Marker_area_brush)m_list[0];
            _temp_bitmap = (Bitmap)marker.b.Clone();


            if (e.Button == MouseButtons.Left)
            {
                using (var graphics = Graphics.FromImage(marker.b))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    graphics.FillEllipse(brush_paint, pos_Ic.X - (int)(_brush_size / 2), pos_Ic.Y - (int)(_brush_size / 2), (int)_brush_size, (int)_brush_size);
                }
                currentStatus = CurrentStatus.Painting;
            }

            if (e.Button == MouseButtons.Right)
            {               
                using (var graphics = Graphics.FromImage(marker.b))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    graphics.FillEllipse(brush_erase, pos_Ic.X - (int)(_brush_size / 2), pos_Ic.Y - (int)(_brush_size / 2), (int)_brush_size, (int)_brush_size);
                }
                currentStatus = CurrentStatus.Erasing;
            }
            _curremt_marker = marker;
            Program.ViewerImage.Invalidate();
            return true;
        }
        public override bool MouseMove(MouseEventArgs e)
        {
            previous_pos = pos;
            pos = e.Location;
            Program.ViewerImage.Invalidate();
            if (currentStatus != CurrentStatus.Painting && currentStatus != CurrentStatus.Erasing)
                return false;
            Brush brush;
            if (currentStatus == CurrentStatus.Painting)
                brush = brush_paint;
            else
                brush = brush_erase;
            var pos_Ic          = Program.ViewerImage.Ct.Wc2Ic(pos);
            var previous_pos_Ic = Program.ViewerImage.Ct.Wc2Ic(previous_pos);
            var marker = _curremt_marker;
            using (var graphics = Graphics.FromImage(marker.b))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.FillEllipse(brush, pos_Ic.X - (int)(_brush_size / 2), pos_Ic.Y - (int)(_brush_size / 2), (int)_brush_size, (int)_brush_size);
                graphics.FillPolygon(brush, GetFillCorners(pos_Ic, previous_pos_Ic, (int)(_brush_size / 2)));
            }
            Program.ViewerImage.Invalidate();
            Program.ViewerImage.Pb.Update();
            return true;
        }
        public override bool MouseUp(MouseEventArgs e)
        {            
            if (currentStatus != CurrentStatus.Painting && currentStatus != CurrentStatus.Erasing)
                return false; 
            //if (currentStatus == CurrentStatus.Erasing)
                //_curremt_marker.b.MakeTransparent(color_to_erase);
            currentStatus = CurrentStatus.None;
            Program.ViewerImage.Invalidate();
            return true;
        }
        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.OemOpenBrackets) // [
            {
                if (_brush_size > 4) _brush_size = _brush_size / 1.2;
                _brush_size = Math.Max(4, Math.Min(1000, _brush_size));
                Program.ViewerImage.Invalidate();
                return true;
            }
            if (e.KeyData == Keys.OemCloseBrackets) // ]
            {
                if (_brush_size < 1000) _brush_size = _brush_size * 1.2;
                _brush_size = Math.Max(4, Math.Min(1000, _brush_size));
                Program.ViewerImage.Invalidate();
                return true;
            }
            if (e.KeyData == Keys.C) //ctrl + D
            {
                var size = Program.Record.ImSize;
                //найдем есть ли уже маркер такого типа на этом кадре
                int currentFrame = Program.Rewinder.ImNum;
                var m_list = Program.Record.MarkersList.FindAll(
                    m => m.FrameStart == currentFrame &&
                    m.TypeText == "area_brush");


                if (m_list.Count == 1)
                {
                    Marker_area_brush marker = (Marker_area_brush)m_list[0];
                    marker.b = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); ;
                    Program.ViewerImage.Invalidate();
                }
                return true;
            }
            if (e.KeyData == Keys.Z) //UNDO
            {
                if (_temp_bitmap != null)
                {
                    var size = Program.Record.ImSize;
                    //найдем есть ли уже маркер такого типа на этом кадре
                    int currentFrame = Program.Rewinder.ImNum;
                    var m_list = Program.Record.MarkersList.FindAll(
                        m => m.FrameStart == currentFrame &&
                        m.TypeText == "area_brush");


                    if (m_list.Count == 1)
                    {
                        Marker_area_brush marker = (Marker_area_brush)m_list[0];
                        marker.b = (Bitmap)_temp_bitmap.Clone();
                        Program.ViewerImage.Invalidate();
                    }
                }
                return true;
            }
            return false;
        }

        public override bool MouseWheel(MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Alt)
            {
                _brush_size = _brush_size + (double)(e.Delta)/5;
                _brush_size = Math.Max(4, Math.Min(1000, _brush_size));
                Program.ViewerImage.Invalidate();
                return true;
            }
            return false;
        }

        public override bool Paint(Graphics g)
        {
            int visible_brush_size = (int)System.Math.Round(_brush_size * Program.ViewerImage.Ct.Icm);
            g.DrawEllipse(Pens.Red, pos.X - visible_brush_size / 2, pos.Y - visible_brush_size / 2, visible_brush_size, visible_brush_size);
            return false;
        }

        private PointF[] GetFillCorners(PointF p1, PointF p2, double r)
        {
            double x1 = p1.X;
            double y1 = p1.Y;
            double x2 = p2.X;
            double y2 = p2.Y;
            double D = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            double xu =  (y2 - y1) / D;
            double yu = -(x2 - x1) / D;
            return new PointF[] 
            {                
                new PointF((float)(x1 - r * xu), (float)(y1 - r * yu)),
                new PointF((float)(x1 + r * xu), (float)(y1 + r * yu)),
                new PointF((float)(x2 + r * xu), (float)(y2 + r * yu)),
                new PointF((float)(x2 - r * xu), (float)(y2 - r * yu))
            };
        }
    }
}