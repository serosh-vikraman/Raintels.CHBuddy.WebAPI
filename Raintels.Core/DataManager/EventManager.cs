using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Raintels.Core.Interface;
using Raintels.Entity;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Raintels.Core.DataManager
{
    public class EventManager : IEventManager
    {
        private readonly string connectionString;
        public EventManager(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("RaintelsDB");
        }

        public async Task<EventDataModel> CreateEvent(EventDataModel eventDetails)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("saveEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_ID", eventDetails.EventID);
                    cmd.Parameters.AddWithValue("P_EventName", eventDetails.EventName);
                    cmd.Parameters.AddWithValue("P_SocialPlatform", eventDetails.SocialPlatform);
                    cmd.Parameters.AddWithValue("P_EventCode", eventDetails.EventCode);
                    cmd.Parameters.AddWithValue("P_EventStartDateTIme", eventDetails.EventStartDateTIme);
                    cmd.Parameters.AddWithValue("P_EventEndDateTIme", eventDetails.EventEndDateTIme);
                    cmd.Parameters.AddWithValue("P_TimeZone", eventDetails.TimeZone);
                    cmd.Parameters.AddWithValue("P_CreatedBye", eventDetails.CreatedBy);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    return eventDetails;

                }

            }
        }

        public async Task<List<EventDataModel>> GetEvent(long userId, long EventId)
        {
            List<EventDataModel> eventList = new List<EventDataModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_UserId", userId);
                    cmd.Parameters.AddWithValue("P_EventId", EventId);
                    con.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        await sda.FillAsync(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            eventList.Add(new EventDataModel()
                            {
                                EventID = Convert.ToInt32(dt.Rows[i]["EventID"].ToString()),
                                EventName = dt.Rows[i]["EventName"].ToString(),
                                EventCode = dt.Rows[i]["EventCode"].ToString(),
                                EventStartDateTIme = dt.Rows[i]["EventStartDateTIme"].ToString(),
                                EventEndDateTIme = dt.Rows[i]["EventEndDateTIme"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return eventList;
        }
    }
}
