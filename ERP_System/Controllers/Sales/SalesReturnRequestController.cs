using ERP_System.Models;
using ERP_System.Models.Purchase;
using ERP_System.Models.Sales;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ERP_System.Controllers.Sales
{
    public class SalesReturnRequestController : Controller
    {
        public IActionResult SalesReturnRequest()
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
                string Query = @"select distinct SQ.SalesRReqId,SQ.BPId,BPM.BPName , SQ.BPContPerId  , concat(BPCD.fname,' ', BPCD.Lname ) as BPContactPerName ,
                                SQ.VendorRefNo ,SQ.CurSource , SQ.CntryId ,
                                CM.CntryCurrCode ,SQ.StateId ,
                                SM.StateName ,SQ.BPAddrId  ,BPAD.AddrType ,BPAD.Addr , SQ.BrchId ,BM.BrchName ,SQ.BrchGSTINNo ,
                                SQ.FinyrId ,FM.Finyr ,SQ.FinyrNum ,SQ.Status,
                                CONVERT(varchar, SQ.PostingDt, 105) AS PostingDt,
                                CONVERT(varchar, SQ.ReturnDt, 105) AS ReturnDt,
                                CONVERT(varchar, SQ.DocumentDt, 105) AS DocumentDt,
                                SQ.SalsEmpId , SEM.SalsEmpName ,
                                SQ.EmpId ,concat(EM.EmpFname ,' ',EM.EmpLname) as Owner,
                                SQ.DocType ,SQ.ShipToAddr , SQ.BillToAddr ,SQ.ShipTypeId ,STM.ShipTypeName ,
                                SQ.JrnlMemo ,SQ.PymntTId , PTM.PymntCode,
                                SQ.PrjId ,PM.PrjName ,SQ.DutyStatus , SQ.TotBefDis , SQ.DisPer ,SQ.DisAmt ,
                                SQ.Freight ,SQ.Tax ,SQ.TotPayDue ,SQ.DocNum ,SQ.DraftFlag ,SQ.IsActive ,
                                SQ.CreatedBy ,SQ.CreateDate ,SQ.CreateTS ,SQ.UpdatedBy ,SQ.UpdateDate ,SQ.UpdateTS ,SQ.Remarks 

                                from Sales_RetReq SQ  With (NOLOCK)
                                inner join BP_Mst BPM on BPM.BPId = SQ.BPId and BPM.IsActive ='Y'
                                left outer join BPCont_Det BPCD on BPCD.BPContDetId = SQ.BPContPerId and BPCD.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = SQ.CntryId and CM.IsActive ='Y'
                                left outer join State_Mst SM on SM.StateId = SQ.StateId and SM.IsActive ='Y'
                                left outer join BPAddr_Det BPAD on BPAD.BPId = SQ.BPId and BPAD.BPAddrId = SQ.BPAddrId 
                                and BPAD.IsActive ='Y'
                                left outer join Branch_Mst BM on BM.BrchId = SQ.BrchId and BM.IsActive ='Y'
                                left outer join Finyr_mst FM on FM.FinyrId = SQ.FinyrId and FM.IsActive ='Y'
                                left outer join Sals_Emp_mst SEM on SEM.SalsEmpId = SQ.SalsEmpId and SEM.IsActive ='Y'
                                left outer join EmployeeMst EM on EM.EmpId = SQ.EmpId and EM.IsActive ='Y'
                                left outer join ShippingType_mst STM on STM.ShipTypeId = SQ.ShipTypeId and STM.IsActive ='Y'
                                left outer join Pymnt_Terms_Mst PTM on PTM.PymntTId = SQ.PymntTId and PTM.IsActive ='Y'
                                left outer join Project_Mst PM on PM.PrjId = SQ.PrjId and PM.IsActive ='Y' where SQ.DraftFlag ='N' ";
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
                string Query = @"select distinct PQ.* ,BPM.BPName , concat(BPCD.fname,' ', BPCD.Lname ) as BPContactPerName ,
                CM.CntryCurrCode ,SM.StateName ,BPAD.AddrType ,BPAD.Addr ,BM.BrchName ,FM.Finyr ,SEM.SalsEmpName ,
                concat(EM.EmpFname ,' ',EM.EmpLname) as Owner,STM.ShipTypeName ,PTM.PymntCode,
                CONVERT(varchar, PQ.PostingDt, 105) AS PostingDt,
                CONVERT(varchar, PQ.ReturnDt, 105) AS ReturnDt,
                CONVERT(varchar, PQ.DocumentDt, 105) AS DocumentDt,
                PM.PrjName 
                from Sales_RetReq PQ  With (NOLOCK)
                inner join BP_Mst BPM on BPM.BPId = PQ.BPId and BPM.IsActive ='Y'
                left outer join BPCont_Det BPCD on BPCD.BPContDetId = PQ.BPContPerId and BPCD.IsActive ='Y'
                left outer join Country_Mst CM on CM.CntryId = PQ.CntryId and CM.IsActive ='Y'
                left outer join State_Mst SM on SM.StateId = PQ.StateId and SM.IsActive ='Y'
                left outer join BPAddr_Det BPAD on BPAD.BPId = PQ.BPId and BPAD.BPAddrId = PQ.BPAddrId 
                and BPAD.IsActive ='Y'
                left outer join Branch_Mst BM on BM.BrchId = PQ.BrchId and BM.IsActive ='Y'
                left outer join Finyr_mst FM on FM.FinyrId = PQ.FinyrId and FM.IsActive ='Y'
                left outer join Sals_Emp_mst SEM on SEM.SalsEmpId = PQ.SalsEmpId and SEM.IsActive ='Y'
                left outer join EmployeeMst EM on EM.EmpId = PQ.EmpId and EM.IsActive ='Y'
                left outer join ShippingType_mst STM on STM.ShipTypeId = PQ.ShipTypeId and STM.IsActive ='Y'
                left outer join Pymnt_Terms_Mst PTM on PTM.PymntTId = PQ.PymntTId and PTM.IsActive ='Y'
                left outer join Project_Mst PM on PM.PrjId = PQ.PrjId and PM.IsActive ='Y' where PQ.DraftFlag ='Y'";
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
                string Query = @"Select * FROM Attach_Det where DocId='" + Id + "' and DocType ='SRR'";
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
        public IActionResult GETITEMDET(string ID, string Flag)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select distinct SQD.*,IM.ItemName , TCM.TaxCode ,UM.UomCode , UM.UomName ,CM.CntryCurrCode , WHM.WhsName ,
                                BM.BrchName ,SM.ServiceName ,IBD.BarCode
                                from Sales_RetReq_det SQD With (NOLOCK)
                                left outer join Item_Mst IM on IM.ItemId = SQD.ItemId and IM.IsActive ='Y'
                                left outer join TaxCode_Mst TCM on TCM.TaxCodeId = SQD.TaxCodeId and TCM.IsActive ='Y'
                                left outer join Unit_Mst UM on UM.UomId = SQD.UomId and UM.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = SQD.CntryId and CM.IsActive ='Y'
                                left outer join WareHouse_Mst WHM on WHM.WhsId = SQD.WhsId and WHM.IsActive ='Y'
                                left outer join Branch_Mst BM on BM.BrchId = SQD.ItemBrchId and BM.IsActive ='Y'
                                left outer join SAC_Mst SM on SM.ServiceId = SQD.ServiceId and SM.IsActive ='Y'
                                left outer join Item_BarCode_det IBD on IBD.ItemId = SQD.ItemId and IBD.IsActive ='Y'
                                where SQD.SalesRReqId =' " + ID + "' and SQD.RowStatus in ('" + Flag + "','O','T')";
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
        public IActionResult GETFREIGHT(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT pqf.*, fm.FretName
FROM Sales_RetReq_Fret pqf
INNER JOIN Freight_Mst fm ON pqf.FretId = fm.FretId
WHERE pqf.SalesRReqId ='" + Id + "'";
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
                    Query = @"select distinct SQT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                        from Sales_RetReq_Tax SQT
                        inner join Sales_RetReq SQ on SQ.SalesRReqId = SQT.SalesRReqId
                        inner join Sales_RetReq_det SQD on SQD.SalesRReqId = SQ.SalesRReqId 
                        and SQD.LineNum =SQT.LineNum 
                        inner join TaxCode_Mst TCM on TCM.TaxCodeId = SQD.TaxCodeId  
                        AND SQT.TaxCodeId = SQD.TaxCodeId 
                        and SQT.LineNum = SQD.LineNum 
                        and TCM.IsActive ='Y'
                        where SQT.SalesRReqId = '" + id + "' And SQT.LineNum='" + lineNum + "' And SQT.Tax_type ='" + flag + "' ";
                }
                else
                {
                    Query = @"   select distinct SQT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                            from Sales_RetReq_Tax SQT
                                inner join Sales_RetReq SQ on SQ.SalesRReqId = SQT.SalesRReqId
                                inner join Sales_RetReq_det SQD on SQD.SalesRReqId = SQ.SalesRReqId
                                inner join Freight_Mst FM  on FM.FretId = SQT.LineNum 
                                inner join TaxCode_Mst TCM on TCM.TaxCodeId = SQD.TaxCodeId  
                               
                                and TCM.IsActive ='Y'
                            where SQT.SalesRReqId = '" + id + "' And   SQT.LineNum='" + lineNum + "' And  SQT.Tax_type ='" + flag + "' ";
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
        public IActionResult GETBLANKETAGGRIMENT(string Id)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"Select * From [BlKAgrmt] where BPId='" + Id + "' and Status='A' And IsActive='Y' ";
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
        public IActionResult GETCOPYFROM(string Flag,string BPId)
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
                                        Where BLKA.Trans_type='S' AND BLKA.Status ='A'";
                        break;
                    case "SD":
                        Query = @"Select SO.* ,BPM.BPName
                                From Sales_Delivery SO
                                inner join BP_Mst BPM on BPM.BPId = SO.BPId and BPM.IsActive = 'Y'
                                where SO.DraftFlag = 'N' And SO.Status = 'O' And  SO.BPId ='" + BPId + "'";
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
        public IActionResult GETTAXDATA(string SalesRReqId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT * FROM [Sales_RetReq_Tax_det] where SalesRReqId='" + SalesRReqId + "'";
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
        public IActionResult POSTUPDATE(SalesReturnRequest item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Lastid = "";
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));

                if (item.SalesRReqId != "" && item.SalesRReqId != null)
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[Sales_RetReq]", "SalesRReqId", item.SalesRReqId, "");
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
                    Lastid = item.SalesRReqId;
                }
                else
                {
                    item.SalesRReqId = null;
                 
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
                        string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_RetReq]", "SalesRReqId");
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.CommandTimeout = 300;
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "SELECT MAX(SalesRReqId) AS SalesRReqId FROM Sales_RetReq;";
                                SqlDataReader rdr = cmd.ExecuteReader();
                                {
                                    while (rdr.Read())
                                    {
                                        Lastid = rdr["SalesRReqId"].ToString();
                                    }
                                }
                            }

                        }
                    }
                }
                if (item.DraftFlag == "Y")
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Sales Return Request Drafted Successfully..!" });
                }
                else
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Sales Return Request Added Successfully..!" });
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
FROM Sales_RetReq 
inner join Finyr_mst on Sales_RetReq.FinyrId = Finyr_mst.FinyrId 
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
        public IActionResult ADDITEMATTACHMENT([FromBody] List<Models.Sales.Attchment> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    item.DocType = "SRR";
                    if (item.AttchDetId != "" && item.AttchDetId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Attach_Det]", "AttchDetId", item.AttchDetId, "");
                        if (item.DocId != null || item.DocId != "")
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
                        item.AttchDetId = null;
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.DocId != null || item.DocId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Attach_Det]", "AttchDetId");
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
        public IActionResult ADDITEMDETAILS([FromBody] List<SalesReturnRequest_Item> Data)
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
                    if (item.SalesRReqDetId  != "" && item.SalesRReqDetId  != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        if (item.RowStatus == "O") { 
                            item.OpenQty = item.Qty;
                        }

                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Sales_RetReq_det]", "SalesRReqDetId", item.SalesRReqDetId , "");
                        if (item.SalesRReqId != null || item.SalesRReqId != "")
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
                        item.SalesRReqDetId  = null;
                        item.OpenQty = item.Qty;
                        item.RowStatus = "O";
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.SalesRReqId != null || item.SalesRReqId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_RetReq_det]", "SalesRReqDetId");
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
                ADDTAX(Data[0].SalesRReqId, "I");
                COPYFROMSTATUSCHECK(Data[0].SalesRReqId);
                return Json(new { Success = true, Message = "Sales Return Request Item Details Added  Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public void COPYFROMSTATUSCHECK(string ID)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"exec  [dbo].[usp_UpdBaseDocData] 'Sales_RetReq','Sales_RetReq_det','SalesRReqId','" + ID + "' ";
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
        public void ADDTAX(string ID, string Flag)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"exec [dbo].[usp_UpdPurTax] 'Sales_RetReq_det','SalesRReqId','Sales_RetReq_Tax','SalesRReqTaxId','Sales_RetReq_Fret','SalesRReqFretId','" + ID + "','" + Flag + "'";
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
        public IActionResult ADDFREIGHT([FromBody] List<Freight_SalesReturnRequest> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                int index = 1;
                foreach (var item in Data)
                {
                    if (item.SalesRReqFretId != "" && item.SalesRReqFretId != null)
                    {

                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");

                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Sales_RetReq_Fret]", "SalesRReqFretId", item.SalesRReqFretId, "");
                        if (item.SalesRReqId != null || item.SalesRReqId != "")
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
                        item.SalesRReqFretId = null;
                        // Format the date as YYYY-MM-DD
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.SalesRReqId != null || item.SalesRReqId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_RetReq_Fret]", "SalesRReqFretId");
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
                ADDTAX(Data[0].SalesRReqId, "F");
                return Json(new { Success = true, Message = "Sales Return Request Item Details Added  Successfully..!" });
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

                string Query = "Delete from [Sales_RetReq] where SalesRReqId='" + Id + "'";
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
                return Json(new { success = true, message = "Sales Return Request  Deleted Successfully..!" });
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

                string Query = "Delete from [Sales_RetReq_det] where SalesRReqDetId ='" + Id + "'";
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
        public IActionResult ADDTAXINFO([FromBody] SalesReturnRequest_Taxinfo_Mst item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));


                if (!string.IsNullOrEmpty(item.SalesRReqTaxDetId))
                {


                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[Sales_RetReq_Tax_det]", "SalesRReqTaxDetId", item.SalesRReqTaxDetId, "");
                    if (!string.IsNullOrEmpty(item.SalesRReqId))
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
                    item.SalesRReqTaxDetId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (!string.IsNullOrEmpty(item.SalesRReqId))
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_RetReq_Tax_det]", "SalesRReqTaxDetId");
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
