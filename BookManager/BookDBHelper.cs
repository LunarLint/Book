using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    public class BookDBHelper
    {
        private static SqlConnection conn = new SqlConnection();
        public static SqlDataAdapter da;
        public static DataSet ds;
        public static DataTable dt;

        //DB connect
        public static void ConnectDB()
        {
            conn.ConnectionString = String.Format("Data Source=({0}) ; " +
                 "initial Catalog = {1} ; "
                 + "integrated Security = {2} ; " +
                 "Timeout = 3", "local", "BookDB", "SSPI");

            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        //DB갱신
        public static void BookSelectQuery(int bookamount = -1)
        {
            try
            {
                ConnectDB();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (bookamount == -1)
                {
                    cmd.CommandText = "select * from BookManager";
                }
                else
                {
                    cmd.CommandText = "select * from BookManager where Isbn = " + bookamount;
                }

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds, "BookManager");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        // 대여
        public static void updateQuery(string isbn, string userid, string username)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string sqlcommand = "";

                sqlcommand = "update BookManager set UserId=@p1, " + "UserName=@p2, isBorrowed=@p3, " +
                    "BorrowedAt=@p4 where Isbn=@p5";
                cmd.Parameters.AddWithValue("@p1", userid);
                cmd.Parameters.AddWithValue("@p2", username);
                cmd.Parameters.AddWithValue("@p3", "true");
                cmd.Parameters.AddWithValue("@p4", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@p5", isbn);

                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
                conn.Close();
            }
        }

        //반납
        public static void updateQuery(string isbn)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string sqlcommand = "";

                sqlcommand = "update BookManager set UserId=null, " + "UserName=null, isBorrowed=@p1, " +
                    "BorrowedAt=null where Isbn=@p2";

                cmd.Parameters.AddWithValue("@p1", "false");
                cmd.Parameters.AddWithValue("@p2", isbn);

                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
                conn.Close();
            }
        }

        //추가, 삭제, 수정
        public static void executeQuery(string isbn, string name, string publisher, string page, string command)
        {
            string sqlcommand = "";

            if(command == "insert")
            {
                 sqlcommand = "insert into BookManager(Isbn, Name, Publisher, Page, isBorrowed) values (@p1, @p2, @p3, @p4, 'false')";
            }
            else if(command == "delete")
            {
                 sqlcommand = "delete from BookManager where Isbn = @p1";
            }
            else
            {
                sqlcommand = "update BookManager set Name = @p2, Publisher = @p3, Page = @p4 where Isbn = @p1";
            }

            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", isbn);
                cmd.Parameters.AddWithValue("@p2", name);
                cmd.Parameters.AddWithValue("@p3", publisher);
                cmd.Parameters.AddWithValue("@p4", page);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
                conn.Close();
            }
        }

        //삭제
        public static void BookDeleteQuery(string isbn, string name, string publisher, string page)
        {
            executeQuery(isbn, name, publisher, page, "delete");
        }

        //추가
        public static void BookAddQuery(string isbn, string name, string publisher, string page)
        {
            executeQuery(isbn, name, publisher, page, "insert");
        }

        //수정
        public static void BookUpdateQuery(string isbn, string name, string publisher, string page)
        {
            executeQuery(isbn, name, publisher, page, "update");
        }
    }

}
