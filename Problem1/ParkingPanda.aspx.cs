using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Problem1.Models;

namespace Problem1
{
    public partial class ParkingPanda : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    logged.Visible = false;
            //    user.Visible = false;
            //    save.Visible = false;
            //}
            lbl_notify.Visible = false;
        }

        protected async void btn_Login_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //ApiRequest(txt_userName.Text, txt_password.Text, "users");
                using (var client = CreateHttpClient(txt_userName.Text, txt_password.Text))
                {
                    var apiResponse = await client.GetAsync("users");
                    ApiResponse(apiResponse);
                }
            }
        }

        protected async void btn_Save_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //get userID and apiPassword from cookies
                var apiUser = Request.Cookies["apiUser"];
                if (apiUser != null)
                {
                    var userID = apiUser["UserID"];
                    var apiPassword = apiUser["apiPassword"];
                    var userEmail = apiUser["UserEmail"];

                    //ApiRequest(txt_email.Text, apiPassword, "users/" + userID);
                    using (var client = CreateHttpClient(userEmail, apiPassword))
                    {
                        var updateUser = new UpdateUserData
                        {
                            Email = txt_email.Text,
                            ReceiveEmail = cbox_email.Checked,
                            FirstName = txt_firstName.Text,
                            LastName = txt_lastName.Text,
                            Phone = txt_phone.Text,
                            ReceiveSmsNotifications = cbox_sms.Checked,
                            Password = txt_changePwd.Text == "" ? null : txt_changePwd.Text,
                            CurrentPassword = txt_currentPwd.Text == "" ? null : txt_currentPwd.Text
                        };
                        var response = await client.PutAsJsonAsync<UpdateUserData>("users/" + userID, updateUser);

                        ApiResponse(response);
                    }
                }
            }

        }

        protected void btn_Logout_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["apiUser"] != null)
            {
                HttpCookie myCookie = new HttpCookie("apiUser");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            ClearTextBox(this.Controls);
        }

        /// <summary>
        /// Clear all the textbox in recursive way
        /// </summary>
        /// <param name="cc"></param>
        private void ClearTextBox(ControlCollection cc)
        {
            foreach (Control item in cc)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
                if (item.Controls != null)
                {
                    ClearTextBox(item.Controls);
                }
            }
        }

        private void ApiResponse(HttpResponseMessage apiResponse)
        {
            //deserialize JSON result 
            var result = JsonConvert.DeserializeObject<ResponseOfUser>(
                    apiResponse.Content.ReadAsStringAsync().Result);
            if (apiResponse.IsSuccessStatusCode)
            {
                PopulateUserData(result);
                login.Visible = false;
                logged.Visible = true;
                user.Visible = true;
                save.Visible = true;
            }          
            lbl_notify.Text = result.Message;
            lbl_notify.Visible = true;
        }

        private void PopulateUserData(ResponseOfUser result)
        {
            //Populate user information to controls
            var user = result.Data;
            txt_firstName.Text = user.FirstName;
            txt_lastName.Text = user.LastName;
            txt_email.Text = user.Email;
            txt_phone.Text = user.Phone;
            cbox_email.Checked = user.ReceiveEmail;
            cbox_sms.Checked = user.ReceiveSmsNotifications;

            lbl_loggedUser.Text = user.FirstName;

            CreateCookies(user);
        }

        /// <summary>
        /// Generate cookies for subrequest to api
        /// </summary>
        /// <param name="user"></param>
        /// 
        private void CreateCookies(UserData user)
        {
            //generate user cookies
            HttpCookie userApiCred = new HttpCookie("apiUser");
            userApiCred.Expires = DateTime.Now.AddHours(12);
            userApiCred.Values["UserID"] = user.Id.ToString();
            userApiCred.Values["apiPassword"] = user.ApiPassword;
            userApiCred.Values["UserEmail"] = user.Email;

            Response.Cookies.Add(userApiCred);
        }

        /// <summary>
        /// Create httpclient for each api request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private HttpClient CreateHttpClient(string username, string password)
        {
            //generate and encode user login credential to pass
            var loginCred = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{username}:{password}"));

            //Use httpclient to send request
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://dev.parkingpanda.com/api/v2/");
            //implement basic authentication
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", loginCred);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


    }
}