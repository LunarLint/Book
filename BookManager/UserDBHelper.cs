using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    public class UserDBHelper
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
                "Timeout = 3", "local", "UserDB", "SSPI");

            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        //DB 갱신
        public static void UserSelectQuery(int userid = -1)
        {
            try
            {
                ConnectDB();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (userid == -1)
                {
                    cmd.CommandText = "select * from UserManager";
                }
                else
                {
                    cmd.CommandText = "select * from UserManager where Id = " + userid;
                }

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds, "UserManager");
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

        //추가, 삭제, 수정
        public static void executeQuery(string id, string name, string command)
        {
            string sqlcommand = "";

            if (command == "insert")
            {
                sqlcommand = "insert into UserManager(Id, Name) values (@p1, @p2)";
            }
            else if (command == "delete")
            {
                sqlcommand = "delete from UserManager where Id = @p1";
            }
            else
            {
                sqlcommand = "update UserManager set Name = @p2 where Id = @p1";
            }

            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", id);
                cmd.Parameters.AddWithValue("@p2", name);
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
        public static void UserDeleteQuery(string id, string name)
        {
            executeQuery(id, name, "delete");
        }

        //추가
        public static void UserAddQuery(string id, string name)
        {
            executeQuery(id, name, "insert");
        }

        //수정
        public static void UserUpdateQuery(string id, string name)
        {
            executeQuery(id, name, "update");
        }
    }
}
