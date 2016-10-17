using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace QuintilesIMS_test_p2
{
    class UserInfor
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Init_Password { get; set; }
        public string Role { get; set; }
        public string Reason_For_Access { get; set; }

        public List<UserInfor> GetUserList()
        {
            //return new List<UserInfor> {
            //     new UserInfor { UserName= "Susan Smith", Email="ssmith@comany1.com",Init_Password="susan12%#?",
            //         Role = "SuperUser", Reason_For_Access="Because I am \"cool\", I can do whatever I want." },
            //     new UserInfor { UserName= "Alex O'Connor", Email="alexoconnor@univ1.edu",Init_Password="itsuniv1",
            //         Role = "Readonlly",Reason_For_Access="I need to access report for budget < 1M $"},
            //     new UserInfor { UserName= "John J. Peterson", Email="john.p@comany2.com",Init_Password="J.Pe1234!",
            //         Role = "Auditor", Reason_For_Access = "Access to 1) all reports; 2) server system logs for \"Audit\" and [app]_Access_Log" },
            //     new UserInfor {UserName="Chen, Mei 陈梅",Email="chehmei12@123.com",Init_Password="<:-)>{;=0}",
            //         Role = "ReadOnly", Reason_For_Access="我负责中国分公司财务"}

            List<UserInfor> ItemList = new List<UserInfor>();
            var connectionString = ConfigurationManager.ConnectionStrings["QuintilesIMS_test"].ConnectionString;
            string queryString = "SELECT Name, Email, Init_Password, Role, Reason_For_Access FROM dbo.Tb_Batch_Item";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ItemList.Add(new UserInfor
                        {
                            UserName = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            Init_Password = reader[2].ToString(),
                            Role = reader[3].ToString(),
                            Reason_For_Access = reader[4].ToString()
                        });
                    }
                }

            }
            return ItemList;
        }
    }
}
