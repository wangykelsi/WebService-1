using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterSystems.Data.CacheClient;
using System.Data;
using WebService.CommonLibrary;

namespace WebService.DataMethod
{
    public class PsTreatmentIndicators
    {
        //SetData TDY 2014-12-1
        public static bool SetData(DataConnection pclsCache, string UserId, int SortNo, string AssessmentType, string AssessmentName, DateTime AssessmentTime, string Result, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            bool IsSaved = false;
            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return IsSaved;

                }
                int flag = (int)Ps.TreatmentIndicators.SetData(pclsCache.CacheConnectionObject, UserId, SortNo, AssessmentType, AssessmentName, AssessmentTime, Result, revUserId, TerminalName, TerminalIP, DeviceType);
                if (flag == 1)
                {
                    IsSaved = true;
                }
                return IsSaved;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "保存失败！");
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsTreatmentIndicators.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return IsSaved;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        // GetPsTreatmentIndicators   TDY 2014-12-1
        public static DataTable GetPsTreatmentIndicators(DataConnection pclsCache, string UserId)
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("SortNo", typeof(int)));
            list.Columns.Add(new DataColumn("AssessmentType", typeof(string)));
            list.Columns.Add(new DataColumn("AssessmentName", typeof(string)));
            list.Columns.Add(new DataColumn("AssessmentTime", typeof(DateTime)));
            list.Columns.Add(new DataColumn("Result", typeof(string)));
            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.TreatmentIndicators.GetPsTreatmentIndicators(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                //cmd.Parameters.Add("InvalidFlag", CacheDbType.Int).Value = InvalidFlag;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Rows.Add(cdr["SortNo"].ToString(), cdr["AssessmentType"].ToString(), cdr["AssessmentName"].ToString(), cdr["AssessmentTime"].ToString(), cdr["Result"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsTreatmentIndicators.GetPsTreatmentIndicators", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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