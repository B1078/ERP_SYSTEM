namespace ERP_System.Models.Masters
{
    public class EmployeeMaster
    {
      public string? EmpId {get;set;}
      public string? EmpCode {get;set;}
      public string? EmpTitle {get;set;}
      public string? EmpFname {get;set;}
      public string? EmpMname {get;set;}
      public string? EmpLname {get;set;}
      public string? EmpDob {get;set;}
      public string? EmpBplace {get;set;}
      public string? EmpGender {get;set;}
      public string? EmpMartSt {get;set;}
      public string? EmpBldgrp {get;set;}
      public string? EmpPaddr {get;set;}
      public string? EmpPCntryId {get;set;}
      public string? EmpPStateId {get;set;}
      public string? EmpPCity {get;set;}
        public string? errormessage { get; set; }
        public string? EmpCAddr {get;set;}
      public string? EmpCCntryId {get;set;}
      public string? EmpCStateId {get;set;}
      public string? EmpCCity {get;set;}
      public string? EmpPhone {get;set;}
      public string? EmpMobNo {get;set;}
      public string? EmpWhspNo {get;set;}
      public string? EmpEmail {get;set;}
      public string? EmpSecEmail {get;set;}
      public string? EmpRelign { get; set; }
        public string? DeptId { get; set; }
        public string? IsActive { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public TimeOnly? UpdateTS { get; set; }
        public DateOnly? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public TimeOnly? CreateTS { get; set; }
    }
}
