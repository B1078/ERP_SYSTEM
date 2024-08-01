using ERP_System.Models.User;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ERP_System.Models.Setups;
using ERP_System.Models.Inventory;

namespace ERP_System.Controllers.Setups
{
    public class MandatoryFieldSetupController : Controller
    {
        public IActionResult MandatoryFieldSetup()
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
				string Query = @"select distinct MM.MenuName , mm.MenuId 
												from Menu_Mst MM
												inner join Menu_Tab_Det MTD on MTD.MenuId = mm.MenuId  ";
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
		public IActionResult GETTABLEDATA(string id)
		{
			try
			{
				string ConnectionString = HttpContext.Session.GetString("ConnectionString");
				string Query = @"select tablename from Menu_Tab_Det
where menuid = "+id+"";
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
		public IActionResult TABLEDATA(string id)
		{
			try
			{
				string ConnectionString = HttpContext.Session.GetString("ConnectionString");
				string Query = @"   SELECT 
								MM.MenuId,
								MM.menuName,
								MTD.TableName,
								C.COLUMN_NAME,
								MTMD.MTMandId,
								IsMandatory = CASE  
												WHEN C.IS_NULLABLE = 'YES' THEN 'N'
												ELSE 'Y' 
											 END
							FROM 
								menu_mst MM 
							INNER JOIN 
								Menu_Tab_Det MTD ON MM.MenuId = MTD.MenuId AND MM.IsActive = 'Y'
							INNER JOIN 
								INFORMATION_SCHEMA.COLUMNS C ON MTD.TableName = C.TABLE_NAME
								AND C.COLUMN_NAME NOT IN ('IsActive', 'CreatedBy', 'CreateDate', 'CreateTS', 'UpdatedBy', 'UpdateDate', 'UpdateTS')
								AND COLUMNPROPERTY(OBJECT_ID(C.TABLE_SCHEMA + '.' + C.TABLE_NAME), C.COLUMN_NAME, 'IsIdentity') = 0
							LEFT JOIN 
								Menu_TabMand_Det MTMD ON MTD.MenuId = MTMD.MenuId AND C.COLUMN_NAME = MTMD.FieldName
							WHERE 
								MTD.TableName  = '" + id + "';";
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
        public IActionResult POSTDATA([FromBody] List<Mdfield_setup_mst> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    if (item.MTMandId != "" && item.MTMandId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Menu_TabMand_Det]", "MTMandId", item.MTMandId, "");
                        if (item.MenuId != null || item.MenuId != "")
                        {
                            using (SqlConnection con = new SqlConnection(ConnectionString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(Query, con))
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {
                        item.MTMandId = null;
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.MenuId != null || item.MenuId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Menu_TabMand_Det]", "MTMandId");
                            using (SqlConnection con = new SqlConnection(ConnectionString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                return Json(new { Success = true, Message = "Madatory Feilds Details Saved  Successfully..!" });

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

