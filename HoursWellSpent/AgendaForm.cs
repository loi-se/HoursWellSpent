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
using System.Drawing.Imaging;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

using System.Web;



using System.Web;

namespace HoursWellSpent
{
    public partial class AgendaForm : Form
    {
        // All the calendaritems in the calendar:
        public List<CalendarItem> _items = new List<CalendarItem>();
        CalendarItem contextItem = null;

        // Data table used to store the calendar items for later XML processing:
        private DataTable dt;

        public List<DateTime> _BoldedDates = new List<DateTime>();
        //public DateTime[] _BoldedDates = new DateTime[];
        public Dictionary<string, string> Helptext = new Dictionary<string,string>();
 


        // Data table used to store the calendar items for Linq queries used in search function / declaration function / graphs:
        public DataTable dt_Linq_Query;

        private string startuppath = "";
        //private int DayOffset = 6;

        private int DayOffset = 6;
        private int DayOffsetMax = 0;
        private DateTime pickedDateTime;
        private string OpenedAgenda = "" ;
        private string OpenedAgendaPath = "";
        private Chart Chart1;
        private Chart_controller _ChartController;
        private static int[] moveByDays = { 6, 7, 8, 9, 10, 4, 5 };

        public Boolean _Copy = false;

        public APEngine APEngine1;
        private Random random;

        double totalhours = 0;
        double foundhours = 0;
        // public DoubleBufferedPanel doublebufferedpanel_1 = new DoubleBufferedPanel();
        public Graphics g;
        public ArrayList paintQueue;


        // FORM EVENTS / CONSTRUCTOR-------------------------------------------------------------------------------------------------------------------------------------

        public AgendaForm()
        {
            InitializeComponent();

        }

        // Form load event:
     
        private void Form1_Load(object sender, EventArgs e)
        {
            // Make a new chart controller with which you can create charts:
            _ChartController = new Chart_controller();
            DayOffset = Convert.ToInt32(nUViewRange.Value);
            //TimeSpan ts = monthCalendar2.ViewEnd.Date - monthCalendar2.ViewStart.Date;
            //// Difference in days.
            //int differenceInDays = ts.Days;

            //DayOffsetMax = differenceInDays;
            //nUViewRange.Maximum = DayOffsetMax;

            DateTime _currendate = DateTime.Now;

            pContextInfo.Visible = false;
            tSearchText.Focus();
            CreateDataTable();
           
            calendar1.TimeScale = CalendarTimeScale.SixtyMinutes;
            cRelationType.SelectedIndex = 0;
            startuppath = Application.UserAppDataPath;

            calendar1.SetViewRange(DateTime.Now, DateTime.Now.AddDays(DayOffset));

            //calendar1.SelectionStart = DateTime.Now.AddDays(7);

            DeleteItems();
  
            cAgendaItems.SelectedIndex = 0;
            Ccharttype.SelectedIndex = 0;
            cStatType.SelectedIndex = 0;
            cBRelText.SelectedIndex = 0;
            cBTimeScale.SelectedIndex = 0;
            //lSelWeekNumber = calendar1.GetWeekof

            lSelWeekNumber.Text = WeekOfYear(monthCalendar2.SelectionStart).ToString();
            lSelYear.Text = monthCalendar2.SelectionStart.Year.ToString();
            lSelMonth.Text = monthCalendar2.SelectionStart.Month.ToString();
            lSelDay.Text = monthCalendar2.SelectionStart.Day.ToString();
            

            APEngine.bmp = new Bitmap(pPhysicsCloud.Width, pPhysicsCloud.Height);
            APEngine.setCollisionResponseMode(APEngine.STANDARD);
            g = pPhysicsCloud.CreateGraphics();
            //SetDoubleBuffered(pGraphics);      
            APEngine.setDefaultContainer(g);
            paintQueue = new ArrayList();
            random = new Random();
            monthCalendar2.Focus();

            // Populate the active calendar item object because othwerwise you get a bug when the user cancels the first Calendaritem.
            //-------------
            Global_variables._GActiveCalendarItem = new CalendarItem(calendar1);
            Global_variables._GActiveCalendarItem._price = 0;
            Global_variables._GActiveCalendarItem._variouscosts = 0;
            Global_variables._GActiveCalendarItem._Kil = 0;
            Global_variables._GActiveCalendarItem._Kilprice = 0;

            Global_variables._GActiveCalendarItem.Text = "-";

            Global_variables._GActiveCalendarItem._project = "";
            Global_variables._GActiveCalendarItem._activity = "";
            Global_variables._GActiveCalendarItem._client = "";
            Global_variables._GActiveCalendarItem._note = "";

            Global_variables._GActiveCalendarItem.BackgroundColor = Color.White;
            //-----------------------


            //Split container distances:

            splitContainer1.SplitterDistance = Convert.ToInt32(0.26 * Screen.PrimaryScreen.Bounds.Width);
            splitContainerDecl.SplitterDistance = Convert.ToInt32(0.75 * pDeclaration_Controls.Width);
            splitContainerStats.SplitterDistance = Convert.ToInt32(0.75 * pGraphControl.Width);
            splitContainerRel.SplitterDistance = Convert.ToInt32(0.75 * pPhysicsControls.Width);

            monthCalendar2.SelectionRange = new SelectionRange(_currendate, _currendate.AddDays(DayOffset));

            //monthCalendar2.BoldedDates.Add(
            //monthCalendar1.Width = pMonthCalendar.Width - 100;

            //monthCalendar1.Height = pMonthCalendar.Height - 100 ;
            //// pBoggleHolder.Dock = DockStyle.Fill;
            ////pBoggleHolder.BorderStyle = BorderStyle.FixedSingle;

            //monthCalendar1.Location = new Point(
            //   pMonthCalendar.ClientSize.Width / 2 - pMonthCalendar.Size.Width / 2,
            //    pMonthCalendar.ClientSize.Height / 2 - pMonthCalendar.Size.Height / 2);
            //monthCalendar1.Anchor = AnchorStyles.None;
            //pBoggleHolder.BorderStyle = BorderStyle.Fixed3D;
            createHelpTexts();


            test();
        }

        // Create the various helptextst in the program:
        private void createHelpTexts()
        {
            Helptext.Add("agenda-view", "Agenda view." + "\r\n\r\n" + "Hold the left mouse-button and drag your mouse to select a time range." +
                     "Then press ENTER or INSERT to make a new Agenda item in that time range." + "\r\n\r\n" + " You can also double-click on the Agenda to create a new Agenda item." +
                     " To edit existing agenda items please double-click on an agenda item. Then the agenda item edit menu opens." 
                     + "\r\n\r\n" + "Press the right mouse-button to show the Contextmenustrip. " + "With the timescale option in the Contextmenustrip you can change" +
                     " the timescale of the calendar and register your time more precisely." + "\r\n\r\n" +
                     "Use the mouse scroll-button to scroll trough the agenda when using a small timescale");


            Helptext.Add("agenda-item-selected", "Double-click on agenda items to edit them." + "\r\n\r\n" +
                         "You can change the time range of the selected agenda item by dragging the boundary of the agenda item." + "\r\n" +
                          "You can also drag a selected agenda item to a new place in the agenda by holding the left mouse-button on the agenda item and dragging the mouse to the desired location");

            Helptext.Add("month-calendar", "Month calendar" + "\r\n\r\n" + "Clicking on a date will show a new view range in the agenda view." +
                          " The colored dates are currently in the view range of the agenda. The standard view range exists of 7 days (one full week)" +
                          " from Monday to Sunday." + " It is recommended to use a calendar view-range of 7 days or less because otherwise the " +
                             " time-range can't be set by dragging the border of the agenda item.");

            Helptext.Add("agenda-item-gridview", "This is an overview of all the agenda items in your agenda." +
                " You can click on a column header to sort the corresponding column. And you can click on" +
                " a row header to go to the selected agenda item in the Agenda view.");

            Helptext.Add("declaration", "Create hour declaration reports for a certain date range. If you want to create a hour declaration report " +
                         "for the month: Juli then you have to select a start-date of 'Juli 1' and an end-date of 'Juli 31'.");

            Helptext.Add("statistics", "Show statistics about your agenda for a certain date range. " +
                "If you for example want to view statistics for the whole month juli then you have to select a start date of 'Juli 1' and an end-date of 'Juli 31'.");


            Helptext.Add("statistics-rel", "With this function you can create a statistical view of how the agenda items relate to each other. " +
                    "You first have to select a certain date-range. If you for example want to view statistics for the whole month Juli then you have to select a start-date " +
                    "of 'Juli 1' and an end-date of 'Juli 31'.");

            Helptext.Add("agenda-item-creating", "Agenda item creation screen." + "\r\n\r\n" +
                         "In this screen you can create a new agenda item or edit an agenda item." +
                          "Only the title field is obligatory");

            Helptext.Add("view-range", "With this option you can change the view-range of the calendar." + " A view-range of 7 means that there are" +
                         " seven days visible in the calendar. It is recommended to use a calendar view-range of 7 days or less because otherwise the " +
                             " time-range can't be set by dragging the border of the agenda item.");

            Helptext.Add("delete", "Delete the selected agenda item.");

            Helptext.Add("paste", "Paste the selected agenda item in the agenda in the selected time range.");

            Helptext.Add("copy", "Copy the selected agenda item.");

            Helptext.Add("timescale", "With the timescale option you can change" +
                        " the timescale of the calendar and register your time more precisely." + "\r\n\r\n" +
                        " Use the mouse scroll-button to scroll trough the agenda when using a small timescale.");

            Helptext.Add("help", "To view help information please press the 'show' context information button in the lower left corner." +
                " This way you can see context information about functionalities you are using that will further assist you.");

            Helptext.Add("about", "HoursWellSpent is an hour registration program for self-employed people / freelancers who want to keep track of their" +
                        " working hours. The application can also be used as an Agenda." + "\r\n\r\n" +
                        " Version: 1.0" + "\r\n\r\n" +
                        "Copyright© 2015");
        }


        // Form closing event:
        private void AgendaForm_FormClosing(object sender, FormClosingEventArgs e)
        {


            DialogResult dialogResult = MessageBox.Show("Save your agenda before closing?", "Agenda: " + OpenedAgenda, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SaveAgenda();
            }
            else if (dialogResult == DialogResult.No)
            {

            }

        }

        // Test code for the agenda:
        private void test()
        {
            Random r = new Random();

            int i = 0;
            int size = 5000;
            int z = 0;
            for (i = 1; i <= size; i++)
            {
                CalendarItem _calendaritem = new CalendarItem(calendar1);
                if (i == 0.1 * size)
                {
                    z = 1;
                }
                else if (i == 0.2 * size)
                {
                    z = 2;
                }
                else if (i == 0.3 * size)
                {
                    z = 3;
                }
                else if (i == 0.4 * size)
                {
                    z = 4;
                }
                else if (i == 0.5 * size)
                {
                    z = 5;
                }
                else if (i == 0.6 * size)
                {
                    z = 6;
                }
                else if (i == 0.7 * size)
                {
                    z = 7;
                }
                else if (i == 0.8 * size)
                {
                    z = 8;
                }
                else if (i == 0.9 * size)
                {
                    z = 9;
                }
                //int i1;
                //for (i1 = 0; i1 < 25; i1++)
                //{

                //}

                _calendaritem._activity = "activity " + z.ToString();
                _calendaritem._project = "project " + z.ToString();
                _calendaritem._client = "client " + z.ToString();
                _calendaritem.Text = "Title " + z.ToString();
                _calendaritem._price = r.Next(0, 200);
                _calendaritem._variouscosts = r.Next(0, 1000);
                _calendaritem._Kilprice = 0.6;
                _calendaritem._Kil = r.Next(0, 5000);
                _calendaritem.BackgroundColor = Color.Yellow;
                //_calendaritem.

                _calendaritem._note = "note " + z.ToString();
                
                DateTime _newdate = new DateTime(r.Next(2000, 2015), r.Next(1, 12), r.Next(1, 28), r.Next(0, 23), r.Next(0, 59), 0);
                //DateTime _newdate = new DateTime(r.Next(2000, 2015), r.Next(1, 12), r.Next(1, 28), r.Next(0, 24), r.Next(0, 60), r.Next(0, 60));

                _calendaritem.StartDate = _newdate;
                _calendaritem.EndDate = _newdate.AddHours(r.Next(1,12));

                _items.Add(_calendaritem);
                PlaceItems();
            }

        }

        // Datatable used for saving to XML:
        private void CreateDataTable()
        {
            dt = new DataTable();
            dt.TableName = "agenda";
            dt.Clear();
            dt.Columns.Add("title");

            dt.Columns.Add("starttime");
            //dt.Columns.Add("daystart");

            dt.Columns.Add("endtime");
            //dt.Columns.Add("dayend");
            dt.Columns.Add("color");
            dt.Columns.Add("price");
            dt.Columns.Add("kil");
            dt.Columns.Add("kilprice");
            dt.Columns.Add("variouscosts");
            dt.Columns.Add("client");

            dt.Columns.Add("project");
            dt.Columns.Add("activity");
            dt.Columns.Add("note");
        }

        // Datatable used for linq queries in the declaration / and relative statistics tab.
        private void Create_DataTable_Linq_Query()
        {
            dt_Linq_Query = new DataTable();
            dt_Linq_Query.TableName = "linq_query";
            dt_Linq_Query.Clear();
            dt_Linq_Query.Columns.Add("title");

            dt_Linq_Query.Columns.Add("starttime", typeof (DateTime));
            //dt.Columns.Add("daystart");

            dt_Linq_Query.Columns.Add("endtime", typeof (DateTime));
            //dtColumn.DataType = System.Type.GetType("System.DateTime");

            //dt.Columns.Add("dayend");
            dt_Linq_Query.Columns.Add("color");
            dt_Linq_Query.Columns.Add("price");
            dt_Linq_Query.Columns.Add("kil");
            dt_Linq_Query.Columns.Add("kilprice");
            dt_Linq_Query.Columns.Add("variouscosts");
            dt_Linq_Query.Columns.Add("minutes");
            dt_Linq_Query.Columns.Add("client");

            dt_Linq_Query.Columns.Add("project");
            dt_Linq_Query.Columns.Add("activity");
            dt_Linq_Query.Columns.Add("note");


            dt_Linq_Query.Rows.Clear();

            foreach (CalendarItem item in _items)
            {
                DataRow dtrow = dt_Linq_Query.NewRow();
                if (item.Text.Length > 0)
                {
                    dtrow["title"] = item.Text;
                    dtrow["starttime"] = item.StartDate;
                    //dtrow["daystart"] = item.DayStart;
                    dtrow["endtime"] = item.EndDate;
                    //dtrow["dayend"] = item.DayEnd;
                    //dtrow["color"] = item.BackgroundColor.Name.ToString();

                    dtrow["color"] = item.BackgroundColor.ToArgb().ToString();
                    // dtrow["color"] = HexConverter(item.BackgroundColor);

                    dtrow["client"] = item._client;
                    dtrow["price"] = item._price;
                    dtrow["kil"] = item._Kil;
                    dtrow["kilprice"] = item._Kilprice;

                    dtrow["variouscosts"] = item._variouscosts;
                    dtrow["minutes"] = item.Duration.TotalMinutes;

                    dtrow["project"] = item._project;
                    dtrow["activity"] = item._activity;
                    dtrow["note"] = item._note;

                    dt_Linq_Query.Rows.Add(dtrow);

                }

            }

            dt_Linq_Query = dt_Linq_Query.DefaultView.ToTable(true, "title", "starttime", "endtime", "color", "client", "price", "kil", "kilprice", "variouscosts", "minutes", "project", "activity", "note");


        }



        // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // MENU STRIP ------------------------------------------------------------------------------------------------------------------------------------------

        // Save the agenda:

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test();
        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Helptext["help"], "Help" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Helptext["about"], "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void SaveAgenda()
        {
            try
            {

                if (OpenedAgendaPath.Length > 0)
                {
                    dt.Rows.Clear();

                    //foreach (CalendarItem item in calendar1.Items)
                    //{
                     foreach (CalendarItem item in _items)
                    {
                        DataRow dtrow = dt.NewRow();
                        if (item.Text.Length > 0)
                        {
                            dtrow["title"] = item.Text;
                            dtrow["starttime"] = item.StartDate;
                            //dtrow["daystart"] = item.DayStart;
                            dtrow["endtime"] = item.EndDate;
                            //dtrow["dayend"] = item.DayEnd;
                            //dtrow["color"] = item.BackgroundColor.Name.ToString();
                            dtrow["color"] = item.BackgroundColor.ToArgb().ToString();
                            
                            dtrow["client"] = item._client;
                            dtrow["price"] = item._price;
                            dtrow["kil"] = item._Kil;
                            dtrow["kilprice"] = item._Kilprice;

                            dtrow["variouscosts"] = item._variouscosts;

                            dtrow["project"] = item._project;
                            dtrow["activity"] = item._activity;
                            dtrow["note"] = item._note;

                            dt.Rows.Add(dtrow);

                        }

                    }

                    dt = dt.DefaultView.ToTable(true, "title", "starttime", "endtime", "color", "client", "price", "kil", "kilprice", "variouscosts", "project", "activity", "note");


                    dt.WriteXml(OpenedAgendaPath.Replace(".xml", "") + ".xml", XmlWriteMode.IgnoreSchema);
                }
                else
                {
                    //DialogResult _Message = MessageBox.Show("You have no agenda opened",
                    // "Important",
                    // MessageBoxButtons.OK,
                    // MessageBoxIcon.Warning,
                    // MessageBoxDefaultButton.Button1);
                    SaveAgendaAs();

                }

            }
            catch (Exception e2)
            {
                MessageBox.Show("An error occurred: '{0}':  " + e2);
            }


        }

        // Save the agenda AS
        private void SaveAgendaAs()
        {
            //int i = 0;
            int int_starttime = 0;
            int int_endtime = 0;
            DataTable distinctValues;

            distinctValues = new DataTable();
            distinctValues.TableName = "agenda";
            //distinctValues.Clear();
            //distinctValues.Columns.Add("title");
            //distinctValues.Columns.Add("starttime");
            //distinctValues.Columns.Add("endtime");
            //distinctValues.Columns.Add("color");


            string myfile = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = startuppath;
            saveFileDialog1.Filter = "xml files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (_items.Count > 0)
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {


                        dt.Rows.Clear();

                        //foreach (CalendarItem item in calendar1.Items)
                        //{
                        foreach (CalendarItem item in _items)
                        {
                            DataRow dtrow = dt.NewRow();
                            if (item.Text.Length > 0)
                            {
                                dtrow["title"] = item.Text;
                                dtrow["starttime"] = item.StartDate;
                                //dtrow["daystart"] = item.DayStart;
                                dtrow["endtime"] = item.EndDate;
                                //dtrow["dayend"] = item.DayEnd;
                                //dtrow["color"] = item.BackgroundColor.Name.ToString();

                                dtrow["color"] = item.BackgroundColor.ToArgb().ToString();
                                // dtrow["color"] = HexConverter(item.BackgroundColor);

                                dtrow["client"] = item._client;
                                dtrow["price"] = item._price;
                                dtrow["kil"] = item._Kil;
                                dtrow["kilprice"] = item._Kilprice;
                                dtrow["variouscosts"] = item._variouscosts;

                                dtrow["project"] = item._project;
                                dtrow["activity"] = item._activity;
                                dtrow["note"] = item._note;

                                dt.Rows.Add(dtrow);

                            }

                        }

                        dt = dt.DefaultView.ToTable(true, "title", "starttime", "endtime", "color", "client", "price", "kil", "kilprice", "variouscosts", "project", "activity", "note");

                        if (File.Exists(saveFileDialog1.FileName.ToString()))
                        {
                            File.Delete(saveFileDialog1.FileName.ToString());
                        }


                        dt.WriteXml(saveFileDialog1.FileName.ToString().Replace(".xml", "") + ".xml", XmlWriteMode.IgnoreSchema);

                        OpenedAgenda = Path.GetFileName(saveFileDialog1.FileName);
                        OpenedAgendaPath = saveFileDialog1.FileName;
                        tOpenedAgenda.Text = OpenedAgenda;
                    }

                    catch (Exception e2)
                    {
                        MessageBox.Show("An error occurred: '{0}':  " + e2);
                    }
                }
            }
            else
            {
                DialogResult _Message = MessageBox.Show("You have no agenda items placed yet.",
                "Important",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);
            }
        }


        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }


        // Save:
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveAgenda();
        }

        // Save Agenda As:
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAgendaAs();
        }

        // Export the current agenda view to JPEG:
        private void calendarAsJpegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string myfile = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = startuppath;
            saveFileDialog1.Filter = "xml files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;


            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {

                    Bitmap bmp = new Bitmap(pAgendaForm.Width, pAgendaForm.Height);
                    pAgendaForm.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(saveFileDialog1.FileName.ToString() + ".Jpeg", ImageFormat.Jpeg);

                }
                catch (Exception e2)
                {
                    MessageBox.Show("An error occurred: '{0}':  " + e2);
                }

            }
        }

        // Open an Agenda:
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //dt = new DataTable();
            //dt.TableName = "agenda";
            //dt.Clear();
            //dt.Columns.Add("title");
            //dt.Columns.Add("starttime");
            //dt.Columns.Add("endtime");
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = startuppath;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //DeleteItems();
                    _items.Clear();
                    //calendar1.
                    // calendar1.Re
                    calendar1.Items.Clear();
                    dt.Clear();
                    _BoldedDates.Clear();
                    //_BoldedDates = new List<DateTime>();
                    calendar1.Refresh();
                    totalhours = 0;
                    clearComboboxes();

                    //richTextBoxEx1.Text = "";
                    string XMLFilepath = openFileDialog1.FileName;

                    string InputFile = XMLFilepath;
                    //string T = ExtractAllTextFromPdf(InputFile);
                    //Count the words
                    //Globals.totalnumberofwords = GetWordCountFromString(T);

                    //MessageBox.Show(numberofwords.ToString());
                    //webBrowser1.Url = new Uri(InputFile);
                    //lPDFfiletext.Text = Path.GetFileName((PDFFilepath.ToString()));

                    //dt.ReadXml(@startuppath + "test.xml");
                    dt.ReadXml(InputFile);

                    //int i = 0;
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        CalendarItem item = new CalendarItem(calendar1);

                        string price;

                        item.Text = (string)dtrow["title"];
                        item.StartDate = Convert.ToDateTime(dtrow["starttime"]);
                        item.EndDate = Convert.ToDateTime(dtrow["endtime"]);

                       // monthCalendar2.BoldedDates.Add(item.StartDate);
                       

                        if (!_BoldedDates.Contains(item.StartDate.Date))
                        {
                            _BoldedDates.Add(item.StartDate.Date);
                        }
                        //_BoldedDates[i] = item.StartDate;


                        //price = (string)dtrow["price"];
                        item._price = Convert.ToDouble(dtrow["price"]);
                        item._Kil = Convert.ToDouble(dtrow["kil"]);
                        item._Kilprice = Convert.ToDouble(dtrow["kilprice"]);

                        item._variouscosts = Convert.ToDouble(dtrow["variouscosts"]);
                        item._client = (string)dtrow["client"];

                        item._project = (string)dtrow["project"];
                        item._activity = (string)dtrow["activity"];
                        item._note = (string)dtrow["note"];


                        //item.DayEnd = item.EndDate.Day;


                        //item.

                        totalhours = totalhours + ((item.Duration.TotalMinutes) / 60);

                        //item.DayStart = (CalendarDay)(dtrow["endday"]);

                       //item.BackgroundColor = Color.FromName((string)dtrow["color"]);
                        int _ARGB = Convert.ToInt32((string)dtrow["color"]);

                       item.BackgroundColor = Color.FromArgb(_ARGB);
                        _items.Add(item);

                        //i = i +1;
                    }
                    
                   //monthCalendar2.BoldedDates = _BoldedDates;

                    monthCalendar2.Refresh();
                    //monthCalendar2.UpdateMonths();

                    OpenedAgenda = Path.GetFileName(openFileDialog1.FileName);
                    OpenedAgendaPath = openFileDialog1.FileName;
                    tOpenedAgenda.Text = OpenedAgenda;
                    PlaceItems();

                    lTotalHours.Text = totalhours.ToString();
                    lTotalAgendaItems.Text = _items.Count.ToString();
                    //calendar1.Refresh();
                    showAgendaItems();


                }
                catch (Exception e2)
                {
                    MessageBox.Show("An error occurred: '{0}':  " + e2);
                }
            }
        }


        // Create a new agenda (everything from the current agenda needs to be cleared).
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _items.Clear();
            //calendar1.
            // calendar1.Re
            calendar1.Items.Clear();
            dt.Clear();
            _BoldedDates = new List<DateTime>();
           // _BoldedDates.Clear();
            clearComboboxes();
            totalhours = 0;

            calendar1.Refresh();
            OpenedAgendaPath = "";
            OpenedAgenda = "";
            tOpenedAgenda.Text = "";
        }


        // Exit the application:
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        // Clear comboboxes when a  new agenda is opened or created:
        private void clearComboboxes()
        {
            // Clear comboboxes on declaration page:
            cBProjects.Items.Clear();
            cBClients.Items.Clear();
            cBActivity.Items.Clear();

            // Clear comboboxes on statistics page:
            cStatActivity.Items.Clear();
            cStatClient.Items.Clear();
            cStatProject.Items.Clear();

            // Clear comboxes on relational statistics page:

            cBRelProjects.Items.Clear();
            cBRelActivity.Items.Clear();
            cBRelClient.Items.Clear();

        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------------


        // CALENDAR --------------------------------------------------------------------------------------------------------------------------------------------


        // Place items when load event is triggered:
        private void calendar1_LoadItems(object sender, CalendarLoadEventArgs e)
        {
            PlaceItems();
        }
        
        // Event that is triggered when agenda item is created:
        private void calendar1_ItemCreated(object sender, CalendarItemCancelEventArgs e)
        {
            _items.Add(e.Item);
            lTotalAgendaItems.Text = _items.Count.ToString();

            //showAgendaItems();
        }

        // 
        private void calendar1_ItemMouseHover(object sender, CalendarItemEventArgs e)
        {
            //Text = e.Item.Text;
        }

        private void calendar1_ItemClick(object sender, CalendarItemEventArgs e)
        {
            //MessageBox.Show(e.Item.Text);
            tDuration.Text = e.Item.Duration.Hours.ToString();
            tDurationMin.Text = e.Item.Duration.TotalMinutes.ToString();
            tStartDate.Text = e.Item.StartDate.ToString();
            tEndDate.Text = e.Item.EndDate.ToString();
            Global_variables._GActiveCalendarItem = e.Item;

            tContextInfo.Text = "";
            tContextInfo.Text = Helptext["agenda-item-selected"];

        }

        private void calendar1_ItemDoubleClick(object sender, CalendarItemEventArgs e)
        {
            //if (e.Item.Selected == true)
            //{
            //MessageBox.Show("hi");
            IEnumerable<CalendarItem> selecteditems = calendar1.GetSelectedItems();
            List<CalendarItem> _selecteditems = selecteditems.ToList();

            tContextInfo.Text = "";
            tContextInfo.Text = Helptext["agenda-item-creating"];

            if (_selecteditems.Count > 0 )
            {
                Global_variables._GActiveCalendarItem = e.Item;
                Global_variables._Gitems = new List<CalendarItem>();
                Global_variables._Gitems = _items;

                AgendaItem Agendaitem_form = new AgendaItem();
                //Agendaitem_form.Show();
                Agendaitem_form.ShowDialog();



                e.Item._price = Global_variables._Gprice;
                e.Item._Kil = Global_variables._GKil;
                e.Item._Kilprice = Global_variables._GKilprice;
                e.Item._variouscosts = Global_variables._Gvariouscosts;
                e.Item._client = Global_variables._Gclient;

                int _ARGB = Convert.ToInt32(Global_variables._GItemcolor);
                e.Item.BackgroundColor = Color.FromArgb(_ARGB);

                //e.Item.BackgroundColor = Color.FromName(Global_variables._GItemcolor);
                e.Item.Text = Global_variables._GItemtext;
                e.Item._project = Global_variables._GProject;
                e.Item._activity = Global_variables._GActivity;
                e.Item._note = Global_variables._Gnote;
            }
            //else
            //{
            //    MessageBox.Show("hi");
            //}
            //showAgendaItems();

            //MessageBox.Show(e.Item.Text);
        }

        private void calendar1_ItemDeleted(object sender, CalendarItemEventArgs e)
        {
            _items.Remove(e.Item);
        }

        private void calendar1_DayHeaderClick(object sender, CalendarDayEventArgs e)
        {
            calendar1.SetViewRange(e.CalendarDay.Date, e.CalendarDay.Date);
        }

        private void calendar1_ItemCreating(object sender, CalendarItemCancelEventArgs e)
        {

          //  MessageBox.Show(sender.ToString());
            //Global_variables._GActiveCalendarItem = null;
            Global_variables._Gitems = new List<CalendarItem>();
            Global_variables._Gitems = _items;

              tContextInfo.Text = "";
              tContextInfo.Text = Helptext["agenda-item-creating"];
            

            if (e.Item.Tag == "enter" || e.Item.Tag == "insert" || e.Item.Tag == "doubleclick")
            {

                // Fresh Item!.
                //-------------
                Global_variables._GActiveCalendarItem = null;
                Global_variables._GActiveCalendarItem = new CalendarItem(calendar1);
                Global_variables._GActiveCalendarItem._price = 0;
                Global_variables._GActiveCalendarItem._Kil = 0;
                Global_variables._GActiveCalendarItem._Kilprice = 0;
                Global_variables._GActiveCalendarItem._variouscosts = 0;
                Global_variables._GActiveCalendarItem.Text = "-";

                Global_variables._GActiveCalendarItem._project = "";
                Global_variables._GActiveCalendarItem._activity = "";
                Global_variables._GActiveCalendarItem._client = "";
                Global_variables._GActiveCalendarItem._note = "";

                Global_variables._GActiveCalendarItem.BackgroundColor = Color.White;
                //-----------------------

                //MessageBox.Show("enter pressed");
                AgendaItem Agendaitem_form = new AgendaItem();
                //Agendaitem_form.Show();
                Agendaitem_form.ShowDialog();


            }
            else
            {

                AgendaItem Agendaitem_form = new AgendaItem();
                //Agendaitem_form.Show();
                Agendaitem_form.ShowDialog();


              
            }

            e.Item._price = Global_variables._Gprice;
            e.Item._Kil = Global_variables._GKil;
            e.Item._Kilprice = Global_variables._GKilprice;
            e.Item._variouscosts = Global_variables._Gvariouscosts;
            e.Item._client = Global_variables._Gclient;
            int _ARGB = Convert.ToInt32(Global_variables._GItemcolor);
            e.Item.BackgroundColor = Color.FromArgb(_ARGB);
            e.Item.Text = Global_variables._GItemtext;

            e.Item._project = Global_variables._GProject;
            e.Item._activity = Global_variables._GActivity;
            e.Item._note = Global_variables._Gnote;


            totalhours = totalhours + ((e.Item.Duration.TotalMinutes) / 60);
            lTotalHours.Text = totalhours.ToString();
            //if (!_BoldedDates.Contains(e.Item.StartDate.Date))
            //{
            //_BoldedDates.Add(e.Item.StartDate.Date);
            //}
            //monthCalendar2.BoldedDates = _BoldedDates;


            //Application.Run(Agendaitem_form);
            // Agendaitem_form.Focus();
        }


        private void calendar1_KeyDown(object sender, KeyEventArgs e)
        {

 
        }

        private void calendar1_MouseHover(object sender, EventArgs e)
        {
            tContextInfo.Text = "";
            tContextInfo.Text = Helptext["agenda-view"];
        }


        private void calendar1_DragDrop(object sender, DragEventArgs e)
        {
            //MessageBox.Show("HI!");
            //if (!_BoldedDates.Contains(e.Item.StartDate.Date))
            //{
            //    _BoldedDates.Add(e.Item.StartDate.Date);
            //}
            //monthCalendar2.BoldedDates = _BoldedDates;

        }

        // Search methods:
        private void bSearch_Click(object sender, EventArgs e)
        {

            if (tSearchText.Text.Length >= 3)
            {
                // searchItems(tSearchText.Text);



                try
                {
                    //var results = from DataRow myRow in dt_Linq_Query.Rows
                    //where (int)myRow["RowNo"] == 1
                    // select myRow;
                    Create_DataTable_Linq_Query();
                    Regex rx = new Regex(tSearchText.Text, RegexOptions.IgnoreCase);

                    var query = from r in dt_Linq_Query.AsEnumerable()
                                where
                                      rx.IsMatch(r.Field<string>("title")) ||
                                      rx.IsMatch(r.Field<string>("project")) ||
                                      rx.IsMatch(r.Field<string>("client")) ||
                                      rx.IsMatch(r.Field<string>("activity")) ||
                                      rx.IsMatch(r.Field<string>("note"))
                                select r;

                    DataTable newDT = new DataTable();

                    foreach (var v in query)
                    {
                        newDT = query.CopyToDataTable<DataRow>();
                        break;
                    }

                    if (newDT.Rows.Count > 0)
                    {
                        // Checks whether the entire result is null OR
                        // contains no resulting records.

                        // DataTable newDT = query.CopyToDataTable();
                        //newDT.Export
                        tab2Dphysics.SelectedIndex = 1;
                        lFoundAgendaItems.Text = newDT.Rows.Count.ToString();
                        showAgendaItems_Query(newDT);
                    }
                    else
                    {
                        tab2Dphysics.SelectedIndex = 1;
                        showAgendaItems_Query(newDT);
                        lFoundAgendaItems.Text = newDT.Rows.Count.ToString() ;

                        //textBox1.Text = "";
                        MessageBox.Show("No results were found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            else
            {
                MessageBox.Show("Query needs to have 3 characters or more");
            }
            //Me


        }

        private void searchItems(string _itemtext)
        {
            //return false;

            foreach (CalendarItem item in _items)
            {
                if (item.Text == _itemtext)
                {
                    calendar1.SetViewRange(item.StartDate, item.StartDate.AddDays(6));
                    monthCalendar2.SelectionRange = new SelectionRange(item.StartDate, item.StartDate.AddDays(6));
                    //return true;
                }

            }

        }

        private void DeleteItems()
        {
            foreach (CalendarItem item in _items)
            {
                //if (calendar1.ViewIntersects(item))
                //{
                calendar1.Items.Remove(item);
                //}
            }
        }

        private void PlaceItems()
        {
            foreach (CalendarItem item in _items)
            {
                if (calendar1.ViewIntersects(item))
                {
                calendar1.Items.Add(item);


                }
            }
        }


        private void cBTimeScale_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cBTimeScale.SelectedIndex == 0)
            {
                calendar1.TimeScale = CalendarTimeScale.SixtyMinutes;
            }

            else if (cBTimeScale.SelectedIndex == 1)
            {
                calendar1.TimeScale = CalendarTimeScale.ThirtyMinutes;
            }

            else if (cBTimeScale.SelectedIndex == 2)
            {
                calendar1.TimeScale = CalendarTimeScale.FifteenMinutes;
            }

            else if (cBTimeScale.SelectedIndex == 3)
            {
                calendar1.TimeScale = CalendarTimeScale.TenMinutes;
            }

            else if (cBTimeScale.SelectedIndex == 4)
            {
                calendar1.TimeScale = CalendarTimeScale.SixMinutes;
            }

            else if (cBTimeScale.SelectedIndex == 5)
            {
                calendar1.TimeScale = CalendarTimeScale.FiveMinutes;
            }
            calendar1.Focus();
        }

        private void cBTimeScale_MouseHover(object sender, EventArgs e)
        {
            tContextInfo.Text = Helptext["timescale"];
        }



        private void nUViewRange_ValueChanged(object sender, EventArgs e)
        {
            DayOffset = Convert.ToInt32(nUViewRange.Value);
            tContextInfo.Text = Helptext["view-range"];
        }

        private void bCopy_Click(object sender, EventArgs e)
        {
            _Copy = true;
        }

        private void bCopy_MouseHover(object sender, EventArgs e)
        {
            tContextInfo.Text = Helptext["copy"];
        }

        private void bPaste_Click(object sender, EventArgs e)
        {
            calendar1.CreateItemOnSelection(string.Empty, true);
        }

        
        private void bPaste_MouseHover_1(object sender, EventArgs e)
        {
            tContextInfo.Text = Helptext["paste"];
        }

     
        private void bDelete_Click(object sender, EventArgs e)
        {
            calendar1.DeleteSelectedItems();
        }

        private void bDelete_MouseHover_1(object sender, EventArgs e)
        {
            tContextInfo.Text = Helptext["delete"];
        }



        private void bNext_Click_1(object sender, EventArgs e)
        {
            DateTime _selectionstart = monthCalendar2.SelectionStart;
            DateTime _NewSelectionstart = _selectionstart.AddDays(DayOffset + 1);
            DateTime _NewSelectionend = _NewSelectionstart.AddDays(DayOffset);

            monthCalendar2.SelectionRange = new SelectionRange(_NewSelectionstart, _NewSelectionend);

            calendar1.SetViewRange(new System.DateTime(_NewSelectionstart.Year, _NewSelectionstart.Month, _NewSelectionstart.Day), new System.DateTime(_NewSelectionend.Year, _NewSelectionend.Month, _NewSelectionend.Day));
            //monthCalendar2.Refresh();
           // MessageBox.Show("hi");

            lSelWeekNumber.Text = WeekOfYear(monthCalendar2.SelectionStart).ToString();
            lSelYear.Text = monthCalendar2.SelectionStart.Year.ToString();
            lSelMonth.Text = monthCalendar2.SelectionStart.Month.ToString();
            lSelDay.Text = monthCalendar2.SelectionStart.Day.ToString();
        }

        //private void bPrevious_Click(object sender, EventArgs e)
        //{

        //}

        private void bNext_MouseHover(object sender, EventArgs e)
        {
            tContextInfo.Text = "";
            tContextInfo.Text = "Show next agenda view-range";
        }


        private void bPrevious_Click_1(object sender, EventArgs e)
        {
            DateTime _selectionstart = monthCalendar2.SelectionStart;
            DateTime _NewSelectionend = _selectionstart.AddDays(- 1);
            DateTime _NewSelectionstart = _NewSelectionend.AddDays(- DayOffset);
           

            monthCalendar2.SelectionRange = new SelectionRange(_NewSelectionstart, _NewSelectionend);

            calendar1.SetViewRange(new System.DateTime(_NewSelectionstart.Year, _NewSelectionstart.Month, _NewSelectionstart.Day), new System.DateTime(_NewSelectionend.Year, _NewSelectionend.Month, _NewSelectionend.Day));

            lSelWeekNumber.Text = WeekOfYear(monthCalendar2.SelectionStart).ToString();
            lSelYear.Text = monthCalendar2.SelectionStart.Year.ToString();
            lSelMonth.Text = monthCalendar2.SelectionStart.Month.ToString();
            lSelDay.Text = monthCalendar2.SelectionStart.Day.ToString();
        }

        private void bPrevious_MouseHover(object sender, EventArgs e)
        {
            tContextInfo.Text = "";
            tContextInfo.Text = "Show previous agenda view-range";
        }


        //public static int GetWeekNumber(DateTime dtPassed)
        //{

        //    int Weeknum = (int)(Math.Ceiling((decimal)dtPassed.Day / 7)) + (((new DateTime(dtPassed.Year, dtPassed.Month, 1).DayOfWeek) > dtPassed.DayOfWeek) ? 1 : 0); 


        //    //CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    //int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return Weeknum;
        //}

        //public static int GetWeekOfMonth(DateTime date)
        //{
        //    DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

        //    while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
        //        date = date.AddDays(1);

        //    return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        //} 


        public int WeekOfYear(DateTime date)
        {
        DateTime startOfYear = new DateTime(date.Year, 1, 1);
        DateTime endOfYear = new DateTime(date.Year, 12, 31);
        // ISO 8601 weeks start with Monday 
        // The first week of a year includes the first Thursday 
        // This means that Jan 1st could be in week 51, 52, or 53 of the previous year...
        int numberDays = date.Subtract(startOfYear).Days + 
        				moveByDays[(int) startOfYear.DayOfWeek];
        int weekNumber = numberDays / 7;
        switch (weekNumber)
            {
            case 0:
                // Before start of first week of this year - in last week of previous year
                weekNumber = WeekOfYear(startOfYear.AddDays(-1));
                break;
            case 53:
                // In first week of next year.
                if (endOfYear.DayOfWeek < DayOfWeek.Thursday)
                    {
                    weekNumber = 1;
                    }
                break;
            }
        return weekNumber;
        }
    

        // --------------------------------------------------------------------------------------------------------------------------------------------------


        // MONTH CALENDAR --------------------------------------------------------------------------------------------------------------------------------------

        // Change active date in monthcalendar
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            pickedDateTime = dateTimePicker1.Value;

           // DateTime StartDate = new DateTime(2011, 9, 21);
            //DateTime EndDate   = new DateTime(2011, 9, 25);

           // monthCalendar2.SelectionRange = new SelectionRange(StartDate, EndDate);
            
            //monthCalendar2.R;
           // monthCalendar2.Sek
            monthCalendar2.ViewStart = pickedDateTime;
            monthCalendar2.SelectionRange = new SelectionRange(pickedDateTime, pickedDateTime.AddDays(DayOffset));
            //monthCalendar2.ViewStart = pickedDateTime;
            //monthCalendar2.Refresh();
            calendar1.SetViewRange(pickedDateTime, pickedDateTime.AddDays(DayOffset));
            tab2Dphysics.SelectedIndex = 0;


            lSelWeekNumber.Text = WeekOfYear(pickedDateTime).ToString();
            lSelYear.Text = pickedDateTime.Year.ToString();
            lSelMonth.Text = pickedDateTime.Month.ToString();
            lSelDay.Text = pickedDateTime.Day.ToString();
            //monthCalendar2.SelectionStart = pickedDateTime;



        }

        private void monthCalendar2_MouseHover(object sender, EventArgs e)
        {
            tContextInfo.Text = "";
            tContextInfo.Text = Helptext["month-calendar"];
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            //MessageBox.Show("hi");

            DateTime enddate = monthCalendar2.SelectionStart.AddDays(DayOffset);
            calendar1.SetViewRange(monthCalendar2.SelectionStart, enddate);
            monthCalendar2.SelectionRange = new SelectionRange(monthCalendar2.SelectionStart, enddate);
            tab2Dphysics.SelectedIndex = 0;
            //lSelWeekNumber.Text = GetWeekNumber(monthCalendar1.SelectionStart).ToString();

            lSelWeekNumber.Text = WeekOfYear(monthCalendar2.SelectionStart).ToString();
            lSelYear.Text = monthCalendar2.SelectionStart.Year.ToString();
            lSelMonth.Text = monthCalendar2.SelectionStart.Month.ToString();
            lSelDay.Text = monthCalendar2.SelectionStart.Day.ToString();

            //dateTimePicker1.Value = monthCalendar2.SelectionStart;
        }


        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            //MessageBox.Show("hi");
        }

        private void monthCalendar2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void monthCalendar2_Paint(object sender, PaintEventArgs e)
        {
            //monthCalendar2.BoldedDates = _BoldedDates;

        }


        // --------------------------------------------------------------------------------------------------------------------------------------------------


        // Context Menu Strip-----------------------------------------------------------------------------------------------------------------------------------

        private void hourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.SixtyMinutes;
            cBTimeScale.SelectedIndex = 0;
        }

        private void minutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.ThirtyMinutes;
            cBTimeScale.SelectedIndex = 1;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.FifteenMinutes;
            cBTimeScale.SelectedIndex = 2;
        }

        private void minutesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.TenMinutes;
            cBTimeScale.SelectedIndex = 3;
        }

        private void minutesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.SixMinutes;
            cBTimeScale.SelectedIndex = 4;
        }


        private void minutesToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.FiveMinutes;
            cBTimeScale.SelectedIndex = 5;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextItem = calendar1.ItemAt(contextMenuStrip1.Bounds.Location);
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.DeleteSelectedItems();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (_Copy == true)
            //{
                calendar1.CreateItemOnSelection(string.Empty, true);
                
                //_Copy = false;
            //}
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Copy = true;
        }



        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //IEnumerable<CalendarItem> selecteditems = calendar1.GetSelectedItems();
            //List<CalendarItem> _selecteditems = selecteditems.ToList();

            //if (_selecteditems.Count > 0)
            //{
            //    Global_variables._GActiveCalendarItem = e.Item;
            //    Global_variables._Gitems = new List<CalendarItem>();
            //    Global_variables._Gitems = _items;

            //    AgendaItem Agendaitem_form = new AgendaItem();
            //    //Agendaitem_form.Show();
            //    Agendaitem_form.ShowDialog();



            //    e.Item._price = Global_variables._Gprice;
            //    e.Item._variouscosts = Global_variables._Gvariouscosts;
            //    e.Item._client = Global_variables._Gclient;

            //    int _ARGB = Convert.ToInt32(Global_variables._GItemcolor);
            //    e.Item.BackgroundColor = Color.FromArgb(_ARGB);

            //    //e.Item.BackgroundColor = Color.FromName(Global_variables._GItemcolor);
            //    e.Item.Text = Global_variables._GItemtext;
            //    e.Item._project = Global_variables._GProject;
            //    e.Item._activity = Global_variables._GActivity;
            //    e.Item._note = Global_variables._Gnote;
            //}

        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //calendar1.ActivateEditMode();
            //AgendaItem Agendaitem_form = new AgendaItem();
            //Agendaitem_form.Show();
            //Agendaitem_form.ShowDialog();

            //e.Item._price = Global_variables._Gprice;
            //e.Item._client = Global_variables._Gclient;
            //e.Item.BackgroundColor = Global_variables._GItemcolor;
            //e.Item.Text = Global_variables._GItemtext;



            //Global_variables._GActiveCalendarItem = calendar1.sele

        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.GetSelectedItems())
            {
                item.Pattern = System.Drawing.Drawing2D.HatchStyle.DiagonalCross;
                item.PatternColor = Color.Empty;
                calendar1.Invalidate(item);
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------------

        // MAIN TAB PAGE ----------------------------------------------------------------------------------------------------------------------------------------


        private void tabAgenda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab2Dphysics.SelectedIndex == 0)
            {
                tContextInfo.Text = Helptext["agenda-view"];
            }

            if (tab2Dphysics.SelectedIndex == 1)
            {
                tContextInfo.Text = Helptext["agenda-item-gridview"];
            }


            if (tab2Dphysics.SelectedIndex == 2)
            {
                tContextInfo.Text = Helptext["declaration"];

                if (_items.Count > 0)
                {

                   // Get the unique project/activity/client names:
                    foreach (CalendarItem item in _items)
                    {
                        string _projects = item._project;
                        _projects = _projects.ToLower();


                        if (!cBProjects.Items.Contains(_projects))
                        {
                            if (_projects.Length > 0)
                            {
                                cBProjects.Items.Add(_projects);
                            }
                        }

                        string _clients = item._client;
                        _clients = _clients.ToLower();

                        if (!cBClients.Items.Contains(_clients))
                        {
                            if (_clients.Length > 0)
                            {
                                cBClients.Items.Add(_clients);
                            }
                        }

                        string _activities = item._activity;
                        _activities = _activities.ToLower();

                        if (!cBActivity.Items.Contains(_activities))
                        {
                            if (_activities.Length > 0)
                            {
                                cBActivity.Items.Add(_activities);
                            }
                        }
                    }

                    if (cBProjects.Items.Count > 0)
                    {
                        cBProjects.SelectedIndex = 0;
                    }
                    if (cBClients.Items.Count > 0)
                    {
                        cBClients.SelectedIndex = 0;
                    }
                    if (cBActivity.Items.Count > 0)
                    {

                        cBActivity.SelectedIndex = 0;
                    }
                }



            }
            else if (tab2Dphysics.SelectedIndex == 3)
            {

                tContextInfo.Text = Helptext["statistics"];

                if (_items.Count > 0)
                {

                    Populate_stat_filter_comboboxes();
                }
            }

            else if (tab2Dphysics.SelectedIndex == 4)
            {
                tContextInfo.Text = Helptext["statistics-rel"];

                if (_items.Count > 0)
                {

                    // Get the unique project/activity/client names:
                    foreach (CalendarItem item in _items)
                    {
                        string _projects = item._project;
                        _projects = _projects.ToLower();


                        if (!cBRelProjects.Items.Contains(_projects))
                        {
                            if (_projects.Length > 0)
                            {
                                cBRelProjects.Items.Add(_projects);
                            }
                        }

                        string _clients = item._client;
                        _clients = _clients.ToLower();

                        if (!cBRelClient.Items.Contains(_clients))
                        {
                            if (_clients.Length > 0)
                            {
                                cBRelClient.Items.Add(_clients);
                            }
                        }

                        string _activities = item._activity;
                        _activities = _activities.ToLower();

                        if (!cBRelActivity.Items.Contains(_activities))
                        {
                            if (_activities.Length > 0)
                            {
                                cBRelActivity.Items.Add(_activities);
                            }
                        }
                    }

                    if (cBRelProjects.Items.Count > 0)
                    {
                        cBRelProjects.SelectedIndex = 0;
                    }
                    if (cBRelClient.Items.Count > 0)
                    {
                        cBRelClient.SelectedIndex = 0;
                    }
                    if (cBRelActivity.Items.Count > 0)
                    {

                        cBRelActivity.SelectedIndex = 0;
                    }
                }



            }

        }




        //--------------------------------------------------------------------------------------------------------------------------------------------------------


        // AGENDA ITEM (GRIDVIEW) --------------------------------------------------------------------------------------------------------------------------------------

        private void showAgendaItems()
        {
            pAgendaItems.Controls.Clear();
            DataGridView GView = new DataGridView();
            GView.Name = "Gridview_AgendaItems";
            GView.BackgroundColor = Color.White;
            //GView.ScrollBars = ScrollBars.None;
            GView.ScrollBars = ScrollBars.Both;
            GView.ColumnHeadersHeight = 40;
            GView.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular);
            GView.RowsDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular);

            pAgendaItems.AutoScroll = false;


            DataGridViewColumn newCol0 = new DataGridViewTextBoxColumn();
            newCol0.HeaderText = "Index";
            newCol0.Width = Convert.ToInt16(60);
            newCol0.ValueType = typeof(System.Int16);
            newCol0.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol0);

            DataGridViewColumn newCol1 = new DataGridViewTextBoxColumn();
            newCol1.HeaderText = "Item text";
            newCol1.Width = Convert.ToInt16(100);
            newCol1.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol1);

            DataGridViewColumn newCol1_1 = new DataGridViewTextBoxColumn();
            newCol1_1.HeaderText = "Date";
            newCol1_1.Width = Convert.ToInt16(150);
            newCol1_1.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol1_1);

            DataGridViewColumn newCol2 = new DataGridViewTextBoxColumn();
            newCol2.HeaderText = "Start time";
            newCol2.Width = Convert.ToInt16(150);
            newCol2.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol2);


            DataGridViewColumn newCol3 = new DataGridViewTextBoxColumn();
            newCol3.HeaderText = "End time";
            newCol3.Width = Convert.ToInt16(150);
            newCol3.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol3);


            DataGridViewColumn newCol4 = new DataGridViewTextBoxColumn();
            newCol4.HeaderText = "Year";
            newCol4.Width = Convert.ToInt16(100);
            newCol4.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol4);

            DataGridViewColumn newCol5 = new DataGridViewTextBoxColumn();
            newCol5.HeaderText = "Month";
            newCol5.Width = Convert.ToInt16(100);
            newCol5.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol5);


            DataGridViewColumn newCol6 = new DataGridViewTextBoxColumn();
            newCol6.HeaderText = "Day Start";
            newCol6.Width = Convert.ToInt16(100);
            //newCol6.ValueType = typeof(System.Int16);
            newCol6.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol6);

            DataGridViewColumn newCol7 = new DataGridViewTextBoxColumn();
            newCol7.HeaderText = "Day End";
            newCol7.Width = Convert.ToInt16(100);
           // newCol7.ValueType = typeof(System.Int16);
            newCol7.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol7);

            DataGridViewColumn newCol8 = new DataGridViewTextBoxColumn();
            newCol8.HeaderText = "Duration (hours)";
            newCol8.Width = Convert.ToInt16(100);
            newCol8.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol8);

            DataGridViewColumn newCol9 = new DataGridViewTextBoxColumn();
            newCol9.HeaderText = "Duration (Minutes)";
            newCol9.Width = Convert.ToInt16(100);
            newCol9.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol9);

            DataGridViewColumn newCol10 = new DataGridViewTextBoxColumn();
            newCol10.HeaderText = "Color";
            newCol10.Width = Convert.ToInt16(100);
            newCol10.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol10);

            DataGridViewColumn newCol11 = new DataGridViewTextBoxColumn();
            newCol11.HeaderText = "Price";
            newCol11.Width = Convert.ToInt16(100);
            newCol11.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol11);

            DataGridViewColumn newCol11_2 = new DataGridViewTextBoxColumn();
            newCol11_2.HeaderText = "Kilometers";
            newCol11_2.Width = Convert.ToInt16(100);
            newCol11_2.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol11_2);

            DataGridViewColumn newCol11_3 = new DataGridViewTextBoxColumn();
            newCol11_3.HeaderText = "Price per KM";
            newCol11_3.Width = Convert.ToInt16(100);
            newCol11_3.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol11_3);

            DataGridViewColumn newCol11_1 = new DataGridViewTextBoxColumn();
            newCol11_1.HeaderText = "Various costs";
            newCol11_1.Width = Convert.ToInt16(100);
            newCol11_1.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol11_1);

            DataGridViewColumn newCol12 = new DataGridViewTextBoxColumn();
            newCol12.HeaderText = "Client";
            newCol12.Width = Convert.ToInt16(100);
            newCol12.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol12);


            DataGridViewColumn newCol13 = new DataGridViewTextBoxColumn();
            newCol13.HeaderText = "Project";
            newCol13.Width = Convert.ToInt16(100);
            newCol13.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol13);

            DataGridViewColumn newCol14 = new DataGridViewTextBoxColumn();
            newCol14.HeaderText = "Activity";
            newCol14.Width = Convert.ToInt16(100);
            newCol14.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol14);

            try
            {
                int i = 1;
                // Show each unique found word in the Gridview:
                foreach (CalendarItem _item in _items)
                {

                    DataGridViewRow row = (DataGridViewRow)GView.Rows[0].Clone();

                    row.Cells[0].Value = Convert.ToInt32(i.ToString());

                    row.Cells[1].Value = _item.Text;
                    row.Cells[2].Value = _item.Date.ToString();
                    row.Cells[3].Value = _item.StartDate.ToString();
                    row.Cells[4].Value = _item.EndDate.ToString();

                    row.Cells[5].Value = _item.Date.Year.ToString();
                    row.Cells[6].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_item.Date.Month);

                    row.Cells[7].Value = _item.StartDate.Day.ToString();

                    row.Cells[8].Value = _item.EndDate.Day.ToString();
                    row.Cells[9].Value = Convert.ToInt32(_item.Duration.TotalHours);
                    row.Cells[10].Value = Convert.ToInt32(_item.Duration.TotalMinutes);

                    row.Cells[11].Value = _item.BackgroundColor.Name.ToString();

                    row.Cells[12].Value = _item._price;
                    row.Cells[13].Value = _item._Kil;
                    row.Cells[14].Value = _item._Kilprice;

                    row.Cells[15].Value = _item._variouscosts;

                    row.Cells[16].Value = _item._client;

                    row.Cells[17].Value = _item._project;
                    row.Cells[18].Value = _item._activity;

                    GView.Rows.Add(row);
                    i = i + 1;
                }

                GView.Width = pAgendaItems.Width;
                GView.Height = pAgendaItems.Height;

                GView.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(gridview_rowclick_AgendaItems);
                GView.MouseHover += new EventHandler(gridview_mousehover_AgendaItems);
                //GView.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(gridview_rowclick_noffiles);
                GView.PerformLayout();
                pAgendaItems.Controls.Add(GView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        public void gridview_mousehover_AgendaItems(object sender, EventArgs e)
        {
            tContextInfo.Text = "";
            tContextInfo.Text = Helptext["agenda-item-gridview"];

        }



        public void gridview_rowclick_AgendaItems(object sender, DataGridViewCellMouseEventArgs e)
        {

            int value = e.RowIndex;
            string date = "";
            tab2Dphysics.SelectedIndex = 0;
            //computerword = c.Rows[e.RowIndex].Cells[2].Value.ToString();


            if (pAgendaItems.Controls.ContainsKey("Gridview_AgendaItems"))
            {
                Control[] controls = pAgendaItems.Controls.Find("Gridview_AgendaItems", true);

                foreach (DataGridView c in controls)
                {
                    if (c.Name == "Gridview_AgendaItems")
                    {
                        try
                        {
                            // retrieve the word from the row:
                            date = c.Rows[e.RowIndex].Cells[2].Value.ToString();

                            DateTime _AgendaItem = Convert.ToDateTime(date);


                            monthCalendar2.ViewStart = new System.DateTime(_AgendaItem.Year, _AgendaItem.Month, _AgendaItem.Day);
                            //monthCalendar2.SelectionRange = new SelectionRange(pickedDateTime, pickedDateTime.AddDays(DayOffset));


                            monthCalendar2.SelectionRange = new SelectionRange(new System.DateTime(_AgendaItem.Year, _AgendaItem.Month, _AgendaItem.Day), new System.DateTime(_AgendaItem.Year, _AgendaItem.Month, _AgendaItem.Day).AddDays(DayOffset));
                            monthCalendar2.Refresh();

                            calendar1.SetViewRange(new System.DateTime(_AgendaItem.Year, _AgendaItem.Month, _AgendaItem.Day), new System.DateTime(_AgendaItem.Year, _AgendaItem.Month, _AgendaItem.Day).AddDays(DayOffset));
                            
                            
                            lSelWeekNumber.Text = WeekOfYear(monthCalendar2.SelectionStart).ToString();
                            lSelYear.Text = monthCalendar2.SelectionStart.Year.ToString();
                            lSelMonth.Text = monthCalendar2.SelectionStart.Month.ToString();
                            lSelDay.Text = monthCalendar2.SelectionStart.Day.ToString();

                        }
                        catch
                        {
                        }
                    }
                }

            }
        }


        private void tbshowAgendaItems_Click(object sender, EventArgs e)
        {
            showAgendaItems();
        }

        private void tbExportToExcell_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Export gridview to excell?", "Warning",
              MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                btExport_toExcell(pAgendaItems, "Gridview_AgendaItems");
            }
            else if (result == DialogResult.Cancel)
            {
                //code for No
            }

        }


        private void btExport_toExcell(Panel panel1, string controlname)
        {


            Control[] controls = panel1.Controls.Find(controlname, true);

            if (controls.Length > 0)
            {
                foreach (DataGridView c in controls)
                {
                    if (c.Name == controlname)
                    {
                        try
                        {
                            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                            app.Visible = true;
                            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets.Add();
                            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;


                            for (int i = 1; i < c.Columns.Count + 1; i++)
                            {
                                worksheet.Cells[1, i] = c.Columns[i - 1].HeaderText;
                            }
                            for (int i = 0; i < c.Rows.Count - 1; i++)
                            {
                                for (int j = 0; j < c.Columns.Count; j++)
                                {
                                    if (c.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cells[i + 2, j + 1] = c.Rows[i].Cells[j].Value.ToString();
                                    }
                                    else
                                    {
                                        worksheet.Cells[i + 2, j + 1] = "";
                                    }
                                }
                            }
                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show("An error occurred: '{0}':  " + e2);
                        }
                        // c.Series["file extensions"].ChartType = SeriesChartType.Bar;
                    }

                }
            }
            else
            {
                MessageBox.Show("There are no Agenda Items in the Gridview");
            }

        }
        // -------------------------- --------------------------------------------------------------------------------------------------------------------------------------

        // HOUR DECLARATIONS TAB ------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void showAgendaItems_Query(DataTable _DataTable)
        {
            pAgendaItems.Controls.Clear();
            foundhours = 0;

            DataGridView GView = new DataGridView();
            GView.Name = "Gridview_AgendaItems";
            GView.BackgroundColor = Color.White;
            GView.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular);
            GView.RowsDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular);
            //GView.ScrollBars = ScrollBars.None;
            GView.ScrollBars = ScrollBars.Both;
            GView.ColumnHeadersHeight = 40;
            pAgendaItems.AutoScroll = false;

            DataGridViewColumn newCol0 = new DataGridViewTextBoxColumn();
            newCol0.HeaderText = "Index";
            newCol0.Width = Convert.ToInt16(60);
            newCol0.ValueType = typeof(System.Int16);
            newCol0.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol0);

            DataGridViewColumn newCol1 = new DataGridViewTextBoxColumn();
            newCol1.HeaderText = "Item text";
            newCol1.Width = Convert.ToInt16(100);
            newCol1.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol1);

            DataGridViewColumn newCol2 = new DataGridViewTextBoxColumn();
            newCol2.HeaderText = "Start time";
            newCol2.Width = Convert.ToInt16(150);
            newCol2.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol2);


            DataGridViewColumn newCol3 = new DataGridViewTextBoxColumn();
            newCol3.HeaderText = "End time";
            newCol3.Width = Convert.ToInt16(150);
            newCol3.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol3);

            DataGridViewColumn newCol4 = new DataGridViewTextBoxColumn();
            newCol4.HeaderText = "Price";
            newCol4.Width = Convert.ToInt16(100);
            newCol4.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol4);

            DataGridViewColumn newCol4_2 = new DataGridViewTextBoxColumn();
            newCol4_2.HeaderText = "Kilometers";
            newCol4_2.Width = Convert.ToInt16(100);
            newCol4_2.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol4_2);

            DataGridViewColumn newCol4_3 = new DataGridViewTextBoxColumn();
            newCol4_3.HeaderText = "Price per KM";
            newCol4_3.Width = Convert.ToInt16(100);
            newCol4_3.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol4_3);

            DataGridViewColumn newCol4_1 = new DataGridViewTextBoxColumn();
            newCol4_1.HeaderText = "Various costs";
            newCol4_1.Width = Convert.ToInt16(100);
            newCol4_1.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol4_1);

            DataGridViewColumn newCol5 = new DataGridViewTextBoxColumn();
            newCol5.HeaderText = "Minutes";
            newCol5.Width = Convert.ToInt16(100);
            newCol5.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol5);

            DataGridViewColumn newCol6 = new DataGridViewTextBoxColumn();
            newCol6.HeaderText = "Client";
            newCol6.Width = Convert.ToInt16(100);
            newCol6.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol6);


            DataGridViewColumn newCol7 = new DataGridViewTextBoxColumn();
            newCol7.HeaderText = "Project";
            newCol7.Width = Convert.ToInt16(100);
            newCol7.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol7);

            DataGridViewColumn newCol8 = new DataGridViewTextBoxColumn();
            newCol8.HeaderText = "Activity";
            newCol8.Width = Convert.ToInt16(100);
            newCol8.SortMode = DataGridViewColumnSortMode.Automatic;
            GView.Columns.Add(newCol8);


            try
            {
                int i = 1;
                // Show each unique found word in the Gridview:


                foreach (DataRow dtrow in _DataTable.Rows)
                {


                    DataGridViewRow row = (DataGridViewRow)GView.Rows[0].Clone();

                    row.Cells[0].Value = Convert.ToInt32(i.ToString());

                    row.Cells[1].Value = (string)dtrow["title"];
                 
                    row.Cells[2].Value = Convert.ToDateTime(dtrow["starttime"]);
                    row.Cells[3].Value = Convert.ToDateTime(dtrow["endtime"]);
                    row.Cells[4].Value = (string)(dtrow["price"]);
                    row.Cells[5].Value = (string)(dtrow["kil"]);
                    row.Cells[6].Value = (string)(dtrow["kilprice"]);

                    row.Cells[7].Value = (string)(dtrow["variouscosts"]);
                    row.Cells[8].Value = (string)(dtrow["minutes"]);

                    foundhours = foundhours + (Convert.ToDouble((string)(dtrow["minutes"])) / 60);


                    row.Cells[9].Value = (string)dtrow["client"];
                    row.Cells[10].Value = (string)dtrow["project"];

                    row.Cells[11].Value = (string)dtrow["activity"];

                    GView.Rows.Add(row);
                    i = i + 1;

                }

                GView.Width = pAgendaItems.Width;
                GView.Height = pAgendaItems.Height;

                lFoundHours.Text = foundhours.ToString();

                GView.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(gridview_rowclick_AgendaItems);
                GView.MouseHover += new EventHandler(gridview_mousehover_AgendaItems);
                //GView.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(gridview_rowclick_noffiles);
                GView.PerformLayout();
                pAgendaItems.Controls.Add(GView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

       



        private void bQuery_Click(object sender, EventArgs e)
        {

            try
            {
                //var results = from DataRow myRow in dt_Linq_Query.Rows
                //where (int)myRow["RowNo"] == 1
                // select myRow;
                DataGridViewRow row;

                Create_DataTable_Linq_Query();

                //var results = from myRow in dt_Linq_Query.Rows where myRow.Field("RowNo") == 1 select myRow;


                //var query = from r in dt_Linq_Query.AsEnumerable()
                //            where r.Field<string>("project") == "12" &&
                //                  r.Field<string>("client") == "12" &&
                //                  r.Field<DateTime>("endtime") <= DateTimeEnd.Value.Date &&
                //                  r.Field<DateTime>("endtime") >= DateTimeStart.Value.Date
                //            select r;

                //List<DataRow> _query;


                //_query = from r in dt_Linq_Query.AsEnumerable()
                           // where r.Field<DateTime>("endtime") >= DateTimeStart.Value.Date &&
                              //    r.Field<DateTime>("endtime") <= DateTimeEnd.Value.Date
                            //select r;



                var query = from r in dt_Linq_Query.AsEnumerable()
                            where r.Field<DateTime>("starttime").Date >= DateTimeStart.Value.Date &&
                                  r.Field<DateTime>("endtime").Date <= DateTimeEnd.Value.Date
                            select r;

                //var query = from r in dt_Linq_Query.AsEnumerable()
                //            where r.Field(r=>DateTime.Compare(date1, date2)>0);
                //            select r;

                //if (checkProject.Checked == true && checkActivity.Checked == true && checkClient.Checked == true)
                //{
                //query =  query.Where(r.Field<string>("project") == cBProjects.Text; 

                if (checkProject.Checked == true)
                {
                    query = query.Where(p => String.Equals(p.Field<string>("project"), cBProjects.Text, StringComparison.CurrentCultureIgnoreCase));
                    //query = query.Where(p => p.Field<string>("project") == cBProjects.Text);
                }
                if (checkClient.Checked == true)
                {
                   // query = query.Where(p => p.Field<string>("client") == cBClients.Text);
                    query = query.Where(p => String.Equals(p.Field<string>("client"), cBClients.Text, StringComparison.CurrentCultureIgnoreCase ));

                    //String.Equals(s, "Foo", StringComparison.CurrentCultureIgnoreCase));
                }
                if (checkActivity.Checked == true)
                {
                    query = query.Where(p => String.Equals(p.Field<string>("activity"), cBActivity.Text, StringComparison.CurrentCultureIgnoreCase));
                    //query = query.Where(p => p.Field<string>("activity") == cBActivity.Text);
                }

                //var   query = from r in dt_Linq_Query.AsEnumerable()
                //       where
                //             checkClient.Checked == true ? r.Field<string>("project") == cBProjects.Text &&
                //              //r.Field<string>("project") == checkProject.Checked == true ? : cBProjects.Text &&
                //             checkClient.Checked == true ? r.Field<string>("client") == cBClients.Text  &&
                //             checkClient.Checked == true ? r.Field<string>("activity") == cBActivity.Text  && 

                //             r.Field<DateTime>("endtime") >= DateTimeStart.Value.Date &&
                //             r.Field<DateTime>("endtime") <= DateTimeEnd.Value.Date                        
                //       select r;

                //}

                DataTable newDT = new DataTable();


                //convertDataTableToString(newDT);




                foreach (var v in query)
                {
                    newDT = query.CopyToDataTable<DataRow>();
                    break;
                }


               
                //List<DataRow> _query = 
                if (newDT.Rows.Count > 0)
                {
                    // Checks whether the entire result is null OR
                    // contains no resulting records.

                   // DataTable newDT = query.CopyToDataTable();
                    //newDT.Export

                    //showAgendaItems_Query(newDT);
                    dG_declaration.Rows.Clear();

                    double totalminutes = 0;
                    //double totalprice;
                    double totalincome = 0;
                    double totalhours = 0;
                    double totalkil = 0;
                    double totalkilometerprice = 0;
                    double totalvariouscosts = 0;
                    //string format = "{0, 0}\t{1, 50}\t{2, 20}\t{3, 20}\t{4, 20}\t{5, 20}";

                    //sb.AppendLine(string.Format(format, "Title", "Hour price", "-", "Hours", "-", "Income"));
                   // lines.Add(ne);


                    //lines.Add(new List<string> { "Title", "Hour price", "-", "Hours", "-", "Income" });

                    // Columns:
                    //dG_declaration.ColumnCount = 11;
                    //dG_declaration.Columns[0].Name = "Title";
                    //dG_declaration.Columns[1].Name = "Project";
                    //dG_declaration.Columns[2].Name = "Date";
                    //dG_declaration.Columns[3].Name = "Client";
                    
                    //dG_declaration.Columns[4].Name = "Activity";

                    //dG_declaration.Columns[5].Name = "Hour price";
                    //dG_declaration.Columns[6].Name = "-";
                    //dG_declaration.Columns[7].Name = "Hours";
                    //dG_declaration.Columns[8].Name = "Minutes";
                    //dG_declaration.Columns[9].Name = "-";
                    //dG_declaration.Columns[10].Name = "Income";


                    dG_declaration.ColumnCount = 14;
                    dG_declaration.Columns[0].Name = " ";
                    dG_declaration.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[1].Name = " ";
                    dG_declaration.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[2].Name = " ";
                   // dG_declaration.Columns[2].ValueType = .DateTime;
                    dG_declaration.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    
                    dG_declaration.Columns[3].Name = " ";
                    dG_declaration.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dG_declaration.Columns[4].Name = " ";
                    dG_declaration.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dG_declaration.Columns[5].Name = " ";
                    dG_declaration.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[6].Name = " ";
                    dG_declaration.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[7].Name = " ";
                    dG_declaration.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[8].Name = " ";
                    dG_declaration.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[9].Name = " ";
                    dG_declaration.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[10].Name = " ";
                    dG_declaration.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[11].Name = " ";
                    dG_declaration.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dG_declaration.Columns[12].Name = " ";
                    dG_declaration.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dG_declaration.Columns[13].Name = " ";
                    dG_declaration.Columns[13].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //dG_declaration.Columns[14].Name = " ";
                    //dG_declaration.Columns[14].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //dG_declaration.Columns[15].Name = " ";
                    //dG_declaration.Columns[15].SortMode = DataGridViewColumnSortMode.NotSortable;
                    // Columns------------------


                    dG_declaration.Rows.Add(new Object[] { "Hour declaration", "", "", "", "", "", "", "", "", "", "", "", "", ""});
                    dG_declaration.Rows.Add(new Object[] { "Date range", DateTimeStart.Value.Date, DateTimeEnd.Value.Date, "", "", "", "", "", "", "", "", "", "", ""});
                    dG_declaration.Rows.Add(new Object[] { "Hours", "....Name", "", "", "", "", "", "", "", "", "", "", "", ""});
                    dG_declaration.Rows.Add(new Object[] { " ", "", "", "", "", "", "", "", "", "", "", "", "", ""});

                    dG_declaration.Rows.Add(new Object[] { "Title", "Project", "Date", "Client", "Activity", "Hour Price", "Hours", "Minutes", "Hour-price declaration", "KM", "Price per KM", "KM declaration", "Various cost declaration", "Total"});
              

                    foreach (DataRow dtrow in newDT.Rows)
                    {
                        //sb.AppendLine((string)dtrow["title"] + "  "  + (string)(dtrow["price"]) + "   " + (string)(dtrow["minutes"]));

                        double minutes = Convert.ToDouble((string)(dtrow["minutes"]));
                        double hour = minutes / 60;
                        double price = Convert.ToDouble((string)(dtrow["price"]));
                        double variouscosts = Convert.ToDouble((string)(dtrow["variouscosts"]));

                        double kil = Convert.ToDouble((string)(dtrow["kil"]));
                        double kilprice = Convert.ToDouble((string)(dtrow["kilprice"]));
                        double totalkilprice = kil * kilprice;
                        
                     

                        double income = (minutes / 60) * price;
                        double itemtotal = income + variouscosts + totalkilprice;

                       // "{0,-10} | {1,-10} | {2,5}"
                        //sb.AppendLine(string.Format(format, (string)dtrow["title"], price, "X", hour, "=", "€" + income));
                        //lines.Add(new List<string> { });

                        


                        //row = new DataGridViewRow((string)dtrow["title"].ToString(), price.ToString(), "X", hour.ToString(), "=", "€" + income.ToString() );

                        dG_declaration.Rows.Add(new Object[] { (string)dtrow["title"].ToString(), (string)dtrow["project"].ToString(), Convert.ToDateTime(dtrow["starttime"]), (string)dtrow["client"].ToString(), (string)dtrow["activity"].ToString(), price.ToString(), hour.ToString(), minutes.ToString(), "€ " + income.ToString(), kil.ToString(), "€ " + kilprice.ToString(), "€ " + totalkilprice.ToString(), "€ " + variouscosts.ToString(), "€ " + itemtotal.ToString() });
                       
                        //sb.AppendLine(string.Format((string)dtrow["title"] + "\t\t\t" + price + "\t\t" + "X\t\t" + hour + "\t\t" + "=\t\t" + "€" + income + "\t\t"));
                        //sb.AppendLine(string.Format("{%-20s}{%-10s}{%-5s}{%-5s}{%-5s}{%-5s}", (string)dtrow["title"], price, "X", hour, "=", "€" + income));

                        totalminutes = totalminutes + minutes;
                        totalincome = totalincome + income + variouscosts + totalkilprice;
                        totalhours = totalhours + hour;
                        totalkil = totalkil + kil;
                        totalkilometerprice = totalkilometerprice + totalkilprice;
                        //totalminutes = totalminutes + Convert.ToDouble((string)(dtrow["minutes"]));
                        totalvariouscosts = totalvariouscosts + variouscosts;

                        //totalprice = (Convert.ToDouble(string)(dtrow["minutes"])) / 60) * Convert.ToDouble((string)(dtrow["price"]));
                    }

                    //sb.AppendLine("--------------------------------------------------------------- +");

                    //sb.AppendLine((string.Format(format , " ", "  ", " ", totalminutes, " ", "€" + totalincome)));
                    //lines.Add(new List<string> { "", "", "", totalminutes.ToString(), "", "€" + totalincome.ToString() });

                    // Empthy row:
                    dG_declaration.Rows.Add(new Object[] { " ", "", "", "", "", "", "", "", "", "", "", "", "", ""});

                    //// ------------------------------New Row
                    //row = (DataGridViewRow)dG_declaration.Rows[0].Clone();
                    //row.Cells[0].Value = "";
                    //row.Cells[1].Value = "";
                    //row.Cells[2].Value = "";
                    //row.Cells[3].Value = "";
                    //row.Cells[4].Value = "";
                    //row.Cells[5].Value = "";
                    //row.Cells[6].Value = "";
                    //row.Cells[7].Value = "";
                    //row.Cells[8].Value = "";
                    //row.Cells[9].Value = "";
                    //row.Cells[10].Value = "";

                    ////row.DefaultCellStyle.BackColor = Color.LightGreen;
                    //dG_declaration.Rows.Add(row);
                    // ---------------------------------------



                    // ------------------------------New Row
                    row = (DataGridViewRow)dG_declaration.Rows[0].Clone();
                    row.Cells[0].Value = " ";
                    row.Cells[1].Value = " ";
                    row.Cells[2].Value = " ";
                    row.Cells[3].Value = " ";
                    row.Cells[4].Value = " ";
                    row.Cells[5].Value = " ";
                    row.Cells[6].Value = totalhours.ToString(); ;
                    row.Cells[7].Value = totalminutes.ToString();
                    row.Cells[8].Value = " ";
                    row.Cells[9].Value = totalkil.ToString();
                    row.Cells[10].Value = " ";
                    row.Cells[11].Value = "€ " + totalkilometerprice.ToString();
                    row.Cells[12].Value = "€ " + totalvariouscosts.ToString();
                    row.Cells[13].Value = "€ " + totalincome.ToString();
                    row.Cells[13].Style.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

                    //row.Cells[14].Value = "";
                    //row.Cells[15].Value = "€ " + totalincome.ToString();

                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    dG_declaration.Rows.Add(row);
                    // ---------------------------------------

                    dG_declaration.Rows.Add(new Object[] { " ", "", "", "", "", "", "", "", "", "", "", "", "", "" });
                    dG_declaration.Rows.Add(new Object[] { "Date:", "", "", "", "", "", "", "", "", "", "", "", "", "" });
                    dG_declaration.Rows.Add(new Object[] { System.DateTime.Now.Date.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", ""});

                    dG_declaration.Rows.Add(new Object[] { " ", "", "", "", "", "", "", "", "", "", "", "", "", ""});
                    dG_declaration.Rows.Add(new Object[] { "", "Client signature ", "", "", "", "", "", "", "", "", "", "", "Contractor signature:", "" });
                    dG_declaration.Rows.Add(new Object[] { " ", "", "", "", "", "", "", "", "", "", "", "", "", "" });
                    dG_declaration.Rows.Add(new Object[] { "", "________", "", "", "", "", "", "", "", "", "", "", "________", "" });

                    //dG_declaration.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(gridview_rowclick_AgendaItems);

                 

                }
                else
                {
                    //showAgendaItems_Query(newDT);
                    //textBox1.Text = "";
                    MessageBox.Show("There were no results");
                    dG_declaration.Rows.Clear();
                    dG_declaration.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //MessageBox.Show(newDT.Rows.Count.ToString());
            

        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Export gridview to excell?", "Warning",
             MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                btExport_toExcell(pHour_Declarations, "dG_declaration");
            }
            else if (result == DialogResult.Cancel)
            {
                //code for No
            }
        }

        private void tStExpPDF_Click(object sender, EventArgs e)
        {




            string myfile = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = startuppath;
            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (_items.Count > 0)
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {



                        //Creating iTextSharp Table from the DataTable data
                        PdfPTable pdfTable = new PdfPTable(dG_declaration.ColumnCount);
                        pdfTable.DefaultCell.Padding = 3;
                        pdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                        //pdfTable.DefaultCell.Width = 70;
                        pdfTable.WidthPercentage = 70;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTable.DefaultCell.BorderWidth = 1;

                        //Adding Header row
                        foreach (DataGridViewColumn column in dG_declaration.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                            //cell.Border = 0;
                            cell.Border = PdfPCell.NO_BORDER;
                            pdfTable.AddCell(cell);
                        }

                        //Adding DataRow
                        foreach (DataGridViewRow row in dG_declaration.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value == null)
                                {
                                    pdfTable.AddCell("");
                                }
                                else
                                {
                                    pdfTable.AddCell(cell.Value.ToString());
                                }
                            }
                        }

                        //Exporting to PDF
                        //string folderPath = "C:\\PDFs\\";
                        //if (!Directory.Exists(folderPath))
                        //{
                        //    Directory.CreateDirectory(folderPath);
                        //}
                        using (FileStream stream = new FileStream(saveFileDialog1.FileName.ToString().Replace(".pdf", "") + ".pdf", FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            pdfDoc.Add(pdfTable);
                            pdfDoc.Close();
                            stream.Close();
                        }

                        //dt.WriteXml(saveFileDialog1.FileName.ToString().Replace(".xml", "") + ".xml", XmlWriteMode.IgnoreSchema);

                        //OpenedAgenda = Path.GetFileName(saveFileDialog1.FileName);
                        //OpenedAgendaPath = saveFileDialog1.FileName;
                        //tOpenedAgenda.Text = OpenedAgenda;
                    }

                    catch (Exception e2)
                    {
                        MessageBox.Show("An error occurred: '{0}':  " + e2);
                    }
                }
            }
            else
            {
                DialogResult _Message = MessageBox.Show("You have no agenda items placed yet.",
                "Important",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);
            }





            
        }


        // -------------------------- --------------------------------------------------------------------------------------------------------------------------------------


        // Statistics -------------------------------------------------------------------------------------------------------------------------------------------------------

        private void bShow_Click(object sender, EventArgs e)
        {
            // Agenda items per day of week:
            if (cAgendaItems.SelectedIndex == 0)
            {
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.showchart_daysofweek(_items, pGraph.Width, pGraph.Height, false);
                pGraph.Controls.Add(Chart1);
            }
            // Hours per day of week:
            else if (cAgendaItems.SelectedIndex == 1)
            {
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.showchart_daysofweek(_items, pGraph.Width, pGraph.Height, true);
                pGraph.Controls.Add(Chart1);

            }
            // Agenda items per month:
            else if (cAgendaItems.SelectedIndex == 2)
            {
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.showchart_months(_items, pGraph.Width, pGraph.Height, false);
                pGraph.Controls.Add(Chart1);

            }
            // hours per month:
            else if (cAgendaItems.SelectedIndex == 3)
            {
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.showchart_months(_items, pGraph.Width, pGraph.Height, true);
                pGraph.Controls.Add(Chart1);

            }
            else if (cAgendaItems.SelectedIndex == 4)
            {
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.showchart_years(_items, pGraph.Width, pGraph.Height, true);
                pGraph.Controls.Add(Chart1);

            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int i = 0;
            string myfile = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = startuppath;
            saveFileDialog1.Filter = "xml files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            {
                if (pGraph.Controls.ContainsKey("Chart1"))
                {
                    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        try
                        {

                            Control[] controls = pGraph.Controls.Find("Chart1", true);

                            foreach (Chart c in controls)
                            {
                                if (c.Name == "Chart1")
                                {
                                    c.SaveImage(saveFileDialog1.FileName.ToString() + ".Jpeg", ImageFormat.Jpeg);
                                }
                            }


                            //Bitmap bmp = new Bitmap(circleMap.Width, circleMap.Height);
                            //circleMap.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                            //bmp.Save(saveFileDialog1.FileName.ToString() + ".Jpeg", ImageFormat.Jpeg);

                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show("An error occurred: '{0}':  " + e2);
                        }

                    }
                }
            }
        }

        private void bApplyCharttype_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ccharttype.SelectedItem == "Point")
                {
                    //pGraph.Controls.Clear();
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Point;

                   
                    //pGraph.Controls.Add(Chart1);
                }
                else if (Ccharttype.SelectedItem == "FastPoint")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.FastPoint;
                }
                else if (Ccharttype.SelectedItem == "Bubble")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Bubble;

                }
                else if (Ccharttype.SelectedItem == "Line")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Line;
                }
                else if (Ccharttype.SelectedItem == "Spline")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Spline;
                }
                else if (Ccharttype.SelectedItem == "StepLine")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StepLine;
                }
                else if (Ccharttype.SelectedItem == "FastLine")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.FastLine;
                }
                else if (Ccharttype.SelectedItem == "Bar")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Bar;
                }
                else if (Ccharttype.SelectedItem == "StackedBar")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StackedBar;
                }
                else if (Ccharttype.SelectedItem == " StackedBar100")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StackedBar100;

                }
                else if (Ccharttype.SelectedItem == "Column")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Column;
                }
                else if (Ccharttype.SelectedItem == "StackedColumn")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StackedColumn;
                }
                else if (Ccharttype.SelectedItem == "StackedColumn100")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StackedColumn100;
                }
                else if (Ccharttype.SelectedItem == "Area")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Area;
                }
                else if (Ccharttype.SelectedItem == "SplineArea")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.SplineArea;
                }
                else if (Ccharttype.SelectedItem == "StackedArea")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StackedArea;
                }
                else if (Ccharttype.SelectedItem == "StackedArea100")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.StackedArea100;
                }
                else if (Ccharttype.SelectedItem == "Pie")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Pie;
                }
                else if (Ccharttype.SelectedItem == "Doughnut")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Doughnut;
                }
                else if (Ccharttype.SelectedItem == "Stock")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Stock;
                }
                else if (Ccharttype.SelectedItem == "Candlestick")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Candlestick;
                }
                else if (Ccharttype.SelectedItem == "Range")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Range;
                }
                else if (Ccharttype.SelectedItem == "SplineRange")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.SplineRange;
                }
                else if (Ccharttype.SelectedItem == "RangeBar")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.RangeBar;
                }
                else if (Ccharttype.SelectedItem == "RangeColumn")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.RangeColumn;
                }
                else if (Ccharttype.SelectedItem == "Radar")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Radar;
                }
                else if (Ccharttype.SelectedItem == "Polar")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Polar;
                }
                else if (Ccharttype.SelectedItem == "ErrorBar")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.ErrorBar;
                }
                else if (Ccharttype.SelectedItem == "BoxPlot")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.BoxPlot;
                }
                else if (Ccharttype.SelectedItem == "Renko")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Renko;
                }
                else if (Ccharttype.SelectedItem == "ThreeLineBreak")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.ThreeLineBreak;
                }
                else if (Ccharttype.SelectedItem == "Kagi")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Kagi;
                }
                else if (Ccharttype.SelectedItem == "PointAndFigure")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.PointAndFigure;
                }
                else if (Ccharttype.SelectedItem == "Funnel")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Funnel;
                }
                else if (Ccharttype.SelectedItem == "Pyramid")
                {
                    Chart1.Series["Agenda_items"].ChartType = SeriesChartType.Pyramid;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cStatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cStatType.SelectedIndex == 0)
            {
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = false;
                nUInterval.Enabled = false;
            
            }
            // Hours per project:
            else if (cStatType.SelectedIndex == 1)
            {
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = false;
                nUInterval.Enabled = false;

            }

            // Incomer per activity:
            else if (cStatType.SelectedIndex == 2)
            {
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = false;
                nUInterval.Enabled = false;
            }

            // Hours per activity: 
            else if (cStatType.SelectedIndex == 3)
            {
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = false;
                nUInterval.Enabled = false;
            }

            // Income per client
            else if (cStatType.SelectedIndex == 4)
            {
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = false;
                nUInterval.Enabled = false;
            }

                // Hours per client: 
            else if (cStatType.SelectedIndex == 5)
            {
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = false;
                nUInterval.Enabled = false;
            }

            // Income per month!!
            else if (cStatType.SelectedIndex == 6)
            {
                //Populate_stat_filter_comboboxes();
               // gBFilter.Visible = true;
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = true;
                nUInterval.Enabled = true;
            }

            // Hours per month
            else if (cStatType.SelectedIndex == 7)
            {
          
                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = true;
                nUInterval.Enabled = true;
            }

               // KM per month
            else if (cStatType.SelectedIndex == 8)
            {

                gBStatFilter.Visible = false;
                bApplyInterval.Enabled = true;
                nUInterval.Enabled = true;
            }

                 // Income per month per filter
            else if (cStatType.SelectedIndex == 9)
            {
                gBStatFilter.Visible = true;
                rStatProject.Checked = true;
                bApplyInterval.Enabled = true;
                nUInterval.Enabled = true;
            }

                 // Hours per month per filter
            else if (cStatType.SelectedIndex == 10)
            {
                gBStatFilter.Visible = true;
                rStatProject.Checked = true;
                bApplyInterval.Enabled = true;
                nUInterval.Enabled = true;
            }

                   // KM per month per filter
            else if (cStatType.SelectedIndex == 11)
            {
                gBStatFilter.Visible = true;
                rStatProject.Checked = true;
                bApplyInterval.Enabled = true;
                nUInterval.Enabled = true;
            }



         

        }

        private void bTest_Click(object sender, EventArgs e)
        {
            // Incomer per project
            if (cStatType.SelectedIndex == 0)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.income_per_project(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, false);
                pGraph.Controls.Add(Chart1);
            }
            // Hours per project:
            else if (cStatType.SelectedIndex == 1)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.income_per_project(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, true);
                pGraph.Controls.Add(Chart1);

            }

            // Incomer per activity:
            else if (cStatType.SelectedIndex == 2)
            {
                gBStatFilter.Visible = false;
                 Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.income_per_activity(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, false);
                pGraph.Controls.Add(Chart1);
               

            }
            
            // Hours per activity: 
            else if (cStatType.SelectedIndex == 3)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.income_per_activity(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, true);
                pGraph.Controls.Add(Chart1);



            }

            // Income per client
            else if (cStatType.SelectedIndex == 4)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.income_per_client(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, false);
                pGraph.Controls.Add(Chart1);


            }

                // Hours per client: 
            else if (cStatType.SelectedIndex == 5)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.income_per_client(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, true);
                pGraph.Controls.Add(Chart1);


            }
            
            // Income per month!!
            else if (cStatType.SelectedIndex == 6)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.month_income(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "income");
                pGraph.Controls.Add(Chart1);


            }

            // Hours per month:
            else if (cStatType.SelectedIndex == 7)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.month_income(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "hours");
                pGraph.Controls.Add(Chart1);


            }

                     // KM per month:
            else if (cStatType.SelectedIndex == 8)
            {
                gBStatFilter.Visible = false;
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.month_income(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "kilometer");
                pGraph.Controls.Add(Chart1);


            }

            // Income per filter:
            else if (cStatType.SelectedIndex == 9)
            {         
            //Populate_stat_filter_comboboxes();
            gBStatFilter.Visible = true;
            if (rStatProject.Checked == true)
            {
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.month_income_project(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "income", cStatProject.Text);
                pGraph.Controls.Add(Chart1);

            }
            else if (rStatActivity.Checked == true)
            {

                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.month_income_activity(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "income", cStatActivity.Text);
                pGraph.Controls.Add(Chart1);
            }
            else if (rStatClient.Checked == true)
            {
                Create_DataTable_Linq_Query();
                pGraph.Controls.Clear();
                Chart1 = new Chart();
                Chart1 = _ChartController.month_income_client(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "income", cStatClient.Text);
                pGraph.Controls.Add(Chart1);

            }


            }

            // Hours per month per filter:
            else if (cStatType.SelectedIndex == 10)
            {
                //Populate_stat_filter_comboboxes();
                gBStatFilter.Visible = true;
                if (rStatProject.Checked == true)
                {
                    Create_DataTable_Linq_Query();
                    pGraph.Controls.Clear();
                    Chart1 = new Chart();
                    Chart1 = _ChartController.month_income_project(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "hours", cStatProject.Text);
                    pGraph.Controls.Add(Chart1);

                }
                else if (rStatActivity.Checked == true)
                {

                    Create_DataTable_Linq_Query();
                    pGraph.Controls.Clear();
                    Chart1 = new Chart();
                    Chart1 = _ChartController.month_income_activity(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "hours", cStatActivity.Text);
                    pGraph.Controls.Add(Chart1);
                }
                else if (rStatClient.Checked == true)
                {
                    Create_DataTable_Linq_Query();
                    pGraph.Controls.Clear();
                    Chart1 = new Chart();
                    Chart1 = _ChartController.month_income_client(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "hours", cStatClient.Text);
                    pGraph.Controls.Add(Chart1);

                }
            }

               // KM per month per filter:
            else if (cStatType.SelectedIndex == 11)
            {
                //Populate_stat_filter_comboboxes();
                gBStatFilter.Visible = true;
                if (rStatProject.Checked == true)
                {
                    Create_DataTable_Linq_Query();
                    pGraph.Controls.Clear();
                    Chart1 = new Chart();
                    Chart1 = _ChartController.month_income_project(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "kilometer", cStatProject.Text);
                    pGraph.Controls.Add(Chart1);

                }
                else if (rStatActivity.Checked == true)
                {

                    Create_DataTable_Linq_Query();
                    pGraph.Controls.Clear();
                    Chart1 = new Chart();
                    Chart1 = _ChartController.month_income_activity(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "kilometer", cStatActivity.Text);
                    pGraph.Controls.Add(Chart1);
                }
                else if (rStatClient.Checked == true)
                {
                    Create_DataTable_Linq_Query();
                    pGraph.Controls.Clear();
                    Chart1 = new Chart();
                    Chart1 = _ChartController.month_income_client(dt_Linq_Query, _items, pGraph.Width, pGraph.Height, DPstatStartDate.Value.Date, DPstatEndDate.Value.Date, "kilometer", cStatClient.Text);
                    pGraph.Controls.Add(Chart1);

                }
            }
        }


        private void Populate_stat_filter_comboboxes()
        {
            if (_items.Count > 0)
            {

                // Get the unique project/activity/client names:
                foreach (CalendarItem item in _items)
                {
                    string _projects = item._project;
                    _projects = _projects.ToLower();



                    if (!cStatProject.Items.Contains(_projects))
                    {
                        if (_projects.Length > 0)
                        {
                            cStatProject.Items.Add(_projects);
                        }
                    }

                    string _clients = item._client;
                    _clients = _clients.ToLower();

                    if (!cStatClient.Items.Contains(_clients))
                    {
                        if (_clients.Length > 0)
                        {
                            cStatClient.Items.Add(_clients);
                        }
                    }

                    string _activities = item._activity;
                    _activities = _activities.ToLower();

                    if (!cStatActivity.Items.Contains(_activities))
                    {
                        if (_activities.Length > 0)
                        {
                            cStatActivity.Items.Add(_activities);
                        }
                    }
                }

                if (cStatProject.Items.Count > 0)
                {
                    cStatProject.SelectedIndex = 0;
                }
                if (cStatClient.Items.Count > 0)
                {
                    cStatClient.SelectedIndex = 0;
                }
                if (cStatActivity.Items.Count > 0)
                {

                    cStatActivity.SelectedIndex = 0;
                }
            }

        }

        private void bApplyInterval_Click(object sender, EventArgs e)
        {
            Chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, Convert.ToDouble(nUInterval.Value));
        }

        // Physics cloud: ---------------------------------------------------------------------------------------------------------------------------------------------


        private void tStrRelSaveAsJPEG_Click(object sender, EventArgs e)
        {
            string myfile = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = startuppath;
            saveFileDialog1.Filter = "xml files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;


            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {

                    Bitmap bmp = new Bitmap(pPhysicsCloud.Width, pPhysicsCloud.Height);
                    pPhysicsCloud.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(saveFileDialog1.FileName.ToString() + ".Jpeg", ImageFormat.Jpeg);

                }
                catch (Exception e2)
                {
                    MessageBox.Show("An error occurred: '{0}':  " + e2);
                }

            }
        }


        private void bRelShow_Click(object sender, EventArgs e)
        {
            timerGame.Enabled = true;
            create_physics_worldcloud(false);
        }


        public void create_physics_worldcloud(Boolean enlarge)
        {
            //try
            //{
            Create_DataTable_Linq_Query();

            var query = from r in dt_Linq_Query.AsEnumerable()
                        where r.Field<DateTime>("endtime").Date >= dPRelStartdate.Value.Date &&
                              r.Field<DateTime>("endtime").Date <= dPRelEnddate.Value.Date
                        select r;

            //if (checkProject.Checked == true && checkActivity.Checked == true && checkClient.Checked == true)
            //{
            //query =  query.Where(r.Field<string>("project") == cBProjects.Text; 

            if (checkRelProject.Checked == true)
            {
                query = query.Where(p => String.Equals(p.Field<string>("project"), cBRelProjects.Text, StringComparison.CurrentCultureIgnoreCase));
                //query = query.Where(p => p.Field<string>("project") == cBProjects.Text);
            }
            if (checkRelClient.Checked == true)
            {
                // query = query.Where(p => p.Field<string>("client") == cBClients.Text);
                query = query.Where(p => String.Equals(p.Field<string>("client"), cBRelClient.Text, StringComparison.CurrentCultureIgnoreCase));

                //String.Equals(s, "Foo", StringComparison.CurrentCultureIgnoreCase));
            }
            if (checkRelActivity.Checked == true)
            {
                query = query.Where(p => String.Equals(p.Field<string>("activity"), cBRelActivity.Text, StringComparison.CurrentCultureIgnoreCase));
                //query = query.Where(p => p.Field<string>("activity") == cBActivity.Text);
            }

            //var   query = from r in dt_Linq_Query.AsEnumerable()
            //       where
            //             checkClient.Checked == true ? r.Field<string>("project") == cBProjects.Text &&
            //              //r.Field<string>("project") == checkProject.Checked == true ? : cBProjects.Text &&
            //             checkClient.Checked == true ? r.Field<string>("client") == cBClients.Text  &&
            //             checkClient.Checked == true ? r.Field<string>("activity") == cBActivity.Text  && 

            //             r.Field<DateTime>("endtime") >= DateTimeStart.Value.Date &&
            //             r.Field<DateTime>("endtime") <= DateTimeEnd.Value.Date                        
            //       select r;

            //}

            DataTable newDT = new DataTable();


            //convertDataTableToString(newDT);




            foreach (var v in query)
            {
                newDT = query.CopyToDataTable<DataRow>();
                break;
            }




            APEngine.particles.Clear();

            paintQueue.Clear();
            double gravityY = 4;
            double gravityX = 0;
            int Wallwidth = 30;
            //double wheeldiameter = 25;


            //Wheeldiameter = 0.025 * this.Height;
            // set up the events, main loop handler, and the engine. you don't have to use
            // enterframe. you just need to call the ApeEngine.step() method wherever
            // and however your handling your program cycle.
            // the argument here is the deltaTime value. Higher values result in faster simulations.
            double timestep = 0.33;


            APEngine.init(timestep, pPhysicsCloud.Width, pPhysicsCloud.Height);
            // SELECTIVE is better for dealing with lots of little particles colliding, 
            // as in the little rects and circles in this example
            APEngine.setCollisionResponseMode(APEngine.SELECTIVE);

            // gravity -- particles of varying masses are affected the same
            APEngine.addMasslessForce(new Vector(gravityX, gravityY));



            double scalefactor = 1;
            double max = 10;
            double min = 0;
            scalefactor = Convert.ToDouble(nUScaleFactor.Value);

            if (enlarge == true)
            {

                scalefactor = scalefactor * 1.2;
            }
            else
            {

            }


            RectangleParticle Top = new RectangleParticle(0.01 * pPhysicsCloud.Width, 0.01 * pPhysicsCloud.Height, 2 * pPhysicsCloud.Width, Wallwidth, 0, true, 100000000, 0.3, 0.2, Color.White,
            "");
            APEngine.addParticle(Top);


            RectangleParticle Bottom = new RectangleParticle(0.01 * pPhysicsCloud.Width, 0.99 * pPhysicsCloud.Height, 2 * pPhysicsCloud.Width, Wallwidth, 0, true, 100000000, 0.3, 0.2, Color.Black,
            "");
            APEngine.addParticle(Bottom);

            RectangleParticle right = new RectangleParticle(0.99 * pPhysicsCloud.Width, 0.01 * pPhysicsCloud.Height, Wallwidth, 2 * pPhysicsCloud.Width, 0, true, 100000000, 0.3, 0.2, Color.White,
           "");
            APEngine.addParticle(right);


            // RectangleParticle right = new RectangleParticle(pGraphics.Width -  Wallwidth, pGraphics.Height, Wallwidth, pGraphics.Height, 0, true, 100000000, 0.3, 0.2, Color.Black,
            //"Top");
            // APEngine.addParticle(right);

            RectangleParticle left = new RectangleParticle(0.01 * pPhysicsCloud.Width, 0.01 * pPhysicsCloud.Height, Wallwidth, 2 * pPhysicsCloud.Width, 0, true, 100000000, 0.3, 0.2, Color.White,
           "");
            APEngine.addParticle(left);

            //  RectangleParticle left = new RectangleParticle(0 + Wallwidth, 0, Wallwidth, 2 * pGraphics.Width, 0, true, 100000000, 0.3, 0.2, Color.Black,
            //"Top");
            //  APEngine.addParticle(left);


            int i = 1;


            Color Objectcolor = new Color();
            Double Radius = 0;

            //if (newDT.Rows.Count < 500)
            //{
                foreach (DataRow dtrow in newDT.Rows)
                {
                    //sb.AppendLine((string)dtrow["title"] + "  "  + (string)(dtrow["price"]) + "   " + (string)(dtrow["minutes"]));

                    double minutes = Convert.ToDouble((string)(dtrow["minutes"]));
                    double hour = minutes / 60;
                    double price = Convert.ToDouble((string)(dtrow["price"]));

                    double kil = Convert.ToDouble((string)(dtrow["kil"]));
                    double kilprice = Convert.ToDouble((string)(dtrow["kilprice"]));
                    double totalkil = 0;
                    totalkil = kil * kilprice;


                    double variouscosts = Convert.ToDouble((string)(dtrow["variouscosts"]));
                    string title = "";
                    // Objectcolor = item.BackgroundColor;
                    //Objectcolor = (string)dtrow["color"];
                    int _ARGB = Convert.ToInt32((string)dtrow["color"]);

                    Objectcolor = Color.FromArgb(_ARGB);

                    // Hours
                    if (cRelationType.SelectedIndex == 0)
                    {
                        Radius = scalefactor * hour;

                    }
                    // Income:
                    else if (cRelationType.SelectedIndex == 1)
                    {

                        Radius = scalefactor * (((hour * price) / 100) + (variouscosts / 100) + (totalkil / 100));
                    }


                    // Title:
                    if (cBRelText.SelectedIndex == 0)
                    {
                        title = (string)dtrow["title"].ToString();

                    }
                    // Project:
                    else if (cBRelText.SelectedIndex == 1)
                    {

                        title = (string)dtrow["project"].ToString();
                    }

                     // Client
                    else if (cBRelText.SelectedIndex == 2)
                    {

                        title = (string)dtrow["client"].ToString();
                    }
                    // Activity
                    else if (cBRelText.SelectedIndex == 3)
                    {

                        title = (string)dtrow["activity"].ToString();
                    }
                    // Hours
                    else if (cBRelText.SelectedIndex == 4)
                    {

                        title = Convert.ToString(Convert.ToDouble((string)dtrow["minutes"].ToString()) / 60);
                    }
                    // Income
                    else if (cBRelText.SelectedIndex == 5)
                    {

                        double totalincome = (Convert.ToDouble((string)dtrow["price"]) * (Convert.ToDouble((string)dtrow["minutes"].ToString()) / 60)) + (Convert.ToDouble((string)dtrow["variouscosts"]) + totalkil);
                        title = "€ " + totalincome.ToString();
                    }



                    if (PhysicsEngineSettings.objtype == "circle")
                    {
                        CircleParticle _CircelParticle = new CircleParticle(random.Next(100, pPhysicsCloud.Width - 100), random.Next(100, pPhysicsCloud.Height - 100), Radius, false, 1, PhysicsEngineSettings.Elasticity, PhysicsEngineSettings.Friction, Objectcolor, title);
                        APEngine.addParticle(_CircelParticle);
                    }
                    else if (PhysicsEngineSettings.objtype == "rectangle")
                    {
                        // RectangleParticle _RectangleParticle = new RectangleParticle(random.Next(100, pPhysicsCloud.Width - 100), random.Next(100, pPhysicsCloud.Height - 100), scalefactor * item.Duration.TotalMinutes, scalefactor * item.Duration.TotalMinutes, 0, false, 1, PhysicsEngineSettings.Elasticity, PhysicsEngineSettings.Friction, Objectcolor,
                        //(string)dtrow["title"].ToString());
                        //APEngine.addParticle(_RectangleParticle);
                    }

                    i = i + 1;
                }

                paintQueue = APEngine.getAll();
            }
            //else
            //{
            //    MessageBox.Show("To many results"
            //}
        

        private void pPhysicsCloud_Paint(object sender, PaintEventArgs e)
        {
            pPhysicsCloud.BackgroundImage = APEngine.bmp;
        }

        private void timerGame_Tick(object sender, EventArgs e)
        {
            updateWorld();
            paintWorld();
            //  Me.BackgroundImage = New Bitmap(600, 300)
            //  Me.BackgroundImage = APEngine.bmp
            pPhysicsCloud.Invalidate();
        }

        public void updateWorld()
        {
            APEngine.StepUp();
            //If Not rotatingRect Is Nothing Then rotatingRect.setRotation(rotatingRect.getRotation() + 0.03)
        }


        public void paintWorld()
        {
            for (int i = 0; i <= paintQueue.Count - 1; i++)
            {
                //'//TG TODO need to write code that determined the type of objects and sets their method. 
                // paintQueue.Item(i).paint()
                if ((paintQueue[i] is RectangleParticle))
                {
                    ((RectangleParticle)paintQueue[i]).paint();
                }
                else if (paintQueue[i] is CircleParticle)
                {
                    //DirectCast(paintQueue.Item(i), CircleParticle).paint()
                    CircleParticle Cirpart = (CircleParticle)paintQueue[i];

                    Cirpart.paint();
                    CircleParticle _with1 = (CircleParticle)paintQueue[i];
                    float r = 0;
                    r = Convert.ToInt64(_with1.getRadius());
                    //  e.Graphics.DrawEllipse(New Pen(Color.Red), CSng(.curr.x), CSng(.curr.y), r, r)
                }
                else if ((paintQueue[i] is SpringConstraint))
                {
                    ((SpringConstraint)paintQueue[i]).paint();
                    SpringConstraint springpart = (SpringConstraint)paintQueue[i];

                    springpart.paint();
                }
                else
                {
                    //paintQueue.Item(i).paint()
                }
            }
            // paintfps(g)
            //strategy.show()
            ////TG TODO not sure if I should be clearing the screen, otherwise the screen does not refesh properly, need to investigate the best approach.
            //g.clearRect(0,0,Stage.SCREEN_WIDTH,Stage.SCREEN_HEIGHT);
        }

        // CONTEXT information--------------------------------------------------------------------------------------------------------------------------------
        private void toolStripButtonHide_Click(object sender, EventArgs e)
        {
            pContextInfo.Visible = false;


        }

        private void toolStripButtonShow_Click(object sender, EventArgs e)
        {
            pContextInfo.Visible = true;
        }

     
     

      


     

      

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
      
        // -------------------------- --------------------------------------------------------------------------------------------------------------------------------------


    }
}