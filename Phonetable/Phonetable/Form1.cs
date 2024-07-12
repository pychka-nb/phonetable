using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using static System.Windows.Forms.DataFormats;
using System.Net;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics.Metrics;

namespace Phonetable
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            Connect = new OleDbConnection("Provider=SQLOLEDB;Data Source=192.168.9.5;Initial Catalog=KIS;Persist Security Info=TRUE;User ID=sa;Password=sa");
            //Connect.Open();


            CC();
            Abonent();
            Telephone();
        }
        private void CC()
        {
            OleDbDataAdapter dataAdapter = null;
            string selectSQL = "SELECT * FROM [dbo].[Baranov_phonetable]";
            dataAdapter = new OleDbDataAdapter(selectSQL, Connect);
            DataTable table1 = new DataTable();
            dataAdapter.Fill(table1);
            table1.Columns["City_codes"].ColumnName = "��� ������";
            table1.Columns["Country_codes"].ColumnName = "��� ������";
            table1.Columns["City_name"].ColumnName = "������";
            table1.Columns["Country_name"].ColumnName = "�����";
            dataGridView1.DataSource = table1;
            dataGridView1.Columns["id"].Visible = false;
        }
        private void Abonent()
        {
            OleDbDataAdapter dataAdapter = null;
            string selectSQL = "SELECT * FROM [dbo].[Baranov_abonent]";
            dataAdapter = new OleDbDataAdapter(selectSQL, Connect);
            DataTable table2 = new DataTable();
            dataAdapter.Fill(table2);
            table2.Columns["name"].ColumnName = "���";
            table2.Columns["surname"].ColumnName = "�������";
            table2.Columns["patronymic"].ColumnName = "��������";
            table2.Columns["address"].ColumnName = "�����";
            table2.Columns["birthdate"].ColumnName = "���� ��������";
            table2.Columns["comment"].ColumnName = "�����������";
            dataGridView2.DataSource = table2;
            dataGridView2.Columns["id"].Visible = false;
        }
        private void Telephone()
        {
            string selectSQL = @"SELECT * 
FROM [dbo].[Baranov_contact] 
JOIN Baranov_abonent 
ON Baranov_contact.id_abonent=Baranov_abonent.id 
JOIN Baranov_phonetable
ON Baranov_contact.id_cc=Baranov_phonetable.id ";
            OleDbDataAdapter dataAdapter = null;
            dataAdapter = new OleDbDataAdapter(selectSQL, Connect);
            DataTable table3 = new DataTable();
            dataAdapter.Fill(table3);
            table3.Columns["surname"].ColumnName = "�������";
            table3.Columns["name"].ColumnName = "���";
            table3.Columns["patronymic"].ColumnName = "��������";
            table3.Columns["Country_name"].ColumnName = "�����";
            //table3.Columns["birthdate"].ColumnName = "���� ��������";
            //table3.Columns["Country"].ColumnName = "���� ��������";
            //table3.Columns["surname"].DisplayIndex = 0;
            table3.Columns.Remove("comment");
            table3.Columns.Remove("address");
            table3.Columns.Remove("birthdate");
            table3.Columns.Remove("id1");
            table3.Columns.Remove("City_codes");
            table3.Columns.Remove("Country_codes");
            table3.Columns.Remove("City_name");
            //table3.Columns.Remove("Country_name");

            table3.Columns.Remove("id2");

            table3.Columns["phone"].ColumnName = "�������";
            table3.Columns["type"].ColumnName = "���";
            dataGridView3.DataSource = table3;
            dataGridView3.Columns["id"].Visible = false;
            dataGridView3.Columns["id_abonent"].Visible = false;
            dataGridView3.Columns["id_cc"].Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var form = new Form2();
            var res = form.ShowDialog();
            if (res == DialogResult.OK)
            {
                var CityCodes = form.CityCodes.Text;
                var CountryCodes = form.CountryCodes.Text;
                var City = form.City.Text;
                var Country = form.Country.Text;
                if (City == "" || CityCodes == "" || CountryCodes == "" || Country == "")
                {
                    MessageBox.Show("������ ������!");
                }
                else
                {
                    Connect.Open();
                    var tabl = Connect.CreateCommand();

                    tabl.CommandText = @"INSERT INTO [dbo].[Baranov_phonetable]
                (City_codes,Country_codes,City_name,Country_name)
VALUES('" + CityCodes + "','" + CountryCodes + "','" + City + "','" + Country + "')";
                    OleDbDataReader com = tabl.ExecuteReader();
                    Connect.Close();
                    CC();
                }
            }
        }
        private OleDbConnection Connect;

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var form = new Form2();
                var row = dataGridView1.SelectedRows[0];
                form.CityCodes.Text = row.Cells["��� ������"].Value.ToString();
                form.CountryCodes.Text = row.Cells["��� ������"].Value.ToString();
                form.City.Text = row.Cells["������"].Value.ToString();
                form.Country.Text = row.Cells["�����"].Value.ToString();
                var res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    var CityCodes = form.CityCodes.Text;
                    var CountryCodes = form.CountryCodes.Text;
                    var City = form.City.Text;
                    var Country = form.Country.Text;
                    var id = row.Cells["Id"].Value;
                    if (City == "" || CityCodes == "" || CountryCodes == "" || Country == "")
                    {
                        MessageBox.Show("������ ������!");
                    }
                    else
                    {
                        Connect.Open();
                        var tabl = Connect.CreateCommand();
                        tabl.CommandText = @"UPDATE [dbo].[Baranov_phonetable]
SET City_Codes='" + CityCodes + "',Country_Codes='" + CountryCodes + "',City_name='" + City + "',Country_name='" + Country + "' WHERE [Baranov_phonetable].id='" + id + "'";
                        OleDbDataReader com = tabl.ExecuteReader();
                        Connect.Close();
                        CC();
                    }

                }
                else
                {
                    MessageBox.Show("�������� ������!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow startingBalanceRow = dataGridView1.SelectedRows[0];
                Connect.Open();
                var command = Connect.CreateCommand();
                command.CommandText = string.Format(@"DELETE Baranov_phonetable
                                WHERE id = '{0}'", startingBalanceRow.Cells["id"].Value);
                OleDbDataReader com = command.ExecuteReader();
                Connect.Close();
                CC();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new Form3();
            var res = form.ShowDialog();
            if (res == DialogResult.OK)
            {
                var surname = form.surname.Text;
                var name = form.name.Text;
                var patronymic = form.patronymic.Text;
                var address = form.address.Text;
                var birthdate = form.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                var comment = form.comment.Text;
                if (surname == "" || name == "" || patronymic == "" || address == "")
                {
                    MessageBox.Show("������ ������!");
                }
                else
                {
                    Connect.Open();
                    var tabl = Connect.CreateCommand();

                    tabl.CommandText = @"INSERT INTO [dbo].[Baranov_abonent]
                (surname,name,patronymic,address,birthdate,comment)
VALUES('" + surname + "','" + name + "','" + patronymic + "','" + address + "','" + birthdate + "','" + comment + "')";
                    OleDbDataReader com = tabl.ExecuteReader();
                    Connect.Close();
                    Abonent();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var form = new Form3();
                var row = dataGridView2.SelectedRows[0];
                form.surname.Text = row.Cells["�������"].Value.ToString();
                form.name.Text = row.Cells["���"].Value.ToString();
                form.patronymic.Text = row.Cells["��������"].Value.ToString();
                form.address.Text = row.Cells["�����"].Value.ToString();
                form.comment.Text = row.Cells["�����������"].Value.ToString();
                form.dateTimePicker1.Text = row.Cells["���� ��������"].Value.ToString();
                var res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    var surname = form.surname.Text;
                    var name = form.name.Text;
                    var patronymic = form.patronymic.Text;
                    var address = form.address.Text;
                    var birthdate = form.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    var comment = form.comment.Text;
                    var id = row.Cells["Id"].Value;
                    if (surname == "" || name == "" || patronymic == "" || address == "")
                    {
                        MessageBox.Show("������ ������!");
                    }
                    else
                    {
                        Connect.Open();
                        var tabl = Connect.CreateCommand();
                        tabl.CommandText = @"UPDATE [dbo].[Baranov_abonent]
SET surname='" + surname + "',name='" + name + "',patronymic='" + patronymic + "',address='" + address + "', birthdate='" + birthdate + "' ,comment='" + comment + "' WHERE [Baranov_abonent].id='" + id + "'";
                        OleDbDataReader com = tabl.ExecuteReader();
                        Connect.Close();
                        Abonent();
                    }
                }
                else
                {
                    MessageBox.Show("�������� ������!");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow startingBalanceRow = dataGridView2.SelectedRows[0];
                Connect.Open();
                var command = Connect.CreateCommand();
                command.CommandText = string.Format(@"DELETE Baranov_abonent
                                WHERE id = '{0}'", startingBalanceRow.Cells["id"].Value);
                OleDbDataReader com = command.ExecuteReader();
                Connect.Close();
                Abonent();

            }
        }

        private void button8_Click(object sender, EventArgs e)//���������� �������� 
        {
            var form = new Form4();
            //�������
            OleDbDataAdapter dataAdapter1 = null;
            string selectSQL1 = "SELECT * FROM [dbo].[Baranov_abonent]";
            dataAdapter1 = new OleDbDataAdapter(selectSQL1, Connect);
            DataTable table2 = new DataTable();
            dataAdapter1.Fill(table2);

            var dict1 = new Dictionary<int, string>();
            foreach (DataRow row in table2.Rows)
            {
                dict1.Add((int)row["id"], row["surname"].ToString() + " " + row["name"].ToString() + " " + row["patronymic"].ToString());
            }
            form.AbonentData = dict1;
            //�����
            OleDbDataAdapter dataAdapter3 = null;
            string selectSQL3 = "SELECT * FROM [dbo].[Baranov_phonetable]";
            dataAdapter3 = new OleDbDataAdapter(selectSQL3, Connect);
            DataTable table1 = new DataTable();
            dataAdapter3.Fill(table1);

            var dict3 = new Dictionary<int, string>();
            foreach (DataRow row in table1.Rows)
            {
                dict3.Add((int)row["id"], row["Country_name"].ToString());
            }
            form.CountryData = dict3;

            //������� � ���
            OleDbDataAdapter dataAdapter2 = null;
            string selectSQL2 = "SELECT * FROM [dbo].[Baranov_contact]";
            dataAdapter2 = new OleDbDataAdapter(selectSQL2, Connect);
            DataTable table3 = new DataTable();
            dataAdapter2.Fill(table3);
            var dict2 = new Dictionary<int, string>();
            var dicttype = new Dictionary<int, string>();
            /*foreach (DataRow dataRow in table3.Rows)
            {
                //dict2.Add((int)dataRow["id"], dataRow["phone"].ToString());
                //dicttype.Add((int)dataRow["id"], dataRow["type"].ToString());
               
            }*/
            dicttype.Add(0, "��������");
            dicttype.Add(1, "���������");
            dicttype.Add(2, "�������");
            //form.ContactData = dict2;
            form.TypeData = dicttype;

            var res = form.ShowDialog();
            if (res == DialogResult.OK)
            {
                var ContactBox = form.ContactBox.Text;
                if (ContactBox == "")
                {
                    MessageBox.Show("������ ������!");
                }
                else
                {
                    var dataAdapter4 = "";

                    dataAdapter4 += "INSERT INTO [dbo].[Baranov_contact] (id_abonent,id_cc,phone,type) ";
                    //dataAdapter4 += "VALUES (" + form.AbonentID + ", " + form.CountryID + " ,'" + form.ContactID + "','��������')";
                    dataAdapter4 += "VALUES (" + form.AbonentID + ", " + form.CountryID + " ,'" + ContactBox + "','" + form.TypeID + "')";

                    Connect.Open();
                    var tabl = Connect.CreateCommand();
                    tabl.CommandText = dataAdapter4;
                    OleDbDataReader com = tabl.ExecuteReader();
                    Connect.Close();
                    Telephone();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var form = new Form4();
            //�������
            OleDbDataAdapter dataAdapter1 = null;
            string selectSQL1 = "SELECT * FROM [dbo].[Baranov_abonent]";
            dataAdapter1 = new OleDbDataAdapter(selectSQL1, Connect);
            DataTable table2 = new DataTable();
            dataAdapter1.Fill(table2);

            var dict1 = new Dictionary<int, string>();
            foreach (DataRow row in table2.Rows)
            {
                dict1.Add((int)row["id"], row["surname"].ToString() + " " + row["name"].ToString() + " " + row["patronymic"].ToString());
            }
            form.AbonentData = dict1;
            //�����
            OleDbDataAdapter dataAdapter3 = null;
            string selectSQL3 = "SELECT * FROM [dbo].[Baranov_phonetable]";
            dataAdapter3 = new OleDbDataAdapter(selectSQL3, Connect);
            DataTable table1 = new DataTable();
            dataAdapter3.Fill(table1);

            var dict3 = new Dictionary<int, string>();
            foreach (DataRow row in table1.Rows)
            {
                dict3.Add((int)row["id"], row["Country_name"].ToString());
            }
            form.CountryData = dict3;

            //������� � ���
            OleDbDataAdapter dataAdapter2 = null;
            string selectSQL2 = "SELECT * FROM [dbo].[Baranov_contact]";
            dataAdapter2 = new OleDbDataAdapter(selectSQL2, Connect);
            DataTable table3 = new DataTable();
            dataAdapter2.Fill(table3);
            var dict2 = new Dictionary<int, string>();
            var dicttype = new Dictionary<int, string>();
            /*foreach (DataRow dataRow in table3.Rows)
            {
                //dict2.Add((int)dataRow["id"], dataRow["phone"].ToString());
                //dicttype.Add((int)dataRow["id"], dataRow["type"].ToString());
               
            }*/
            dicttype.Add(0, "��������");
            dicttype.Add(1, "���������");
            dicttype.Add(2, "�������");
            form.TypeData = dicttype;

            var dgv = dataGridView3;
            var selected = dgv.SelectedRows;

            if (selected.Count == 0)
            {
                MessageBox.Show("�� ������� ������");
                return;
            }
            int abonentID = (int)dgv["id_abonent", selected[0].Index].Value;
            int countryID = (int)dgv["id_cc", selected[0].Index].Value;
            string typeID = dgv["���", selected[0].Index].Value.ToString();
            typeID = typeID.Replace(" ", "");
            //int typeID = (int)dgv["���", selected[0].Index].Value;
            //��������� ComboBox � �������� �������� �� ������� � �������� ��������
            form.AbonentID = abonentID;
            form.CountryID = countryID;
            form.TypeID = typeID;
            form.ContactBox.Text = selected[0].Cells["�������"].Value.ToString();
            var res = form.ShowDialog();
            if (res == DialogResult.OK)
            {
                var id = selected[0].Cells["Id"].Value;
                var phone = form.ContactBox.Text;
                if (phone == "")
                {
                    MessageBox.Show("������ ������!");
                }
                else
                {
                    var request3 = "";
                    request3 += "UPDATE [dbo].[Baranov_contact]";
                    request3 += "SET id_abonent= '" + form.AbonentID + "',id_cc='" + form.CountryID + "',phone='" + phone + "',type='" + form.TypeID + "'WHERE [Baranov_contact].id='" + id + "'";
                    //request3 += "WHERE abonent_id = '" + abonentID + "' AND contact_id = '" + contactID + "' ";
                    Connect.Open();
                    var tabl = Connect.CreateCommand();
                    tabl.CommandText = request3;
                    OleDbDataReader com = tabl.ExecuteReader();
                    Connect.Close();
                    Telephone();
                }
            }

        }

        private void button10_Click(object sender, EventArgs e)//��������
        {
            var dgv = dataGridView3;
            var selected = dgv.SelectedRows;

            if (selected.Count == 0)
            {
                MessageBox.Show("�� ������� ������");
                return;
            }

            int abonentID = (int)dgv["id_abonent", selected[0].Index].Value;
            int countryID = (int)dgv["id_cc", selected[0].Index].Value;
            var request3 = "";

            request3 += "DELETE [dbo].[Baranov_contact]";
            request3 += "WHERE id_abonent = '" + abonentID + "' AND id_cc = '" + countryID + "' ";

            Connect.Open();
            var tabl = Connect.CreateCommand();
            tabl.CommandText = request3;
            OleDbDataReader com = tabl.ExecuteReader();
            Connect.Close();
            Telephone();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updatePhoneHelp();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            updatePhoneHelp();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            updatePhoneHelp();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //updatePhoneHelp();
        }
        private void updatePhoneHelp()
        {



            string selectSQL = string.Format(@"SELECT * 
FROM [dbo].[Baranov_contact] 
JOIN Baranov_abonent 
ON Baranov_contact.id_abonent=Baranov_abonent.id 
JOIN Baranov_phonetable
ON Baranov_contact.id_cc=Baranov_phonetable.id 
WHERE Baranov_abonent.surname + ' ' + Baranov_abonent.name + ' ' + Baranov_abonent.patronymic LIKE '%{0}%' 
AND Baranov_phonetable.Country_name LIKE '%{1}%' 
AND Baranov_contact.phone LIKE '%{2}%'", textBox1.Text, textBox2.Text, textBox3.Text);


            OleDbDataAdapter dataAdapter = null;
            dataAdapter = new OleDbDataAdapter(selectSQL, Connect);
            DataTable table3 = new DataTable();
            dataAdapter.Fill(table3);
            table3.Columns["surname"].ColumnName = "�������";
            table3.Columns["name"].ColumnName = "���";
            table3.Columns["patronymic"].ColumnName = "��������";
            table3.Columns["Country_name"].ColumnName = "�����";
            //table3.Columns["birthdate"].ColumnName = "���� ��������";
            //table3.Columns["Country"].ColumnName = "���� ��������";
            //table3.Columns["surname"].DisplayIndex = 0;
            table3.Columns.Remove("comment");
            table3.Columns.Remove("address");
            table3.Columns.Remove("birthdate");
            table3.Columns.Remove("id1");
            table3.Columns.Remove("City_codes");
            table3.Columns.Remove("Country_codes");
            table3.Columns.Remove("City_name");
            //table3.Columns.Remove("Country_name");

            table3.Columns.Remove("id2");

            table3.Columns["phone"].ColumnName = "�������";
            table3.Columns["type"].ColumnName = "���";
            dataGridView3.DataSource = table3;
            dataGridView3.Columns["id"].Visible = false;
            dataGridView3.Columns["id_abonent"].Visible = false;
            dataGridView3.Columns["id_cc"].Visible = false;
            dataGridView3.DataSource = table3;
        }

        private void updatePhoneH()
        {
            string selectSQL = string.Format(@"SELECT * 
FROM [dbo].[Baranov_contact] 
JOIN Baranov_abonent 
ON Baranov_contact.id_abonent=Baranov_abonent.id 
JOIN Baranov_phonetable
ON Baranov_contact.id_cc=Baranov_phonetable.id 
WHERE (DATEDIFF(day,Baranov_abonent.birthdate, '{0}'))=0 ", dateTimePicker1.Value.ToString("yyyyMMdd"));
            /*(DATEDIFF(day, Baranov_abonent.birthdate, CONVERT(DATETIME, '20021220 00:00:00'))) = 0//����� ��� ���� dateTimePicker1.Value.Date.ToString("yyyyMMdd")*/

            OleDbDataAdapter dataAdapter = null;
            dataAdapter = new OleDbDataAdapter(selectSQL, Connect);
            DataTable table3 = new DataTable();
            dataAdapter.Fill(table3);
            table3.Columns["surname"].ColumnName = "�������";
            table3.Columns["name"].ColumnName = "���";
            table3.Columns["patronymic"].ColumnName = "��������";
            table3.Columns["Country_name"].ColumnName = "�����";
            //table3.Columns["birthdate"].ColumnName = "���� ��������";
            //table3.Columns["Country"].ColumnName = "���� ��������";
            //table3.Columns["surname"].DisplayIndex = 0;
            table3.Columns.Remove("comment");
            table3.Columns.Remove("address");
            table3.Columns.Remove("birthdate");
            table3.Columns.Remove("id1");
            table3.Columns.Remove("City_codes");
            table3.Columns.Remove("Country_codes");
            table3.Columns.Remove("City_name");
            //table3.Columns.Remove("Country_name");

            table3.Columns.Remove("id2");

            table3.Columns["phone"].ColumnName = "�������";
            table3.Columns["type"].ColumnName = "���";
            dataGridView3.DataSource = table3;
            dataGridView3.Columns["id"].Visible = false;
            dataGridView3.Columns["id_abonent"].Visible = false;
            dataGridView3.Columns["id_cc"].Visible = false;
            dataGridView3.DataSource = table3;
        }


        private void button7_Click(object sender, EventArgs e)
        {
            updatePhoneH();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Telephone();
        }

    }
}
