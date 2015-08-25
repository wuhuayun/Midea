using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Server
{
    ///<summary>RFID读写器</summary>
    public class ReadingHead
    {
        ///<summary>读头API</summary>
        public ModuleTech.Reader modulerReader = null;
        private bool moduleTech_isConnect = false;
        ///<summary>读头设置</summary>
        public AscmReadingHead ascmReadingHead { get; set; }
        ///<summary>线程</summary>
        public System.Threading.Thread thread = null;
        public bool bThread = false;
        public System.Threading.ManualResetEvent resumeEvent = new System.Threading.ManualResetEvent(false);
        public volatile bool paused;
        public void Pause()
        {
            paused = true;
            //StopRead();
            resumeEvent.Reset();

        }
        public void Resume()
        {
            //int ireturn = StartReadTag();
            //if (ireturn == 1)
            //{
            //}
            paused = false;
            resumeEvent.Set();
        }
        public void StartThread()
        {
        }
        public void StopThread()
        {
            if (bThread)
                bThread = false;
            if (thread != null)
            {
                thread.Abort();
            }
            ModuleTech_Disconnect();
            //StopRead();
        }
        private void ModuleTech_Disconnect()
        {
            if (moduleTech_isConnect)
            {
                modulerReader.Disconnect();
                moduleTech_isConnect = false;
                modulerReader = null;
            }
            //moduleTech_threadRead = false;
            //if (moduleTech_thread != null)
            //{
            //    moduleTech_thread.Abort();
            //    //moduleTech_threadReader.Join();
            //    moduleTech_thread = null;
            //}
        }
    }
}
