using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuintilesIMS_test_p2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get UserInformation from in-memory collection
            UserInfor userInfor = new UserInfor();
            List<UserInfor> userList = userInfor.GetUserList();

            //check each user's password
            foreach (var user in userList)
            {               
                if (PasswordValidator.IsValid(user.UserName,user.Email,user.Init_Password))
                {
                    Console.WriteLine("{0}'s password is valid",user.UserName);
                }
                else
                {
                    Console.WriteLine("{0}'s password is invalid",user.UserName);
                    //display error message
                    foreach (var error in PasswordValidator.ErrorMessage)
                    {
                        Console.WriteLine("  ! " + error);
                    }
                    PasswordValidator.ErrorMessage.Clear();
                    //create new password
                    user.Init_Password = PasswordGenerator.GetPassword(user.UserName,user.Email);
                    Console.WriteLine("  {0}'s new password is {1}", user.UserName, user.Init_Password);
                }
                Console.WriteLine();
            }

            //create batch_item xml file
            Batch_Item_Writer writer = new Batch_Item_Writer();
            writer.GenerateXML(userList);
            writer.GenerateCSV(userList);
        }
    }
}
