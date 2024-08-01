using ERP_System.Models.Purchase;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ERP_System.Controllers.Purchase
{
    public class PurchaseRequestController : Controller
    {
        public IActionResult PurchaseRequest()
        {
            if (HttpContext.Session.GetString("User_Id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult PurchaseRequestTest()
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
                string Query = @"select PR.PurReqId ,PR.ReqtType ,PR.ReqtId ,[user].UserFulname ,concat(EM1.EmpFname, ' ',EM1.EmpLname ) as ReqEmpName
                                    ,PR.DeptId ,PR.EmailAddr ,
                                    PR.Notify ,PR.StateId ,PR.BrchId ,PR.BrchGSTINNo ,PR.FinyrId ,PR.FinyrNum ,PR.Status ,
                                    CONVERT(VARCHAR(10), PR.PostingDt, 105) as PostingDt,
                                    CONVERT(VARCHAR(10), PR.ValidUntilDt, 105) as ValidUntilDt,
                                    CONVERT(VARCHAR(10), PR.Documentdt,  105) as Documentdt,
                                    CONVERT(VARCHAR(10), PR.RequiredDt ,  105) as RequiredDt,
                                    PR.EmpId ,PR.DocType ,PR.TotBefDis ,PR.Freight ,PR.Tax,PR.TotPayDue,PR.IsActive ,
                                    PR.CreateDate ,PR.CreatedBy ,PR.CreateTS ,PR.UpdatedBy ,PR.UpdateDate ,PR.UpdateTS ,PR.Remarks ,
                                    DM.DeptName ,SM.StateName ,BM.BrchName ,FM.Finyr ,concat(EM.EmpFname,' ',EM.EmpLname) as Owner
                                    from Pur_Req PR
                                    left outer join Dept_Mst DM on DM.DeptId = PR.DeptId and DM.IsActive ='Y'
                                    left outer join State_Mst SM on SM.StateId = PR.StateId and SM.IsActive ='Y'
                                    left outer join Branch_Mst BM on BM.BrchId = PR.BrchId and BM.IsActive ='Y'
                                    left outer join Finyr_mst FM on FM.FinyrId = PR.FinyrId and FM.IsActive ='Y'
                                    left outer join EmployeeMst EM on EM.EmpId = PR.EmpId and EM.IsActive ='Y'
                                    left outer join [user] on [user].UserId =PR.ReqtId and [user].IsActive ='Y'
                                    left outer join EmployeeMst EM1 on EM1.EmpId = PR.ReqtId and EM1.IsActive ='Y' where DraftFlag='N'";
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
                string Query = @"select PR.PurReqId ,PR.ReqtType ,PR.ReqtId ,[user].UserFulname ,concat(EM1.EmpFname, ' ',EM1.EmpLname ) as ReqEmpName
                                    ,PR.DeptId ,PR.EmailAddr ,
                                    PR.Notify ,PR.StateId ,PR.BrchId ,PR.BrchGSTINNo ,PR.FinyrId ,PR.FinyrNum ,PR.Status ,
                                    CONVERT(VARCHAR(10), PR.PostingDt, 105) as PostingDt,
                                    CONVERT(VARCHAR(10), PR.ValidUntilDt, 105) as ValidUntilDt,
                                    CONVERT(VARCHAR(10), PR.Documentdt,  105) as Documentdt,
                                    CONVERT(VARCHAR(10), PR.RequiredDt ,  105) as RequiredDt,
                                    PR.EmpId ,PR.DocType ,PR.TotBefDis ,PR.Freight ,PR.Tax,PR.TotPayDue,PR.IsActive ,
                                    PR.CreateDate ,PR.CreatedBy ,PR.CreateTS ,PR.UpdatedBy ,PR.UpdateDate ,PR.UpdateTS ,PR.Remarks ,
                                    DM.DeptName ,SM.StateName ,BM.BrchName ,FM.Finyr ,concat(EM.EmpFname,' ',EM.EmpLname) as Owner
                                    from Pur_Req PR
                                    left outer join Dept_Mst DM on DM.DeptId = PR.DeptId and DM.IsActive ='Y'
                                    left outer join State_Mst SM on SM.StateId = PR.StateId and SM.IsActive ='Y'
                                    left outer join Branch_Mst BM on BM.BrchId = PR.BrchId and BM.IsActive ='Y'
                                    left outer join Finyr_mst FM on FM.FinyrId = PR.FinyrId and FM.IsActive ='Y'
                                    left outer join EmployeeMst EM on EM.EmpId = PR.EmpId and EM.IsActive ='Y'
                                    left outer join [user] on [user].UserId =PR.ReqtId and [user].IsActive ='Y'
                                    left outer join EmployeeMst EM1 on EM1.EmpId = PR.ReqtId and EM1.IsActive ='Y' where DraftFlag='Y'";
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
        public IActionResult GETREQUESTER()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select * From  [User] where IsActive='Y'";
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
        public IActionResult GETATACHMENT(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"Select * From Pur_Req_Attach_Det where PurReqId ='"+Id+"'";
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
        public IActionResult GETPRICE(string Itemid,string Pricelistid)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"Select Price From Price_List_Det where ItemId= '" + Itemid + "' and PListId='"+ Pricelistid + "'";
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
        public IActionResult GETITEMDET(string PurReqId, string Flag)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");

                string Query = @"select PRD.* ,IM.ItemName ,BPM.BPName ,UM.UomCode ,UM.UomName ,
CM.CntryCurrCode ,WHM.WhsCode ,WHM.WhsName
,BM.BrchName ,IM.HSNNo ,IBD.BarCode ,TCM.TaxCode ,SM.ServiceName,
 CONVERT(varchar, PRD.ItemReqdDt, 120) AS ItemReqdDt1
from Pur_Req_det PRD
left outer join Item_Mst IM on IM.ItemId = PRD.ItemId and IM.IsActive ='Y'
left outer join BP_Mst BPM on BPM.BPId = PRD.BPId and BPM.IsActive ='Y'
left outer join Unit_Mst UM on UM.UomId = PRD.UomId and UM.IsActive ='Y'
left outer join Country_Mst CM on CM.CntryId = PRD.CntryId and CM.IsActive ='Y'
left outer join WareHouse_Mst WHM on WHM.WhsId = PRD.WhsId and WHM.IsActive ='Y'
left outer join Branch_Mst BM on BM.BrchId = PRD.ItemBrchId and BM.IsActive ='Y'
left outer join SAC_Mst SM on SM.ServiceId = PRD.ServiceId and SM.IsActive ='Y'
left outer join Item_BarCode_det IBD on IBD.ItemId = IM.ItemId and IBD.IsActive ='Y'
left outer join TaxCode_Mst TCM on TCM.TaxCodeId = PRD.TaxCodeId and TCM.IsActive ='Y'
where PRD.PurReqId='" + PurReqId + "' and PRD.RowStatus in ('" + Flag + "','O','T')";


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
                                    object? value = rdr.IsDBNull(i) ? "" : rdr.GetValue(columnName);
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
        public IActionResult GETFREIGHT(string PurReqId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT prf.*, fm.FretName
FROM Pur_Req_Fret prf
INNER JOIN Freight_Mst fm ON prf.FretId = fm.FretId
WHERE prf.PurReqId ='" + PurReqId + "'";
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
                                    object? value = rdr.IsDBNull(i) ? "" : rdr.GetValue(columnName);
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
        public IActionResult TAXFRIGHT(string id, string lineNum, string flag)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @" ";
                if (flag == "T")
                {
                    Query = @"select distinct PRT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                             from pur_req_tax PRT
                             inner join Pur_Req PR on PR.PurReqId = PRT.PurReqId
                             inner join Pur_Req_det PRD on PRD.PurReqId = PR.PurReqId 
                             and PRD.LineNum =PRT.LineNum 
                             inner join TaxCode_Mst TCM on TCM.TaxCodeId = PRD.TaxCodeId  
                             AND PRT.TaxCodeId = PRD.TaxCodeId 
                             and PRD.LineNum = PRT.LineNum 
                             and TCM.IsActive ='Y'
                             where PRT.PurReqId = '" + id + "' And PRT.LineNum='" + lineNum + "' And PRT.Tax_type ='" + flag + "' ";
                }
                else
                {
                    Query = @"   select distinct PRT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                                   from pur_req_tax PRT
                                     inner join Pur_Req PR on PR.PurReqId = PRT.PurReqId
                                     inner join Pur_Req_det PRD on PRD.PurReqId = PR.PurReqId 
                                     inner join Freight_Mst FM  on FM.FretId = PRT.LineNum 
                                     inner join TaxCode_Mst TCM on TCM.TaxCodeId = PRD.TaxCodeId  
                                   
                                     and TCM.IsActive ='Y'
                                 where PRT.PurReqId = '" + id + "' And   PRT.LineNum='" + lineNum + "' And  PRT.Tax_type ='" + flag + "' ";
                }
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
                                    object? value = rdr.IsDBNull(i) ? "" : rdr.GetValue(columnName);
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
        public IActionResult POSTUPDATE(PurchaseRequest item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Lastid = "";
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
          
                if (item.PurReqId != "" && item.PurReqId != null)
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[Pur_Req]", "PurReqId", item.PurReqId, "");
                    if (item.ReqtType != null || item.ReqtType != "")
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
                    Lastid = item.PurReqId;
                    return Json(new { Success = true, LastId = Lastid, Message = "Purchase Request Updated Successfully..!" });
                }
                else
                {
                    item.PurReqId = null;
                    item.DocNum = GETDOCNUM(item.DraftFlag).ToString();
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (item.ReqtType != null || item.ReqtType != "")
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[Pur_Req]", "PurReqId");
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.CommandTimeout = 300;
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "SELECT MAX(PurReqId) AS PurReqId FROM Pur_Req;";
                                SqlDataReader rdr = cmd.ExecuteReader();
                                {
                                    while (rdr.Read())
                                    {
                                        Lastid = rdr["PurReqId"].ToString();
                                    }
                                }
                            }

                        }
                    }
                }
                if (item.DraftFlag == "Y") { 
                    return Json(new { Success = true, LastId = Lastid, Message = "Purchase Request Drafted Successfully..!" });
                } else { 
                   return Json(new { Success = true, LastId = Lastid, Message = "Purchase Request Added Successfully..!" });
                } ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public int GETDOCNUM(string flag) {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"
SELECT ISNULL(MAX(DocNum),0)+1 AS DocNum
FROM Pur_Req 
inner join Finyr_mst on Pur_req.FinyrId = Finyr_mst.FinyrId 
and Finyr_mst.IsActive ='Y'  where DraftFlag='" + flag + "'; ";
            int docnum=0;
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
                            docnum = Convert.ToInt32( rdr["DocNum"]);
                        }
                    }
                    con.Close();
                }
            }
            return docnum;

        }
        public IActionResult ADDITEMATTACHMENT([FromBody] List<PurAggrementAttachment> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    if (item.PurRAttchDetId != "" && item.PurRAttchDetId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Pur_Req_Attach_Det]", "PurRAttchDetId", item.PurRAttchDetId, "");
                        if (item.PurReqId != null || item.PurReqId != "")
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
                        item.PurRAttchDetId = null;
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.PurReqId != null || item.PurReqId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Pur_Req_Attach_Det]", "PurRAttchDetId");
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
        public IActionResult ADDITEMDETAILS([FromBody] List<purchase_de_mst> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                int index =1;

                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {

                    item.LineNum = index.ToString();
                   
                    if (item.PurRDetId != "" && item.PurRDetId != null)
                    {
                       //var Date = item.ItemReqdDt;
                       //DateTime date = DateTime.ParseExact(Date, "dd-MM-yyyy", null);
                       //string formattedDate = date.ToString("yyyy-MM-dd");
                       //item.ItemReqdDt = formattedDate;
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        if (item.RowStatus == "O")
                        {
                            item.OpenQty = item.Qty;
                        }
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Pur_Req_det]", "PurRDetId", item.PurRDetId, "");
                        if (item.PurReqId != null || item.PurReqId != "")
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
                        item.PurRDetId = null;
                        item.OpenQty = item.Qty;
                        item.RowStatus = "O";
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.PurReqId != null || item.PurReqId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Pur_Req_det]", "PurRDetId");
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
                ADDTAX(Data[0].PurReqId,"I");
                return Json(new { Success = true, Message = "Purchase Request Item Details Added  Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public void ADDTAX(string ID,string Flag) {
            try{
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"exec [dbo].[usp_UpdPurTax] 'Pur_Req_det','PurReqId','Pur_Req_Tax','PurRTaxId','Pur_Req_Fret','PurRFretId','" + ID + "','" + Flag + "'";
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
            catch(Exception ex)
            {
                throw ex;
            }
            
          
        }
        public IActionResult ADDFREIGHT([FromBody] List<Freight_Detail> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                int index = 1;
                foreach (var item in Data)
                {
                    if (item.PurRFretId != "" && item.PurRFretId != null)
                    {

                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Pur_Req_Fret]", "PurRFretId", item.PurRFretId, "");
                        if (item.PurReqId != null || item.PurReqId != "")
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
                        item.PurRFretId = null;
                        // Format the date as YYYY-MM-DD
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.PurReqId != null || item.PurReqId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Pur_Req_Fret]", "PurRFretId");
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
                ADDTAX(Data[0].PurReqId, "F");
                return Json(new { Success = true, Message = "Purchase Request Item Details Added  Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult DELETEATTACHMENT(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [Pur_Req_Attach_Det] where PurRAttchDetId='" + Id + "'";
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

                string Query = "Delete from [Pur_Req] where PurReqId='" + Id + "'";
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
                return Json(new { success = true, message = "Purchase Request  Deleted Successfully..!" });
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

                string Query = "Delete from [Pur_Req_det] where PurRDetId='" + Id + "'";
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
                return Json(new { success = true, message = "Details  Deleted Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}
