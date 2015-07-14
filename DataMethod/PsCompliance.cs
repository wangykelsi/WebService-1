using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterSystems.Data.CacheClient;
using System.Data;
using WebService.CommonLibrary;
using InterSystems.Data.CacheTypes;
using WebService.DataClass;

namespace WebService.DataMethod
{
    public class PsCompliance
    {
        // 陆遥 2015-04-08 插入某条数据
        public static int SetData(DataConnection pclsCache, string PatientId, int Date, string PlanNo, Double Compliance, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Compliance.SetData(pclsCache.CacheConnectionObject, PatientId, Date, PlanNo, Compliance, revUserId, TerminalName, TerminalIP, DeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        // 陆遥 2015-04-08 获取患者某计划内某天的依从率
        public static double GetComplianceByDay(DataConnection pclsCache, string PatientId, int Date, string PlanNo)
        {
            double ret = 0.0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (double)Ps.Compliance.GetComplianceByDay(pclsCache.CacheConnectionObject, PatientId, Date, PlanNo);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetComplianceByDay", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        // 陆遥 2015-04-08 获取某计划的某段时间(包括端点)的依从率列表
        public static DataTable GetComplianceListByPeriod(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Date", typeof(string)));
            list.Columns.Add(new DataColumn("Compliance", typeof(string)));

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetComplianceListByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.Int).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.Int).Value = EndDate;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Rows.Add(cdr["Date"].ToString(), cdr["Compliance"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetComplianceListByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        // 陆遥 2015-04-08根据计划编码和日期，获取依从率
        public static DataTable GetTasksByDate(DataConnection pclsCache, string PatientId, int Date, string PlanNo)
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("TaskID", typeof(string)));
            list.Columns.Add(new DataColumn("TaskName", typeof(string)));
            list.Columns.Add(new DataColumn("Status", typeof(string)));

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetTasksByDate(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("Date", CacheDbType.Int).Value = Date;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Rows.Add(cdr["TaskID"].ToString(), cdr["TaskName"].ToString(), cdr["Status"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetTasksByDate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        // 陆遥 2015-04-08 在当天根据任务状态的完成情况输出相应的任务
        public static DataTable GetTaskByStatus(DataConnection pclsCache, string PatientId, string PlanNo, int PiStatus)
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Id", typeof(string)));
            list.Columns.Add(new DataColumn("Status", typeof(string)));
            list.Columns.Add(new DataColumn("TaskCode", typeof(string)));
            list.Columns.Add(new DataColumn("TaskName", typeof(string)));
            list.Columns.Add(new DataColumn("TaskType", typeof(string)));
            list.Columns.Add(new DataColumn("Instruction", typeof(string)));
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetTaskByStatus(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("PiStatus", CacheDbType.Int).Value = PiStatus;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Rows.Add(cdr["Id"].ToString(), cdr["Status"].ToString(), cdr["TaskCode"].ToString(), cdr["TaskName"].ToString(), cdr["TaskType"].ToString(), cdr["Instruction"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetTaskByStatus", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        //陆遥 2015-04-17 获取当天完成的(或未完成的)任务数
        public static int GetTaskNumber(DataConnection pclsCache, string PatientId, string PlanNo, int PiStatus)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Compliance.GetTaskNumber(pclsCache.CacheConnectionObject, PatientId, PlanNo, PiStatus);

                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetTaskNumber", "数据库操作异常！ error information" + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //GetCompliacneRate 计算某段时间的依从率 LS 2015-03-30
        public static string GetCompliacneRate(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {
            string compliacneRate = "";
            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetComplianceListByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                cdr = cmd.ExecuteReader();

                double sum = 0;
                int count = 0;
                while (cdr.Read())
                {
                    sum += Convert.ToDouble(cdr["Compliance"]);
                    count++;
                }

                if (count != 0)
                {
                    //compliacneRate = ((int)((sum / count) * 100)).ToString();
                    compliacneRate = (Math.Round(sum / count, 2, MidpointRounding.AwayFromZero) * 100).ToString(); //保留整数

                }

                return compliacneRate;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsCompliacne.GetCompliacneRate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        //GetSignDetailByPeriod 通过Ps.Compliance中的date获取当天某项生理参数值，形成系列 LS 2015-04-17
        public static DataTable GetSignDetailByPeriod(DataConnection pclsCache, string PatientId, string PlanNo, string ItemType, string ItemCode, int StartDate, int EndDate)
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("RecordDate", typeof(string)));
            list.Columns.Add(new DataColumn("RecordTime", typeof(string)));
            list.Columns.Add(new DataColumn("Value", typeof(string)));
            list.Columns.Add(new DataColumn("Unit", typeof(string)));
            CacheCommand cmd = null;
            CacheDataReader cdr = null;


            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.ComplianceDetail.GetSignDetailByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("ItemType", CacheDbType.NVarChar).Value = ItemType;
                cmd.Parameters.Add("ItemCode", CacheDbType.NVarChar).Value = ItemCode;
                cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                cdr = cmd.ExecuteReader();

                while (cdr.Read())
                {
                    list.Rows.Add(cdr["RecordDate"].ToString(), cdr["RecordTime"].ToString(), cdr["Value"].ToString(), cdr["Unit"].ToString());
                }

                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsCompliacne.GetSignDetailByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        //CaculateWeekDay 判断日期是星期几 LS 2015-03-27 
        public static string CaculateWeekDay(string date)
        {
            string week = "星期一";  //待标记颜色
            try
            {
                string weekDayEn = Convert.ToDateTime(date).DayOfWeek.ToString();
                switch (weekDayEn)
                {
                    case "Monday":
                        week = "星期一";
                        break;
                    case "Tuesday":
                        week = "星期二";
                        break;
                    case "Wednesday":
                        week = "星期三";
                        break;
                    case "Thursday":
                        week = "星期四";
                        break;
                    case "Friday":
                        week = "星期五";
                        break;
                    case "Saturday":
                        week = "星期六";
                        break;
                    case "Sunday":
                        week = "星期日";
                        break;
                    default: break;
                }

                return week;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsCompliance.CaculateWeekDay", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        //GetTasksComByPeriod 其他任务的依从情况（不包括生理测量） LS 2010505
        public static List<CompliacneDetailByD> GetTasksComByPeriod(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {

            List<CompliacneDetailByD> resultList = new List<CompliacneDetailByD>();

            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Date", typeof(string)));
            list.Columns.Add(new DataColumn("ComplianceValue", typeof(double)));
            list.Columns.Add(new DataColumn("TaskType", typeof(string)));
            list.Columns.Add(new DataColumn("TaskId", typeof(string)));
            list.Columns.Add(new DataColumn("TaskName", typeof(string)));
            list.Columns.Add(new DataColumn("Status", typeof(int)));

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetTasksComByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.Int).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.Int).Value = EndDate;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Rows.Add(cdr["Date"].ToString(), Convert.ToDouble(cdr["ComplianceValue"]), cdr["TaskType"].ToString(), cdr["TaskId"].ToString(), cdr["TaskName"].ToString(), Convert.ToInt32(cdr["Status"]));
                }

                //确保排序
                DataView dv = list.DefaultView;
                dv.Sort = "Date Asc, TaskType desc, Status Asc";
                DataTable list_sort = dv.ToTable();
                list_sort.Rows.Add("end", 0, "", "", "", 0);  //用于最后一天输出

                if (list_sort.Rows.Count > 1)
                {
                    string temp_date = list_sort.Rows[0]["Date"].ToString();
                    string temp_type = list_sort.Rows[0]["TaskType"].ToString();
                    string temp_str = "";
                    temp_str += "该天依从率：" + list_sort.Rows[0]["ComplianceValue"].ToString() + "<br>";
                    temp_str += "<b><span style='font-size:14px;'>" + list_sort.Rows[0]["TaskType"].ToString() + "：</span></b><br>";

                    CompliacneDetailByD CompliacneDetailByD = new CompliacneDetailByD();
                    CompliacneDetailByD.Date = list_sort.Rows[0]["Date"].ToString();
                    // CompliacneDetailByD.ComplianceValue = list_sort.Rows[0]["ComplianceValue"].ToString();

                    if (Convert.ToDouble(list_sort.Rows[0]["ComplianceValue"]) == 0)  //某天依从率
                    {
                        CompliacneDetailByD.drugBullet = "";
                        CompliacneDetailByD.drugColor = "#DADADA";
                    }
                    else if (Convert.ToDouble(list_sort.Rows[0]["ComplianceValue"]) == 1)
                    {
                        CompliacneDetailByD.drugBullet = "";
                        CompliacneDetailByD.drugColor = "#777777";
                    }
                    else
                    {
                        CompliacneDetailByD.drugBullet = "amcharts-images/drug.png";
                        CompliacneDetailByD.drugColor = "";
                    }


                    if (Convert.ToInt32(list_sort.Rows[0]["Status"]) == 1)  //某天某项任务的完成情况
                    {
                        temp_str += list_sort.Rows[0]["TaskName"].ToString() + "complete  ";
                    }
                    else
                    {
                        //temp_str += list_sort.Rows[0]["TaskName"].ToString() + "noncomplete  ";
                        temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[0]["TaskName"].ToString() + "noncomplete  " + "：</span></b>";
                    }


                    //只有一条数据
                    if (list_sort.Rows.Count == 2)
                    {
                        CompliacneDetailByD.Events = temp_str;
                        resultList.Add(CompliacneDetailByD);
                    }

                    //＞一条数据
                    if (list_sort.Rows.Count > 2)
                    {
                        for (int i = 1; i <= list_sort.Rows.Count - 1; i++)
                        {
                            if (temp_date == list_sort.Rows[i]["Date"].ToString())  //同一天
                            {
                                if (temp_type == list_sort.Rows[i]["TaskType"].ToString())     //同天同任务类型
                                {
                                    if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        temp_str += list_sort.Rows[i]["TaskName"].ToString() + "complete  ";
                                    }
                                    else
                                    {
                                        //temp_str += list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  ";
                                        temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[i]["TaskName"].ToString() + "noncomplete " + "</span></b>";
                                    }
                                }
                                else   //同天不同任务类型
                                {
                                    temp_str += "<br>";
                                    temp_str += "<b><span style='font-size:14px;'>" + list_sort.Rows[i]["TaskType"].ToString() + "：</span></b><br>";

                                    if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        temp_str += list_sort.Rows[i]["TaskName"].ToString() + "complete  ";
                                    }
                                    else
                                    {
                                        //temp_str += list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  ";
                                        temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  </span></b>";
                                    }


                                    temp_type = list_sort.Rows[i]["TaskType"].ToString();
                                }

                            }
                            else   //不同天
                            {
                                //上一天输出
                                CompliacneDetailByD.Events = temp_str;
                                resultList.Add(CompliacneDetailByD);

                                if (list_sort.Rows[i]["Date"].ToString() != "end")
                                {
                                    //获取新一天
                                    CompliacneDetailByD = new CompliacneDetailByD();
                                    CompliacneDetailByD.Date = list_sort.Rows[i]["Date"].ToString();
                                    //CompliacneDetailByD.ComplianceValue = list_sort.Rows[i]["ComplianceValue"].ToString();

                                    if (Convert.ToDouble(list_sort.Rows[i]["ComplianceValue"]) == 0)  //某天依从率
                                    {
                                        CompliacneDetailByD.drugBullet = "";
                                        CompliacneDetailByD.drugColor = "#DADADA";
                                    }
                                    else if (Convert.ToDouble(list_sort.Rows[i]["ComplianceValue"]) == 1)
                                    {
                                        CompliacneDetailByD.drugBullet = "";
                                        CompliacneDetailByD.drugColor = "#777777";
                                    }
                                    else
                                    {
                                        CompliacneDetailByD.drugBullet = "amcharts-images/drug.png";
                                        CompliacneDetailByD.drugColor = "";
                                    }

                                    temp_str = "";
                                    temp_str += "该天依从率：" + list_sort.Rows[i]["ComplianceValue"].ToString() + "<br>";
                                    temp_str += "<b><span style='font-size:14px;'>" + list_sort.Rows[i]["TaskType"].ToString() + "：</span></b><br>";

                                    if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        temp_str += list_sort.Rows[i]["TaskName"].ToString() + "complete  ";
                                    }
                                    else
                                    {
                                        //temp_str += list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  ";
                                        temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  </span></b>";
                                    }

                                    temp_date = list_sort.Rows[i]["Date"].ToString();
                                    temp_type = list_sort.Rows[i]["TaskType"].ToString();
                                }
                            }
                        }

                    }

                }

                return resultList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetTasksComByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        // 废止 GetDrugComByPeriod 李山 2010505 药物的依从情况 
        public static List<CompliacneDetailByD> GetDrugComByPeriod(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {


            List<CompliacneDetailByD> resultList = new List<CompliacneDetailByD>();

            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Date", typeof(string)));
            //list.Columns.Add(new DataColumn("ComplianceValue", typeof(double)));  药物的依从情况单独算出
            //list.Columns.Add(new DataColumn("TaskType", typeof(string)));   //只用药
            list.Columns.Add(new DataColumn("TaskId", typeof(string)));
            list.Columns.Add(new DataColumn("TaskName", typeof(string)));
            list.Columns.Add(new DataColumn("Status", typeof(int)));

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetDrugComByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.Int).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.Int).Value = EndDate;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())   //, cdr["TaskType"].ToString()
                {
                    list.Rows.Add(cdr["Date"].ToString(), cdr["TaskId"].ToString(), cdr["TaskName"].ToString(), Convert.ToInt32(cdr["Status"]));
                }

                //确保排序
                DataView dv = list.DefaultView;
                dv.Sort = "Date Asc, Status Asc";
                DataTable list_sort = dv.ToTable();
                list_sort.Rows.Add("end", "", "", 0);  //用于最后一天输出

                if (list_sort.Rows.Count > 1)
                {
                    string temp_date = list_sort.Rows[0]["Date"].ToString();
                    string temp_str = "";
                    //temp_str += "该天用药依从率："+ "<br>";
                    //temp_str += "<b><span style='font-size:14px;'>用药情况：</span></b><br>";
                    double count = 0;
                    double noncomplete = 0;

                    CompliacneDetailByD CompliacneDetailByD = new CompliacneDetailByD();
                    CompliacneDetailByD.Date = list_sort.Rows[0]["Date"].ToString();


                    if (Convert.ToInt32(list_sort.Rows[0]["Status"]) == 1)  //某天某项任务的完成情况
                    {
                        temp_str += list_sort.Rows[0]["TaskName"].ToString() + "complete  ";
                        count++;
                    }
                    else
                    {
                        //temp_str += list_sort.Rows[0]["TaskName"].ToString() + "noncomplete  ";
                        temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[0]["TaskName"].ToString() + "noncomplete  " + "</span></b>";
                        count++;
                        noncomplete++;
                    }


                    //只有一条数据
                    if (list_sort.Rows.Count == 2)
                    {
                        if ((noncomplete / count) == 1)          //某天药物依从率
                        {
                            //根本未完成
                            CompliacneDetailByD.drugBullet = "";
                            CompliacneDetailByD.drugColor = "#DADADA";
                        }
                        else if ((noncomplete / count) == 0)
                        {
                            //完成
                            CompliacneDetailByD.drugBullet = "";
                            CompliacneDetailByD.drugColor = "#777777";
                        }
                        else
                        {
                            CompliacneDetailByD.drugBullet = "amcharts-images/drug.png";
                        }

                        CompliacneDetailByD.Events = temp_str;
                        resultList.Add(CompliacneDetailByD);
                    }

                    //＞一条数据
                    if (list_sort.Rows.Count > 2)
                    {
                        for (int i = 1; i <= list_sort.Rows.Count - 1; i++)
                        {
                            if (temp_date == list_sort.Rows[i]["Date"].ToString())  //同一天
                            {
                                if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                {
                                    temp_str += list_sort.Rows[i]["TaskName"].ToString() + "complete  ";
                                    count++;
                                }
                                else
                                {
                                    //temp_str += list_sort.Rows[0]["TaskName"].ToString() + "noncomplete  ";
                                    temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  " + "</span></b>";
                                    count++;
                                    noncomplete++;
                                }

                            }
                            else   //不同天
                            {
                                //上一天输出
                                if ((noncomplete / count) == 1)          //某天药物依从率
                                {
                                    //根本未完成
                                    CompliacneDetailByD.drugBullet = "";
                                    CompliacneDetailByD.drugColor = "#DADADA";
                                }
                                else if ((noncomplete / count) == 0)
                                {
                                    //完成
                                    CompliacneDetailByD.drugBullet = "";
                                    CompliacneDetailByD.drugColor = "#777777";
                                }
                                else
                                {
                                    CompliacneDetailByD.drugBullet = "amcharts-images/drug.png";
                                }


                                CompliacneDetailByD.Events = temp_str;
                                resultList.Add(CompliacneDetailByD);

                                if (list_sort.Rows[i]["Date"].ToString() != "end")
                                {
                                    //获取新一天
                                    CompliacneDetailByD = new CompliacneDetailByD();
                                    CompliacneDetailByD.Date = list_sort.Rows[i]["Date"].ToString();

                                    temp_str = "";
                                    //temp_str += "该天用药依从率："+ "<br>";
                                    //temp_str += "<b><span style='font-size:14px;'>用药情况：</span></b><br>";
                                    count = 0;
                                    noncomplete = 0;

                                    if (Convert.ToInt32(list_sort.Rows[0]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        temp_str += list_sort.Rows[i]["TaskName"].ToString() + "complete  ";
                                        count++;
                                    }
                                    else
                                    {
                                        //temp_str += list_sort.Rows[0]["TaskName"].ToString() + "noncomplete  ";
                                        temp_str += "<b><span style='font-size:14px;color:red;'>" + list_sort.Rows[i]["TaskName"].ToString() + "noncomplete  " + "</span></b>";
                                        count++;
                                        noncomplete++;
                                    }

                                    temp_date = list_sort.Rows[i]["Date"].ToString();
                                }
                            }
                        }

                    }

                }

                return resultList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Compliance.GetTasksComByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
    }
}