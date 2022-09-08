using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookManager
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            if (UserDataManager.Users.Count > 0)
            {
                dataGridView1.DataSource = UserDataManager.Users;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UserDTO user = dataGridView1.CurrentRow.DataBoundItem as UserDTO;

            textBox1.Text = user.Id.ToString();
            textBox2.Text = user.Name;
        }

        //갱신
        private void update()
        {
            UserDataManager.Load();
            dataGridView1.DataSource = null;
            if (UserDataManager.Users.Count > 0)
            {
                dataGridView1.DataSource = UserDataManager.Users;
            }
        }

        //추가, 수정, 삭제
        private void Add_or_Delete(string id, string name, string command)
        {
            int.TryParse(id, out int idNum);
            if (idNum <= 0)
            {
                MessageBox.Show("0 이하의 숫자가 입력됨");
                return;
            }

            string contents = "";

            bool check = UserDataManager.DA(command, id, name, out contents);

            if (check)
            {
                update();
            }
            MessageBox.Show(contents);
        }

        //추가
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("빈칸이 있음");
            }
            else
            {
                Add_or_Delete(textBox1.Text, textBox2.Text, "insert");
                WriteLog("[유저 추가] 유저 코드 : " + textBox1.Text + " 유저명 :" + textBox2.Text);
            }
        }

        //수정
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("빈칸이 있음");
            }
            else
            {
                Add_or_Delete(textBox1.Text, textBox2.Text, "update");
                WriteLog("[유저 수정] 유저 코드 : " + textBox1.Text + " 유저명 :" + textBox2.Text);
            }
        }

        //삭제
        private void button3_Click(object sender, EventArgs e)
        {
            Add_or_Delete(textBox1.Text, textBox2.Text, "delete");
            WriteLog("[유저 삭제] 유저 코드 : " + textBox1.Text + " 유저명 :" + textBox2.Text);
        }

        private void WriteLog(string contents)
        {
            string logContents = $"[{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}]{contents}";

            BookDataManager.BookLog(logContents);
        }
    }
}
