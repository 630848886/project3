using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RESTUtil;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace P3starter
{
    public partial class Form1 : Form
    {
        RestUtil rest = null;       // Reference to the REST DLL we wrote

        Footer footer = null;

        public Form1()
        {
            InitializeComponent();
            rest = new RestUtil("http://ist.rit.edu/api");

            Populate();

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            footerLabel.Text = "title:\ntweet:\nby:\ntwitter:\nfacebook:\n";
            // If we have not loaded the footer data, load it now (only done once)
            if (footer == null)
            {
                // get degrees data from the API
                string json = rest.getRESTData("/footer/");

                // Cast to the degrees object
                footer = JsonConvert.DeserializeObject<Footer>(json);
            }
            label8.Text = footer.social.title + "\n" + footer.social.tweet + "\n" + footer.social.by;
            linkLabel1.Text = footer.social.twitter;
            linkLabel2.Text = footer.social.facebook;
        }

        public void Populate()
        {
            // Get About from the API
            string jsonAbout = rest.getRESTData("/about/");

            // write it out for debug
            Console.WriteLine(jsonAbout);

            // Convert the JSON to a C# object
            About about = JToken.Parse(jsonAbout).ToObject<About>();

            // title
            tb_title.Text = about.title;

            // description
            tb_aboutDesc.Text = about.description;

            // Quote
            tb_aboutQuote.Text = about.quote;

            // Quote Author
            lbl_quoteAuthor.Text = about.quoteAuthor;

            // ===============================================
            // Get resources for the link
            string jsonRes = rest.getRESTData("/resources/");
            Resources resources = JToken.Parse(jsonRes).ToObject<Resources>();
            llb_resourcesLink.Text = resources.coopEnrollment.RITJobZoneGuidelink;



        }   // end Populate       
        

        People people = null;
        private void btn_people_Click(object sender, EventArgs e)
        {
            // ===============================================
            // Get people information

            string jsonPeople = rest.getRESTData("/people/");

            // Cast to a People object
            people = JToken.Parse(jsonPeople).ToObject<People>();

            peopleComboBox.Items.Clear();
            //Console.WriteLine(jsonPeople);

            foreach (Faculty thisfac in people.faculty)
            {
                peopleComboBox.Items.Add(thisfac.name + "(faculty)");
            }

            foreach (Staff thisstaff in people.staff)
            {
                peopleComboBox.Items.Add(thisstaff.name + "(staff)");
            }

            if (peopleComboBox.Items.Count > 0)
            {
                peopleComboBox.SelectedIndex = 0;
                findPeopleImage();
            }                
        }

        private void findPeopleImage()
        {
            foreach (Faculty thisfac in people.faculty)
            {
                if (thisfac.name + "(faculty)" == peopleComboBox.GetItemText(peopleComboBox.SelectedItem))
                {
                    pictureBox1.Load(thisfac.imagePath);
                    label9.Text = "Phone: " + thisfac.phone;
                    label10.Text = "Email: " + thisfac.email;
                    return;
                }
            }

            foreach (Staff thisstaff in people.staff)
            {
                if (thisstaff.name + "(staff)" == peopleComboBox.GetItemText(peopleComboBox.SelectedItem))
                {
                    pictureBox1.Load(thisstaff.imagePath);
                    label9.Text = "Phone: " + thisstaff.phone;
                    label10.Text = "Email: " + thisstaff.email;
                    return;
                }
            }
        }

        
        private void llb_resourcesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(llb_resourcesLink.Text);
            llb_resourcesLink.LinkVisited = true;

        }

        // =========================================================

        // Create an attribute to hold the Employment information
        Employment emp = null;

        private void loadEmploymentData()
        {
            // If we have not loaded the employment data, load it now (only done once)
            if(emp == null)
            {
                // get employment data from the API
                string jsonEmp = rest.getRESTData("/employment/");

                // Cast to the employment object
                emp = JToken.Parse(jsonEmp).ToObject<Employment>();
            }
        } // end loadEmploymentData

        private void btn_LoadDataView_Click(object sender, EventArgs e)
        {
            loadEmploymentData();

            for(var i=0; i<emp.coopTable.coopInformation.Count; i++)
            {
                dataGridView1.Rows.Add();   // add a row before adding data

                dataGridView1.Rows[i].Cells[0].Value =
                    emp.coopTable.coopInformation[i].employer;
                dataGridView1.Rows[i].Cells[1].Value =
                    emp.coopTable.coopInformation[i].degree;
                dataGridView1.Rows[i].Cells[2].Value =
                    emp.coopTable.coopInformation[i].city;
                dataGridView1.Rows[i].Cells[3].Value =
                    emp.coopTable.coopInformation[i].term;
            }

        }   // end btn_LoadDataView_Click

        private void btn_LoadListView_Click(object sender, EventArgs e)
        {
            loadEmploymentData();

            listView1.Clear();
            listView1.View = View.Details;  // Each item appears on a separate line

            listView1.Width = 500;          // set the width of the display
            listView1.Columns.Add("Employer", 130);
            listView1.Columns.Add("Degree", 80);
            listView1.Columns.Add("City", 100);
            listView1.Columns.Add("Term", 80);

            // Add information from the Employment class
            ListViewItem item;
            for(var i=0; i<emp.coopTable.coopInformation.Count; i++)
            {
                item = new ListViewItem(new string[] {
                    emp.coopTable.coopInformation[i].employer,
                    emp.coopTable.coopInformation[i].degree,
                    emp.coopTable.coopInformation[i].city,
                    emp.coopTable.coopInformation[i].term
                }  );

                listView1.Items.Add(item);

            } // end for
        }

        List<Course> courses = null;
        private void btn_LoadCourse_Click(object sender, EventArgs e)
        {
            // If we have not loaded the course data, load it now (only done once)
            if (courses == null)
            {
                courses = new List<Course>();

                // get course data from the API
                string json = rest.getRESTData("/course/");

                // Cast to the course object
                courses = JsonConvert.DeserializeObject<List<Course>>(json);
            }

            courseDataGridView.Rows.Clear();

            int i = 0;
            foreach (var c in courses)
            {
                courseDataGridView.Rows.Add();   // add a row before adding data

                courseDataGridView.Rows[i].Cells[0].Value = c.courseID;
                courseDataGridView.Rows[i].Cells[1].Value = c.title;
                ++i;
            }
        }

        private void courseDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (courses != null)
            {
                MessageBox.Show(courses[courseDataGridView.CurrentCell.RowIndex].description);
            }   
        }

        News news = null;
        bool isOldFlag = true;

        private void btn_LoadNews_Click(object sender, EventArgs e)
        {
            // If we have not loaded the news data, load it now (only done once)
            if (news == null)
            {
                // get news data from the API
                string json = rest.getRESTData("/news/");

                // Cast to the news object
                news = JsonConvert.DeserializeObject<News>(json);
            }

            newsGridView.Rows.Clear();

            if (news.year == null || news.year.Count < 1)
            {
                MessageBox.Show("No data get!");
                return;
            }

            isOldFlag = false;
            int i = 0;
            foreach (var n in news.year)
            {
                newsGridView.Rows.Add();   // add a row before adding data

                newsGridView.Rows[i].Cells[0].Value = n.date;
                newsGridView.Rows[i].Cells[1].Value = n.title;
                ++i;
            }
        }

        private void btn_LoadOldNews_Click(object sender, EventArgs e)
        {
            // If we have not loaded the news data, load it now (only done once)
            if (news == null)
            {
                // get news data from the API
                string json = rest.getRESTData("/news/");

                // Cast to the news object
                news = JsonConvert.DeserializeObject<News>(json);
            }

            newsGridView.Rows.Clear();

            if (news.older == null || news.older.Count < 1)
            {
                MessageBox.Show("No data get!");
                return;
            }

            isOldFlag = true;
            int i = 0;
            foreach (var n in news.older)
            {
                newsGridView.Rows.Add();   // add a row before adding data

                newsGridView.Rows[i].Cells[0].Value = n.date;
                newsGridView.Rows[i].Cells[1].Value = n.title;
                ++i;
            }
        }

        private void newsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (news != null)
            {
                if (isOldFlag)
                {
                    MessageBox.Show(news.older[newsGridView.CurrentCell.RowIndex].description);
                }
                else
                {
                    MessageBox.Show(news.year[newsGridView.CurrentCell.RowIndex].description);
                }
            }
        }

        Researches researches = null;
        private void btn_LoadResearch_Click(object sender, EventArgs e)
        {
            // If we have not loaded the researches data, load it now (only done once)
            if (courses == null)
            {
                // get researches data from the API
                string json = rest.getRESTData("/research/");

                // Cast to the researches object
                researches = JsonConvert.DeserializeObject<Researches>(json);
            }

            researchByInterestAreaGridView.Rows.Clear();
            researchByFacultyGridView.Rows.Clear();

            int i = 0;
            foreach (var r in researches.byInterestArea)
            {
                researchByInterestAreaGridView.Rows.Add();   // add a row before adding data

                researchByInterestAreaGridView.Rows[i].Cells[0].Value = r.areaName;
                ++i;
            }

            i = 0;
            foreach (var r in researches.byFaculty)
            {
                researchByFacultyGridView.Rows.Add();   // add a row before adding data

                researchByFacultyGridView.Rows[i].Cells[0].Value = r.facultyName;
                researchByFacultyGridView.Rows[i].Cells[1].Value = r.username;
                ++i;
            }
        }

        private void researchByInterestAreaGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (researches.byInterestArea != null)
            {
                string output = string.Empty;
                foreach (var c in researches.byInterestArea[researchByInterestAreaGridView.CurrentCell.RowIndex].citations)
                {
                    output += c + "\n";
                }
                MessageBox.Show(output);
            }
        }

        private void researchByFacultyGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (researches.byFaculty != null)
            {
                string output = string.Empty;
                foreach (var c in researches.byFaculty[researchByFacultyGridView.CurrentCell.RowIndex].citations)
                {
                    output += c + "\n";
                }
                MessageBox.Show(output);
            }
        }

        Degrees degrees = null;
        private void btn_LoadDegree_Click(object sender, EventArgs e)
        {
            // If we have not loaded the degrees data, load it now (only done once)
            if (degrees == null)
            {
                // get degrees data from the API
                string json = rest.getRESTData("/degrees/");

                // Cast to the degrees object
                degrees = JsonConvert.DeserializeObject<Degrees>(json);
            }

            undergraduateGridView.Rows.Clear();
            graduateGridView.Rows.Clear();

            int i = 0;
            foreach (var d in degrees.undergraduate)
            {
                undergraduateGridView.Rows.Add();   // add a row before adding data

                undergraduateGridView.Rows[i].Cells[0].Value = d.degreeName;
                undergraduateGridView.Rows[i].Cells[1].Value = d.title;
                ++i;
            }

            i = 0;
            foreach (var d in degrees.graduate)
            {
                graduateGridView.Rows.Add();   // add a row before adding data

                graduateGridView.Rows[i].Cells[0].Value = d.degreeName;
                graduateGridView.Rows[i].Cells[1].Value = d.title;
                ++i;
            }
        }

        private void undergraduateGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (degrees.undergraduate != null)
            {
                string output = degrees.undergraduate[undergraduateGridView.CurrentCell.RowIndex].description + "\n";
                foreach (var c in degrees.undergraduate[undergraduateGridView.CurrentCell.RowIndex].concentrations)
                {
                    output += c + "\n";
                }
                MessageBox.Show(output);
            }
        }

        private void graduateGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (degrees.graduate != null)
            {
                string output = degrees.graduate[graduateGridView.CurrentCell.RowIndex].description + "\n";
                foreach (var c in degrees.graduate[graduateGridView.CurrentCell.RowIndex].concentrations)
                {
                    output += c + "\n";
                }
                MessageBox.Show(output);
            }
        }

        Minors minors = null;
        private void btn_LoadMinor_Click(object sender, EventArgs e)
        {
            // If we have not loaded the minors data, load it now (only done once)
            if (minors == null)
            {
                // get minors data from the API
                string json = rest.getRESTData("/minors/");

                // Cast to the minors object
                minors = JsonConvert.DeserializeObject<Minors>(json);
            }

            minorGridView.Rows.Clear();

            int i = 0;
            foreach (var m in minors.UgMinors)
            {
                minorGridView.Rows.Add();   // add a row before adding data

                minorGridView.Rows[i].Cells[0].Value = m.name;
                minorGridView.Rows[i].Cells[1].Value = m.title;
                ++i;
            }
        }

        private void minorGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (minors.UgMinors != null)
            {
                string output = minors.UgMinors[minorGridView.CurrentCell.RowIndex].description + "\n";
                foreach (var c in minors.UgMinors[minorGridView.CurrentCell.RowIndex].courses)
                {
                    output += c + "\n";
                }
                MessageBox.Show(output);
            }
        }

        private void peopleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            findPeopleImage();
        }
    } // end class Form1
} // end namespace
