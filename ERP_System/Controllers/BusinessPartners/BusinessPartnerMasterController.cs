using ERP_System.Models.BusinessPartners;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ERP_System.Models.Inventory;
using ERP_System.Models.Sales;
using ERP_System.Models.Masters;
using Newtonsoft.Json;

namespace ERP_System.Controllers.BusinessPartners
{
    public class BusinessPartnerMasterController : Controller
    {
        public IActionResult BusinessPartnerMaster()
        {
            if (HttpContext.Session.GetString("User_Id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult GET()
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT 
                                T0.*, 
                                T1.StateName, 
                                T2.CntryName, 
	                            T4.PListName,
	                            T5.PymntCode,
                                STRING_AGG(T3.WTCode, ',') AS WTCode,
	                            T6.CntryCurrCode AS BPCurrencyCode 
                            FROM 
                                BP_Mst T0
                            INNER JOIN 
                                State_Mst T1 ON T0.StateId = T1.StateId
                            INNER JOIN 
                                Country_Mst T2 ON T0.CntryId = T2.CntryId
                            LEFT JOIN 
                                Price_List_Mst T4 ON T0.PListId = T4.PListId
                            LEFT JOIN 
                                Pymnt_Terms_Mst T5 ON T0.PymntTId = T5.PymntTId
                            LEFT JOIN 
                                WHT_mst T3 ON T0.WT_appid LIKE '%' + CAST(T3.WTId AS VARCHAR) + '%'
                            LEFT OUTER JOIN		
	                            Country_Mst T6 ON T6.CntryId  = T0.BPCurrId 
                            GROUP BY 
                                T0.BPId, T0.BPCode, T0.BPName, T0.BPAliasName, T0.BPGrpId, T0.BPCardType, T0.BPAddr, T0.BPCity,
                                T0.CntryId, T0.StateId, T0.BPMobNo, T0.BPEmail, T0.BPPhone1, T0.BPPhone2, T0.BPPANNo, T0.BPITRFilling,
                                T0.BPACBal, T0.BPCurrType, T0.BPCurrId, T0.IsActive, T0.BPValidFrdt, T0.BPValidTodt, T0.CreatedBy,
                                T0.CreateDate, T0.CreateTS, T0.UpdatedBy, T0.UpdateDate, T0.UpdateTS, T0.PymntTId, T0.IntrstRate,
                                T0.PListId, T0.Discount, T0.CredLimit, T0.CommitLimit, T0.FatherCardId, T0.FatherType, T0.ConnBPId,
                                T0.PlngGroup, T0.PaymBlock, T0.SinglePaym, T0.BankAllCodeId, T0.HouseBankId, T0.PaymBlockRemark,
                                T0.WTLiable, T0.WTType, T0.Ass_WT_type, T0.WT_appid, T1.StateName, T2.CntryName,T4.PListName,
	                            T5.PymntCode,T6.CntryCurrCode ";
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
        public IActionResult GETACCOUNTING(string BPId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select distinct BPM.BPId,BPM.bpcode,BPM.BPName, BPM.FatherCardId as CBPId,
BPM1.BPName as CBPName ,
BPM.FatherType ,BPM.ConnBPId ,  BPM2.BPName as ConnectedCustmer,
BPM.PlngGroup ,BPM.PaymBlock , BPM.PaymBlockRemark,BPM.SinglePaym ,BPM.BankAllCodeId ,BACM.Code ,
BPM.HouseBankId , BM.BankName 
from BP_Mst BPM
left outer join BP_Mst BPM1 on BPM1.bpid = BPM.FatherCardId 
left outer join BP_Mst BPM2 on BPM2.bpid = BPM.ConnBPId 
left outer join Bank_AllCode_Mst BACM on BACM.BankAllCodeId = BPM.BankAllCodeId 
left outer join Bank_Mst BM on BM.BankId = BPM.HouseBankId  where BPM.bpid='" + BPId + "'";
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
        public IActionResult GETATACHMENT(string BpId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select  BPA.*, T0.BPCode,T0.BPName
from BPAttach_Det BPA With (NOLOCK)
inner join BP_mst T0 on T0.BPId = BPA.BPId 
where BPA.BPId='" + BpId + "'order by T0.BPId ";
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
        public IActionResult GETTAXDATA(string BPId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT * FROM [BPTax_Det] where BPId='" + BPId + "'";
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
                List<UnitMst> units = JsonConvert.DeserializeObject<List<UnitMst>>(data);

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

                            string insertQuery = genrate.GenerateInsertQuery(unit, "[BP_Mst]", "BPId");

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
        public IActionResult POSTDATA(BusinessPartnerMaster Data)
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
                string insertQuery = genrate.GenerateInsertQuery(Data, "[BP_Mst]", "BPId");
                string BPId = "";
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT MAX(BPId) AS BPId FROM BP_Mst;";
                        SqlDataReader rdr = cmd.ExecuteReader();
                        {
                            while (rdr.Read())
                            {
                                BPId = rdr["BPId"].ToString();
                            }
                        }
                    }
                    con.Close();

                }
                return Json(new { Success = true, LastId = BPId, Message = " Business Partner Details Added Successfully..!" });
            }
            catch (SqlException ex)
            {
                int errorCode = ex.ErrorCode;
                if (errorCode == -2146232060)
                {
                    return StatusCode(500, "This Business Partner Code Is Alredy In System Please Define New..!");
                }
                else
                {
                    return StatusCode(500, ex.Message.ToString());
                }
            }
        }
        public IActionResult UPDATEDATA(BusinessPartnerMaster Data)
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
                string Query = genrate.GenerateUpdateQuery(Data, "[BP_Mst]", "BPId", Data.BPId, "");
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
                return Json(new { Success = true, LastId = Data.BPId, Message = " Business Partner Details Updated Successfully..!" });
            }
            catch (SqlException ex)
            {
                int errorCode = ex.ErrorCode;
                if (errorCode == -2146232060)
                {
                    return StatusCode(500, "This Business Partner Code Is Alredy In System Please Define New..!");
                }
                else
                {
                    return StatusCode(500, ex.Message.ToString());
                }
            }
        }
        public IActionResult DELETE(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [BP_Mst] where BPId='" + Id + "'";
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
                return Json(new { success = true, message = "Business Partner Details Deleted Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult ADDCONTACTDT(BPContactDetails item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                if (item.BPContDetId != "" && item.BPContDetId != null && item.BPContDetId != "0")
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[Item_BarCode_det]", "BPContDetId", item.BPContDetId, "");
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
                }
                else
                {
                    item.BPContDetId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (item.BPId != null || item.BPId != "")
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[BPCont_Det]", "BPContDetId");
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
                return Json(new { Success = true, Message = "Contact Person Details Saved Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult ADDADDRESSDT(BPAddress item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                if (item.BPAddrId != "" && item.BPAddrId != null && item.BPAddrId != "0")
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[BPAddr_Det]", "BPAddrId", item.BPAddrId, "");
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
                }
                else
                {
                    item.BPAddrId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (item.BPId != null || item.BPId != "")
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[BPAddr_Det]", "BPAddrId");
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
                return Json(new { Success = true, Message = "Address Details Saved Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult ADDBANKDT(BPBankDetails item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                if (item.BPBankDetId != "" && item.BPBankDetId != null && item.BPBankDetId != "0")
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[BPBank_Det]", "BPBankDetId", item.BPBankDetId, "");
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
                }
                else
                {
                    item.BPBankDetId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (item.BPId != null || item.BPId != "")
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[BPBank_Det]", "BPBankDetId");
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
                return Json(new { Success = true, Message = "Address Details Saved Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult ADDITEMATTACHMENT([FromBody] List<BPAttachment> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    if (item.BPAttachId != "" && item.BPAttachId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[BPAttach_Det]", "BPAttachId", item.BPAttachId, "");
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
                    }
                    else
                    {
                        item.BPAttachId = null;
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.BPId != null || item.BPId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[BPAttach_Det]", "BPAttachId");
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

        public IActionResult POSTACCOUNTING(BusinessPartnerMaster Data)
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
                string insertQuery = genrate.GenerateInsertQuery(Data, "[BP_Mst]", "BPId");
                string BPId = "";
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT MAX(BPId) AS BPId FROM BP_Mst;";
                        SqlDataReader rdr = cmd.ExecuteReader();
                        {
                            while (rdr.Read())
                            {
                                BPId = rdr["BPId"].ToString();
                            }
                        }
                    }
                    con.Close();

                }
                return Json(new { Success = true, LastId = BPId, Message = " Business Partner Details Added Successfully..!" });
            }
            catch (SqlException ex)
            {
                int errorCode = ex.ErrorCode;
                if (errorCode == -2146232060)
                {
                    return StatusCode(500, "This Business Partner Code Is Alredy In System Please Define New..!");
                }
                else
                {
                    return StatusCode(500, ex.Message.ToString());
                }
            }
        }
        public IActionResult DELETEATTCH(string Id)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {

                string Query = "Delete from [BPAttach_Det] where BPAttachId='" + Id + "'";
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
                return Json(new { success = true, message = "Attchament Details Deleted Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public IActionResult ADDTAXINFO([FromBody] BPTaxinfo_Mst item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));


                if (!string.IsNullOrEmpty(item.BPTaxId))
                {


                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[BPTax_Det]", "BPTaxId", item.BPTaxId, "");
                    if (!string.IsNullOrEmpty(item.BPId))
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
                    item.BPTaxId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (!string.IsNullOrEmpty(item.BPId))
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[BPTax_Det]", "BPTaxId");
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
                return Json(new { Success = true, Message = "Tax Information Saved Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}
