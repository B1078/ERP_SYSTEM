using ERP_System.Models;
using ERP_System.Models.Purchase;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ERP_System.Controllers.Purchase
{
    public class PurchaseBlanketAgreementController : Controller
    {
        public IActionResult PurchaseBlanketAgreement()
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
                string Query = @"
                                select BLA.* , BPM.BPName , concat(BPM1.Fname,' ',BPM1.Mname,BPM1.Lname) as ConnPerName ,
                                CONVERT(VARCHAR(10), BLA.SignDate, 105) AS SignDate1,
                                CONVERT(VARCHAR(10), BLA.StartDate, 105) AS StartDate1,
                                CONVERT(VARCHAR(10), BLA.EndDate, 105) AS EndDate1,
                                CONVERT(VARCHAR(10), BLA.TermDate, 105) AS TermDate1
                                ,CM.CntryCurrCode ,FM.Finyr ,PM.PrjCode ,PM.PrjName 
                                ,PTM.PymntCode ,STM.ShipTypeName ,concat(EM.EmpFname,' ',EM.EmpLname ) as owner,
                                PLM.PListName 
                                from BlKAgrmt BLA
                                inner join BP_Mst BPM on BLA.BPId = BPM.BPId and BPM.IsActive ='Y'
                                left outer join BPCont_Det BPM1 on BLA.ConnBPId = BPM1.BPContDetId and BPM1.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = BLA.BPCurrId and CM.IsActive ='Y'
                                left outer join Finyr_mst FM on FM.FinyrId = BLA.FinyrId and FM.IsActive ='Y'
                                left outer join Project_Mst PM on PM.PrjId = BLA.PrjId and PM.IsActive ='Y'
                                left outer join Pymnt_Terms_Mst PTM  on PTM.PymntTId = BLA.PymntTId and PTM.IsActive ='Y'
                                left outer join ShippingType_mst STM on STM.ShipTypeId = BLA.ShipTypeId and STM.IsActive ='Y'
                                left outer join EmployeeMst EM on EM.EmpId = BLA.EmpId   
                                left outer join Price_List_Mst PLM on PLM.PListId = BLA.PListId and PLM.IsActive ='Y' where Trans_type ='P'  
                                ";
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
        public IActionResult GETDRAFT()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"
select BLA.* , BPM.BPName , concat(BPM1.Fname,' ',BPM1.Mname,BPM1.Lname) as ConnPerName ,
CONVERT(VARCHAR(10), BLA.SignDate, 105) AS SignDate1,
CONVERT(VARCHAR(10), BLA.StartDate, 105) AS StartDate1,
CONVERT(VARCHAR(10), BLA.EndDate, 105) AS EndDate1,
CONVERT(VARCHAR(10), BLA.TermDate, 105) AS TermDate1
,CM.CntryCurrCode ,FM.Finyr ,PM.PrjCode ,PM.PrjName 
,PTM.PymntCode ,STM.ShipTypeName ,concat(EM.EmpFname,' ',EM.EmpLname ) as owner,
PLM.PListName 
from BlKAgrmt BLA
inner join BP_Mst BPM on BLA.BPId = BPM.BPId and BPM.IsActive ='Y'
left outer join BPCont_Det BPM1 on BLA.ConnBPId = BPM1.BPContDetId and BPM1.IsActive ='Y'
left outer join Country_Mst CM on CM.CntryId = BLA.BPCurrId and CM.IsActive ='Y'
left outer join Finyr_mst FM on FM.FinyrId = BLA.FinyrId and FM.IsActive ='Y'
left outer join Project_Mst PM on PM.PrjId = BLA.PrjId and PM.IsActive ='Y'
left outer join Pymnt_Terms_Mst PTM  on PTM.PymntTId = BLA.PymntTId and PTM.IsActive ='Y'
left outer join ShippingType_mst STM on STM.ShipTypeId = BLA.ShipTypeId and STM.IsActive ='Y'
left outer join EmployeeMst EM on EM.EmpId = BLA.EmpId   
left outer join Price_List_Mst PLM on PLM.PListId = BLA.PListId and PLM.IsActive ='Y' where Trans_type ='P'  

";
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
        public IActionResult GETSALES()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"
  select BLA.* , BPM.BPName , concat(BPM1.Fname,' ',BPM1.Mname,BPM1.Lname) as ConnPerName ,
  CONVERT(VARCHAR(10), BLA.SignDate, 105) AS SignDate1,
  CONVERT(VARCHAR(10), BLA.StartDate, 105) AS StartDate1,
  CONVERT(VARCHAR(10), BLA.EndDate, 105) AS EndDate1,
  CONVERT(VARCHAR(10), BLA.TermDate, 105) AS TermDate1
  ,CM.CntryCurrCode ,FM.Finyr ,PM.PrjCode ,PM.PrjName 
  ,PTM.PymntCode ,STM.ShipTypeName ,concat(EM.EmpFname,' ',EM.EmpLname ) as owner,
  PLM.PListName 
  from BlKAgrmt BLA
  inner join BP_Mst BPM on BLA.BPId = BPM.BPId and BPM.IsActive ='Y'
  left outer join BPCont_Det BPM1 on BLA.ConnBPId = BPM1.BPContDetId and BPM1.IsActive ='Y'
  left outer join Country_Mst CM on CM.CntryId = BLA.BPCurrId and CM.IsActive ='Y'
  left outer join Finyr_mst FM on FM.FinyrId = BLA.FinyrId and FM.IsActive ='Y'
  left outer join Project_Mst PM on PM.PrjId = BLA.PrjId and PM.IsActive ='Y'
  left outer join Pymnt_Terms_Mst PTM  on PTM.PymntTId = BLA.PymntTId and PTM.IsActive ='Y'
  left outer join ShippingType_mst STM on STM.ShipTypeId = BLA.ShipTypeId and STM.IsActive ='Y'
  left outer join EmployeeMst EM on EM.EmpId = BLA.EmpId   
  left outer join Price_List_Mst PLM on PLM.PListId = BLA.PListId and PLM.IsActive ='Y' where Trans_type ='S'
";
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
        public IActionResult GETBPCON(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select * From  [BPCont_Det] where BPId='" + Id + "' and IsActive='Y'";
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
        public IActionResult GETATACHMENT(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select isnull(Remark,'')as Remark1 ,* From  [BlKAttach_Det]  where BlKAId='" + Id + "'";
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
        public IActionResult GETCURRPHONE(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select BPPhone1,BP_Mst.CntryId,CntryCurrCode,BPEmail
from BP_Mst 
inner join Country_Mst on BP_Mst.CntryId = Country_Mst.CntryId 
 where BP_Mst.BPId='" + Id + "' and BP_Mst.IsActive='Y'";
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
        public IActionResult GETDETAILS(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select BLAD.BlKItemId , BLAD.BlKAId , BLAD.ItemId , BLAD.LineNum , BLAD.ItemName , BLAD.ItemGrpId , BLAD.PlanQty ,
                                BLAD.UnitPrice , BLAD.ItemShipTypeId , BLAD.ItemPrjId , BLAD.CntryId , BLAD.RetPortion , 
                                CONVERT(nvarchar(10),BLAD.WrrtyEnd, 105) AS WrrtyEnd, BLAD.UomId , BLAD.ItemRemark , BLAD.BarCode , BLAD.OpenQty , BLAD.RowStatus ,
                                BLAD.ActualPrice , BLAD.PriceUpd ,IM.ItemName as Item  , IGM.ItemGrpname ,
                                STM.ShipTypeName ,PM.PrjCode ,PM.PrjName ,CM.CntryCurrCode ,UM.UomCode 
                                from BlKItem_Det BLAD
                                inner join BlKAgrmt BLA on BLA.BlKAId = BLAD.BlKAId 
                                inner join Item_Mst IM on IM.ItemId = BLAD.ItemId and IM.IsActive ='Y'
                                LEFT outer  join Item_Grp_Mst IGM on IGM.ItemGrpId = BLAD.ItemGrpId and IGM.IsActive ='Y'
                                left outer join ShippingType_mst STM on STM.ShipTypeId = BLAD.ItemShipTypeId and STM.IsActive ='Y'
                                left outer join Project_Mst PM on PM.PrjId = BLAD.ItemPrjId and PM.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = BLAD.CntryId and CM.IsActive ='Y'
                                left outer join Unit_Mst UM on UM.UomId = BLAD.UomId and UM.IsActive ='Y'
                                where BLAD.BlKAId = " + Id + "";
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
        public IActionResult GETBARCODE(string BarCodeId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select * from Item_BarCode_det where BarCodeDetId='" + BarCodeId + "' and IsActive='Y'";
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
        public IActionResult GETPRICE(string Itemid, string Pricelistid)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"Select Price From Price_List_Det where ItemId= '" + Itemid + "' and PListId='" + Pricelistid + "'";
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
        public IActionResult GETCOPYFROM(string Flag)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"";
                switch (Flag)
                {
                    case "PR":
                        Query = @"Select PR.* ,
                                            case when PR.ReqtType ='U' then 'User'
                                            else 'Employee' end Requester
                                            FROm Pur_Req  PR Where DraftFlag ='N' And Status='O'";
                        break;
                    case "PBA":
                        Query = @"Select BLKA.*,BM.BPName ,
                                        case when AgrmtMthd='I' then 'Item Method' 
                                        else 'Monetary Method' end as AgreementMethod,
                                        case when AgrmtType ='S' then 'Specific'
                                        else 'General' end as AgreementType
                                        FROm BlKAgrmt BLKA
                                        inner join BP_Mst BM on BM.BPId = BLKA.BPId and BM.IsActive ='Y'
                                        Where BLKA.Trans_type='P' AND BLKA.Status ='A'";
                        break;
                    default:
                        break;
                }
                List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        con.Open();
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

        [HttpPost]
        public IActionResult POSTUPDATE(BlancateAgreement item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Lastid = "";
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                if (item.BlKAId != "" && item.BlKAId != null)
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[BlKAgrmt]", "BlKAId", item.BlKAId, "");
                    if (item.BPId != null || item.BPId != "")
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
                    Lastid = item.BlKAId;
                }
                else
                {
                    item.BlKAId = null;
                    item.DocNum = GETDOCNUM(item.DraftFlag).ToString();
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (item.BPId != null || item.BPId != "")
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[BlKAgrmt]", "BlKAId");
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.CommandTimeout = 300;
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "SELECT MAX(BlKAId) AS BlKAId FROM BlKAgrmt;";
                                SqlDataReader rdr = cmd.ExecuteReader();
                                {
                                    while (rdr.Read())
                                    {
                                        Lastid = rdr["BlKAId"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                if (item.DraftFlag == "Y")
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Purchase Order  Drafted Successfully..!" });
                }
                else
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Purchase Order  Added Successfully..!" });
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public int GETDOCNUM(string flag)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"
SELECT ISNULL(MAX(DocNum),0)+1 AS DocNum
FROM BlKAgrmt 
inner join Finyr_mst on BlKAgrmt.FinyrId = Finyr_mst.FinyrId 
and Finyr_mst.IsActive ='Y'   where DraftFlag='" + flag + "'; ";
            int docnum = 0;
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    con.Open();
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    {
                        while (rdr.Read())
                        {
                            docnum = Convert.ToInt32(rdr["DocNum"]);
                        }
                    }
                    con.Close();
                }
            }
            return docnum;

        }
        public IActionResult ADDITEMATTACHMENT([FromBody] List<PurAgrementAttachment> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    if (item.BlKAttachId != "" && item.BlKAttachId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[BlKAttach_Det]", "BlKAttachId", item.BlKAttachId, "");
                        if (item.BlKAId != null || item.BlKAId != "")
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
                        item.BlKAttachId = null;
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.BlKAId != null || item.BlKAId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[BlKAttach_Det]", "BlKAttachId");
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
                return Json(new { Success = true, Message = "Attachment Details Saved  Successfully..!" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult ADDITEMDETAILS([FromBody] List<BlKItem_Det_mst> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                int index = 1;


                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {

                    item.LineNum = index.ToString();
                    if (item.BlKItemId != "" && item.BlKItemId != null)
                    {
                        if (item.RowStatus == "O")
                        {
                            item.OpenQty = item.PlanQty;
                        }

                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[BlKItem_Det]", "BlKItemId", item.BlKItemId, "");
                        if (item.BlKAId != null || item.BlKAId != "")
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
                        item.BlKItemId = null;
                        item.OpenQty = item.PlanQty;
                        item.RowStatus = "O";

                        Genrate_Query genrate = new Genrate_Query();
                        if (item.BlKAId != null || item.BlKAId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[BlKItem_Det]", "BlKItemId");
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
                    index++;
                }
                
                return Json(new { Success = true, Message = "Purchase Blanket Aggreement Item Details Added  Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public void COPYFROMSTATUSCHECK(string ID)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"exec  [dbo].[usp_UpdBaseDocData] 'BlKAgrmt','BlKItem_Det','BlKAId','" + ID + "' ";
            if (ID != "")
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }

        }
        public IActionResult DELETEATTACHMENT(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [Attach_Det] where AttchDetId='" + Id + "'";
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
                return Json(new { success = true, message = "Attachment Deleted Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult DELETEMAINDATA(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [BlKAgrmt] where BlKAId='" + Id + "'";
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
                return Json(new { success = true, message = "Purchase Blanket Aggrrement  Deleted Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult DELETEDETAILS(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [BlKItem_Det] where BlKItemId='" + Id + "'";
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
                return Json(new { success = true, message = "Item Deleted Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}
