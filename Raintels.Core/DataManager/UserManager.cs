using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Raintels.Core.Interface;
using Raintels.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Raintels.Core.DataManager
{
    public class UserManager : IUserManager
    {
        private readonly string connectionString;
        public UserManager(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("RaintelsDB");
        }

        public int CreateUser(UserModel user)
        {
            int Id = 0;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("CreateUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_Email", user.Email);
                    cmd.Parameters.AddWithValue("P_GoogleID", user.GoogleID);
                    con.Open();
                    using (MySqlDataAdapter adap = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adap.Fill(dt);
                        if (dt != null & dt.Rows.Count > 0)
                            Id = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                    con.Close();
                    return Id;

                }

            } 
        }
    }
}
