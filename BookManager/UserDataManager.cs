using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    public class UserDataManager
    {
        public static List<UserDTO> Users = new List<UserDTO>();

        static UserDataManager()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                UserDBHelper.UserSelectQuery();
                Users.Clear();

                foreach (DataRow item in UserDBHelper.dt.Rows)
                {
                    UserDTO user = new UserDTO();

                    user.Id = int.Parse(item["Id"].ToString());
                    user.Name = item["Name"].ToString();

                    Users.Add(user);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        //추가, 수정, 삭제
        public static bool DA(string command, string id, string name, out string contents)
        {
            UserDBHelper.UserSelectQuery(int.Parse(id));

            contents = "";
            if (command == "insert")
            {
                return UserAdd(id, name, ref contents);
            }
            else if (command == "delete")
            {
                return UserDelete(id, name, ref contents);
            }
            else
            {
                return UserUpdate(id, name, ref contents);
            }
        }

        //추가
        private static bool UserAdd(string id, string name, ref string contents)
        {
            if (UserDBHelper.dt.Rows.Count == 0)
            {
                UserDBHelper.UserAddQuery(id, name);
                contents = $"{id}번이 추가됨";
                return true;
            }
            else
            {
                contents = $"{id}번이 이미 있음";
                return false;
            }
        }

        //수정
        private static bool UserUpdate(string id, string name, ref string contents)
        {
            if (UserDBHelper.dt.Rows.Count != 0)
            {
                UserDBHelper.UserUpdateQuery(id, name);
                contents = $"{id}번이 수정됨";
                return true;
            }
            else
            {
                contents = $"{id}번이 없음";
                return false;
            }
        }

        //삭제
        private static bool UserDelete(string id, string name, ref string contents)
        {
            if (UserDBHelper.dt.Rows.Count != 0)
            {
                UserDBHelper.UserDeleteQuery(id, name);
                contents = $"{id}번이 삭제됨";
                return true;
            }
            else
            {
                contents = $"{id}번이 없음";
                return false;
            }
        }
    }
}
