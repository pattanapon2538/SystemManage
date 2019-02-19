using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SystemManage.Database;

namespace SystemManage.Models
{
    public class TaskModel
    {
        public int TaskID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public string TaskName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public string DescriptionTask { get; set; }
        public double TotalPercent { get; set; }
        public int Task_level { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public string DescriptionTest { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public int TestID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public DateTime TestSentDate { get; set; }
        public int TestStatus { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public string DescriptionQA { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public int QAID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public DateTime QASentDate { get; set; }
        public int QAStatus { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int ProjectID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public List<string> SubTasksName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public List<string> SubTasksDis { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public List<int> SubTaskDevID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public List<DateTime> SubTaskSendDate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public List<DateTime> SubTaskCreateDate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public List<DateTime> SubTaskUpdate { get; set; }
        public int Status { get; set; }
        public int SubDevID { get; set; }
        public DateTime SubTaskDateSend { get; set; }
        public string SubTasksDes { get; set; }
        public string SubTaskNames { get; set; }
        public int SubTaskID { get; set; }
        public int Handle { get; set; }
        public LevelTask level { get; set; }
        public int HaveDefect { get; set; }
        public string Comment_Dev { get; set; }
        public string Comment_Tester { get; set; }
        public string Comment_QA { get; set; }
        public string Comment_CM { get; set; }

        public List<ProjectMember> DevList { get; set; }
        public List<ProjectMember> TestList { get; set; }
        public List<ProjectMember> QAList { get; set; }

        public List<string> DevName { get; set; }
        public List<string> TestName { get; set; }
        public List<string> QAName { get; set; }

        public enum LevelTask
        {
            ยาก,
            ปานกลาง,
            ง่าย,
        }
        ///////////////////////////
        public int Send { get; set; }
    }
}