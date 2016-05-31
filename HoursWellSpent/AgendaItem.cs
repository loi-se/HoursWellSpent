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
using System.Reflection;

namespace HoursWellSpent
{
    public partial class AgendaItem : Form
    {

        public Color _Color;
        public string _ColorName = "";

        public AgendaItem()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void AgendaItem_Load(object sender, EventArgs e)
        {
            AgendaForm _AgendaForm = new AgendaForm();

            cProjects.Items.Clear();
            cClient.Items.Clear();
            cActivity.Items.Clear();

            NUhourprice.DecimalPlaces = 2;

            try
            {
                if (Global_variables._Gitems.Count > 0)
                {
                    foreach (CalendarItem item in Global_variables._Gitems)
                    {
                        string _projects = item._project;
                        _projects = _projects.ToLower();


                        if (!cProjects.Items.Contains(_projects))
                        {
                            if (_projects.Length > 0)
                            {
                                cProjects.Items.Add(_projects);
                            }
                        }


                        string _clients = item._client;
                        _clients = _clients.ToLower();

                        if (!cClient.Items.Contains(_clients))
                        {
                            if (_clients.Length > 0)
                            {
                                cClient.Items.Add(_clients);
                            }
                        }

                        string _activities = item._activity;
                        _activities = _activities.ToLower();

                        if (!cActivity.Items.Contains(_activities))
                        {
                            if (_activities.Length > 0)
                            {
                                cActivity.Items.Add(_activities);
                            }
                        }
                    }

                    //cActivity.SelectedIndex = 0;
                    //cClient.SelectedIndex = 0;
                    //c
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            _Color = Color.White;
            // CYAN:
            _ColorName = "-16711681";

            tAgendaText.Text = "--";

            if (Global_variables._GActiveCalendarItem == null)
            {

            }
            else
            {
                try
                {
                    cClient.Text = Global_variables._GActiveCalendarItem._client;
                    NUhourprice.Value = Convert.ToDecimal(Global_variables._GActiveCalendarItem._price);

                    nUKil.Value = Convert.ToDecimal(Global_variables._GActiveCalendarItem._Kil);
                    nUKilprice.Value = Convert.ToDecimal(Global_variables._GActiveCalendarItem._Kilprice);

                    nUVariousCosts.Value = Convert.ToDecimal(Global_variables._GActiveCalendarItem._variouscosts);
                    tAgendaText.Text = Global_variables._GActiveCalendarItem.Text;
                    bPickColor.BackColor = Global_variables._GActiveCalendarItem.BackgroundColor;
                    cProjects.Text = Global_variables._GActiveCalendarItem._project;
                    cActivity.Text = Global_variables._GActiveCalendarItem._activity;
                    tNote.Text = Global_variables._GActiveCalendarItem._note;
                    //_ColorHex = Global_variables._GActiveCalendarItem.BackgroundColor.Name;

                    //Global_variables._GActiveCalendarItem.Date.TimeOfDay

                    _Color = Global_variables._GActiveCalendarItem.BackgroundColor;
                    _ColorName = _Color.ToArgb().ToString();
                    // MessageBox.Show(_ColorName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        // When the user doesn't save anything the existing values are used:
        private void AgendaItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void bCancel_Click(object sender, EventArgs e)
        {

            //if (!Global_variables._GActiveCalendarItem.Equals(null))
            //{
            try
            {
                //Global_variables._GItemtext = Global_variables._GActiveCalendarItem._client;


                Global_variables._Gprice = Global_variables._GActiveCalendarItem._price;
                Global_variables._GKil = Global_variables._GActiveCalendarItem._Kil;
                Global_variables._GKilprice = Global_variables._GActiveCalendarItem._Kilprice;

                Global_variables._Gvariouscosts = Global_variables._GActiveCalendarItem._variouscosts;
                Global_variables._GItemtext = Global_variables._GActiveCalendarItem.Text;

                //bPickColor.BackColor = Global_variables._GActiveCalendarItem.BackgroundColor;
                Global_variables._GProject = Global_variables._GActiveCalendarItem._project;
                Global_variables._GActivity = Global_variables._GActiveCalendarItem._activity;
                Global_variables._Gclient = Global_variables._GActiveCalendarItem._client;
                Global_variables._Gnote = Global_variables._GActiveCalendarItem._note;
                //_ColorHex = Global_variables._GActiveCalendarItem.BackgroundColor.Name;

                _Color = Global_variables._GActiveCalendarItem.BackgroundColor;
                _ColorName = _Color.ToArgb().ToString();
                Global_variables._GItemcolor = _ColorName;
            }
            catch
            {
            }

            this.Close();
            //}
        }


        private void bAdd_Click(object sender, EventArgs e)
        {
           

            if (tAgendaText.Text.Length > 1 && tAgendaText.Text.Length < 100)
            {
                Global_variables._GItemtext = tAgendaText.Text;
                Global_variables._Gprice = Convert.ToDouble(NUhourprice.Value);

                Global_variables._GKil = Convert.ToDouble(nUKil.Value);
                Global_variables._GKilprice = Convert.ToDouble(nUKilprice.Value);

                Global_variables._Gvariouscosts = Convert.ToDouble(nUVariousCosts.Value);
                Global_variables._Gclient = cClient.Text;

                //_ColorName = _Color.ToKnownColor().ToString();
                //MessageBox.Show(_ColorName);

                Global_variables._GItemcolor = _ColorName;
                Global_variables._GProject = cProjects.Text;
                Global_variables._GActivity = cActivity.Text;
                Global_variables._Gnote = tNote.Text;

                this.Close();
            }
            else
            {
                MessageBox.Show("Title should be between 1 and 100 characters"); 
                Global_variables._GItemtext = "--";
            }
         

        }

     
        // NEEDS TO RETURN THE COLOR NAME AS A STRING:

        private void bPickColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();

            colorDlg.AllowFullOpen = false;
            colorDlg.AnyColor = true;

            colorDlg.SolidColorOnly = false;

            colorDlg.Color = Color.Red;



            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDlg.Color;
                _ColorName = c.ToArgb().ToString();
                bPickColor.BackColor = colorDlg.Color;
            }
        }

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }


        string GetColorName(Color color)
        {
            var colorProperties = typeof(Color)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(Color));
            foreach (var colorProperty in colorProperties)
            {
                var colorPropertyValue = (Color)colorProperty.GetValue(null, null);
                if (colorPropertyValue.R == color.R
                       && colorPropertyValue.G == color.G
                       && colorPropertyValue.B == color.B)
                {
                    return colorPropertyValue.Name;
                    //colorPropertyValue.ToArgb();

                }
            }

            //If unknown color, fallback to the hex value
            //(or you could return null, "Unkown" or whatever you want)

            MessageBox.Show(" This color is not supported yet");
            //return ColorTranslator.ToHtml(color);
            return "notsupported";
            
        }

   
        


    }
}
