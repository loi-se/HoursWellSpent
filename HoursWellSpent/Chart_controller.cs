using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Calendar;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Collections;

namespace HoursWellSpent
{
    class Chart_controller
    {
        public Chart Chart1;
        public string PixelPointWidth = "8";

        // Determines the interval when a scrollbar is active in a chart! With a value of 20 all labels are readable.
        public int _blockSize = 30; 

        // Show number of agenda items per weekday!:
        public Chart showchart_daysofweek(List<CalendarItem> _calendaritems, int _width, int _height, Boolean show_hours)
        {

            try
            {
                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = 100;

                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;

                int monday = 0;
                int tuesday = 0;
                int wednesday = 0;
                int thursday = 0;
                int friday = 0;
                int saturday = 0;
                int sunday = 0;

                if (show_hours == false)
                {
                    foreach (CalendarItem _item in _calendaritems)
                    {

                        if (_item.Date.DayOfWeek.ToString() == "Monday")
                        {
                            monday = monday + 1;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Tuesday")
                        {
                            tuesday = tuesday + 1;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Wednesday")
                        {
                            wednesday = wednesday + 1;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Thursday")
                        {
                            thursday = thursday + 1;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Friday")
                        {
                            friday = friday + 1;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Saturday")
                        {
                            saturday = saturday + 1;
                        }

                        else if (_item.Date.DayOfWeek.ToString() == "Sunday")
                        {
                            sunday = sunday + 1;
                        }
                    }

                    Chartarea1_AxisY_Title = "Agenda items";
                }
                else if (show_hours == true)
                {
                    foreach (CalendarItem _item in _calendaritems)
                    {

                        if (_item.Date.DayOfWeek.ToString() == "Monday")
                        {
                            monday = monday + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Tuesday")
                        {
                            tuesday = tuesday + Convert.ToInt32(_item.Duration.TotalHours); ;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Wednesday")
                        {
                            wednesday = wednesday + Convert.ToInt32(_item.Duration.TotalHours); ;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Thursday")
                        {
                            thursday = thursday + Convert.ToInt32(_item.Duration.TotalHours); ;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Friday")
                        {
                            friday = friday + Convert.ToInt32(_item.Duration.TotalHours); ;
                        }
                        else if (_item.Date.DayOfWeek.ToString() == "Saturday")
                        {
                            saturday = saturday + Convert.ToInt32(_item.Duration.TotalHours);
                        }

                        else if (_item.Date.DayOfWeek.ToString() == "Sunday")
                        {
                            sunday = sunday + Convert.ToInt32(_item.Duration.TotalHours); ;
                        }
                    }
                    Chartarea1_AxisY_Title = "Hours";
                }


                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisY.Title = Chartarea1_AxisY_Title;

                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Days of the week";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i;

                for (i = 0; i < 7; i++)
                {
                    string dayofweek = "";
                    int agendaitems = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    if (i == 0)
                    {
                        dayofweek = "Monday";
                        agendaitems = monday;
                    }
                    else if (i ==1)
                    {
                        dayofweek = "Tuesday";
                        agendaitems = tuesday;
                    }
                    else if (i == 2)
                    {
                        dayofweek = "Wednesday";
                        agendaitems = wednesday;
                    }
                    else if (i == 3)
                    {
                        dayofweek = "Thursday";
                        agendaitems = thursday;
                    }
                    else if (i == 4)
                    {
                        dayofweek = "Friday";
                        agendaitems = friday;
                    }
                    else if (i == 5)
                    {
                        dayofweek = "Saturday";
                        agendaitems = saturday;
                    }
                    else if (i == 6)
                    {
                        dayofweek = "Sunday";
                        agendaitems = sunday;
                    }




                    dp.SetValueXY(i, agendaitems);
                    dp.AxisLabel = dayofweek;
                    dp.Label = agendaitems.ToString();
                    dp.LegendText = dayofweek;

                    series1.Points.Add(dp);

                }
                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Agenda items";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;



                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;


                //--------------------------------------------------------------------------------------------------scrollbar
                //Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                //Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 7;

                //// enable autoscroll
                //Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                //// let's zoom to [0,blockSize] (e.g. [0,100])
                ////Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                //int position = 0;
                //int size = blockSize;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //// disable zoom-reset button (only scrollbar's arrows are available)
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                //// set scrollbar small change to blockSize (e.g. 100)
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                // ----------------------------------------------------------------------------------------------scrollbar



                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }




        // Show number of total agenda items per month:
        public Chart showchart_months(List<CalendarItem> _calendaritems, int _width, int _height, Boolean show_hours)
        {

            try
            {
                string Chartarea1_AxisY_Title = "";
                int blockSize = 100;
                //ArrayList foldersizes = new ArrayList();
                //pnoffilesfolder.Controls.Clear();
                //pnoffilesfolder.AutoScroll = true;

                Chart1 = new Chart();

                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;

                int january = 0;
                int february = 0;
                int march = 0;
                int april = 0;
                int may = 0;
                int june = 0;
                int july = 0;
                int august = 0;
                int september = 0;
                int october = 0;
                int november = 0;
                int december = 0;


                if (show_hours == false)
                {
                    foreach (CalendarItem _item in _calendaritems)
                    {

                        if (_item.Date.Month == 1)
                        {
                            january = january + 1;
                        }
                        else if (_item.Date.Month == 2)
                        {
                            february = february + 1;
                        }
                        else if (_item.Date.Month == 3)
                        {
                            march = march + 1;
                        }
                        else if (_item.Date.Month == 4)
                        {
                            april = april + 1;
                        }
                        else if (_item.Date.Month == 5)
                        {
                            may = may + 1;
                        }
                        else if (_item.Date.Month == 6)
                        {
                            june = june + 1;
                        }

                        else if (_item.Date.Month == 7)
                        {
                            july = july + 1;
                        }
                        else if (_item.Date.Month == 8)
                        {
                            august = august + 1;
                        }
                        else if (_item.Date.Month == 9)
                        {
                            september = september + 1;
                        }
                        else if (_item.Date.Month == 10)
                        {
                            october = october + 1;
                        }
                        else if (_item.Date.Month == 11)
                        {
                            november = november + 1;
                        }
                        else if (_item.Date.Month == 12)
                        {
                            december = december + 1;
                        }
                    }
                    Chartarea1_AxisY_Title = "Agenda items";
                }
                else if (show_hours == true)
                {

                    foreach (CalendarItem _item in _calendaritems)
                    {

                        if (_item.Date.Month == 1)
                        {
                            january = january + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 2)
                        {
                            february = february + Convert.ToInt32(_item.Duration.TotalHours); 
                        }
                        else if (_item.Date.Month == 3)
                        {
                            march = march + Convert.ToInt32(_item.Duration.TotalHours); 
                        }
                        else if (_item.Date.Month == 4)
                        {
                            april = april + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 5)
                        {
                            may = may + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 6)
                        {
                            june = june + Convert.ToInt32(_item.Duration.TotalHours);
                        }

                        else if (_item.Date.Month == 7)
                        {
                            july = july + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 8)
                        {
                            august = august + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 9)
                        {
                            september = september + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 10)
                        {
                            october = october + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 11)
                        {
                            november = november + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                        else if (_item.Date.Month == 12)
                        {
                            december = december + Convert.ToInt32(_item.Duration.TotalHours);
                        }
                    }

                    Chartarea1_AxisY_Title = "Hours";

                }




                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisY.Title = Chartarea1_AxisY_Title;

                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Month";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i;

                for (i = 0; i < 12; i++)
                {
                    string month = "";
                    int agendaitems = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    if (i == 0)
                    {
                        month = "January";
                        agendaitems = january;
                    }
                    else if (i == 1)
                    {
                        month = "February";
                        agendaitems = february;
                    }
                    else if (i == 2)
                    {
                        month = "March";
                        agendaitems = march;
                    }
                    else if (i == 3)
                    {
                        month = "April";
                        agendaitems = april;
                    }
                    else if (i == 4)
                    {
                        month = "May";
                        agendaitems = may;
                    }
                    else if (i == 5)
                    {
                        month = "June";
                        agendaitems = june;
                    }
                    else if (i == 6)
                    {
                        month = "July";
                        agendaitems = july;
                    }
                    else if (i == 7)
                    {
                        month = "August";
                        agendaitems = august;
                    }
                    else if (i == 8)
                    {
                        month = "September";
                        agendaitems = september;
                    }
                    else if (i == 9)
                    {
                        month = "October";
                        agendaitems = october;
                    }
                    else if (i == 10)
                    {
                        month = "November";
                        agendaitems = november;
                    }
                    else if (i == 11)
                    {
                        month = "December";
                        agendaitems = december;
                    }
                   



                    dp.SetValueXY(i, agendaitems);
                    dp.AxisLabel = month;
                    dp.Label = agendaitems.ToString();
                    dp.LegendText = month;

                    series1.Points.Add(dp);

                }

                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Agenda items";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;



                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;


                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;


            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }






        public Chart showchart2(List<CalendarItem> _calendaritems, int _width, int _height)
        {

            try
            {
                int blockSize = 100;
                //ArrayList foldersizes = new ArrayList();
                //pnoffilesfolder.Controls.Clear();
                //pnoffilesfolder.AutoScroll = true;

                Chart1 = new Chart();

                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;

                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisY.Title = "Duration in hours";

                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Agenda items";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";

                int i2 = 0;
                foreach (CalendarItem _item in _calendaritems)
                {

                    //FolderItem _folderitem = item;
                    dp = new DataPoint();
                    //yvalue = ((-10 * i) ^ 2) - (3 * i) - (4)

                    // int noffilestopdir = _folderitem.noffiles_topdirectory;

                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);
                    dp.SetValueXY(i2, _item.Duration.TotalHours);
                    //dp.Label = yvalue
                    //dp.LegendText = "0-10"


                    dp.AxisLabel = _item.Text;
                    dp.Label = _item.Date.Day.ToString() + "/" + _item.Date.Month.ToString() + "/" + _item.Date.Year.ToString();


                    //series1.Points[i2].AxisLabel = "Second Point";
                    //dp.YValues(i) = yvalue
                    series1.Points.Add(dp);

                    i2 = i2 + 1;
                }

                //if (analyzer.AllFolders.Count <= 7)
                //{
                //    chartwidth = (analyzer.AllFolders.Count * 30) * 10;
                //}
                //else
                //{
                //    chartwidth = (analyzer.AllFolders.Count * 30);
                //}

                //chartheight = analyzer.AllFolders.Count * 30;

                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = series1.Name;
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                // Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);
                //Chart1.Size = AutoSize
                //pnoffilesfolder.Controls.Add(Chart1);
                Chart1.Location = new Point(offsetX, offsetY);

                //Chart1.ChartAreas["ChartArea1"].Area3DStyle.Rotation = NturnaroundY.Value;
                //Chart1.ChartAreas["ChartArea1"].Area3DStyle.PointDepth = Ngraphdepth.Value;



                //chartcursor = Chart1.ChartAreas("ChartArea1").CursorX
                //chartcursor.IsUserEnabled = True
                //chartcursor.IsUserSelectionEnabled = True
                //chartcursor.LineWidth = 1
                //chartcursor.LineDashStyle = ChartDashStyle.DashDotDot
                //chartcursor.Interval = 0.01
                //chartcursor.LineColor = Color.Red
                //chartcursor.SelectionColor = Color.Yellow

                //' Set cursor selection color of X axis cursor
                //Chart1.ChartAreas("ChartArea1").CursorX.SelectionColor = Color.Yellow

                //Chart1.ChartAreas("ChartArea1").AxisX.ScaleView.Zoomable = True
                //Chart1.ChartAreas("ChartArea1").AxisY.ScaleView.Zoomable = True


                //Chart1.ChartAreas("ChartArea1").CursorX.AutoScroll = True
                //Chart1.ChartAreas("ChartArea1").CursorY.AutoScroll = True


                //Chart1.ChartAreas("ChartArea1").CursorX.IsUserSelectionEnabled = True
                //Chart1.ChartAreas("ChartArea1").CursorY.IsUserSelectionEnabled = True


                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;

                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = _calendaritems.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                // ----------------------------------------------------------------------------------------------scrollbar
                //Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                //Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                //Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;

                Chart1.ChartAreas["ChartArea1"].Position.X = 0;
                Chart1.ChartAreas["ChartArea1"].Position.Y = 0;
                Chart1.ChartAreas["ChartArea1"].Position.Width = 100;
                Chart1.ChartAreas["ChartArea1"].Position.Height = 100;

                //Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Interval = IntervalAutoMode.VariableCount

                //Chart1.Margin = 50;
                //if (Chart1.Titles.Count > 0)
                //{
                //    Chart1.Titles.RemoveAt(0);
                //}
                //Chart1.Titles.Add("chart");
                ////Chart1.Titles(0).Text = 

                //Chart1.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);
                //Chart1.Titles[0].BorderColor = Color.White;

                //Chart1.Titles[0].BackColor = Color.White;


                //Chart1.Titles[0].Alignment = System.Drawing.ContentAlignment.TopCenter;
                //CheckShowGrid.Checked = true;

                //return Chart1;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }




        public Chart showchart_years(List<CalendarItem> _calendaritems, int _width, int _height, Boolean show_hours)
        {

            try
            {
                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = 100;
                ArrayList _Years = new ArrayList();
                ArrayList _Yearscount = new ArrayList();


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;

                int monday = 0;
                int tuesday = 0;
                int wednesday = 0;
                int thursday = 0;
                int friday = 0;
                int saturday = 0;
                int sunday = 0;


                    foreach (CalendarItem _item in _calendaritems)
                    {

                        if (!_Years.Contains(_item.Date.Year.ToString()))
                        {
                            _Years.Add(_item.Date.Year.ToString());
                        }
                    }

                    _Years.Sort();
                    Chartarea1_AxisY_Title = "Agenda items";

                    int yearscount = 0;
                    
                        foreach (string _year in _Years)
                        {
                            yearscount = 0;
                            foreach (CalendarItem _item in _calendaritems)
                            {
                                if (_year == _item.Date.Year.ToString())
                                {
                                    yearscount = yearscount + 1;
                                   
                                }

                            }
                            _Yearscount.Add(yearscount);
                        }

                    
                    Chartarea1_AxisY_Title = "Agenda items";
             


                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisY.Title = Chartarea1_AxisY_Title;

                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Year";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i;

                for (i = 0; i < _Years.Count; i++)
                {
                    string dayofweek = "";
                    int agendaitems = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    dp.SetValueXY(i, _Yearscount[i].ToString());
                    dp.AxisLabel = _Years[i].ToString();
                    dp.Label = _Yearscount[i].ToString();

                    series1.Points.Add(dp);

                }
                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Agenda items";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;



                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;


                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = _Years.Count  + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                 //----------------------------------------------------------------------------------------------scrollbar



                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }



        public Chart income_per_project(DataTable _DT, List<CalendarItem> _calendaritems,  int _width, int _height, DateTime startdate, DateTime enddate, Boolean _hours)
        {

            try
            {


                // First get unique project names in Agenda:
                ArrayList _ColProjects = new ArrayList();

                foreach (CalendarItem item in _calendaritems)
                {
                    string _projects = item._project;
                    _projects = _projects.ToLower();


                    if (!_ColProjects.Contains(_projects))
                    {
                        if (_projects.Length > 0)
                        {
                            _ColProjects.Add(_projects);
                        }
                    }
                }

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize;


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;

             
                Chartarea1_AxisY_Title = "Agenda items";

                int yearscount = 0;

              //  Chart1.Titles.Add("Income per project from " + startdate.ToString() + " to " + enddate.ToString() );
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;
               // Chartarea1.AxisY.Title = "Income";


                if (_hours == true)
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Hours per project from " + startdate.ToString() + " to " + enddate.ToString());
                }
                else
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per project from " + startdate.ToString() + " to " + enddate.ToString());
                }
                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Project";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i;

                for (i = 0; i < _ColProjects.Count; i++)
                {

                    double totalincome = 0;
                    double totalhours = 0;

                    var query = from r in _DT.AsEnumerable()
                                where r.Field<DateTime>("endtime").Date >= startdate &&
                                      r.Field<DateTime>("endtime").Date <= enddate
                                select r;

                    //if (checkProject.Checked == true && checkActivity.Checked == true && checkClient.Checked == true)
                    //{
                    //query =  query.Where(r.Field<string>("project") == cBProjects.Text; 

                    query = query.Where(p => String.Equals(p.Field<string>("project"), _ColProjects[i].ToString(), StringComparison.CurrentCultureIgnoreCase));
                    //query = query.Where(p => p.Field<string>("project") == cBProjects.Text);

                    DataTable newDT = new DataTable();
                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    foreach (var v in query)
                    {
                        newDT = query.CopyToDataTable<DataRow>();
                        break;
                    }

                    foreach (DataRow dtrow in newDT.Rows)
                    {

                        double kil = Convert.ToDouble((string)(dtrow["kil"]));
                        double kilprice = Convert.ToDouble((string)(dtrow["kilprice"]));
                        double totalkil = kil * kilprice;


                        if (_hours == true)
                        {
                            totalincome = totalincome + ((Convert.ToDouble(dtrow["minutes"]) / 60));
                            dp.Label = totalincome.ToString() + " (" + _ColProjects[i].ToString() + ")";
                        }
                        else
                        {
                            totalincome = totalincome + (Convert.ToDouble(dtrow["price"]) * (Convert.ToDouble(dtrow["minutes"]) / 60)) + (Convert.ToDouble((string)(dtrow["variouscosts"])) + totalkil);
                            dp.Label = "€" + totalincome.ToString() + " (" + _ColProjects[i].ToString() + ")";
                        }
                        //totalincome = totalincome + (Convert.ToDouble(dtrow["price"]) * (Convert.ToDouble(dtrow["minutes"]) / 60));
                    }


                    int agendaitems = 0;

                   

                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = _ColProjects[i].ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    dp.LegendText = _ColProjects[i].ToString();

                    series1.Points.Add(dp);

                }
                Chart1.Series.Add(series1);

                if (_ColProjects.Count <= 30)
                {
                    Chart1.Series[series1.Name].ChartType = SeriesChartType.Pie;
                }
                else
                {
                    Chart1.Series[series1.Name].ChartType = SeriesChartType.Bar;
                }

                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Project";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;



                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;


                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = _ColProjects.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar

                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }



        // Show the income per activity during a certain period:
        public Chart income_per_activity(DataTable _DT, List<CalendarItem> _calendaritems, int _width, int _height, DateTime startdate, DateTime enddate, Boolean _hours)
        {

            try
            {


                // First get unique project names in Agenda:
                ArrayList _ColActivity = new ArrayList();

                foreach (CalendarItem item in _calendaritems)
                {
                    string _activity = item._activity;
                    _activity = _activity.ToLower();


                    if (!_ColActivity.Contains(_activity))
                    {
                        if (_activity.Length > 0)
                        {
                            _ColActivity.Add(_activity);
                        }
                    }
                }

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize; 


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;


                Chartarea1_AxisY_Title = "Agenda items";

               

               // Chart1.Titles.Add("Income per activity from " + startdate.ToString() + " to " + enddate.ToString());
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;
                //Chartarea1.AxisY.Title = "Income";

                //------------------------------------------------------------------

                if (_hours == true)
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Hours per activity from " + startdate.ToString() + " to " + enddate.ToString());
                }
                else
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per activity from " + startdate.ToString() + " to " + enddate.ToString());
                }



                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Activity";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i;

                for (i = 0; i < _ColActivity.Count; i++)
                {

                    double totalincome = 0;
                    double totalhours = 0;

                    var query = from r in _DT.AsEnumerable()
                                where r.Field<DateTime>("endtime").Date >= startdate &&
                                      r.Field<DateTime>("endtime").Date <= enddate
                                select r;

                    //if (checkProject.Checked == true && checkActivity.Checked == true && checkClient.Checked == true)
                    //{
                    //query =  query.Where(r.Field<string>("project") == cBProjects.Text; 

                    query = query.Where(p => String.Equals(p.Field<string>("activity"), _ColActivity[i].ToString(), StringComparison.CurrentCultureIgnoreCase));
                    //query = query.Where(p => p.Field<string>("project") == cBProjects.Text);

                    DataTable newDT = new DataTable();

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    foreach (var v in query)
                    {
                        newDT = query.CopyToDataTable<DataRow>();
                        break;
                    }

                    foreach (DataRow dtrow in newDT.Rows)
                    {
                        double kil = Convert.ToDouble((string)(dtrow["kil"]));
                        double kilprice = Convert.ToDouble((string)(dtrow["kilprice"]));
                        double totalkil = kil * kilprice;

                        //CalendarItem item = new CalendarItem(calendar1);
                        if (_hours == true)
                        {
                            totalincome = totalincome + ((Convert.ToDouble(dtrow["minutes"]) / 60));
                            dp.Label = totalincome.ToString() + " (" + _ColActivity[i].ToString() + ")";
                        }
                        else
                        {
                            totalincome = totalincome + (Convert.ToDouble(dtrow["price"]) * (Convert.ToDouble(dtrow["minutes"]) / 60)) + (Convert.ToDouble((string)(dtrow["variouscosts"])) + totalkil);
                            dp.Label = "€" + totalincome.ToString() + " (" + _ColActivity[i].ToString() + ")";
                        }


                       // totalincome = totalincome + (Convert.ToDouble(dtrow["price"]) * (Convert.ToDouble(dtrow["minutes"]) / 60));
                    }

                 

                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = _ColActivity[i].ToString();
                   // dp.Label = "€" + totalincome.ToString();
                    dp.LegendText = _ColActivity[i].ToString();

                    series1.Points.Add(dp);

                }
                Chart1.Series.Add(series1);

                if (_ColActivity.Count <= 30)
                {
                    Chart1.Series[series1.Name].ChartType = SeriesChartType.Pie;
                }
                else
                {
                    Chart1.Series[series1.Name].ChartType = SeriesChartType.Bar;
                }
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Activity";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;



                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;


                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = _ColActivity.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar



                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }





        // Show the income per client during a certain period:
        public Chart income_per_client(DataTable _DT, List<CalendarItem> _calendaritems, int _width, int _height, DateTime startdate, DateTime enddate, Boolean _hours)
        {

            try
            {


                // First get unique project names in Agenda:
                ArrayList _ColClient = new ArrayList();

                foreach (CalendarItem item in _calendaritems)
                {
                    string _client = item._client;
                    _client = _client.ToLower();


                    if (!_ColClient.Contains(_client))
                    {
                        if (_client.Length > 0)
                        {
                            _ColClient.Add(_client);
                        }
                    }
                }

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize;


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;


                Chartarea1_AxisY_Title = "Agenda items";
                //Chart1.Titles.Add("Income per client from " + startdate.ToString() + " to " + enddate.ToString());
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;

                if (_hours == true)
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Hours per client from " + startdate.ToString() + " to " + enddate.ToString());
                }
                else
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per client from " + startdate.ToString() + " to " + enddate.ToString());
                }
               

                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Client";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i;

                for (i = 0; i < _ColClient.Count; i++)
                {

                    double totalincome = 0;
                    double totalhours = 0;

                    var query = from r in _DT.AsEnumerable()
                                where r.Field<DateTime>("endtime").Date >= startdate &&
                                      r.Field<DateTime>("endtime").Date <= enddate
                                select r;

                    //if (checkProject.Checked == true && checkActivity.Checked == true && checkClient.Checked == true)
                    //{
                    //query =  query.Where(r.Field<string>("project") == cBProjects.Text; 

                    query = query.Where(p => String.Equals(p.Field<string>("client"), _ColClient[i].ToString(), StringComparison.CurrentCultureIgnoreCase));
                    //query = query.Where(p => p.Field<string>("project") == cBProjects.Text);

                    DataTable newDT = new DataTable();


                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);


                    foreach (var v in query)
                    {
                        newDT = query.CopyToDataTable<DataRow>();
                        break;
                    }

                    foreach (DataRow dtrow in newDT.Rows)
                    {
                        double kil = Convert.ToDouble((string)(dtrow["kil"]));
                        double kilprice = Convert.ToDouble((string)(dtrow["kilprice"]));
                        double totalkil = kil * kilprice;

                        //CalendarItem item = new CalendarItem(calendar1);
                        if (_hours == true)
                        {
                            totalincome = totalincome + ((Convert.ToDouble(dtrow["minutes"]) / 60));
                            dp.Label = totalincome.ToString() + " (" + _ColClient[i].ToString() + ")";
                        }
                        else
                        {
                            totalincome = totalincome + (Convert.ToDouble(dtrow["price"]) * (Convert.ToDouble(dtrow["minutes"]) / 60)) + (Convert.ToDouble((string)(dtrow["variouscosts"])) + totalkil);
                            dp.Label = "€" + totalincome.ToString() + " (" + _ColClient[i].ToString() + ")";
                        }
                    }

                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = _ColClient[i].ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    dp.LegendText = _ColClient[i].ToString();

                    series1.Points.Add(dp);

                }
                Chart1.Series.Add(series1);

                if (_ColClient.Count <= 30)
                {
                    Chart1.Series[series1.Name].ChartType = SeriesChartType.Pie;
                }
                else
                {
                    Chart1.Series[series1.Name].ChartType = SeriesChartType.Bar;
                }
                //Chart1.Series[series1.Name].ChartType = SeriesChartType.Pie;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Client";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;



                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;



                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = _ColClient.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar



                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }

        // Income per month for all items:
        public Chart month_income(DataTable _DT, List<CalendarItem> _calendaritems, int _width, int _height, DateTime startdate, DateTime enddate, string _type)
        {

            try
            {


                // Get all months between the two dates:

                List<Tuple<int, int>> yearmonth = new List<Tuple<int, int>>();
                yearmonth = year_month_Between(startdate, enddate);
                yearmonth.Reverse();
                //

                // First get unique project names in Agenda:

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize;


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                //int chartwidth = yearmonth.Count * 20;

                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;


                Chartarea1_AxisY_Title = "Agenda items";
                //Chart1.Titles.Add("Income per client from " + startdate.ToString() + " to " + enddate.ToString());
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;

                if (_type == "hours")
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Total hours worked per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString());
                }
                else if (_type == "income")
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString());
                }
                else if (_type == "kilometer")
                {
                    Chartarea1.AxisY.Title = "Kilometers";
                    Chart1.Titles.Add("Kilometers per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString());
                }



                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Month";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = yearmonth.Count + 1;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 0.1;

                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1.ChartAreas["ChartArea1"].InnerPlotPosition.Width = yearmonth.Count * 50;
                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;


                //--------------------------------------------------------------------------------------------------scrollbar

                Chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = true;

                //Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisY.IsLabelAutoFit = false;
                //Chart1.ChartAreas["ChartArea1"].AxisX.aut = false;

                // enable autoscroll
                //Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;


                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Size = 5000;

                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar

                //Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                //Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;

                // Chart1.ChartAreas["ChartArea1"].RecalculateAxesScale();
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleBreakStyle.Enabled = false;
                //Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = yearmonth.Count;
                //Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i = 0;

                foreach (Tuple<int, int> items in yearmonth)
                {
                    int year = items.Item1;
                    int month = items.Item2;

                    double totalincome = 0;
                    double totalhours = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    foreach (CalendarItem item in _calendaritems)
                    {

                        if (item.Date.Year == year && item.Date.Month == month)
                        {

                            double kil = item._Kil;
                            double kilprice = item._Kilprice;
                            double totalkil = kil * kilprice;



                            if (_type == "hours")
                            {
                                totalincome = totalincome + item.Duration.TotalHours;
                                dp.Label = totalincome.ToString();
                            }
                            else if (_type == "income")
                            {
                                totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60)) + Convert.ToDouble(item._variouscosts) + totalkil;
                                dp.Label = "€ " + totalincome.ToString();
                            }
                            else if (_type == "kilometer")
                            {
                                totalincome = totalincome + kil;
                                dp.Label = totalincome.ToString();
                            }



                            // totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60));
                        }
                    }


                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = year.ToString() + "/" + month.ToString();
                    //dp.Label = totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    dp.LabelAngle = -90;
                    // dp.LabelBackColor = Color.Yellow;

                    dp.LegendText = year.ToString() + "/" + month.ToString(); ;
                    //dp.BorderWidth = 10;
                    series1.Points.Add(dp);

                    //i = i + 1;

                    //DataPoint dp2 = new DataPoint();
                    //dp2.SetValueXY(i, 0);
                    //dp2.IsEmpty = true;
                    //series1.Points.Add(dp2);



                    i = i + 1;
                    // do stuff with your Model / Style objects here...

                }

                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;

                // Smart style enabled false (gives a better result!):
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = false;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Income";
                Chart1.Series[series1.Name].Legend = "Second";
                //Chart1.Series[series1.Name].BorderWidth = 2;
                //Chart1.Series[series1.Name]["PixelPointWidth"] = PixelPointWidth;
                //Chart1.Series[series1.Name].CustomProperties = "DrawingStyle = Cylinder ,PixelPointWidth = 50";



                //Chart1.Series[series1.Name]["PointWidth"] = "20";
                Chart1.Series[series1.Name].IsXValueIndexed = true;
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }



        public Chart month_income_project(DataTable _DT, List<CalendarItem> _calendaritems, int _width, int _height, DateTime startdate, DateTime enddate, string _type, string _project)
        {

            try
            {


                // Get all months between the two dates:

                List<Tuple<int, int>> yearmonth = new List<Tuple<int, int>>();
                yearmonth = year_month_Between(startdate, enddate);
                yearmonth.Reverse();
                //

                // First get unique project names in Agenda:

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize;


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;


                Chartarea1_AxisY_Title = "Agenda items";
                //Chart1.Titles.Add("Income per client from " + startdate.ToString() + " to " + enddate.ToString());
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;

                if (_type == "hours")
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Total hours worked per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the Project: " + _project);
                }
                else if (_type == "income")
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the Project: " + _project);
                }
                else if (_type == "kilometer")
                {
                     Chartarea1.AxisY.Title = "Kilometers";
                    Chart1.Titles.Add("Kilometers per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the Project: " + _project);
                }



                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Month";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i = 0;

                foreach (Tuple<int, int> items in yearmonth)
                {
                    int year = items.Item1;
                    int month = items.Item2;

                    double totalincome = 0;
                    double totalhours = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    foreach (CalendarItem item in _calendaritems)
                    {

                        if (item.Date.Year == year && item.Date.Month == month)
                        {
                            if (item._project == _project)
                            {
                                double kil = item._Kil;
                                double kilprice = item._Kilprice;
                                double totalkil = kil * kilprice;

                                if (_type == "hours")
                                {
                                    totalincome = totalincome + item.Duration.TotalHours;
                                    dp.Label = totalincome.ToString();
                                }
                                else if (_type == "income")
                                {
                                    totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60)) + Convert.ToDouble(item._variouscosts) + totalkil;
                                    dp.Label = "€ " + totalincome.ToString();
                                }
                                else if (_type == "kilometer")
                                {
                                    totalincome = totalincome + kil;
                                    dp.Label = totalincome.ToString();
                                }
                               // totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60));
                            }
                        }
                    }


                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = year.ToString() + "/" + month.ToString();
                    //dp.Label = totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    dp.LabelAngle = -90;
                    dp.LegendText = year.ToString() + "/" + month.ToString(); ;

                    series1.Points.Add(dp);
                    i = i + 1;
                    // do stuff with your Model / Style objects here...

                }

                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Income";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;
               // Chart1.Series[series1.Name]["PixelPointWidth"] = PixelPointWidth;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;


                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = yearmonth.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar

                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;





                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }


        public Chart month_income_activity(DataTable _DT, List<CalendarItem> _calendaritems, int _width, int _height, DateTime startdate, DateTime enddate, string _type, string _activity)
        {

            try
            {


                // Get all months between the two dates:

                List<Tuple<int, int>> yearmonth = new List<Tuple<int, int>>();
                yearmonth = year_month_Between(startdate, enddate);
                yearmonth.Reverse();
                //

                // First get unique project names in Agenda:

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize;


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;


                Chartarea1_AxisY_Title = "Agenda items";
                //Chart1.Titles.Add("Income per client from " + startdate.ToString() + " to " + enddate.ToString());
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;

                if (_type == "hours")
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Total hours worked per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the activity: " + _activity);
                }
                else if (_type == "income")
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the activity: " + _activity);
                }
                else if (_type == "kilometer")
                {
                    Chartarea1.AxisY.Title = "Kilometers";
                    Chart1.Titles.Add("Kilometers per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the activity: " + _activity);
                }



                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Month";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i = 0;

                foreach (Tuple<int, int> items in yearmonth)
                {
                    int year = items.Item1;
                    int month = items.Item2;

                    double totalincome = 0;
                    double totalhours = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    foreach (CalendarItem item in _calendaritems)
                    {

                        if (item.Date.Year == year && item.Date.Month == month)
                        {
                            if (item._activity == _activity)
                            {

                                double kil = item._Kil;
                                double kilprice = item._Kilprice;
                                double totalkil = kil * kilprice;

                                if (_type == "hours")
                                {
                                    totalincome = totalincome + item.Duration.TotalHours;
                                    dp.Label = totalincome.ToString();
                                }
                                else if (_type == "income")
                                {
                                    totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60)) + Convert.ToDouble(item._variouscosts) + totalkil;
                                    dp.Label = "€ " + totalincome.ToString();
                                }
                                else if (_type == "kilometer")
                                {
                                    totalincome = totalincome + kil;
                                    dp.Label = totalincome.ToString();
                                }

                               // totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60));
                            }
                        }
                    }


                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = year.ToString() + "/" + month.ToString();
                    //dp.Label = totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    dp.LabelAngle = -90;
                    dp.LegendText = year.ToString() + "/" + month.ToString(); ;

                    series1.Points.Add(dp);
                    i = i + 1;
                    // do stuff with your Model / Style objects here...

                }

                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Income";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;
               // Chart1.Series[series1.Name]["PixelPointWidth"] = PixelPointWidth;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;


                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = yearmonth.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar

                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;





                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }


        public Chart month_income_client(DataTable _DT, List<CalendarItem> _calendaritems, int _width, int _height, DateTime startdate, DateTime enddate, string _type, string _client)
        {

            try
            {


                // Get all months between the two dates:

                List<Tuple<int, int>> yearmonth = new List<Tuple<int, int>>();
                yearmonth = year_month_Between(startdate, enddate);
                yearmonth.Reverse();
              
                //

                // First get unique project names in Agenda:

                string Chartarea1_AxisY_Title = "";
                Chart1 = new Chart();
                int blockSize = _blockSize;


                ChartArea Chartarea1 = new ChartArea();

                DataPoint dp = default(DataPoint);
                Font chartfont = new Font("Arial Bold", 12, FontStyle.Bold);

                Chart1.Name = "Chart1";
                int chartwidth = _width - 100;
                int chartheight = _height - 100;
                int offsetX = 10;
                int offsetY = 10;


                Chartarea1_AxisY_Title = "Agenda items";
                //Chart1.Titles.Add("Income per client from " + startdate.ToString() + " to " + enddate.ToString());
                Chartarea1.AxisY.TitleFont = chartfont;
                Chartarea1.AxisY.TitleAlignment = StringAlignment.Center;

                if (_type == "hours")
                {
                    Chartarea1.AxisY.Title = "Hours";
                    Chart1.Titles.Add("Total hours worked per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the client: " + _client);
                }
                else if (_type == "income")
                {
                    Chartarea1.AxisY.Title = "Income";
                    Chart1.Titles.Add("Income per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the client: " + _client);
                }
                else if (_type == "kilometer")
                {
                    Chartarea1.AxisY.Title = "Kilometers";
                    Chart1.Titles.Add("Kilometers per month from " + startdate.Year.ToString() + "/" + startdate.Month.ToString() + " to " + enddate.Year.ToString() + "/" + enddate.Month.ToString() + " for the client: " + _client);
                }



                //------------------------------------------------------------------


                Chartarea1.AxisY.MinorGrid.Enabled = true;
                //chartArea1.AxisY.MajorGrid.Enabled = Tru
                Chartarea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                Chartarea1.AxisX.TitleFont = chartfont;
                Chartarea1.AxisX.TitleAlignment = StringAlignment.Center;
                Chartarea1.AxisX.Title = "Month";
                Chartarea1.AxisX.TextOrientation = TextOrientation.Horizontal;
                Chartarea1.AxisX.MajorGrid.Enabled = true;
                //Chartarea1.AxisX.IntervalType = IntervalAutoMode.FixedCount

                //Chartarea1.AxisX.IsLabelAutoFit = True
                //Chartarea1.AxisX.IntervalOffsetType = IntervalAutoMode.VariableCount

                //Chartarea1.AxisX.LabelStyle.IntervalType = IntervalAutoMode.VariableCount

                Chartarea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;


                Chart1.ChartAreas.Add(Chartarea1);
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart1.Legends.Add(new Legend("Second"));

                Series series1 = new Series();
                series1.Name = "Agenda_items";


                int i = 0;

                foreach (Tuple<int, int> items in yearmonth)
                {
                    int year = items.Item1;
                    int month = items.Item2;

                    double totalincome = 0;
                    double totalhours = 0;

                    dp = new DataPoint();
                    dp.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);

                    foreach (CalendarItem item in _calendaritems)
                    {

                        if (item.Date.Year == year && item.Date.Month == month)
                        {
                            if (item._client == _client)
                            {

                                double kil = item._Kil;
                                double kilprice = item._Kilprice;
                                double totalkil = kil * kilprice;

                                if (_type == "hours")
                                {
                                    totalincome = totalincome + item.Duration.TotalHours;
                                    dp.Label = totalincome.ToString();
                                }
                                else if (_type == "income")
                                {
                                    totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60)) + Convert.ToDouble(item._variouscosts) + totalkil;
                                    dp.Label = "€ " + totalincome.ToString();
                                }
                                else if (_type == "kilometer")
                                {
                                    totalincome = totalincome + kil;
                                    dp.Label = totalincome.ToString();
                                }

                                //totalincome = totalincome + (item._price * (Convert.ToDouble(item.Duration.TotalMinutes) / 60));
                            }
                        }
                    }


                    dp.SetValueXY(i, totalincome);
                    dp.AxisLabel = year.ToString() + "/" + month.ToString();
                    //dp.Label = totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    //dp.Label = "€" + totalincome.ToString();
                    dp.LabelAngle = -90;
                    dp.LegendText = year.ToString() + "/" + month.ToString(); ;
                 

                    series1.Points.Add(dp);
                    i = i + 1;
                    // do stuff with your Model / Style objects here...

                }

                Chart1.Series.Add(series1);
                Chart1.Series[series1.Name].ChartType = SeriesChartType.Column;
                Chart1.Series[series1.Name].SmartLabelStyle.Enabled = true;
                Chart1.Series[series1.Name].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                Chart1.Series[series1.Name]["PieLabelStyle"] = "Outside";
                Chart1.Series[series1.Name]["PieLineColor"] = "Black";

                //Chart1.Series[series1.Name].SmartLabelStyle = LabelCalloutStyle.

                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineColor = Color.Red;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutLineWidth = 2;
                Chart1.Series[series1.Name].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box;
                Chart1.Series[series1.Name].LegendText = "Income";
                Chart1.Series[series1.Name].Legend = "Second";
                Chart1.Series[series1.Name].BorderWidth = 2;
                //Chart1.Series[series1.Name]["PixelPointWidth"] = PixelPointWidth;

                Chart1.Series[series1.Name].IsXValueIndexed = true;



                Chart1.Size = new System.Drawing.Size(chartwidth, chartheight);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                //Chart1 = _MyGraph.plot_graphs(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext.ToString, _ymax, _ymin);
                //    //Bcalculate.Enabled = true;

                //Chart1 = _MyGraph.Plot_derivative_graph(_selectedgraphs, _xmin, _xmax, Xaxistext, Yaxistext, _ymax, _ymin);

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = false;


                //--------------------------------------------------------------------------------------------------scrollbar
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = yearmonth.Count + 1;

                // enable autoscroll
                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;

                // let's zoom to [0,blockSize] (e.g. [0,100])
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                int position = 0;
                int size = blockSize;
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(position, size);

                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;


                // disable zoom-reset button (only scrollbar's arrows are available)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                // set scrollbar small change to blockSize (e.g. 100)
                Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = blockSize;
                //----------------------------------------------------------------------------------------------scrollbar

                Chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;


                Chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;

                Chart1.ChartAreas["ChartArea1"].CursorX.Interval = 0.01;
                Chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                Chart1.ChartAreas["ChartArea1"].CursorY.Interval = 0.01;

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                Chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;

                Chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = False
                Chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
                //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarksNextToAxis = false;

                //Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;





                //Chart1.Size = AutoSize
                Chart1.Location = new Point(offsetX, offsetY);
                Chart1.Dock = DockStyle.Fill;
            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }

            return Chart1;
        }


        // Get each month in date range:
        private List<Tuple<int, int>> year_month_Between(DateTime d0, DateTime d1)
        {
            List<DateTime> datemonth = Enumerable.Range(0, (d1.Year - d0.Year) * 12 + (d1.Month - d0.Month + 1))
                             .Select(m => new DateTime(d0.Year, d0.Month, 1).AddMonths(m)).ToList();
            List<Tuple<int, int>> yearmonth = new List<Tuple<int, int>>();

            foreach (DateTime x in datemonth)
            {
                yearmonth.Add(new Tuple<int, int>(x.Year, x.Month));
            }
            return yearmonth;
        }

    }
}
