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
        public string TaskName { get; set; }
        public string DescriptionTask { get; set; }
        public double TotalPercent { get; set; }
        public int Task_level { get; set; }
        public string DescriptionTest { get; set; }
        public int TestID { get; set; }
        public DateTime TestSentDate { get; set; }
        public byte TestStatus { get; set; }
        public string DescriptionQA { get; set; }
        public int QAID { get; set; }
        public DateTime QASentDate { get; set; }
        public byte QAStatus { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int ProjectID { get; set; }
        public List<string> SubTasksName { get; set; }
        public List<string> SubTasksDis { get; set; }
        public List<int> SubTaskDevID { get; set; }
        public List<DateTime> SubTaskSendDate { get; set; }
        public List<DateTime> SubTaskCreateDate { get; set; }
        public List<DateTime> SubTaskUpdate { get; set; }
        public int Status { get; set; }
        public int SubDevID { get; set; }
        public DateTime SubTaskDateSend { get; set; }
        public string SubTasksDes { get; set; }
        public string SubTaskNames { get; set; }
        public int SubTaskID { get; set; }
        public int Handle { get; set; }
        
        public List<ProjectMember> DevList { get; set; }
        public List<ProjectMember> TestList { get; set; }
        public List<ProjectMember> QAList { get; set; }

        public string DevName { get; set; }
        public List<string> TestName { get; set; }
        public List<string> QAName { get; set; }
    }
}