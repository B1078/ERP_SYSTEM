using ERP_System.Models.User;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace ERP_System.Controllers.Administration
{
    public class UserAuthorizationController : Controller
    {
        public IActionResult UserAuthorization()
        {
			if (HttpContext.Session.GetString("User_Id") == null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
        }
	    [HttpGet]
		public IActionResult MENU_MST_DATA()
		{
			try
			{
				string ConnectionString = HttpContext.Session.GetString("ConnectionString");
				string Query = @"Select * From [Menu_Mst] where IsActive='Y'";
				List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
				using (SqlConnection con = new SqlConnection(ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(Query, con))
					{
						con.Open();
						cmd.CommandText = Query;
						cmd.CommandTimeout = 300;
						SqlDataReader rdr = cmd.ExecuteReader();
						if (rdr.HasRows)
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
        public IActionResult GETMINOTARY()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"Select T1.FieldName, * From Menu_Tab_Det T0
Left Join Menu_TabMand_Det T1 on T0.MenuId =T1.MenuId
where T0.TableName ='UserAuth_Mnu_det' And T1.IsMandatory='Y'";
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        con.Open();
                        cmd.CommandText = Query;
                        cmd.CommandTimeout = 300;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
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
        public IActionResult USERAUTHDATA(string id)
		{
			try
			{
				string ConnectionString = HttpContext.Session.GetString("ConnectionString");
				string Query = @" Select T1.MenuName,T1.MenuID,T0.AuthType,T0.AuthMnuDetId From [UserAuth_Mnu_det] T0
				inner join Menu_Mst T1 On T1.MenuId = T0.MenuId And T1.IsActive='Y'
				Where T0.UserId='" + id + "'";
				List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
				using (SqlConnection con = new SqlConnection(ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(Query, con))
					{
						con.Open();
						cmd.CommandText = Query;
						cmd.CommandTimeout = 300;
						SqlDataReader rdr = cmd.ExecuteReader();
						if (rdr.HasRows)
						{
							while (rdr.Read())
							{
								Dictionary<string, object> row = new Dictionary<string, object>();
								for (int i = 0; i < rdr.FieldCount; i++)
								{
									row.Add(rdr.GetName(i), rdr[i].ToString().Trim());
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
		public IActionResult ADDAUTHORIZATION(AuthData Data)
		{
			string ConnectionString = HttpContext.Session.GetString("ConnectionString");
			try
			{
				TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
				long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
				Genrate_Query genrate = new Genrate_Query();
				List<dynamic> list = new List<dynamic>();
				for (int i = 0; i < Data.Maindata.Length; i++)
				{
					Data.Maindata[i].UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
					Data.Maindata[i].UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
					Data.Maindata[i].CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
					Data.Maindata[i].UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
					Data.Maindata[i].CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
					Data.Maindata[i].CreatedBy = HttpContext.Session.GetString("UserName");
					Data.Maindata[i].UpdatedBy = HttpContext.Session.GetString("UserName");
					list.Add(genrate.GenerateInsertQuery(Data.Maindata[i], "[UserAuth_Mnu_det]", "MenuId"));

				}
				using (SqlConnection con = new SqlConnection(ConnectionString))
				{
					con.Open();
					for (int i = 0; i < list.Count; i++)
					{
						using (SqlCommand cmd = new SqlCommand(list[i], con))
						{
							cmd.CommandText = list[i];
							cmd.CommandTimeout = 300;
							cmd.ExecuteNonQuery();
						}
					}
					con.Close();
					
				}
				return Json(new { Success = true, Message = " Authorization Added Successfully..!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message.ToString());
			}
		}
		public IActionResult UPDATEAUTHORIZATION(AuthData Data)
		{
			string ConnectionString = HttpContext.Session.GetString("ConnectionString");
			try
			{
				TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
				long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
				Genrate_Query genrate = new Genrate_Query();
				List<dynamic> list = new List<dynamic>();
				for (int i = 0; i < Data.Maindata.Length; i++)
				{
					Data.Maindata[i].UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
					Data.Maindata[i].UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
					Data.Maindata[i].UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
					Data.Maindata[i].UpdatedBy = HttpContext.Session.GetString("UserName");
					list.Add(genrate.GenerateUpdateQuery(Data.Maindata[i], "[UserAuth_Mnu_det]", "AuthMnuDetId", Data.Maindata[i].AuthMnuDetId, ""));
				}
				using (SqlConnection con = new SqlConnection(ConnectionString))
				{
					con.Open();
					for (int i = 0; i < list.Count; i++)
					{
						string Query = list[i] + "And UserId='" + Data.Maindata[i].UserId + "'";
						using (SqlCommand cmd = new SqlCommand(Query, con))
						{
							cmd.CommandText = list[i];
							cmd.CommandTimeout = 300;
							cmd.ExecuteNonQuery();
						}
					}
					con.Close();

				}
				return Json(new { Success = true, Message = " Authorization Updated Successfully..!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message.ToString());
			}
		}
		public IActionResult DELETEAUTHDATA(string Id)
		{
			string ConnectionString = HttpContext.Session.GetString("ConnectionString");
			try
			{

				string Query = "Delete from [UserAuth_Mnu_det] where UserId='" + Id + "'";
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
				return Json(new { success = true, message = "Authorization Deleted Successfully..!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message.ToString());
			}
		}
	}
}
