using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.DataClass
{
    public class CompliacneInfo
    {
    }

    public class SignDetail
    {
        public string DetailTime { get; set; }           //详细时间 到时分秒

        public string Value { get; set; }        //是否执行 1：执行； 0：未执行
    }

    public class SignDetailByD
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

    public class SignDetailByP
    {

        public List<SignDetailByD> SignDetailByDs { get; set; }

        public SignDetailByP()
        {
            SignDetailByDs = new List<SignDetailByD>();
        }

    }


    //依从情况
    public class CompliacneDetailByD
    {

        public string Date { get; set; }

        //public string ComplianceValue { get; set; }      

        public string drugBullet { get; set; }

        public string drugColor { get; set; }

        public string Events { get; set; }

    }
}