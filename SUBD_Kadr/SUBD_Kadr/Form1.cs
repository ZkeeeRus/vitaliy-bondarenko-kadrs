using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUBD_Kadr
{
    public partial class Form1 : Form
    {
        public static string txtCon = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Natar_Cadr.mdf;Integrated Security = True; Connect Timeout = 30";
        public Form1()
        {
            InitializeComponent();
        }
        struct Agent
        {
            public string id, fam, Name, Otch, pol, role, phone;
            public Image img;
        }
        List<Agent> lstCADR = new List<Agent>();
        ItemPanel CurrentItem;
        string uslovie = "";

        void GetData()
        {
            lstCADR.Clear();
            uslovie = "";
            if (cbFinde.Text == "Фамилия" && tbp.Text != "")
                uslovie += string.Format(" and Fam LIKE '{0}%' ", tbp.Text);
            else if (cbFinde.Text == "Имя")
                uslovie += string.Format(" and Name Like '{0}%' ", tbp.Text);
            else if (cbFinde.Text == "Отчество")
                uslovie += string.Format(" and OTch Like '{0}%' ", tbp.Text);
            else if (cbFinde.Text == "Телефон")
                uslovie += string.Format(" and phone LIKE '{0}%' ", tbp.Text);

            if (cbPol.Text != "Все")
            {
                if (cbPol.Text == "Мужчины")
                    uslovie += " and OTch like '%ч' ";
                if (cbPol.Text == "Женщины")
                    uslovie += " and OTch like '%а' ";
            }

            SqlConnection con = new SqlConnection(txtCon);
            con.Open();
            string txt = $@"select ID, Fam, Name, Otch,Pol,Photo,Doljnosti,Name_dol,phone
from Doljnosti,Sotrudnik 
where Sotrudnik.Doljnosti=Doljnosti.Id_doljnost {uslovie}";
            SqlCommand com = new SqlCommand(txt, con);
            SqlDataReader rez = com.ExecuteReader();
            while (rez.Read())
            {
                Agent agent1 = new Agent();
                agent1.id = rez["ID"].ToString();
                agent1.fam = rez["Fam"].ToString();
                agent1.Name = rez["Name"].ToString();
                agent1.Otch = rez["OTch"].ToString();
                agent1.pol = rez["Pol"].ToString();
                agent1.role = rez["Name_dol"].ToString();
                agent1.phone = rez["phone"].ToString();

                byte[] masphoto = (byte[])rez["photo"];
                MemoryStream msPhoto = new MemoryStream(masphoto);

                agent1.img = Image.FromStream(msPhoto);


                lstCADR.Add(agent1);
            }
            con.Close();

        }
        void FillPanel()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (Agent agent1 in lstCADR)
            {
                ItemPanel item = new ItemPanel();
                item.lFam.Text = agent1.fam;
                item.lid.Text = agent1.id;
                item.lName.Text = agent1.Name;
                item.lOtch.Text = agent1.Otch;
                item.lPhone.Text = agent1.phone;
                item.lPol.Text = agent1.pol;
                item.lRole.Text = agent1.role;
                item.pbImage.Image = agent1.img;

                flowLayoutPanel1.Controls.Add(item);


            }
        }
        class ItemPanel : Panel
        {
            //public System.Windows.Forms.Panel panel1;
            public System.Windows.Forms.Label lRole;
            public System.Windows.Forms.Label lAdr;
            public System.Windows.Forms.Label lOtch;
            public System.Windows.Forms.Label lName;
            public System.Windows.Forms.Label lFam;
            public System.Windows.Forms.PictureBox pbImage;
            public System.Windows.Forms.Label lPol;
            public System.Windows.Forms.Label lid;
            public System.Windows.Forms.Label lPhone;

            public ItemPanel()
            {
                //this.panel1 = new System.Windows.Forms.Panel();
                this.lid = new System.Windows.Forms.Label();
                this.lPol = new System.Windows.Forms.Label();
                this.lRole = new System.Windows.Forms.Label();
                this.lAdr = new System.Windows.Forms.Label();
                this.lOtch = new System.Windows.Forms.Label();
                this.lName = new System.Windows.Forms.Label();
                this.lFam = new System.Windows.Forms.Label();
                this.lPhone = new System.Windows.Forms.Label();
                this.pbImage = new System.Windows.Forms.PictureBox();

                // 
                // panel1
                // 
                this.BackColor = System.Drawing.Color.FromArgb(153, 189, 220);
                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.Controls.Add(this.lid);
                this.Controls.Add(this.lPol);
                this.Controls.Add(this.lRole);
                this.Controls.Add(this.lAdr);
                this.Controls.Add(this.lOtch);
                this.Controls.Add(this.lName);
                this.Controls.Add(this.lFam);
                this.Controls.Add(this.lPhone);
                this.Controls.Add(this.pbImage);
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
                this.Location = new System.Drawing.Point(15, 178);
                this.Name = "panel1";
                this.Size = new System.Drawing.Size(800, 200);
                this.TabIndex = 0;
                // 
                // lid
                // 
                this.lid.AutoSize = true;
                this.lid.Location = new System.Drawing.Point(22, 188);
                this.lid.Name = "lid";
                this.lid.Size = new System.Drawing.Size(17, 17);
                this.lid.TabIndex = 7;
                this.lid.Text = "a";
                this.lid.Visible = false;
                // 
                // lPol
                // 
                this.lPol.AutoSize = true;
                this.lPol.Location = new System.Drawing.Point(200, 168);
                this.lPol.Name = "lPol";
                this.lPol.Size = new System.Drawing.Size(62, 17);
                this.lPol.TabIndex = 6;
                this.lPol.Text = "label1";
                // 
                // lRole
                // 
                this.lRole.AutoSize = true;
                this.lRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lRole.Location = new System.Drawing.Point(200, 140);
                this.lRole.Name = "lRole";
                this.lRole.Size = new System.Drawing.Size(62, 17);
                this.lRole.TabIndex = 5;
                this.lRole.Text = "label1";

                // 
                // lOtch
                // 
                this.lOtch.AutoSize = true;
                this.lOtch.Font = new System.Drawing.Font("CMicrosoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lOtch.Location = new System.Drawing.Point(200, 84);
                this.lOtch.Name = "lOTch";
                this.lOtch.Size = new System.Drawing.Size(62, 17);
                this.lOtch.TabIndex = 3;
                this.lOtch.Text = "label3";
                // 
                // lName
                // 
                this.lName.AutoSize = true;
                this.lName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lName.Location = new System.Drawing.Point(200, 56);
                this.lName.Name = "lName";
                this.lName.Size = new System.Drawing.Size(62, 17);
                this.lName.TabIndex = 2;
                this.lName.Text = "label2";
                // 
                // lFam
                // 
                this.lFam.AutoSize = true;
                this.lFam.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lFam.Location = new System.Drawing.Point(200, 28);
                this.lFam.Name = "lFam";
                this.lFam.Size = new System.Drawing.Size(62, 17);
                this.lFam.TabIndex = 1;
                this.lFam.Text = "label1";
                // 
                // lPhone
                // 
                this.lPhone.AutoSize = true;
                this.lPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lPhone.Location = new System.Drawing.Point(200, 115);
                this.lPhone.Name = "lPhone";
                this.lPhone.Size = new System.Drawing.Size(62, 17);
                this.lPhone.TabIndex = 1;
                this.lPhone.Text = "label1";
                // 
                // pbImage
                // 
                this.pbImage.Location = new System.Drawing.Point(25, 28);
                this.pbImage.Name = "pbImage";
                this.pbImage.Size = new System.Drawing.Size(160, 160);
                this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.pbImage.TabIndex = 0;
                this.pbImage.TabStop = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetData();
            FillPanel();
        }

        private void cbFinde_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbFinde_TextChanged(object sender, EventArgs e)
        {
            GetData();
            FillPanel();
        }

        private void tbp_TextChanged(object sender, EventArgs e)
        {
            GetData();
            FillPanel();
        }

        private void cbPol_TextChanged(object sender, EventArgs e)
        {
            GetData();
            FillPanel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormADD f = new FormADD();
            f.Show();
        }
    }
}
