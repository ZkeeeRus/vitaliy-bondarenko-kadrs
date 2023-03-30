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
    public partial class FormADD : Form
    {
        public static string txtCon = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Natar_Cadr.mdf;Integrated Security = True; Connect Timeout = 30";
        public FormADD()
        {
            InitializeComponent();
        }
        List<string> lstId = new List<string>();

        byte[] NewPhoto;
        void GetRole()
        {
            lstId.Clear();
            cbDolj.Items.Clear();
            SqlConnection con = new SqlConnection(Form1.txtCon);
            con.Open();
            string txt = "select * from Doljnosti";
            SqlCommand com = new SqlCommand(txt, con);
            SqlDataReader rez = com.ExecuteReader();
            while (rez.Read())
            {
                lstId.Add(rez["Id_doljnost"].ToString());
                cbDolj.Items.Add(rez["Name_dol"].ToString());
            }
            con.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files(*.png; *.jpg; *jpeg; *.bmp)| *.png; *.jpg; *jpeg; *.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pbImage.Image = new Bitmap(openFileDialog1.FileName);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть!", "Внимание",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
 string zapr1 = "";
            string zapr2 = "";
        private void bOk_Click(object sender, EventArgs e)
        {

            if (tbFam.Text != "" && tbName.Text != "" && tbOtch.Text != "" && tbPol.Text != "" && tbPhone.Text != "" && cbDolj.Text != "" && tbEmail.Text != "")
            {
                SqlConnection con = new SqlConnection(txtCon);
                con.Open();
                string txt = $@"insert into Sotrudnik(Name, Fam, OTch, Pol,Email, phone, Doljnosti)
values (N'{tbName.Text}', N'{tbFam.Text}', N'{tbOtch.Text}',N'{tbPol.Text}',N'{tbEmail.Text}',N'{tbPhone.Text}',N'{lstId[cbDolj.SelectedIndex]}')";
                SqlCommand com = new SqlCommand(txt, con);
                com.ExecuteNonQuery();
                con.Close();
                string idnewuser = "";
                con.Open();
                txt = "select * from Sotrudnik";
                com = new SqlCommand(txt, con);
                SqlDataReader rez = com.ExecuteReader();
                while (rez.Read())
                {
                    idnewuser = rez["ID"].ToString();
                }
                con.Close();
                con.Open();
                MemoryStream stream = new MemoryStream();
                pbImage.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                NewPhoto = stream.ToArray();
                byte[] masImages = stream.ToArray();
                string txtqe = $@"update Sotrudnik
set photo=@newphoto
where ID={idnewuser}";
                zapr2 = txtqe;
                com = new SqlCommand(txtqe, con);
                com.Parameters.AddWithValue("@newphoto", NewPhoto);
                com.ExecuteNonQuery();
                con.Close();
                NewPhoto = null;
                MessageBox.Show("Выполено!", "Внимание",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show($"Запрос {zapr1}\nЗапрос2 {zapr2}");
                con.Close();
            }
            else
            {
                MessageBox.Show("вы не ввели все данные");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pbImage.Image = Properties.Resources.free_icon_empire_8467289;
        }

        private void FormADD_Load(object sender, EventArgs e)
        {
            GetRole();
        }
    }
}
