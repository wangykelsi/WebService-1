using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.DataClass
{
    public class Implementation
    {
    }

    public class ImplementationInfo   //Pad和Web版通用  区别：Web显示计划的任务列表，Pad不
    {
        public PatientInfo1 PatientInfo { get; set; }          //病人基本信息-姓名、头像

        public string ProgressRate { get; set; }              //此计划的进度

        public string RemainingDays { get; set; }            //此计划的剩余天数

        public string CompliacneValue { get; set; }         //此计划 最近一周的依从率 或 整个计划依从率

        public List<PlanDeatil> PlanList { get; set; }     //所有计划列表(正在实施中的 和 已经结束的)

        public List<Task> TaskList { get; set; }          //此计划的任务列表

        public chartData chartData { get; set; }        //画图数据-血压、脉率值，分级情况，依从情况

        public ImplementationInfo()
        {
            PatientInfo = new PatientInfo1();          //初始化
            PlanList = new List<PlanDeatil>();
            TaskList = new List<Task>();
            chartData = new chartData();
        }

    }

    //Pad和Phone 区别：（1）PlanList Pad有，Phone只显示正在执行的计划 （2）依从率：Pad 最近一周/整个计划  Phone只最近一周 （3） Phone有血压详细查看（时刻）（4）Phone患者使用，不需再获取基本信息
    public class ImplementationPhone           //Phone版
    {
        public string ProgressRate { get; set; }       //进度

        public string RemainingDays { get; set; }       //剩余天数

        public string CompliacneValue { get; set; }       //最近一周的依从率

        public chartData chartData { get; set; }

        public int StartDate { get; set; }    //最近一周的时间起始，作为血压详细查看（时刻）的时间输入

        public int EndDate { get; set; }

        public ImplementationPhone()
        {
            chartData = new chartData();
        }

    }


    public class PatientInfo1   //病人基本信息
    {

        public string PatientName { get; set; } //姓名

        public string ImageUrl { get; set; }   //头像


    }

    public class chartData  //画图数据集合
    {

        public List<Graph> graphList { get; set; }   //图：点

        public List<GuideList> BPGuide { get; set; }  //图：血压分级区域和最大最小值 种类：收缩压、舒张压

        public chartData()
        {
            graphList = new List<Graph>();
            BPGuide = new List<GuideList>();
        }
    }


    public class GuideList      //血压分级区域和最大最小值
    {

        public List<Guide> Guides { get; set; }   //血压分级区域

        public string original { get; set; }      //初始值

        public string target { get; set; }        //目标值

        public double minimum { get; set; }       //Y值下限

        public double maximum { get; set; }       //Y值上限

        public GuideList()
        {
            Guides = new List<Guide>();
        }
    }

    public class Graph          //图的主要点数据
    {
        //日期
        public string date { get; set; }       //日期，到天

        //public string value { get; set; }          //值
        //public string Grade { get; set; }           //值级别
        //public string lineColor { get; set; }      //点颜色    
        //public string bulletShape { get; set; }    //点形状  

        public string SBPvalue { get; set; }         //收缩压值

        public string SBPGrade { get; set; }         //收缩压等级 

        public string SBPlineColor { get; set; }      //收缩压点颜色

        public string SBPbulletShape { get; set; }    //收缩压形状：一般（圆 round），当天（菱形diamond）



        //是否需要抽取成共同的类？                       因为颜色、数据形状判断可能不一样！
        //舒张压
        public string DBPvalue { get; set; }

        public string DBPGrade { get; set; }

        public string DBPlineColor { get; set; }

        public string DBPbulletShape { get; set; }


        //脉率





        //暂时用不到的
        //public string BPBullet { get; set; }       //客制化血压点 "amcharts-images/star.png"

        //public string description { get; set; }    //血压点的气球文本（另一血压值）

        //public string timeDetail { get; set; }    //测试的具体时间，到s


        //当天用药（包含其他任务的描述）
        public string drugValue { get; set; }         //用药情况画在下部图，保持Y=1

        public string drugBullet { get; set; }       //客制化用药状态-部分完成 "amcharts-images/drug.png" 半白半黑图片

        public string drugColor { get; set; }        //药的其他颜色-完全未完成、完成	

        public string drugDescription { get; set; }       //用药情况描述 "部分完成；未吃:阿司匹林、青霉素；已吃：钙片、板蓝根"  使用叉勾图标

        public Graph()
        {

            //血压初始化？

            drugBullet = "";         //初始化  时间肯定有  默认所有任务（生理测量、用药）为不确定状态，不确定是否执行
            drugValue = "1";
            drugColor = "#FFFFFF";  //白色
            drugDescription = "暂无记录";
        }
    }

    public class Guide          //图的区域划分-风险分级、目标线、初始线    目标线、初始线 字体、线密度不同  分级区域颜色不同，文字不同
    {
        //变量-来自数据库
        public string value { get; set; }       //值或起始值

        public string toValue { get; set; }       //终值或""

        public string label { get; set; }        //中文定义 目标线、偏低、偏高等


        //恒量-根据图设定
        public string lineColor { get; set; }       //直线颜色  目标线  初始线
        public string lineAlpha { get; set; }       //直线透明度 0全透~1
        public string dashLength { get; set; }       //虚线密度  4  8

        public string color { get; set; }            //字体颜色
        public string fontSize { get; set; }       //字体大小  默认14
        public string position { get; set; }       //字体位置 right left
        public string inside { get; set; }        //坐标系的内或外  false

        public string fillAlpha { get; set; }       //区域透明度
        public string fillColor { get; set; }       //
        //public string balloonText { get; set; }       //气球弹出框   

    }

    public class TaskDeatil  //某任务的详细属性
    {
        public string TaskType { get; set; }

        public string TaskId { get; set; }

        public string TaskName { get; set; }

        public string Instruction { get; set; }

    }

    public class Task  //某类型任务的集合
    {
        public string TaskType { get; set; }

        public List<TaskDeatil> TaskDeatilList { get; set; }

        public Task()
        {
            TaskDeatilList = new List<TaskDeatil>();
        }
    }
}