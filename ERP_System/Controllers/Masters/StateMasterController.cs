using ERP_System.Models;
using ERP_System.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace ERP_System.Controllers.Masters
{
    public class StateMasterController : Controller
    {
        private readonly ILogger<StateMasterController> _logger;
        private readonly IConfiguration _configuration; // Add this field
        private string Maindbstring;
        public StateMasterController(ILogger<StateMasterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; // Inject IConfiguration
            Maindbstring = _configuration["Erp_Db_Con2"];
        }
        public IActionResult StateMaster()
        {
            if (HttpContext.Session.GetString("User_Id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult GET()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select SM.*,CM.CntryName 
from State_Mst SM WITH (NOLOCK)
inner join Country_Mst CM on CM.CntryId = SM.CntryId ";
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        con.Open();
                        cmd.CommandText = Query;
                        cmd.CommandTimeout = 300;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        {
                            while (rdr.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < rdr.FieldCount; i++)
                                {
                                    string columnName = rdr.GetName(i);
                                    object? value = rdr.IsDBNull(i) ? null : rdr.GetValue(columnName);
                                    // Convert DATE values to string format without time
                                    if (value is DateTime dateValue && dateValue.TimeOfDay == TimeSpan.Zero)
                                    {
                                        row[columnName] = dateValue.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        row[columnName] = value!;
                                    }
                                }
                                dataList.Add(row);
                            }
                        }
                        con.Close();
                    }
                }
                return Json(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        [HttpPost]

        public IActionResult POSTDATA(StateMaster Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                Data.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                Data.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                Data.UpdatedBy = HttpContext.Session.GetString("UserName");
                Data.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                Data.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                Data.CreatedBy = HttpContext.Session.GetString("UserName");
                Genrate_Query genrate = new Genrate_Query();
                string insertQuery = genrate.GenerateInsertQuery(Data, "[State_Mst]", "StateId");
                var responce = MainDbADD(insertQuery);
                if (responce)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.CommandText = insertQuery;
                            cmd.CommandTimeout = 300;
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();

                    }
                    return Json(new { Success = true, Message = " State Details Added Successfully..!" });
                }
                else { 
                    return Json(new { Success = false, Message = " Error To Add State Details..!" });
                }
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, "A State Details with this State Code Code already exists. Please use a different Code.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        public ActionResult BULKPOST(string data)
        {
            string connectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeSpan.MaxValue.Ticks));
                List<StateMaster> units = JsonConvert.DeserializeObject<List<StateMaster>>(data);

                if (units == null || !units.Any())
                {
                    return StatusCode(400, "Data is null or empty.");
                }

                Genrate_Query genrate = new Genrate_Query();
                List<dynamic> errlist = new List<dynamic>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    foreach (var unit in units)
                    {
                        try
                        {
                            unit.UpdateDate = DateOnly.FromDateTime(DateTime.Now);
                            unit.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                            unit.UpdatedBy = HttpContext.Session.GetString("UserName");
                            unit.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                            unit.CreateDate = DateOnly.FromDateTime(DateTime.Now);
                            unit.CreatedBy = HttpContext.Session.GetString("UserName");
                            string insertQuery = genrate.GenerateInsertQuery(unit, "[State_Mst]", "StateId");
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                // Example: Assuming 'insertQuery' is parameterized properly. Adjust accordingly.
                                cmd.CommandTimeout = 300;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            unit.errormessage = ex.Message;
                            errlist.Add(unit);
                        }
                    }
                }
                string responce = JsonConvert.SerializeObject(errlist);
                return StatusCode(200, responce);
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exception details here
                return StatusCode(500, $"SQL Error: {sqlEx.Message}");
            }
            catch (JsonSerializationException jsonEx)
            {
                // Log JSON serialization exception details here
                return StatusCode(400, $"Data Parsing Error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                // Log general exception details here
                return StatusCode(500, $"General Error: {ex.Message}");
            }
        }
        public IActionResult UPDATEDATA(StateMaster Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                Data.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                Data.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                Data.UpdatedBy = HttpContext.Session.GetString("UserName");
                Genrate_Query genrate = new Genrate_Query();
                string Query = genrate.GenerateUpdateQuery(Data, "[State_Mst]", "StateId", Data.StateId, "");
                var responce = MainDbADD(Query);
                if (responce)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(Query, con))
                        {
                            cmd.CommandText = Query;
                            cmd.CommandTimeout = 300;
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    return Json(new { Success = true, Message = " State Details Updated Successfully..!" });
                }
                else 
                {
                    return Json(new { Success = false, Message = "Error To Update State Details  ..!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult DELETE(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [State_Mst] where StateId='" + Id + "'";
                var responce = MainDbADD(Query);
                if (responce)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(Query, con))
                        {
                            con.Open();
                            cmd.CommandText = Query;
                            cmd.CommandTimeout = 300;
                            cmd.ExecuteNonQuery();
                            SqlDataReader rdr = cmd.ExecuteReader();
                            con.Close();
                        }
                    }
                    return Json(new { success = true, message = "State Details Deleted Successfully..!" });
                }
                else
                {
                    return Json(new { success = false, message = "Error To Delete State Details  ..!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        public bool MainDbADD(string Query)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Maindbstring))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        cmd.CommandText = Query;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
