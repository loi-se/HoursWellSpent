using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HoursWellSpent
{
    public partial class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            //Me.InitializeLifetimeService()

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //Me.SetStyle(ControlStyles.ResizeRedraw, True)

            this.UpdateStyles();
            //Me.DoubleBuffered = True
        }



        //Public Sub DoubleBufferedPanel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        //    Dim a As Graphics = e.Graphics
        //    a.DrawImage(APEngine.bmp, New Point(0, 0))
        //End Sub
    }



	//Public Sub DoubleBufferedPanel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
	//    Dim a As Graphics = e.Graphics
	//    a.DrawImage(APEngine.bmp, New Point(0, 0))
	//End Sub
}
