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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "도서관 관리";
            
            //Grid에 DB 표기
            if (BookDataManager.Books.Count > 0)
            {
                BookGridView.DataSource = BookDataManager.Books;
            }
            if (UserDataManager.Users.Count > 0)
            {
                UserGridView.DataSource = UserDataManager.Users;
            }

            //라벨 설정
            label7.Text = BookGridView.RowCount.ToString();
            label8.Text = UserGridView.RowCount.ToString();
            label9.Text = BookDataManager.Books.Where((x) => x.isBorrowed == "true").Count().ToString();
            label10.Text = BookDataManager.Books.Where((x) => x.isBorrowed == "true" && x.BorrowedAt.AddDays(7) < DateTime.Now).Count().ToString();
        }

        //도서 현황 셀 클릭 시
        private void BookGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BookDTO book = BookGridView.CurrentRow.DataBoundItem as BookDTO;

            IsbnText.Text = book.Isbn.ToString();
            BookNameText.Text = book.Name;

            if (book.UserId.ToString() != null)
            {
                UserIDText.Text = book.UserId.ToString();
            }
        }

        //사용자 현황 셀 클릭 시
        private void UserGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UserDTO user = UserGridView.CurrentRow.DataBoundItem as UserDTO;

            UserIDText.Text = user.Id.ToString();
        }

        // 대여버튼 클릭 시
        private void BorrowButton_Click(object sender, EventArgs e)
        {
            if (IsbnText.Text.Trim() == "" || BookNameText.Text.Trim() == "" || UserIDText.Text.Trim() == "")
            {
                MessageBox.Show("빈 공간을 채워주세요.");
            }
            else
            {
                try
                {
                    BookDTO book = BookDataManager.Books.Single(x => x.Isbn == int.Parse(IsbnText.Text.ToString()));
                    UserDTO user = UserDataManager.Users.Single(x => x.Id == int.Parse(UserIDText.Text.ToString()));

                    if (book.isBorrowed == "true")
                    {
                        MessageBox.Show("이미 대여된 책입니다.");
                    }
                    else
                    {
                        book.UserId = UserIDText.Text.ToString();
                        book.UserName = user.Name;
                        book.isBorrowed = "true";
                        book.BorrowedAt = DateTime.Now;

                        BookGridView.DataSource = null;
                        UserGridView.DataSource = null;

                        BookGridView.DataSource = BookDataManager.Books;
                        UserGridView.DataSource = UserDataManager.Users;

                        BookDataManager.Borrow(IsbnText.Text, UserIDText.Text, user.Name);
                        label9.Text = BookDataManager.Books.Where((x) => x.isBorrowed == "true").Count().ToString();
                        WriteLog("[대여]" + " 책명 : " + BookNameText.Text + " 대여인 : " + UserIDText.Text);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"책번호 {IsbnText.Text}은/는 없습니다.");
                    throw;
                }
            }
        }

        //반납 버튼 클릭 시
        private void ReturnButton_Click(object sender, EventArgs e)
        {
            if (IsbnText.Text.Trim() == "" || BookNameText.Text.Trim() == "")
            {
                MessageBox.Show("빈 공간을 채워주세요.");
            }
            else
            {
                try
                {
                    BookDTO book = BookDataManager.Books.Single(x => x.Isbn == int.Parse(IsbnText.Text.ToString()));

                    if (book.isBorrowed == "false" || book.isBorrowed == null)
                    {
                        MessageBox.Show("아직 대여되지 않은 책입니다.");
                    }
                    else
                    {
                        book.UserId = "";
                        book.UserName = "";
                        book.isBorrowed = "false";
                        book.BorrowedAt = DateTime.Now;

                        BookGridView.DataSource = null;
                        UserGridView.DataSource = null;

                        BookGridView.DataSource = BookDataManager.Books;
                        UserGridView.DataSource = UserDataManager.Users;

                        BookDataManager.Borrow(IsbnText.Text);
                        label9.Text = BookDataManager.Books.Where((x) => x.isBorrowed == "true").Count().ToString();
                        WriteLog("[반납]" + " 책명 : " + BookNameText.Text + " 반납인 : " + UserIDText.Text);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"책번호 {IsbnText.Text}은/는 없습니다.");
                    throw;
                }
            }
        }

        //책 옵션
        private void label14_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
            BookGridView.DataSource = null;
            BookGridView.DataSource = BookDataManager.Books;
            label7.Text = BookGridView.RowCount.ToString();
        }

        //유저 옵션
        private void label15_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
            UserGridView.DataSource = null;
            UserGridView.DataSource = UserDataManager.Users;
            label8.Text = UserGridView.RowCount.ToString();
        }

        private void WriteLog(string contents)
        {
            string logContents = $"[{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}]{contents}";

            BookDataManager.BookLog(logContents);
        }
    }
}
