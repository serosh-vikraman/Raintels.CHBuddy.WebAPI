using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Raintels.Core.Interface;
using Raintels.Entity;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                    cmd.Parameters.AddWithValue("P_EventDetails", eventDetails.EventDetails);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    return eventDetails;

                }

            }
        }

        public async Task<List<EventDataModel>> GetEvent(long userId, long EventId, string EventCode)
        {
            List<EventDataModel> eventList = new List<EventDataModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_UserId", userId);
                    cmd.Parameters.AddWithValue("P_EventId", EventId);
                    cmd.Parameters.AddWithValue("P_EventCode", EventCode);
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
                                EventDetails = dt.Rows[i]["EventDetails"].ToString(),
                                EventCode = dt.Rows[i]["EventCode"].ToString(),
                                EventStartDateTIme = dt.Rows[i]["EventStartDateTIme"].ToString(),
                                EventEndDateTIme = dt.Rows[i]["EventEndDateTIme"].ToString(),
                                CreatedBy = Convert.ToInt32(dt.Rows[i]["CreatedBy"].ToString()),
                            });
                        }
                    }
                    con.Close();
                }
            }
            return eventList;
        }


        public async Task<EventAnalysisDataModel> ManageEventAnalysis(EventAnalysisDataModel eventAnalysisDetails, int type)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("ManageEventAnalysis", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_Id", 0);
                    cmd.Parameters.AddWithValue("P_EventID", eventAnalysisDetails.EventID);
                    cmd.Parameters.AddWithValue("P_Type", type);
                    cmd.Parameters.AddWithValue("P_QnACount", eventAnalysisDetails.QnACount);
                    cmd.Parameters.AddWithValue("P_QnALikeCount", eventAnalysisDetails.QnALikeCount);
                    cmd.Parameters.AddWithValue("P_AddOrDecrease", eventAnalysisDetails.AddOrDecrease);

                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    return eventAnalysisDetails;

                }

            }
        }

        public async Task<List<EventAnalysisDataModel>> GetEventAnalysis(long EventId)
        {
            List<EventAnalysisDataModel> eventList = new List<EventAnalysisDataModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetEventAnalysis", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_EventId", EventId);
                    con.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        await sda.FillAsync(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            eventList.Add(new EventAnalysisDataModel()
                            {
                                EventID = Convert.ToInt32(dt.Rows[i]["EventID"].ToString()),
                                QnACount = Convert.ToInt32(dt.Rows[i]["QnACount"].ToString()),
                                QnALikeCount = Convert.ToInt32(dt.Rows[i]["QnALikeCount"].ToString()),

                            });
                        }
                    }
                    con.Close();
                }
            }
            return eventList;
        }

        public async Task<PollDataModel> savePoll(PollDataModel pollDetails)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SavePoll", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_Id", 0);
                    cmd.Parameters.AddWithValue("P_EventID", pollDetails.EventID);
                    cmd.Parameters.AddWithValue("P_PollType", pollDetails.PollType);
                    cmd.Parameters.AddWithValue("P_PollTitle", pollDetails.PollTitle);
                    cmd.Parameters.AddWithValue("P_isCorrectAnswerApplicable", pollDetails.isCorrectAnswerApplicable);
                    cmd.Parameters.AddWithValue("P_isMultipeCorrectAnswerApplicable", pollDetails.isMultipeCorrectAnswerApplicable);
                    cmd.Parameters.AddWithValue("P_IsActive", pollDetails.IsActive);
                    cmd.Parameters.AddWithValue("P_ParticipatedCount", pollDetails.ParticipatedCount);
                    cmd.Parameters.AddWithValue("P_CorrectAnswerCount", pollDetails.CorrectAnswerCount);
                    cmd.Parameters.AddWithValue("P_PollOptions", pollDetails.xmlPollOptions);

                    con.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    await adapter.FillAsync(ds);
                    // await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    int outPut = Convert.ToInt32(ds.Tables[0].Rows[0][0]);




                    return pollDetails;

                }
            }
        }

        public async Task<List<PollUserViewModel>> GetPollByCode(string EventCode)
        {
            List<PollUserViewModel> pollList = new List<PollUserViewModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetPollByCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_EventCode", EventCode);
                    con.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        await sda.FillAsync(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            pollList.Add(new PollUserViewModel()
                            {
                                EventCode = dt.Rows[i]["EventCode"].ToString(),
                                PollId = Convert.ToInt32(dt.Rows[i]["PollId"].ToString()),
                                PollTitle = dt.Rows[i]["PollTitle"].ToString(),
                                isCorrectAnswerApplicable = Convert.ToInt32(dt.Rows[i]["isCorrectAnswerApplicable"].ToString()),
                                isMultipeCorrectAnswerApplicable = Convert.ToInt32(dt.Rows[i]["isMultipeCorrectAnswerApplicable"].ToString()),

                            });
                        }
                    }
                    con.Close();
                }
            }
            return pollList;
        }

        public async Task<List<PollOptionsViewModel>> GetPollOptions(long PollId)
        {
            List<PollOptionsViewModel> pollList = new List<PollOptionsViewModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetPollOptions", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_PollId", PollId);
                    con.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        await sda.FillAsync(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            pollList.Add(new PollOptionsViewModel()
                            {
                                OptionID = Convert.ToInt32(dt.Rows[i]["OptionID"].ToString()),
                                OptionTitle = dt.Rows[i]["OptionTitle"].ToString(),
                                isCorrect = Convert.ToBoolean(dt.Rows[i]["isCorrect"].ToString()),
                                IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"].ToString()),
                                PollID = Convert.ToInt32(dt.Rows[i]["PollID"].ToString()),

                            });
                        }
                    }
                    con.Close();
                }
            }
            return pollList;
        }

        public async Task<List<PollAnswerMarkingViewModel>> savePollOptionByUser(List<PollAnswerMarkingViewModel> pollDetails)
        {
            var xmlElm = new XElement("TableDetails",
             from ObjDetails in pollDetails
             select new XElement("TableDetail",
                          new XElement("userID", ObjDetails.userID),
                          new XElement("PollID", ObjDetails.PollID),
                          new XElement("OptionID", ObjDetails.OptionID)
                        ));



            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SavePollAnswers", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("P_Options", xmlElm);

                    con.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    await adapter.FillAsync(ds);
                    // await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    int outPut = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    return pollDetails;

                }
            }


        }

        public async Task<List<EventDataModel>> GetLatestEvent(long userId)
        {
            List<EventDataModel> eventList = new List<EventDataModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetLatestEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_UserId", userId);

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
                                EventDetails = dt.Rows[i]["EventDetails"].ToString(),
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
