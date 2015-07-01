using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.DataClass
{
    public class CompliacneInfo
    {
    }

    public class SignDetailByP  //高血压体征详细-日期段
    {

        public List<SignDetailByD> SignDetailByDs { get; set; }

        public SignDetailByP()
        {
            SignDetailByDs = new List<SignDetailByD>();
        }

    }

    public class SignDetailByD  //高血压体征详细-某天
    {
        public string Date { get; set; }        //日期

        public string WeekDay { get; set; }        //星期几
        //public int Count { get; set; }           //某一日下有几个

        public List<SignDetail> SignDetailList { get; set; }

        public SignDetailByD()
        {
            SignDetailList = new List<SignDetail>();
        }

    }


    public class SignDetail   //高血压体征详细-到分 
    {
        public string DetailTime { get; set; }           //详细时间 到时分

        //血压 "135/78"
        //public string BPValue { get; set; }

        public string SBPValue { get; set; }        //收缩压

        public string DBPValue { get; set; }        //舒张压

        public string PulseValue { get; set; }        //脉率  "78"

        public SignDetail()
        {
            SBPValue = "";
            DBPValue = "";
            PulseValue = "";
        }
    }


    //任务依从情况
    public class CompliacneDetailByD
    {

        public string Date { get; set; }

        //public string ComplianceValue { get; set; }      

        public string drugBullet { get; set; }

        public string drugColor { get; set; }

        public string Events { get; set; }

    }
}