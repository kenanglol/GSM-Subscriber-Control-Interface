using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApp4
{
    public partial class gsm_form : Form
    {
        giris ana;
        List<String> tarife_list;
        public gsm_form(giris root,String user,int admin)
        {
            InitializeComponent();
            ana=root;
            username.Text = user;
            if (admin==0)
            {
                fatura_kes.Enabled = false;
                UpdateTarife.Enabled = false;
                Silbutton.Enabled = false;
                AddTarife.Enabled = false;
            }
            else
            {
                fatura_kes.Enabled = true;
                UpdateTarife.Enabled = true;
                Silbutton.Enabled = true;
                AddTarife.Enabled = true;
            }
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("En");
        }

        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-9VF93U9\\KENANSQL;Initial Catalog=GSM;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            connection.Open();
            refreshTableAbone();
            refreshTableTarife();
            refreshTableHat();
            refreshTableFatura();
            
        }


        private void refreshTableAbone()
        {
            SqlCommand comm = new SqlCommand("SELECT * FROM Abone", connection);
            SqlDataAdapter adap = new SqlDataAdapter(comm);
            System.Data.DataTable tablo = new System.Data.DataTable();
            adap.Fill(tablo);
            AboneView.DataSource = tablo;
        }
        private void refreshTableTarife()
        {
            SqlCommand comm = new SqlCommand("SELECT * FROM Tarife", connection);
            SqlDataAdapter adap = new SqlDataAdapter(comm);
            System.Data.DataTable tablo = new System.Data.DataTable();
            adap.Fill(tablo);
            TarifeView.DataSource = tablo;
        }
        private void refreshTableHat()
        {
            tarife_list = new List<string>();
            SqlCommand comm = new SqlCommand("SELECT * FROM Hat", connection);
            SqlDataAdapter adap = new SqlDataAdapter(comm);
            System.Data.DataTable tablo = new System.Data.DataTable();
            adap.Fill(tablo);
            HatView.DataSource = tablo;
            tarife_comboBox.Items.Clear();
            kampanya_update.Items.Clear();
            foreach (DataGridViewRow row in TarifeView.Rows)
            {
                tarife_comboBox.Items.Add(row.Cells[0].Value.ToString());
                kampanya_update.Items.Add(row.Cells[0].Value.ToString());
                tarife_list.Add(row.Cells[0].Value.ToString());
            }    
        }
        private void refreshTableFatura()
        {
            SqlCommand comm = new SqlCommand("SELECT * FROM Fatura", connection);
            SqlDataAdapter adap = new SqlDataAdapter(comm);
            System.Data.DataTable tablo = new System.Data.DataTable();
            adap.Fill(tablo);
            FaturaView.DataSource = tablo;
        }
        private void clear_abone()
        {
            FNametextBox.Clear();
            LNametextBox.Clear();
            AddresstextBox.Clear();
            AbonetextBox.Clear();
        }
        private void clear_tarife()
        {
            tarife_textBox.Clear();
            fiyat_numericUpDown.Value = 0;
            sms_numericUpDown.Value = 0;
            konusma_numericUpDown.Value = 0;
            internet_numericUpDown.Value = 0;
            fatura_checkBox.Checked = false;
        }
        private void clear_hat()
        {
            hatAra.Clear();
            sahip_textBox.Clear();
            numara_textBox.Clear();
            tarife_comboBox.SelectedItem=null;
        }
        private void clear_fatura()
        {
            fatura_textBox.Clear();
            fatura_odeme.Clear();
        }
        private void Addbutton_Click(object sender, EventArgs e)
        {
            if (FNametextBox.Text == "" || LNametextBox.Text == "" || AddresstextBox.Text == "")
            {
                MessageBox.Show("Lütfen kutucuları boş bırakmayınız.", "Eksik Veri Girişi");
            }
            else
            {
                SqlCommand Insert = new SqlCommand();
                Insert.CommandText = "INSERT INTO Abone VALUES ('" + FNametextBox.Text + "','" + LNametextBox.Text + "','" + AddresstextBox.Text + "')";
                Insert.Connection = connection;
                Insert.ExecuteNonQuery();
                refreshTableAbone();
                clear_abone();
            }
        }
        private void AboneView_SelectionChanged(object sender, EventArgs e)
        {
            if (AboneView.SelectedRows.Count != 0)
            {
                Updatename.Text = AboneView.SelectedRows[0].Cells[1].Value.ToString();
                Updatelast.Text = AboneView.SelectedRows[0].Cells[2].Value.ToString();
                Updateaddress.Text = AboneView.SelectedRows[0].Cells[3].Value.ToString();
            }
        }

        private void Updatebutton_Click(object sender, EventArgs e)
        {
            SqlCommand Update = new SqlCommand();
            Update.CommandText = "UPDATE Abone SET FName='"+Updatename.Text
                +"', LName='"+Updatelast.Text+"', Address='"+Updateaddress.Text+"' WHERE Abone_No=" 
                + AboneView.SelectedRows[0].Cells[0].Value.ToString() + ";";
            Update.Connection = connection;
            try
            {
                Update.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }


            refreshTableAbone();
            clear_abone();
        }

        private void Deletebutton_Click_1(object sender, EventArgs e)
        {
            SqlCommand Delete = new SqlCommand();
            Delete.CommandText = "DELETE FROM Abone WHERE Abone_No=" + AbonetextBox.Text + ";";
            Delete.Connection = connection;
            try
            {
                Delete.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }


            refreshTableAbone();
            clear_abone();
        }

        private void Searchbutton_Click_1(object sender, EventArgs e)
        {
            SqlCommand Search = new SqlCommand();
            Search.CommandText = "SELECT * FROM Abone WHERE Abone_No=" + AbonetextBox.Text + ";";
            Search.Connection = connection;
            SqlDataAdapter adap = new SqlDataAdapter(Search);
            System.Data.DataTable tablo = new System.Data.DataTable();
            try
            {
                adap.Fill(tablo);
                AboneView.DataSource = tablo;
                clear_abone();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
                refreshTableAbone();
            }
        }

        private void Silbutton_Click(object sender, EventArgs e)
        {
            SqlCommand Delete = new SqlCommand();
            Delete.CommandText = "DELETE FROM Tarife WHERE Tarife_Adı=" + tarife_bul.Text + ";";
            Delete.Connection = connection;
            try
            {
                Delete.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }
            refreshTableTarife();
            clear_tarife();
        }

        private void Arabutton_Click(object sender, EventArgs e)
        {
            SqlCommand Search = new SqlCommand();
            Search.CommandText = "SELECT * FROM Tarife WHERE Tarife_Adı='" + tarife_bul.Text + "';";
            Search.Connection = connection;
            SqlDataAdapter adap = new SqlDataAdapter(Search);
            System.Data.DataTable tablo = new System.Data.DataTable();
            try
            {
                adap.Fill(tablo);
                TarifeView.DataSource = tablo;
                clear_tarife();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
                refreshTableAbone();
            }
        }


        private void TarifeView_SelectionChanged(object sender, EventArgs e)
        {
            if (TarifeView.SelectedRows.Count != 0)
            {
                update_tarife.Text = TarifeView.SelectedRows[0].Cells[0].Value.ToString();
                if (Convert.ToInt32(TarifeView.SelectedRows[0].Cells[1].Value.ToString())==1)
                {
                    update_fatura.Checked = true;
                }
                else
                {
                    update_fatura.Checked = false;
                }
                update_fiyat.Value = int.Parse(TarifeView.SelectedRows[0].Cells[2].Value.ToString(), NumberStyles.Currency);
                update_konusma.Value = Convert.ToInt32(TarifeView.SelectedRows[0].Cells[3].Value.ToString());
                update_sms.Value = Convert.ToInt32(TarifeView.SelectedRows[0].Cells[4].Value.ToString());
                update_internet.Value = Convert.ToInt32(TarifeView.SelectedRows[0].Cells[5].Value.ToString());
            }
        }

        private void AddTarife_Click(object sender, EventArgs e)
        {
            if (tarife_textBox.Text == "" || fiyat_numericUpDown.Value == 0)
            {
                MessageBox.Show("Lütfen kutucuları boş bırakmayınız.", "Eksik Veri Girişi");
            }
            else
            {
                int fatura = 0;
                if (fatura_checkBox.Checked == true)
                {
                    fatura = 1;
                }
                SqlCommand Insert = new SqlCommand();
                Insert.CommandText = "INSERT INTO Tarife VALUES ('" + tarife_textBox.Text 
                    + "'," + fatura+","+fiyat_numericUpDown.Value+"," +
                    konusma_numericUpDown.Value + ","+sms_numericUpDown.Value+","+internet_numericUpDown.Value+")";
                Insert.Connection = connection;
                Insert.ExecuteNonQuery();
                refreshTableTarife();
                clear_tarife();
            }
        }

        private void UpdateTarife_Click(object sender, EventArgs e)
        {
            int faturalı = 0;
            if (fatura_checkBox.Checked==true)
            {
                faturalı = 1;
            }
            SqlCommand Update = new SqlCommand();
            Update.CommandText = "UPDATE Tarife SET Faturali=" + faturalı
                + ", Fiyat=" + update_fiyat.Value + ", Konusma=" 
                + update_konusma.Value + ", Sms="+ update_sms.Value + ", Internet="
                + update_internet.Value + " WHERE Tarife_Adı="
                + TarifeView.SelectedRows[0].Cells[0].Value.ToString() + ";";
            Update.Connection = connection;
            try
            {
                Update.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }
            refreshTableTarife();
            clear_tarife();
        }
        

        private void Hat_ekle_Click(object sender, EventArgs e)
        {
            SqlCommand Insert = new SqlCommand();
            Insert.CommandText = "INSERT INTO Hat VALUES (" + numara_textBox.Text+","+sahip_textBox.Text+",'"+tarife_comboBox.SelectedItem+ "')";
            Insert.Connection = connection;
            Insert.ExecuteNonQuery();
            refreshTableHat();
            clear_hat();
        }

        private void hat_degistir_Click(object sender, EventArgs e)
        {
            SqlCommand Update = new SqlCommand();
            Update.CommandText = "UPDATE Hat SET Hat_Sahibi=" +sahip_update.Text+
                ",Tarife='"+ tarife_list[kampanya_update.SelectedIndex] + "' WHERE Numara="
                + HatView.SelectedRows[0].Cells[0].Value.ToString() + ";";
            Update.Connection = connection;
            try
            {
                Update.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }
            refreshTableHat();
            clear_hat();
        }

        private void hat_sil_Click(object sender, EventArgs e)
        {
            SqlCommand Delete = new SqlCommand();
            Delete.CommandText = "DELETE FROM Hat WHERE Numara=" + hatAra.Text + ";";
            Delete.Connection = connection;
            try
            {
                Delete.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }
            refreshTableHat();
            clear_hat();
        }

        private void hat_ara_Click(object sender, EventArgs e)
        {
            SqlCommand Search = new SqlCommand();
            Search.CommandText = "SELECT * FROM Hat WHERE Numara=" + hatAra.Text + ";";
            Search.Connection = connection;
            SqlDataAdapter adap = new SqlDataAdapter(Search);
            System.Data.DataTable tablo = new System.Data.DataTable();
            try
            {
                adap.Fill(tablo);
                TarifeView.DataSource = tablo;
                clear_hat();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
                refreshTableHat();
            }
        }

        private void HatView_SelectionChanged(object sender, EventArgs e)
        {
            if (HatView.SelectedRows.Count != 0)
            {
                hat_update.Text = HatView.SelectedRows[0].Cells[0].Value.ToString();
                sahip_update.Text = HatView.SelectedRows[0].Cells[1].Value.ToString();
                kampanya_update.SelectedIndex= tarife_list.IndexOf(HatView.SelectedRows[0].Cells[2].Value.ToString());
            }
        }

        private void fatura_ara_Click(object sender, EventArgs e)
        {
            SqlCommand Search = new SqlCommand();
            Search.CommandText = "SELECT * FROM Fatura WHERE Hat=" + fatura_textBox.Text + ";";
            Search.Connection = connection;
            SqlDataAdapter adap = new SqlDataAdapter(Search);
            System.Data.DataTable tablo = new System.Data.DataTable();
            try
            {
                adap.Fill(tablo);
                FaturaView.DataSource = tablo;
                clear_fatura();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
                refreshTableFatura();
            }
        }

        private void odeme_Click(object sender, EventArgs e)
        {
            SqlCommand Delete = new SqlCommand();
            Delete.CommandText = "DELETE FROM Fatura WHERE Fatura_id=" + fatura_odeme.Text + ";";
            Delete.Connection = connection;
            try
            {
                Delete.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Lütfen istenen veri tipinde veri giriniz ve kutucuğu boş bırakmayınız.",
                    "Hatalı Veri Tipi Girişi");
            }
            refreshTableFatura();
            clear_fatura();
        }

        private void fatura_kes_Click(object sender, EventArgs e)
        {
            SqlCommand kes = new SqlCommand();
            kes.CommandText = "INSERT INTO Fatura SELECT Numara,SYSDATETIME(),Fiyat from Hat,Tarife WHERE Tarife=Tarife_Adı;";
            kes.Connection = connection;
            kes.ExecuteNonQuery();
            refreshTableFatura();
        }


        private void dataupdate_Click(object sender, EventArgs e)
        {
            refreshTableAbone();
            refreshTableTarife();
            refreshTableHat();
            refreshTableFatura();
        }
        private string GetRowValues(DataGridView dgv)
        {
            List<string> list = new List<string>(dgv.Rows.Count);
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var sb = new StringBuilder();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    sb.Append(cell.Value+"\t");
                }
                list.Add(sb.ToString());
            }
            string veri = "";
            foreach(string a in list)
            {
                veri += a;
                veri += '\n';
            }
            return veri;
        }
        private void CreateDocument()
        {
            DataGridView secim = null;
            try
            { 
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                winword.ShowAnimation = false; 
                winword.Visible = false;
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);  
                document.Content.SetRange(0, 0);
                document.Content.Text = "Print Document" + Environment.NewLine;
 
                Paragraph para = document.Content.Paragraphs.Add(ref missing);
                para.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                if (gsm.SelectedIndex == 1)
                {
                    secim = AboneView;
                    para.Range.Text = "Abone Tablosu" + Environment.NewLine;
                }
                else if (gsm.SelectedIndex == 2)
                {
                    secim = TarifeView;
                    para.Range.Text = "Tarife Tablosu" + Environment.NewLine;
                }
                else if (gsm.SelectedIndex == 3)
                {
                    secim = HatView;
                    para.Range.Text = "Hat Tablosu" + Environment.NewLine;
                }
                else if (gsm.SelectedIndex == 4)
                {
                    secim = FaturaView;
                    para.Range.Text = "Fatura Tablosu" + Environment.NewLine;
                }
                para.Range.InsertParagraphAfter();

                Table firstTable = document.Tables.Add(para.Range, secim.RowCount, secim.ColumnCount, ref missing, ref missing);

                foreach (Row row in firstTable.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        cell.Range.Text = secim.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex - 1].Value.ToString();
                    }
                }
                firstTable.Borders.Enable = 1;
                string savePath = "";
                SaveFileDialog sf = new SaveFileDialog();


                if (sf.ShowDialog() == DialogResult.OK)
                {
                    savePath =sf.FileName;
                }
                object filename = savePath+".doc";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Document created successfully !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            DataGridView secim = null;
            try
            {
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                winword.ShowAnimation = false;
                winword.Visible = false;
                object missing = System.Reflection.Missing.Value;
                Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                document.Content.SetRange(0, 0);
                document.Content.Text = "Print Document" + Environment.NewLine;

                Paragraph para = document.Content.Paragraphs.Add(ref missing);
                para.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                if (gsm.SelectedIndex == 1)
                {
                    secim = AboneView;
                    para.Range.Text = "Abone Tablosu" + Environment.NewLine;
                }
                else if (gsm.SelectedIndex == 2)
                {
                    secim = TarifeView;
                    para.Range.Text = "Tarife Tablosu" + Environment.NewLine;
                }
                else if (gsm.SelectedIndex == 3)
                {
                    secim = HatView;
                    para.Range.Text = "Hat Tablosu" + Environment.NewLine;
                }
                else if (gsm.SelectedIndex == 4)
                {
                    secim = FaturaView;
                    para.Range.Text = "Fatura Tablosu" + Environment.NewLine;
                }
                para.Range.InsertParagraphAfter();

                Table firstTable = document.Tables.Add(para.Range, secim.RowCount, secim.ColumnCount, ref missing, ref missing);

                foreach (Row row in firstTable.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        cell.Range.Text = secim.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex - 1].Value.ToString();

                    }
                }
                firstTable.Borders.Enable = 1;

                object copies = "1";
                object pages = "1";
                object range = WdPrintOutRange.wdPrintCurrentPage;
                object items = WdPrintOutItem.wdPrintDocumentContent;
                object pageType = WdPrintOutPages.wdPrintAllPages;
                object oTrue = true;
                object oFalse = false;

                document.PrintOut(
                    ref oTrue, ref oFalse, ref range, ref missing, ref missing, ref missing,
                    ref items, ref copies, ref pages, ref pageType, ref oFalse, ref oTrue,
                    ref missing, ref oFalse, ref missing, ref missing, ref missing, ref missing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearTextboxes_Click(object sender, EventArgs e)
        {
            if (gsm.SelectedIndex == 1)
            {
                clear_abone();
            }
            else if (gsm.SelectedIndex == 2)
            {
                clear_tarife();
            }
            else if (gsm.SelectedIndex == 3)
            {
                clear_hat();
            }
            else if (gsm.SelectedIndex == 4)
            {
                clear_fatura();
            }
        }

        private void saveToolStripButton_Click_1(object sender, EventArgs e)
        {
            CreateDocument();
            String data = "";
            if (gsm.SelectedIndex == 1)
            {
                data = GetRowValues(AboneView);
            }
            else if (gsm.SelectedIndex == 2)
            {
                data = GetRowValues(TarifeView);
            }
            else if (gsm.SelectedIndex == 3)
            {
                data = GetRowValues(HatView);
            }
            else if (gsm.SelectedIndex == 4)
            {
                data = GetRowValues(FaturaView);
            }

        }

        private void gsm_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
