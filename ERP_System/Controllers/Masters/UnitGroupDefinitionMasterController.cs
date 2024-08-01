using ERP_System.Models.Masters;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace ERP_System.Controllers.Masters
{
    public class UnitGroupDefinitionMasterController : Controller
    { 
     public IActionResult UnitGroupDefinitionMaster()
    {
        if (HttpContext.Session.GetString("User_Id") == null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpGet]
    public IActionResult GETDATA()
    {
        try
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @" select Unit_GRP_DefMst.* ,UGM1.UomGRPCode as StockUGrpCode,UGM1.UomGRPName as StockUGrpName ,
                                UGM2.UomGRPCode as BaseGrpCode, UGM2.UomGRPName as BaseUGrpName
                                from Unit_GRP_DefMst 
                                inner join Unit_GRP_Mst UGM1 on UGM1.UomGRPId = Unit_GRP_DefMst.StkUomGRPId and UGM1.IsActive ='Y'
                                inner join Unit_GRP_Mst UGM2 on UGM2.UomGRPId = Unit_GRP_DefMst.BaseUomGRPId and UGM2.IsActive ='Y'";
                                                                                                  
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
                                    row[columnName] = dateValue.ToString("dd /MM/yyyy");
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

    public IActionResult POSTDATA(UnitGrpD_mst Data)
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
            string insertQuery = genrate.GenerateInsertQuery(Data, "[Unit_GRP_DefMst]", "UomGRPDefId");
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
            return Json(new { Success = true, Message = " Unit GroupDefinition Master Added Successfully..!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message.ToString());
        }
    }
    public IActionResult UPDATEDATA(UnitGrpD_mst Data)
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
            string Query = genrate.GenerateUpdateQuery(Data, "[Unit_GRP_DefMst]", "UomGRPDefId", Data.UomGRPDefId, "");
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
            return Json(new { Success = true, Message = "Unit Group Definition Master Updated Successfully..!" });
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

            string Query = "Delete from [Unit_GRP_DefMst] where UomGRPDefId='" + Id + "'";
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
            return Json(new { success = true, message = "Unit Group Definition Master  Deleted Successfully..!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message.ToString());
        }
    }
}
}
