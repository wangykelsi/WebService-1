using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterSystems.Data.CacheClient;
using System.Data;
using WebService.CommonLibrary;
using WebService.DataClass;

namespace WebService.DataMethod
{
    public class CmMstBloodPressure
    {
        //GetBPGrades LS 2015-03-27  获取血压分级规则
        public static List<MstBloodPressure> GetBPGrades(DataConnection pclsCache)
        {

            List<MstBloodPressure> result = new List<MstBloodPressure>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstBloodPressure.GetBPGrades(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    MstBloodPressure MstBloodPressure = new MstBloodPressure();
                    MstBloodPressure.Code = cdr["Code"].ToString();
                    MstBloodPressure.Name = cdr["Name"].ToString();
                    MstBloodPressure.Description = cdr["Description"].ToString();
                    MstBloodPressure.SBP = Convert.ToInt32(cdr["SBP"]);
                    MstBloodPressure.DBP = Convert.ToInt32(cdr["DBP"]);
                    MstBloodPressure.PatientClass = cdr["PatientClass"].ToString();
                    MstBloodPressure.Redundance = cdr["Redundance"].ToString();
                    result.Add(MstBloodPressure);

                }
                return result;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetBPGrades", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //输出用于图形的风险划分
        public static List<GuideList> GetBPGuide(DataConnection pclsCache, string PlanNo, string ItemType, List<MstBloodPressure> Reference)
        {
            List<GuideList> result = new List<GuideList>();
            GuideList SysGuides = new GuideList();
            SysGuides.minimum = Convert.ToDouble(Reference[0].SBP);
            //SysGuides.maximum = Reference[Reference.Count-1].SBP.ToString();
            SysGuides.maximum = Convert.ToDouble(Reference[Reference.Count - 1].SBP);

            GuideList DiaGuides = new GuideList();
            //DiaGuides.minimum = Reference[0].DBP.ToString();
            //DiaGuides.maximum = Reference[Reference.Count - 1].DBP.ToString();
            DiaGuides.minimum = Convert.ToDouble(Reference[0].DBP);
            DiaGuides.maximum = Convert.ToDouble(Reference[Reference.Count - 1].DBP);

            List<Guide> SysGuideList = new List<Guide>();
            List<Guide> DiaGuideList = new List<Guide>();

            try
            {
                //收缩压

                InterSystems.Data.CacheTypes.CacheSysList SysTarget = null;
                SysTarget = PsTarget.GetTargetByCode(pclsCache, PlanNo, "Bloodpressure", "Bloodpressure_1");

                if (SysTarget != null)
                {
                    //初始值
                    Guide originalGuide = new Guide();
                    originalGuide.value = SysTarget[4].ToString(); //值或起始值
                    originalGuide.toValue = "#CC0000";           //终值或""
                    originalGuide.label = "";      //中文定义 目标线、偏低、偏高等
                    //originalGuide.label = "起始" + "：" + originalGuide.value;
                    originalGuide.lineColor = "#FF5151";          //直线颜色  目标线  初始线
                    originalGuide.lineAlpha = "1";//直线透明度 0全透~1
                    originalGuide.dashLength = "8"; //虚线密度  4  8
                    originalGuide.color = "#CC0000";    //字体颜色
                    originalGuide.fontSize = "8"; //字体大小  默认14
                    originalGuide.position = "right"; //字体位置 right left
                    originalGuide.inside = "";//坐标系的内或外  false
                    originalGuide.fillAlpha = "";
                    originalGuide.fillColor = "";
                    SysGuideList.Add(originalGuide);
                    SysGuides.original = originalGuide.value;


                    //目标值
                    Guide tagetGuide = new Guide();
                    tagetGuide.value = SysTarget[3].ToString();
                    tagetGuide.toValue = "#CC0000";
                    tagetGuide.label = "";
                    //tagetGuide.label = "目标" + "：" + tagetGuide.value;
                    tagetGuide.lineColor = "#CC0000";
                    tagetGuide.lineAlpha = "1";
                    tagetGuide.dashLength = "4";
                    tagetGuide.color = "#CC0000";
                    tagetGuide.fontSize = "14";
                    tagetGuide.position = "right";
                    tagetGuide.inside = "";

                    tagetGuide.fillAlpha = "";
                    tagetGuide.fillColor = "";
                    SysGuideList.Add(tagetGuide);
                    SysGuides.target = tagetGuide.value;
                }


                //舒张压

                InterSystems.Data.CacheTypes.CacheSysList DiaTarget = null;
                DiaTarget = PsTarget.GetTargetByCode(pclsCache, PlanNo, "Bloodpressure", "Bloodpressure_2");

                if (DiaTarget != null)
                {
                    //初始值
                    Guide originalGuide = new Guide();
                    originalGuide.value = DiaTarget[4].ToString(); //值或起始值
                    originalGuide.toValue = "#CC0000";           //终值或""
                    originalGuide.label = "";      //中文定义 目标线、偏低、偏高等
                    //originalGuide.label = "起始" + "：" + originalGuide.value;
                    originalGuide.lineColor = "#FF5151";          //直线颜色  目标线  初始线
                    originalGuide.lineAlpha = "1";//直线透明度 0全透~1
                    originalGuide.dashLength = "8"; //虚线密度  4  8
                    originalGuide.color = "#CC0000";    //字体颜色
                    originalGuide.fontSize = "8"; //字体大小  默认14
                    originalGuide.position = "right"; //字体位置 right left
                    originalGuide.inside = "";//坐标系的内或外  false

                    originalGuide.fillAlpha = "";
                    originalGuide.fillColor = "";
                    DiaGuideList.Add(originalGuide);
                    DiaGuides.original = originalGuide.value;

                    //目标值
                    Guide tagetGuide = new Guide();
                    tagetGuide.value = DiaTarget[3].ToString();
                    tagetGuide.toValue = "#CC0000";
                    tagetGuide.label = "";
                    //tagetGuide.label = "目标" + "：" + tagetGuide.value;
                    tagetGuide.lineColor = "#CC0000";
                    tagetGuide.lineAlpha = "1";
                    tagetGuide.dashLength = "4";
                    tagetGuide.color = "#CC0000";
                    tagetGuide.fontSize = "14";
                    tagetGuide.position = "right";
                    tagetGuide.inside = "";

                    tagetGuide.fillColor = "";
                    tagetGuide.fillAlpha = "";
                    DiaGuideList.Add(tagetGuide);
                    DiaGuides.target = tagetGuide.value;
                }



                //风险范围
                for (int i = 0; i <= Reference.Count - 2; i++)
                {
                    //收缩压
                    Guide SysGuide = new Guide();
                    SysGuide.value = Reference[i].SBP.ToString(); //起始值
                    SysGuide.toValue = Reference[i + 1].SBP.ToString();                //终值
                    SysGuide.label = Reference[i].Name;          //偏低、偏高等
                    SysGuide.lineColor = "";     //直线颜色  目标线  初始线
                    SysGuide.lineAlpha = "";   //直线透明度 0全透~1
                    SysGuide.dashLength = "";  //虚线密度  4  8
                    SysGuide.color = "#CC0000";     //字体颜色
                    SysGuide.fontSize = "14";   //字体大小  默认14
                    SysGuide.position = "right";    //字体位置 right left
                    SysGuide.inside = "true";      //坐标系的内或外  false
                    SysGuide.fillAlpha = "0.1";
                    SysGuide.fillColor = CmMstBloodPressure.GetFillColor(SysGuide.label);   //GetFillColor
                    SysGuideList.Add(SysGuide);

                    //舒张压
                    Guide DiaGuide = new Guide();
                    DiaGuide.value = Reference[i].DBP.ToString();
                    DiaGuide.toValue = Reference[i + 1].DBP.ToString();
                    DiaGuide.label = Reference[i].Name;
                    DiaGuide.lineColor = "";
                    DiaGuide.lineAlpha = "";
                    DiaGuide.dashLength = "";
                    DiaGuide.color = "#CC0000";
                    DiaGuide.fontSize = "14";
                    DiaGuide.position = "right";
                    DiaGuide.inside = "true";
                    DiaGuide.fillAlpha = "0.1";
                    DiaGuide.fillColor = CmMstBloodPressure.GetFillColor(DiaGuide.label);
                    DiaGuideList.Add(DiaGuide);

                }

                //一般线
                for (int i = 0; i <= Reference.Count - 1; i++)
                {
                    //收缩压
                    Guide SysGuide = new Guide();
                    SysGuide.value = Reference[i].SBP.ToString();
                    SysGuide.toValue = "#CC0000";
                    SysGuide.label = Reference[i].SBP.ToString();
                    SysGuide.lineColor = "#CC0000";
                    SysGuide.lineAlpha = "0.15";
                    SysGuide.dashLength = "";
                    SysGuide.color = "#CC0000";
                    SysGuide.fontSize = "8";
                    SysGuide.position = "left";
                    SysGuide.inside = "";
                    SysGuide.fillAlpha = "";
                    SysGuide.fillColor = "";
                    SysGuideList.Add(SysGuide);

                    //舒张压
                    Guide DiaGuide = new Guide();
                    DiaGuide.value = Reference[i].DBP.ToString();
                    DiaGuide.toValue = "#CC0000";
                    DiaGuide.label = Reference[i].DBP.ToString();
                    DiaGuide.lineColor = "#CC0000";
                    DiaGuide.lineAlpha = "0.15";
                    DiaGuide.dashLength = "";
                    DiaGuide.color = "#CC0000";
                    DiaGuide.fontSize = "8";
                    DiaGuide.position = "left";
                    DiaGuide.inside = "";
                    DiaGuide.fillAlpha = "";
                    DiaGuide.fillColor = "";
                    DiaGuideList.Add(DiaGuide);

                }

                SysGuides.Guides = SysGuideList;
                DiaGuides.Guides = DiaGuideList;

                result.Add(SysGuides);
                result.Add(DiaGuides);

                return result;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetBPGuide", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
            }
        }

        //GetFillColor LS 2015-04-18  获取血压风险范围颜色
        public static string GetFillColor(string name)
        {
            string colorShow = "gray";  //待标记颜色
            try
            {
                switch (name)
                {
                    case "很高": colorShow = "#930000";  //红色
                        break;
                    case "偏高": colorShow = "#CC0000";  //
                        break;
                    case "警戒": colorShow = "#0000cc"; //
                        break;
                    case "正常": colorShow = "#2894FF";  //
                        break;
                    case "偏低": colorShow = "#FFC78E";  //
                        break;
                    default: break;
                }

                return colorShow;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetFillColor", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        //GetSignGrade LS 2015-03-27  获取某血压值的级别-输出 Cm.MstBloodPressure.Name 名称：很高、偏高、警戒、正常
        public static string GetSignGrade(string BPType, int Value, List<MstBloodPressure> Reference)
        {
            //算法：将值Value与分级的值一一相比，注意  >Value；   <= Value <；   <=Value 等区间，确定其级别（取左边的值）  目前Name少了一个，暂时默认少了偏高
            string Name = "不明范围";  //待标记颜色
            try
            {
                if (BPType == "Bloodpressure_1")  //收缩压 3  标准肯定不只1个值！
                {

                    //按照血压规定  分级保证>2
                    if (Value < Reference[0].SBP)
                    {
                        Name = "错误";
                    }

                    if (Value > Reference[Reference.Count - 1].SBP)
                    {
                        Name = "错误";
                    }

                    if (Name == "不明范围")
                    {
                        if (Reference.Count >= 2)  //前两步已经保证了，数量肯定》2   
                        {
                            for (int i = 0; i <= Reference.Count - 2; i++)  //要求个数>=2
                            {
                                if ((Value >= Reference[i].SBP) && (Value < Reference[i + 1].SBP)) //左闭右开
                                {
                                    Name = Reference[i].Name;  //名字就低
                                    break;
                                }
                            }
                        }
                    }


                }
                else  //舒张压
                {
                    if (Value < Reference[0].DBP)
                    {
                        Name = "错误";
                    }

                    if (Value > Reference[Reference.Count - 1].DBP)
                    {
                        Name = "错误";
                    }

                    if (Name == "不明范围")
                    {
                        if (Reference.Count >= 2)  //前两步已经保证了，数量肯定》2
                        {
                            for (int i = 0; i <= Reference.Count - 2; i++)  //要求个数>=2
                            {
                                if ((Value >= Reference[i].DBP) && (Value < Reference[i + 1].DBP))
                                {
                                    Name = Reference[i].Name;
                                    break;
                                }
                            }
                        }
                    }
                }

                return Name;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetSignGrade", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        //GetSignColor LS 2015-03-27  获取血压值颜色-根据 Cm.MstBloodPressure.Name
        public static string GetSignColor(string name)
        {
            string colorShow = "#000000";  //待标记颜色
            try
            {
                switch (name)
                {
                    case "很高": colorShow = "#FF0000";  //深红色
                        break;
                    case "偏高": colorShow = "#FF60AF";  //微红
                        break;
                    case "警戒": colorShow = "#FFA042"; //橙色
                        break;
                    case "正常": colorShow = "#00DB00";  //绿色
                        break;
                    case "偏低": colorShow = "#8080C0";  //微紫
                        break;
                    default: break;
                }

                return colorShow;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetSignColor", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        //GetBPInfo  LS 2015-03-27  获取血压(收缩压和舒张压）的组合数据-根据 
        public static List<Graph> GetBPInfo(DataConnection pclsCache, string UserId, string PlanNo, string ItemType, int StartDate, int EndDate, List<MstBloodPressure> reference)
        {
            List<Graph> graphList = new List<Graph>();

            //获取系统时间  数据库连接，这样写，应该对的
            string serverTime = "";
            if (pclsCache.Connect())
            {
                serverTime = Convert.ToDateTime(Cm.CommonLibrary.GetServerDateTime(pclsCache.CacheConnectionObject)).ToString("yyyyMMdd");

            }
            pclsCache.DisConnect();

            try
            {
                //收缩压表
                DataTable sysInfo = new DataTable();
                sysInfo = PsCompliance.GetSignDetailByPeriod(pclsCache, UserId, PlanNo, "Bloodpressure", "Bloodpressure_1", StartDate, EndDate);
                //RecordDate、RecordTime、Value、Unit

                //舒张压表
                DataTable diaInfo = new DataTable();
                diaInfo = PsCompliance.GetSignDetailByPeriod(pclsCache, UserId, PlanNo, "Bloodpressure", "Bloodpressure_2", StartDate, EndDate);


                //脉率

                if ((sysInfo.Rows.Count == diaInfo.Rows.Count) && (sysInfo.Rows.Count > 0))
                {

                    for (int rowsCount = 0; rowsCount < sysInfo.Rows.Count; rowsCount++)
                    {
                        Graph Graph = new Graph();
                        Graph.date = sysInfo.Rows[rowsCount]["RecordDate"].ToString();
                        Graph.SBPvalue = sysInfo.Rows[rowsCount]["Value"].ToString();
                        Graph.DBPvalue = diaInfo.Rows[rowsCount]["Value"].ToString();

                        //收缩压
                        if (Graph.SBPvalue != "")
                        {
                            Graph.SBPGrade = CmMstBloodPressure.GetSignGrade("Bloodpressure_1", Convert.ToInt32(Graph.SBPvalue), reference);
                            Graph.SBPlineColor = CmMstBloodPressure.GetSignColor(Graph.SBPGrade);

                        }

                        else
                        {
                            Graph.SBPGrade = "";
                            Graph.SBPlineColor = "";
                        }

                        //舒张压
                        if (Graph.DBPvalue != "")
                        {
                            Graph.DBPGrade = CmMstBloodPressure.GetSignGrade("Bloodpressure_2", Convert.ToInt32(Graph.DBPGrade), reference);
                            Graph.DBPlineColor = CmMstBloodPressure.GetSignColor(Graph.DBPGrade);
                        }
                        else
                        {
                            Graph.DBPGrade = "";
                            Graph.DBPGrade = "";
                        }


                        if (rowsCount != sysInfo.Rows.Count - 1)
                        {
                            Graph.SBPbulletShape = "round";
                            Graph.DBPbulletShape = "round";
                        }
                        else
                        {
                            if (serverTime == Graph.date)  //当天的血压点形状用菱形
                            {
                                Graph.SBPbulletShape = "diamond";
                                Graph.DBPbulletShape = "diamond";
                            }
                            else
                            {
                                Graph.SBPbulletShape = "round";
                                Graph.DBPbulletShape = "round";
                            }

                        }

                        graphList.Add(Graph);
                    }

                }

                return graphList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetBPInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {

            }
        }

        //GetDescription SYF 2015-04-22 根据收缩压获取血压等级说明
        public static string GetDescription(DataConnection pclsCache, int SBP)
        {
            string ret = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (string)Cm.MstBloodPressure.GetDescription(pclsCache.CacheConnectionObject, SBP);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Cm.MstBloodPressure.GetDescription", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //获取血压表全部信息 2015-05-29 GL
        public static DataTable GetBloodPressureList(DataConnection pclsCache)
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Code", typeof(string)));
            list.Columns.Add(new DataColumn("Name", typeof(string)));
            list.Columns.Add(new DataColumn("Description", typeof(string)));
            list.Columns.Add(new DataColumn("SBP", typeof(string)));
            list.Columns.Add(new DataColumn("DBP", typeof(string)));
            list.Columns.Add(new DataColumn("PatientClass", typeof(string)));
            list.Columns.Add(new DataColumn("Redundance", typeof(string)));

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Cm.MstBloodPressure.GetBPGrades(pclsCache.CacheConnectionObject);

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Rows.Add(cdr["Code"].ToString(), cdr["Name"].ToString(), cdr["Description"].ToString(), cdr["SBP"].ToString(), cdr["DBP"].ToString(), cdr["PatientClass"].ToString(), cdr["Redundance"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Cm.MstBloodPressure.GetBPGrades", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //SetData 血压表插数 2015-05-29 GL
        public static int SetBloodPressure(DataConnection pclsCache, string Code, string Name, string Description, int SBP, int DBP, string PatientClass, string Redundance)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Cm.MstBloodPressure.SetData(pclsCache.CacheConnectionObject, Code, Name, Description, SBP, DBP, PatientClass, Redundance);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Cm.MstBloodPressure.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //DeleteData 血压表删数 2015-05-29 GL
        public static int DeleteBloodPressure(DataConnection pclsCache, string Code)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Cm.MstBloodPressure.DeleteData(pclsCache.CacheConnectionObject, Code);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Cm.MstBloodPressure.DeleteData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }



    }
}