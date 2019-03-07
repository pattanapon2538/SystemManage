using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SystemManage.Database;

namespace SystemManage.Models
{
    public class SITModel
    {
        public int SIT_ID { get; set; }
        public int Project_ID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public int Tester_ID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string Name { get; set; }
        public string Detail { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public int Task_ID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public int Dev_ID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public DateTime Send_Date_T { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public DateTime Send_Date_Q { get; set; }
        public List<int> TaskList { get; set; }
        public List<int> StepList { get; set; }
        public int Status { get; set; }
        public string Commnet_T { get; set; }
        public string Comment_Dev { get; set; }
        public string Comment_QA { get; set; }
        public string Comment_CM { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public int QA_ID { get; set; }
        public int Handle { get; set; }

        public List<Task> Task { get; set; }
        public List<ProjectMember> Tester { get; set; }
        public List<ProjectMember> Dev { get; set; }
        public List<ProjectMember> QA { get; set; }

        public int _Step_ID { get; set; }
        public string Task_Name { get; set; }
        public int ID_Task { get; set; }
        public int state { get; set; }
        public Status_SIT Defect { get; set; }

        public enum Status_SIT
        {
            ผ่าน,
            ไม่ผ่าน
        }
        public string QA_Name { get; set; }
        public string Tester_Name { get; set; }
        public string Dev_Name { get; set; }
    }
}