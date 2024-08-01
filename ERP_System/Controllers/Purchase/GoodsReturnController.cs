﻿using ERP_System.Models;
using ERP_System.Models.Purchase;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ERP_System.Controllers.Purchase
{
    public class GoodsReturnController : Controller
    {
        public IActionResult GoodsReturn()
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
                string Query = @"select distinct PO.GDReturnId,PO.BPId,BPM.BPName , PO.BPContPerId  , concat(BPCD.fname,' ', BPCD.Lname ) as BPContactPerName ,
                                PO.VendorRefNo ,PO.CurSource , PO.CntryId ,
                                CM.CntryCurrCode ,PO.StateId ,
                                SM.StateName ,PO.BPAddrId  ,BPAD.AddrType ,BPAD.Addr , PO.BrchId ,BM.BrchName ,PO.BrchGSTINNo ,
                                PO.FinyrId ,FM.Finyr ,PO.FinyrNum ,PO.Status,
                                CONVERT(varchar, PO.PostingDt, 105) AS PostingDt,
                                CONVERT(varchar, PO.DueDt, 105) AS DueDt,
                                CONVERT(varchar, PO.DocumentDt, 105) AS DocumentDt,
                               -- CONVERT(varchar, PO.RequiredDt, 105) AS RequiredDt,
                                PO.SalsEmpId , SEM.SalsEmpName ,
								PO.ConsolidatingBPId,
								PO.ConsolidationType,
                                PO.EmpId ,concat(EM.EmpFname ,' ',EM.EmpLname) as Owner,
                                PO.DocType ,PO.ShipToAddr , PO.BillToAddr ,PO.ShipTypeId ,STM.ShipTypeName ,
                                PO.JrnlMemo ,PO.PymntTId , PTM.PymntCode,
                                PO.PrjId ,PM.PrjName ,PO.DutyStatus , PO.TotBefDis , PO.DisPer ,PO.DisAmt ,
                                PO.Freight ,PO.Tax ,PO.TotPayDue ,PO.DocNum ,PO.DraftFlag ,PO.IsActive ,
                                PO.CreatedBy ,PO.CreateDate ,PO.CreateTS ,PO.UpdatedBy ,PO.UpdateDate ,PO.UpdateTS ,PO.Remarks 

                                from GDReturn PO  With (NOLOCK)
                                inner join BP_Mst BPM on BPM.BPId = PO.BPId and BPM.IsActive ='Y'
                                left outer join BPCont_Det BPCD on BPCD.BPContDetId = PO.BPContPerId and BPCD.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = PO.CntryId and CM.IsActive ='Y'
                                left outer join State_Mst SM on SM.StateId = PO.StateId and SM.IsActive ='Y'
                                left outer join BPAddr_Det BPAD on BPAD.BPId = PO.BPId and BPAD.BPAddrId = PO.BPAddrId 
                                and BPAD.IsActive ='Y'
                                left outer join Branch_Mst BM on BM.BrchId = PO.BrchId and BM.IsActive ='Y'
                                left outer join Finyr_mst FM on FM.FinyrId = PO.FinyrId and FM.IsActive ='Y'
                                left outer join Sals_Emp_mst SEM on SEM.SalsEmpId = PO.SalsEmpId and SEM.IsActive ='Y'
                                left outer join EmployeeMst EM on EM.EmpId = PO.EmpId and EM.IsActive ='Y'
                                left outer join ShippingType_mst STM on STM.ShipTypeId = PO.ShipTypeId and STM.IsActive ='Y'
                                left outer join Pymnt_Terms_Mst PTM on PTM.PymntTId = PO.PymntTId and PTM.IsActive ='Y'
                                left outer join Project_Mst PM on PM.PrjId = PO.PrjId and PM.IsActive ='Y' where PO.DraftFlag ='N' ";
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
                string Query = @"select distinct PO.* ,BPM.BPName , concat(BPCD.fname,' ', BPCD.Lname ) as BPContactPerName ,
CM.CntryCurrCode ,SM.StateName ,BPAD.AddrType ,BPAD.Addr ,BM.BrchName ,FM.Finyr ,SEM.SalsEmpName ,
concat(EM.EmpFname ,' ',EM.EmpLname) as Owner,STM.ShipTypeName ,PTM.PymntCode,   CONVERT(varchar, PO.PostingDt, 105) AS PostingDt,
  CONVERT(varchar, PO.DueDt, 105) AS DueDt,
CONVERT(varchar, PO.DocumentDt, 105) AS DocumentDt,
--CONVERT(varchar, PO.RequiredDt, 105) AS RequiredDt,
PO.ConsolidatingBPId,
PO.ConsolidationType,
PM.PrjName 
from GDReturn PO  With (NOLOCK)
inner join BP_Mst BPM on BPM.BPId = PO.BPId and BPM.IsActive ='Y'
left outer join BPCont_Det BPCD on BPCD.BPContDetId = PO.BPContPerId and BPCD.IsActive ='Y'
left outer join Country_Mst CM on CM.CntryId = PO.CntryId and CM.IsActive ='Y'
left outer join State_Mst SM on SM.StateId = PO.StateId and SM.IsActive ='Y'
left outer join BPAddr_Det BPAD on BPAD.BPId = PO.BPId and BPAD.BPAddrId = PO.BPAddrId 
and BPAD.IsActive ='Y'
left outer join Branch_Mst BM on BM.BrchId = PO.BrchId and BM.IsActive ='Y'
left outer join Finyr_mst FM on FM.FinyrId = PO.FinyrId and FM.IsActive ='Y'
left outer join Sals_Emp_mst SEM on SEM.SalsEmpId = PO.SalsEmpId and SEM.IsActive ='Y'
left outer join EmployeeMst EM on EM.EmpId = PO.EmpId and EM.IsActive ='Y'
left outer join ShippingType_mst STM on STM.ShipTypeId = PO.ShipTypeId and STM.IsActive ='Y'
left outer join Pymnt_Terms_Mst PTM on PTM.PymntTId = PO.PymntTId and PTM.IsActive ='Y'
left outer join Project_Mst PM on PM.PrjId = PO.PrjId and PM.IsActive ='Y' where PO.DraftFlag ='Y'";
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
                string Query = @"Select * FROM Attach_Det where DocId='" + Id + "' and DocType ='GR'";
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
        public IActionResult GETITEMDET(string ID)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"select distinct POD.GDRetDetId  , POD.GDReturnId   , POD.ItemId ,POD.LineNum , POD.Qty ,POD.InfoPrice ,
                                POD.DisPer ,POD.TaxCodeId ,POD.TotalLC ,POD.CalcTotalLC ,POD.UomId ,POD.CntryId ,POD.WhsId , POD.LocName ,
                                POD.BlKAId ,POD.ItemBrchId ,POD.ItemHSNNo ,
                                POD.ServiceId ,POD.TaxAmountLC ,POD.BaseDocType ,POD.BaseDocEntry ,POD.BaseDocLineNum ,
                                POD.CreditMemoQty ,POD.GLAcctName ,POD.BarCode ,POD.Remarks ,POD.CreatedBy ,POD.CreateDate ,POD.CreateTS ,POD.UpdatedBy ,
                                POD.UpdateDate ,POD.UpdateTS ,POD.OpenQty ,POD.RowStatus ,POD.ActualPrice ,POD.PriceUpd ,POD.PrevQty ,'N' as Editflag,
                                IM.ItemName , TCM.TaxCode ,UM.UomCode , UM.UomName ,CM.CntryCurrCode , WHM.WhsName ,
                                BM.BrchName ,SM.ServiceName 
                                from GDReturn_det POD With (NOLOCK)
                                left outer join Item_Mst IM on IM.ItemId = POD.ItemId and IM.IsActive ='Y'
                                left outer join TaxCode_Mst TCM on TCM.TaxCodeId = POD.TaxCodeId and TCM.IsActive ='Y'
                                left outer join Unit_Mst UM on UM.UomId = POD.UomId and UM.IsActive ='Y'
                                left outer join Country_Mst CM on CM.CntryId = POD.CntryId and CM.IsActive ='Y'
                                left outer join WareHouse_Mst WHM on WHM.WhsId = POD.WhsId and WHM.IsActive ='Y'
                                left outer join Branch_Mst BM on BM.BrchId = POD.ItemBrchId and BM.IsActive ='Y'
                                left outer join SAC_Mst SM on SM.ServiceId = POD.ServiceId and SM.IsActive ='Y'
                                left outer join Item_BarCode_det IBD on IBD.ItemId = POD.ItemId and IBD.IsActive ='Y' 
                                where POD.GDReturnId =' " + ID + "'";
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
FROM GDReturn_Fret pqf
INNER JOIN Freight_Mst fm ON pqf.FretId = fm.FretId
WHERE pqf.GDReturnId ='" + Id + "'";
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
                    Query = @"select distinct POT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                        from GDReturn_Tax POT
                        inner join GDReturn SQ on SQ.GDReturnId = POT.GDReturnId
                        inner join GDReturn_det POD on POD.GDReturnId = SQ.GDReturnId 
                        and POD.LineNum =POT.LineNum 
                        inner join TaxCode_Mst TCM on TCM.TaxCodeId = POD.TaxCodeId  
                        AND POT.TaxCodeId = POD.TaxCodeId 
                        and POT.LineNum = POD.LineNum 
                        and TCM.IsActive ='Y'
                        where POT.GDReturnId = '" + id + "' And POT.LineNum='" + lineNum + "' And POT.Tax_type ='" + flag + "' ";
                }
                else
                {
                    Query = @"   select distinct POT.*,TCM.TaxCode , TCM.TaxFormula , TCM.TaxCalRate 
                            from GDReturn_Tax POT
                                inner join GDReturn SQ on SQ.GDReturnId = POT.GDReturnId
                                inner join GDReturn_det POD on POD.GDReturnId = SQ.GDReturnId
                                inner join Freight_Mst FM  on FM.FretId = POT.LineNum 
                                inner join TaxCode_Mst TCM on TCM.TaxCodeId = POD.TaxCodeId  
                                
                                and TCM.IsActive ='Y'
                            where POT.GDReturnId = '" + id + "' And   POT.LineNum='" + lineNum + "' And  POT.Tax_type ='" + flag + "' ";
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
                                            FROm Pur_Req  PR Where DraftFlag ='N' And Status='O' and BPId='"+ BPId + "'";
                        break;
                    case "PBA":
                        Query = @"Select BLKA.*,BM.BPName ,
                                        case when AgrmtMthd='I' then 'Item Method' 
                                        else 'Monetary Method' end as AgreementMethod,
                                        case when AgrmtType ='S' then 'Specific'
                                        else 'General' end as AgreementType
                                        FROm BlKAgrmt BLKA
                                        inner join BP_Mst BM on BM.BPId = BLKA.BPId and BM.IsActive ='Y'
                                        Where BLKA.Trans_type='S' AND BLKA.Status ='A' and BLKA.BPId='" + BPId + "'";
                        break;
                    case "PQ":
                        Query = @"Select * From Pur_Quot where DraftFlag ='N' And Status='O' and IsActive='Y' and BPId='"+ BPId + "'";
                        break;  
                    case "GRPO":
                        Query = @"select * From GRPO where DraftFlag='N' and Status='O' and IsActive='Y' and BPId='"+ BPId + "'";
                        break;
                    case "GRR":
                        Query = @"select * From GD_RetReq where DraftFlag='N' and Status='O' and IsActive='Y' and BPId='"+ BPId + "'";
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
        public IActionResult GETTAXDATA(string GDReturnId)
        {
            try
            {
                string ConnectionString = HttpContext.Session.GetString("ConnectionString");
                string Query = @"SELECT * FROM [GDReturn_Tax_det] where GDReturnId='" + GDReturnId + "'";
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
        public IActionResult POSTUPDATE(GoodsReturn_Mst item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Lastid = "";
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));

                if (item.GDReturnId != "" && item.GDReturnId != null)
                {
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[GDReturn]", "GDReturnId", item.GDReturnId, "");
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
                    Lastid = item.GDReturnId;
                    return Json(new { Success = true, LastId = Lastid, Message = "Goods Return  Updated Successfully..!" });
                }
                else
                {
                    item.GDReturnId = null;
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
                        string insertQuery = genrate.GenerateInsertQuery(item, "[GDReturn]", "GDReturnId");
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.CommandTimeout = 300;
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "SELECT MAX(GDReturnId) AS GDReturnId FROM GDReturn;";
                                SqlDataReader rdr = cmd.ExecuteReader();
                                {
                                    while (rdr.Read())
                                    {
                                        Lastid = rdr["GDReturnId"].ToString();
                                    }
                                }
                            }

                        }
                    }
                }
                if (item.DraftFlag == "Y")
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Goods Return  Drafted Successfully..!" });
                }
                else
                {
                    return Json(new { Success = true, LastId = Lastid, Message = "Goods Return  Added Successfully..!" });
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
FROM GDReturn 
inner join Finyr_mst on GDReturn.FinyrId = Finyr_mst.FinyrId 
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
        public IActionResult ADDITEMATTACHMENT([FromBody] List<GoodsReturnAttchment> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                foreach (var item in Data)
                {
                    item.DocType = "GR";
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
        public IActionResult ADDITEMDETAILS([FromBody] List<GoodsReturn_Item> Data)
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
                    if (item.GDRetDetId != "" && item.GDRetDetId != null)
                    {
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        if (item.RowStatus == "O")
                        {
                            item.OpenQty = item.Qty;
                        }
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[GDReturn_det]", "GDRetDetId", item.GDRetDetId, "");
                        if (item.GDReturnId != null || item.GDReturnId != "")
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
                        item.GDRetDetId = null;
                        item.OpenQty = item.Qty;
                        item.RowStatus = "O";
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.GDReturnId != null || item.GDReturnId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[GDReturn_det]", "GDRetDetId");
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
                ADDTAX(Data[0].GDReturnId, "I");
                ADDSTOCKQTY(Data[0].GDReturnId, "GRPO", Data[0].GDRetDetId == null ? "Y" : "N");
                COPYFROMSTATUSCHECK(Data[0].GDReturnId);
                return Json(new { Success = true, Message = "Goods Returns Item Details Added  Successfully..!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
        public void ADDSTOCKQTY(string ID, string Flag, string add_flag)
        {
            string? ConnectionString = HttpContext.Session.GetString("ConnectionString");
            string Query = @"exec[dbo].[usp_UpdItemWhsStock] 'GDReturn','GDReturn_det','GDReturnId','" + ID + "','" + Flag + "','" + add_flag + "'";
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
            string Query = @"exec [dbo].[usp_UpdPurTax] 'GDReturn_det','GDReturnId','GDReturn_Tax','GDRetTaxId','GDReturn_Fret','GDRetFretId','" + ID + "','" + Flag + "'";
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
            string Query = @"exec  [dbo].[usp_UpdBaseDocData] 'GDReturn','GDReturn_det','GDReturnId','" + ID + "' ";
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
        public IActionResult ADDFREIGHT([FromBody] List<GoodsReturn_Freight> Data)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));
                int index = 1;
                foreach (var item in Data)
                {
                    if (item.GDRetFretId != "" && item.GDRetFretId != null)
                    {

                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");

                        Genrate_Query genrate = new Genrate_Query();
                        string Query = genrate.GenerateUpdateQuery(item, "[GDReturn_Fret]", "GDRetFretId", item.GDRetFretId, "");
                        if (item.GDReturnId != null || item.GDReturnId != "")
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
                        item.GDRetFretId = null;
                        // Format the date as YYYY-MM-DD
                        item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.UpdatedBy = HttpContext.Session.GetString("UserName");
                        item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                        item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                        item.CreatedBy = HttpContext.Session.GetString("UserName");
                        Genrate_Query genrate = new Genrate_Query();
                        if (item.GDReturnId != null || item.GDReturnId != "")
                        {
                            string insertQuery = genrate.GenerateInsertQuery(item, "[GDReturn_Fret]", "GDRetFretId");
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
                ADDTAX(Data[0].GDReturnId, "F");
                return Json(new { Success = true, Message = "Goods Return Item Details Added  Successfully..!" });
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

                string Query = "Delete from [GDReturn] where GDReturnId='" + Id + "'";
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
                return Json(new { success = true, message = "Goods Return  Deleted Successfully..!" });
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

                string Query = "Delete from [GDReturn_det] where GDRetDetId ='" + Id + "'";
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
        public IActionResult ADDTAXINFO([FromBody] GoodsReturnTaxinfo_Mst item)
        {
            string ConnectionString = HttpContext.Session.GetString("ConnectionString");
            try
            {
                TimeSpan currentTimeOfDay = DateTimeOffset.Now.TimeOfDay;
                long ticks = Math.Max(0, Math.Min(currentTimeOfDay.Ticks, TimeOnly.MaxValue.Ticks));


                if (!string.IsNullOrEmpty(item.GDRetTaxDetId))
                {
                    

                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    string Query = genrate.GenerateUpdateQuery(item, "[GDReturn_Tax_det]", "GDRetTaxDetId", item.GDRetTaxDetId, "");
                    if (!string.IsNullOrEmpty(item.GDReturnId))
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
                    item.GDRetTaxDetId = null;
                    item.UpdateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.UpdateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.UpdatedBy = HttpContext.Session.GetString("UserName");
                    item.CreateTS = TimeOnly.FromTimeSpan(new TimeSpan(ticks));
                    item.CreateDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    item.CreatedBy = HttpContext.Session.GetString("UserName");
                    Genrate_Query genrate = new Genrate_Query();
                    if (!string.IsNullOrEmpty(item.GDReturnId))
                    {
                        string insertQuery = genrate.GenerateInsertQuery(item, "[GDReturn_Tax_det]", "GDRetTaxDetId");
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