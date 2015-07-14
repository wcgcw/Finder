using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DataBaseServer;

namespace Finder.util
{
    public class DataPersistenceControl
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void AsyncCaller();
        private ConcurrentQueue<List<ModelReleaseInfo>> Queue;

        private static readonly DataPersistenceControl instance = new DataPersistenceControl();
        private DataPersistenceControl()
        {
            Queue = new ConcurrentQueue<List<ModelReleaseInfo>>();
        }
        
        public static DataPersistenceControl GetInstance()
        {
            return instance;
        }

        public void Init()
        {
            Queue = new ConcurrentQueue<List<ModelReleaseInfo>>();
        }

        public void Add(List<ModelReleaseInfo> data)
        {
            Queue.Enqueue(data);
        }

        public void StartWrite()
        {
            Action actionG = () =>
            {
                List<ModelReleaseInfo> data;
                while (true)
                {
                    if (Program.ProClose)
                    {
                        break;
                    }
                    if (Queue.TryDequeue(out data))
                    {
                        //// 写入数据库
                        #region 数据入库
                        try
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("");
                            TbReleaseInfo tri = new TbReleaseInfo();
                            SQLitecommand cmd = new SQLitecommand();
                            foreach (var mri in data)
                            {
                                if (tri.GetReleaseInfoCount(mri.InfoSource, mri.KeyWords) > 0) continue;

                                sb.Append(tri.GetInsertStr(mri) + ";");
                            }

                            if (sb.ToString().Length > 0)
                            {
                                //执行插入
                                cmd.ExecuteNonQuery(sb.ToString());
                                //清除插入字段串
                                sb.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.Message);
                            Comm.WriteErrorLog(ex.StackTrace);
                        }
                        #endregion

                        log.Info("数据层写入数据库成功");
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            };

            Parallel.Invoke(actionG, actionG);
        }
    }
}
