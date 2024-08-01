using ERP_System.Models.Purchase;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ERP_System.Models.Sales;

namespace ERP_System.Controllers.Sales
{
    public class SalesOrderController : Controller
    {
        public IActionResult SalesOrder()
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
                string Query = @"select distinct SO.SalsOrderId,SO.BPId,BPM.BPName , SO.BPContPerId  , concat(BPCD.fname,' ', BPCD.Lname ) as BPContactPerName ,
                                SO.VendorRefNo ,SO.CurSource , SO.CntryId ,
                                CM.CntryCurrCode ,SO.StateId ,
                                SM.StateName ,SO.BPAddrId  ,BPAD.AddrType ,BPAD.Addr , SO.BrchId ,BM.BrchName ,SO.BrchGSTINNo ,
                                SO.FinyrId ,FM.Finyr ,SO.FinyrNum ,SO.Status,
                                CONVERT(varchar, SO.PostingDt, 105) AS PostingDt,
                                 CONVERT(varchar, SO.DeliveryDt, 105) AS DeliveryDt,
                                CONVERT(varchar, SO.DocumentDt, 105) AS DocumentDt,
                                CONVERT(varchar, SO.RequiredDt, 105) AS RequiredDt,
                                SO.SalsEmpId , SEM.SalsEmpName ,
                                SO.EmpId ,concat(EM.EmpFname ,' ',EM.EmpLname) as Owner,
                                SO.DocType ,SO.ShipToAddr , SO.BillToAddr ,SO.ShipTypeId ,STM.ShipTypeName ,
                                SO.JrnlMemo ,SO.PymntTId , PTM.PymntCode,
                                SO.PrjId ,PM.PrjName ,SO.DutyStatus , SO.TotBefDis , SO.DisPer ,SO.DisAmt ,
                                SO.Freight ,SO.Tax ,SO.TotPayDue ,SO.DocNum ,SO.DraftFlag ,SO.IsActive ,
                                SO.CreatedBy ,SO.CreateDate ,SO.CreateTS ,SO.UpdatedBy ,SO.UpdateDate ,SO.UpdateTS ,SO.Remarks 

                                from Sales_Order SO  With (NOLOCK)
                                inner join BP_Mst BPM on BPM.BPId = SO.BPId and BPM.IsActive ='Y'
                                left outer join BPCont_Det BPCD on BPCD.BPContDetId = SO.BPContPerId and BPCD.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = SO.CntryId and CM.IsActive ='Y'
                                left outer join State_Mst SM on SM.StateId = SO.StateId and SM.IsActive ='Y'
                                left outer join BPAddr_Det BPAD on BPAD.BPId = SO.BPId and BPAD.BPAddrId = SO.BPAddrId 
                                and BPAD.IsActive ='Y'
                                left outer join Branch_Mst BM on BM.BrchId = SO.BrchId and BM.IsActive ='Y'
                                left outer join Finyr_mst FM on FM.FinyrId = SO.FinyrId and FM.IsActive ='Y'
                                left outer join Sals_Emp_mst SEM on SEM.SalsEmpId = SO.SalsEmpId and SEM.IsActive ='Y'
                                left outer join EmployeeMst EM on EM.EmpId = SO.EmpId and EM.IsActive ='Y'
                                left outer join ShippingType_mst STM on STM.ShipTypeId = SO.ShipTypeId and STM.IsActive ='Y'
                                left outer join Pymnt_Terms_Mst PTM on PTM.PymntTId = SO.PymntTId and PTM.IsActive ='Y'
                                left outer join Project_Mst PM on PM.PrjId = SO.PrjId and PM.IsActive ='Y' where SO.DraftFlag ='N' ";
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
                string Query = @"select distinct SO.* ,BPM.BPName , concat(BPCD.fname,' ', BPCD.Lname ) as BPContactPerName ,
CM.CntryCurrCode ,SM.StateName ,BPAD.AddrType ,BPAD.Addr ,BM.BrchName ,FM.Finyr ,SEM.SalsEmpName ,
concat(EM.EmpFname ,' ',EM.EmpLname) as Owner,STM.ShipTypeName ,PTM.PymntCode,   CONVERT(varchar, SO.PostingDt, 105) AS PostingDt,
                                 CONVERT(varchar, SO.DeliveryDt, 105) AS DeliveryDt,
                                CONVERT(varchar, SO.DocumentDt, 105) AS DocumentDt,
                                CONVERT(varchar, SO.RequiredDt, 105) AS RequiredDt,
PM.PrjName 
from Sales_Order SO  With (NOLOCK)
inner join BP_Mst BPM on BPM.BPId = SO.BPId and BPM.IsActive ='Y'
left outer join BPCont_Det BPCD on BPCD.BPContDetId = SO.BPContPerId and BPCD.IsActive ='Y'
left outer join Country_Mst CM on CM.CntryId = SO.CntryId and CM.IsActive ='Y'
left outer join State_Mst SM on SM.StateId = SO.StateId and SM.IsActive ='Y'
left outer join BPAddr_Det BPAD on BPAD.BPId = SO.BPId and BPAD.BPAddrId = SO.BPAddrId 
and BPAD.IsActive ='Y'
left outer join Branch_Mst BM on BM.BrchId = SO.BrchId and BM.IsActive ='Y'
left outer join Finyr_mst FM on FM.FinyrId = SO.FinyrId and FM.IsActive ='Y'
left outer join Sals_Emp_mst SEM on SEM.SalsEmpId = SO.SalsEmpId and SEM.IsActive ='Y'
left outer join EmployeeMst EM on EM.EmpId = SO.EmpId and EM.IsActive ='Y'
left outer join ShippingType_mst STM on STM.ShipTypeId = SO.ShipTypeId and STM.IsActive ='Y'
left outer join Pymnt_Terms_Mst PTM on PTM.PymntTId = SO.PymntTId and PTM.IsActive ='Y'
left outer join Project_Mst PM on PM.PrjId = SO.PrjId and PM.IsActive ='Y' where SO.DraftFlag ='Y'";
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
                string Query = @"Select * FROM Attach_Det where DocId='" + Id + "' and DocType ='SO'";
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
        public IActionResult GETITEMDET(string ID,string Flag)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select distinct SOD.SalsODetId , SOD.SalsOrderId , SOD.ItemId ,SOD.LineNum , SOD.Qty ,SOD.InfoPrice ,
				SOD.DisPer ,SOD.TaxCodeId ,SOD.TotalLC ,SOD.CalcTotalLC ,SOD.UomId ,SOD.CntryId ,SOD.WhsId , SOD.LocName ,
				SOD.BlKAId ,SOD.ItemBrchId ,SOD.ServiceId ,SOD.TaxAmountLC ,SOD.ItemHSNNo ,SOD.BaseDocType ,SOD.BaseDocEntry ,SOD.BaseDocLineNum ,
				SOD.CreditMemoQty ,SOD.GLAcctName ,SOD.BarCode ,SOD.CreatedBy ,SOD.CreateDate ,SOD.CreateTS ,SOD.UpdatedBy ,
				SOD.UpdateDate ,SOD.UpdateTS ,SOD.OpenQty ,SOD.RowStatus ,SOD.ActualPrice ,SOD.PriceUpd ,SOD.PrevQty ,'N' as Editflag,
				IM.ItemName , TCM.TaxCode ,UM.UomCode , UM.UomName ,CM.CntryCurrCode , WHM.WhsName , BM.BrchName ,SM.ServiceName ,IBD.BarCode 
                from Sales_Order_det SOD With (NOLOCK)
                left outer join Item_Mst IM on IM.ItemId = SOD.ItemId and IM.IsActive ='Y'
                left outer join TaxCode_Mst TCM on TCM.TaxCodeId = SOD.TaxCodeId and TCM.IsActive ='Y'
                left outer join Unit_Mst UM on UM.UomId = SOD.UomId and UM.IsActive ='Y'
                left outer join Country_Mst CM on CM.CntryId = SOD.CntryId and CM.IsActive ='Y'
                left outer join WareHouse_Mst WHM on WHM.WhsId = SOD.WhsId and WHM.IsActive ='Y'
                left outer join Branch_Mst BM on BM.BrchId = SOD.ItemBrchId and BM.IsActive ='Y'
                left outer join SAC_Mst SM on SM.ServiceId = SOD.ServiceId and SM.IsActive ='Y'
                left outer join Item_BarCode_det IBD on IBD.ItemId = SOD.ItemId and IBD.IsActive ='Y'
                where SOD.SalsOrderId =' " + ID + "' and SOD.RowStatus in ('" + Flag + "','O','T')";
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
                string Query = @"SELECT sof.*, fm.FretName
FROM Sales_Order_Fret sof
INNER JOIN Freight_Mst fm ON sof.FretId = fm.FretId
WHERE sof.SalsOrderId ='" + Id + "'";
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
                    Query = @"select distinct SOT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                        from Sales_Order_Tax SOT
                        inner join Sales_Order SQ on SQ.SalsOrderId = SOT.SalsOrderId
                        inner join Sales_Order_det SOD on SOD.SalsOrderId = SQ.SalsOrderId 
                        and SOD.LineNum =SOT.LineNum 
                        inner join TaxCode_Mst TCM on TCM.TaxCodeId = SOD.TaxCodeId  
                        AND SOT.TaxCodeId = SOD.TaxCodeId 
                        and SOT.LineNum = SOD.LineNum 
                        and TCM.IsActive ='Y'
                        where SOT.SalsOrderId = '" + id + "' And SOT.LineNum='" + lineNum + "' And SOT.Tax_type ='" + flag + "' ";
                }
                else
                {
                    Query = @"   select distinct SOT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                            from Sales_Order_Tax SOT
                                inner join Sales_Order SQ on SQ.SalsOrderId = SOT.SalsOrderId
                                inner join Sales_Order_det SOD on SOD.SalsOrderId = SQ.SalsOrderId
                                inner join Freight_Mst FM  on FM.FretId = SOT.LineNum 
                                inner join TaxCode_Mst TCM on TCM.TaxCodeId = SOD.TaxCodeId  
                                
                                and TCM.IsActive ='Y'
                            where SOT.SalsOrderId = '" + id + "' And   SOT.LineNum='" + lineNum + "' And  SOT.Tax_type ='" + flag + "' ";
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
                    case "SBA":
                        Query = @"Select BLKA.*,BM.BPName ,
                                        case when AgrmtMthd='I' then 'Item Method' 
                                        else 'Monetary Method' end as AgreementMethod,
                                        case when AgrmtType ='S' then 'Specific'
                                        else 'General' end as AgreementType
                                        FROm BlKAgrmt BLKA
                                        inner join BP_Mst BM on BM.BPId = BLKA.BPId and BM.IsActive ='Y'
                                        Where BLKA.Trans_type='S' AND BLKA.Status ='O'";
                        break;
                    case "SQ":
                        Query = @"Select * From Sales_Quot where DraftFlag ='N' And Status='O' And BPId ='" + BPId + "'";
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
        public IActionResult GETTAXDATA(string SalsOrderId)
    {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT * FROM [Sales_Order_Tax_det] where SalsOrderId='" + SalsOrderId + "'";
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
        public IActionResult POSTUPDATE(SalesOrder item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Lastid = "";
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));

                if (item.SalsOrderId != "" && item.SalsOrderId != null)
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[Sales_Order]", "SalsOrderId", item.SalsOrderId, "");
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
                    Lastid = item.SalsOrderId;
                }
                else
                {
                    item.SalsOrderId = null;
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
                        string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_Order]", "SalsOrderId");
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.CommandTimeout = 300;
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "SELECT MAX(SalsOrderId) AS SalsOrderId FROM Sales_Order;";
                                SqlDataReader rdr = cmd.ExecuteReader();
                                {
                                    while (rdr.Read())
                                    {
                                        Lastid = rdr["SalsOrderId"].ToString();
                                    }
                                }
                            }

                        }
                    }
                }
                if (item.DraftFlag == "Y")
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Sales Order Drafted Successfully..!" });
                }
                else
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Sales Order Added Successfully..!" });
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
FROM Sales_Order 
inner join Finyr_mst on Sales_Order.FinyrId = Finyr_mst.FinyrId 
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
        public IActionResult ADDITEMATTACHMENT([FromBody] List<SalesOrderAttchment> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    item.DocType = "SO";
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
        public IActionResult ADDITEMDETAILS([FromBody] List<SalesOrder_Item> Data)
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
                    if (item.SalsODetId != "" && item.SalsODetId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        if (item.RowStatus == "O")
                        {
                            item.OpenQty = item.Qty;
                        }
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Sales_Order_det]", "SalsODetId", item.SalsODetId, "");
                        if (item.SalsOrderId != null || item.SalsOrderId != "")
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
                        item.SalsODetId = null;
                        item.OpenQty = item.Qty;
                        item.RowStatus = "O";
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.SalsOrderId != null || item.SalsOrderId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_Order_det]", "SalsODetId");
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
                ADDTAX(Data[0].SalsOrderId, "I");
                ADDSTOCKQTY(Data[0].SalsOrderId, "SO", Data[0].SalsODetId == null ? "Y" : "N");
                COPYFROMSTATUSCHECK(Data[0].SalsOrderId);
                return Json(new { Success = true, Message = "Sales Order Item Details Added  Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public void ADDSTOCKQTY(string ID, string Flag, string add_flag)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"exec[dbo].[usp_UpdItemWhsStock] 'Sales_Order','Sales_Order_det','SalsOrderId','" + ID + "','" + Flag + "','" + add_flag + "'";
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
        public void COPYFROMSTATUSCHECK(string ID)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"exec  [dbo].[usp_UpdBaseDocData] 'Sales_Order','Sales_Order_det','SalsOrderId','" + ID + "' ";
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
            string Query = @"exec [dbo].[usp_UpdPurTax] 'Sales_Order_det','SalsOrderId','Sales_Order_Tax','SalsOTaxId','Sales_Order_Fret','SalsOFretId','" + ID + "','" + Flag + "'";
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
        public IActionResult ADDFREIGHT([FromBody] List<SalesOrder_Freight> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                int index = 1;
                foreach (var item in Data)
                {
                    if (item.SalsOFretId != "" && item.SalsOFretId != null)
                    {

                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");

                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[Sales_Order_Fret]", "SalsOFretId", item.SalsOFretId, "");
                        if (item.SalsOrderId != null || item.SalsOrderId != "")
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
                        item.SalsOFretId = null;
                        // Format the date as YYYY-MM-DD
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.SalsOrderId != null || item.SalsOrderId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_Order_Fret]", "SalsOFretId");
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
                ADDTAX(Data[0].SalsOrderId, "F");
                return Json(new { Success = true, Message = "Sales Order Item Details Added  Successfully..!" });
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

                string Query = "Delete from [Sales_Order] where SalsOrderId='" + Id + "'";
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
                return Json(new { success = true, message = "Sales Order  Deleted Successfully..!" });
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

                string Query = "Delete from [Sales_Order_det] where SalsODetId ='" + Id + "'";
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
        public IActionResult ADDTAXINFO([FromBody] SOTaxinfo_Mst item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));


                if (!string.IsNullOrEmpty(item.SalsOTaxDetId))
                {


                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[Sales_Order_Tax_det]", "SalsOTaxDetId", item.SalsOTaxDetId, "");
                    if (!string.IsNullOrEmpty(item.SalsOrderId))
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
                    item.SalsOTaxDetId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (!string.IsNullOrEmpty(item.SalsOrderId))
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[Sales_Order_Tax_det]", "SalsOTaxDetId");
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
