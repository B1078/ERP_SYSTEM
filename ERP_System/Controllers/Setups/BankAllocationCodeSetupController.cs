﻿using ERP_System.Models.BusinessPartners;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ERP_System.Models.Setups;
using System.Data;
using ERP_System.Models.Masters;
using Newtonsoft.Json;

namespace ERP_System.Controllers.Setups
{
    public class BankAllocationCodeSetupController : Controller
    {
        public IActionResult BankAllocationCodeSetup()
        {
            if (HttpContext.Session.GetString("User_Id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult GETBANKCODE()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select * from[Bank_AllCode_Mst] ";
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
        public ActionResult POSTDATAExcel(string data)
        {
            string connectionString = HttpContext.Session.GetString("ConnectionString");

            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeSpan.MaxValue.Ticks));
                List<BankAllocationCodeSetup> units = JsonConvert.DeserializeObject<List<BankAllocationCodeSetup>>(data);

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

                            string insertQuery = genrate.GenerateInsertQuery(unit, "[Bank_AllCode_Mst]", "BankAllCodeId");

                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                // Example: Assuming 'insertQuery' is parameterized properly. Adjust accordingly.
                                cmd.CommandTimeout = 300;

                                // You may add parameters here to avoid SQL Injection
                                // e.g. cmd.Parameters.AddWithValue("@paramName", value);

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
        public IActionResult POSTBANKCODE(BankAllocationCodeSetup Data)
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
                string insertQuery = genrate.GenerateInsertQuery(Data, "[Bank_AllCode_Mst]", "BankAllCodeId");
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
                return Json(new { Success = true, Message = " All Bank Charges Allocation Code Added Successfully..!" });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, "A Bank Charges Allocation Code with this Code already exists. Please use a different Code.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
        public IActionResult UPDATEDATA(BankAllocationCodeSetup Data)
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
                string Query = genrate.GenerateUpdateQuery(Data, "[Bank_AllCode_Mst]", "BankAllCodeId", Data.BankAllCodeId, "");
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
                return Json(new { Success = true, Message = " All Bank Charges Allocation Code Successfully..!" });
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

                string Query = "Delete from [Bank_AllCode_Mst] where BankAllCodeId='" + Id + "'";
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
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
                return Json(new { success = true, message = "All Bank Charges Allocation Code Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}