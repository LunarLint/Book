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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            if (BookDataManager.Books.Count > 0)
            {
                dataGridView1.DataSource = BookDataManager.Books;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BookDTO book = dataGridView1.CurrentRow.DataBoundItem as BookDTO;

            textBox1.Text = book.Isbn.ToString();
            textBox2.Text = book.Name;
            textBox3.Text = book.Publisher;
            textBox4.Text = book.Page.ToString();

        }

        //갱신
        private void update()
        {
            BookDataManager.Load();
            dataGridView1.DataSource = null;
            if (BookDataManager.Books.Count > 0)
            {
                dataGridView1.DataSource = BookDataManager.Books;
            }
        }

        //추가, 수정, 삭제
        private void Add_or_Delete(string isbn, string name, string publisher, string page, string command)
        {
            int.TryParse(isbn, out int isbnNum);
            if(isbnNum <= 0)
            {
                MessageBox.Show("0 이하의 숫자가 입력됨");
                return;
            }

            string contents = "";

            bool check = BookDataManager.DA(command, isbn, name, publisher, page, out contents);

            if (check)
            {
                update();
            }
            MessageBox.Show(contents);
        }

        //추가
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == ""|| textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" )
            {
                MessageBox.Show("빈칸이 있음");
            }
            else
            {
            Add_or_Delete(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, "insert");
            WriteLog("[책 추가] 책 코드 : " + textBox1.Text + " 책명 :" + textBox2.Text + "출판사 : ");
            }
        }

        //삭제
        private void button3_Click(object sender, EventArgs e)
        {
            Add_or_Delete(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, "delete");
            WriteLog("[책 삭제] 책 코드 : " + textBox1.Text + " 책명 :" + textBox2.Text + "출판사 : ");
        }

        //수정
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("빈칸이 있음");
            }
            else
            {
                Add_or_Delete(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, "update");
                WriteLog("[책 수정] 책 코드 : " + textBox1.Text + " 책명 :" + textBox2.Text + " 출판사 : ");
            }
        }

        private void WriteLog(string contents)
        {
            string logContents = $"[{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}]{contents}";

            BookDataManager.BookLog(logContents);
        }
    }
}
