using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Server
{
    public class YnTask
    {
        #region 构造
        public String AppRegistryKey = "";
        public YnTask(string sRegistryKey)
        {
            AppRegistryKey = sRegistryKey;
        }
        ~YnTask()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            //该方法由程序调用，在调用该方法之后对象将被终结。
            //因为我们不希望垃圾回收器再次终结对象，因此需要从终结列表中去除该对象。
            GC.SuppressFinalize(this);
            //因为是由程序调用该方法的，因此参数为true。
            Dispose(true);
        }
        //所有与回收相关的工作都由该方法完成
        private void Dispose(bool disposing)
        {
            lock (this) //避免产生线程错误。
            {
                if (disposing)
                {
                    //需要程序员完成释放对象占用的资源。
                    //MyConn.Close();
                    //MyConn.Dispose();
                    //MyConn=null;
                }

                //对象将被垃圾回收器终结。在这里添加其它和清除对象相关的代码。
            }
        }
        #endregion

        #region 函数
        public bool TaskDay()
        {
            return false;
        }
        #endregion
    }
}
