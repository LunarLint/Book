using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    public class BookDataManager
    {
        public static List<BookDTO> Books = new List<BookDTO>();

        static BookDataManager()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                BookDBHelper.BookSelectQuery();
                Books.Clear();

                foreach(DataRow item in BookDBHelper.dt.Rows)
                {
                    BookDTO book = new BookDTO();
                    book.Isbn = int.Parse(item["Isbn"].ToString());
                    book.Name = item["Name"].ToString();
                    book.Publisher = item["Publisher"].ToString();
                    book.Page = int.Parse(item["Page"].ToString());
                    book.UserId = item["UserId"].ToString();
                    book.UserName = item["UserName"].ToString();
                    book.isBorrowed = item["isBorrowed"].ToString();
                    book.BorrowedAt = item["BorrowedAt"].ToString()
                         == "" ? new DateTime() : DateTime.Parse(item["BorrowedAt"].ToString());

                    Books.Add(book);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        //텍스트 파일
        public static void BookLog(string c)
        {
            DirectoryInfo di = new DirectoryInfo("BookHIstory");

            //폴더 없을 시
            if (di.Exists == false)
            {
                di.Create();
            }

            using (StreamWriter sw = new StreamWriter("BookHistory\\BookLog.txt", true))
            {
                sw.WriteLine(c);
            }
        }

        internal static void Borrow()
        {
            throw new NotImplementedException();
        }

        // 대여
        public static void Borrow(string isbn, string userid, string username)
        {
            try
            {
                BookDBHelper.updateQuery(isbn, userid, username);
            }
            catch (Exception)
            {

                //throw;
            }
        }

        //반납
        public static void Borrow(string isbn)
        {
            try
            {
                BookDBHelper.updateQuery(isbn);
            }
            catch (Exception)
            {

                //throw;
            }
        }

        public static bool DA(string command, string isbn, string name, string publisher, string page, out string contents)
        {
            BookDBHelper.BookSelectQuery(int.Parse(isbn));

            contents = "";
            if(command == "insert")
            {
                return BookAdd(isbn, name, publisher, page, ref contents);
            }
            else if(command == "delete")
            {
                return BookDelete(isbn, name, publisher, page, ref contents);
            }
            else
            {
                return BookUpdate(isbn, name, publisher, page, ref contents);
            }
        }

        //삭제
        private static bool BookDelete(string isbn, string name, string publisher, string page, ref string contents)
        {
            if(BookDBHelper.dt.Rows.Count != 0)
            {
                BookDBHelper.BookDeleteQuery(isbn, name, publisher, page);
                contents = $"{isbn}번이 삭제됨";
                return true;
            }
            else
            {
                contents = $"{isbn}번이 없음";
                return false;
            }
        }

        //추가
        private static bool BookAdd(string isbn, string name, string publisher, string page, ref string contents)
        {
            if(BookDBHelper.dt.Rows.Count == 0)
            {
                BookDBHelper.BookAddQuery(isbn, name, publisher, page);
                contents = $"{isbn}번이 추가됨";
                return true;
            }
            else
            {
                contents = $"{isbn}번이 이미 있음";
                return false;
            }
        }

        //수정
        private static bool BookUpdate(string isbn, string name, string publisher, string page, ref string contents)
        {
            if (BookDBHelper.dt.Rows.Count != 0)
            {
                BookDBHelper.BookUpdateQuery(isbn, name, publisher, page);
                contents = $"{isbn}번이 수정됨";
                return true;
            }
            else
            {
                contents = $"{isbn}번이 없음";
                return false;
            }
        }
    }
}
