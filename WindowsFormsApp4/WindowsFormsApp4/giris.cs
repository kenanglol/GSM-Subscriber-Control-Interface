using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class giris : Form
    {
        public giris()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9VF93U9\\KENANSQL;Initial Catalog=GSM;Integrated Security=True");

        private void sign_up_Click(object sender, EventArgs e)
        {
            kayıt kayıtol = new kayıt();
            kayıtol.Show();
        }

        private void sign_in_Click(object sender, EventArgs e)
        {
            SqlCommand isExist = new SqlCommand();
            isExist.CommandText = "SELECT * FROM Kullanıcı WHERE kullanıcı_adı='" + UNametextBox.Text + "' AND sifre='" + PasstextBox.Text + "';";
            isExist.Connection = conn;
            if (conn.State==ConnectionState.Closed)
            {
                conn.Open();
            }
            if (isExist.ExecuteScalar()!=null)
            {
                SqlDataAdapter adap = new SqlDataAdapter(isExist);
                System.Data.DataTable tablo = new System.Data.DataTable();
                adap.Fill(tablo);
                int adm=(int)tablo.Rows[0][2];
                gsm_form yeni = new gsm_form(this,UNametextBox.Text,adm);
                yeni.Show();
                this.Hide() ;
            }
            else
            {
                UNametextBox.Clear();
                PasstextBox.Clear();
                MessageBox.Show("Kullanıcı Adı ya da Şifreniz Hatalı",
                "Giriş Hatası");
            }
        }
    }
}
