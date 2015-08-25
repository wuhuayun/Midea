using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MideaAscm.Dal.Vehicle.Entities;
using System.Threading;
using ModuleTech;
using MideaAscm.Dal.Base.Entities;
using System.Net.Sockets;
using System.Net;
using MideaAscm.Services.Vehicle;
using MideaAscm.Services.Base;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using YnFrame.Services;
using MideaAscm.Dal.Base;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.Warehouse;

namespace MideaAscm.Server
{
    public partial class MainForm : Form
    {
        #region 变量定义
        //定义环境变量
        private string LogPath = "Log";
        private string LogPathSub = "";
        private System.DateTime dtCurrent;
        public static MainForm mainForm;
        public string AppRegistryKey = "YouNengSoft\\MideaAscmServer";
        //定义托盘变量
        private ContextMenu notifyiconMnu;
        private bool bCanClose = false;

        //时钟锁
        private static ReaderWriterLock m_readerWriterLock_unloadingPoint = new ReaderWriterLock();
        private static ReaderWriterLock m_readerWriterLock_readingHead = new ReaderWriterLock();

        //定义一个委托实现回调函数
        public delegate void CallBackDelegateWriteLog(string message);
        #endregion

        #region 窗口
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.chkSwitch.Checked = true;
            //当前日期
            this.dtCurrent = System.DateTime.Today;
            LogPathSub = dtCurrent.ToString("yyyyMM");
            //初始化托盘程序的各个要素 
            InitializeNotifyIcon();
            this.AddLog("ASCM网络服务端程序启动...");
            mainForm = this;
            //this.dateTimePicker1.Enabled = this.radioButtonManual.Checked;

            //string server = "http://localhost:8537/";
            //string server = "http://10.16.9.42:83/";

            //MideaAscm.Client.DalInterface.AscmWsInterface.oWebProxy = oWebProxy;
            //MideaAscm.Client.DalInterface.AscmWsInterface.webSericeUrl = server + "AscmWebService.asmx";


        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //关闭窗体,不关闭窗口
            if (!bCanClose)
            {
                this.Hide();
                e.Cancel = true;//不真的离开,只是隐藏
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            ////关闭卸货点线程
            //CloseUnloadingPointRead();
            ////关闭读写器线程
            //CloseReadingHeadRead();
            ////删除托盘
            //notifyIcon1.Dispose();

        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            //关闭系统
            DialogResult DlgResult;
            DlgResult = MessageBox.Show("确认要关闭数据服务程序吗？", "确认...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (DlgResult == DialogResult.Yes)
            {
                //关闭监听和线程
                //MyServerListener.Stop();
                try
                {
                    //this.myServerThreadAssess.Abort();
                }
                catch
                {
                }
                bCanClose = true;
                this.Close();
            }
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
        #endregion

        #region 托盘操作
        private void InitializeNotifyIcon()
        {
            //设定托盘程序的各个属性 
            notifyIcon1.Text = "ASCM服务";
            notifyIcon1.Visible = true;

            //定义一个MenuItem数组，并把此数组同时赋值给ContextMenu对象 
            MenuItem[] mnuItms = new MenuItem[3];
            mnuItms[0] = new MenuItem();
            mnuItms[0].Text = "ASCM服务";
            mnuItms[0].Click += new System.EventHandler(this.TrayMenuShowMessage);

            mnuItms[1] = new MenuItem("-");

            mnuItms[2] = new MenuItem();
            mnuItms[2].Text = "退出系统";
            mnuItms[2].Click += new System.EventHandler(this.TrayMenuClose);
            mnuItms[2].DefaultItem = true;

            notifyiconMnu = new ContextMenu(mnuItms);
            notifyIcon1.ContextMenu = notifyiconMnu;
            //为托盘程序加入设定好的ContextMenu对象 
        }
        public void TrayMenuShowMessage(object sender, System.EventArgs e)
        {
            this.Show();
        }
        public void TrayMenuClose(object sender, System.EventArgs e)
        {
            //关闭系统 
            //MessageBox.Show("请打开窗口点击关闭系统！");
            btnExit_Click(sender, e);
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
        #endregion

        #region 日志
        public void AddLog(string sMsg)
        {
            //写窗口(日志)
            //每天一个Text
            if (dtCurrent.ToString("yyyy-MM-dd").CompareTo(System.DateTime.Today.ToString("yyyy-MM-dd")) != 0)
            {
                listBoxLog.Items.Clear();
            }
            listBoxLog.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + sMsg);
            WriteLogFile(sMsg);
        }
        public void WriteLogFile(string sMsg)
        {
            //写日志文件
            //bool bReturn = true;
            string sLogDir = "", sLogFileWithPath = "";
            System.DateTime DtTmp;
            sLogDir = Application.StartupPath + "\\" + LogPath + "\\" + LogPathSub;
            DtTmp = System.DateTime.Now;

            try
            {
                //判断日志目录是否存在，否则创建
                if (!Directory.Exists(sLogDir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(sLogDir);
                }
                //打开文件,一天一个日志
                sLogFileWithPath = sLogDir + "\\YnServer" + DtTmp.ToString("yyyyMMdd") + ".txt";
                StreamWriter MyStreamWriter = File.AppendText(sLogFileWithPath);
                MyStreamWriter.WriteLine("{0}{1},{2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), sMsg);
                MyStreamWriter.Flush();
                MyStreamWriter.Close();
            }
            catch
            {
                //bReturn = false;
            }
            finally
            {
            }
            //return bReturn;
        }
        #endregion

        #region 设置
        private void button_setup_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region 卸货点
        private void button_unloadingPoint_Run_Click(object sender, EventArgs e)
        {
        }

        private void button_unloadingPoint_Stop_Click(object sender, EventArgs e)
        {
            //CloseUnloadingPointRead();
            button_unloadingPoint_Run.Enabled = true;
            button_unloadingPoint_Stop.Enabled = false;
        }
        public class UnloadingPointThreadClass
        {
            private MainForm form = null;
            // 回调委托        
            private CallBackDelegateWriteLog callBackDelegateWriteLog;
            private int count = 0;
            public UnloadingPointThreadClass(MainForm form, CallBackDelegateWriteLog callBackDelegateWriteLog)
            {
                this.form = form;
                this.callBackDelegateWriteLog = callBackDelegateWriteLog;
            }
            public void Read()
            {
                //    try
                //    {
                //        if (form.listUnloadingPointController != null)
                //        {
                //            for (int iUnloadingPointController = 0; iUnloadingPointController < form.listUnloadingPointController.Count;iUnloadingPointController++ )
                //            {
                //                UnloadingPointController unloadingPointController = form.listUnloadingPointController[iUnloadingPointController];
                //                if (unloadingPointController.Connected == false)
                //                {

                //                }

                //                //更新状态
                //                int[] statusdata = unloadingPointController.ReadStatus();

                //                foreach (AscmUnloadingPoint ascmUnloadingPoint in unloadingPointController.listAscmUnloadingPoint)
                //                for(int i=0;i<unloadingPointController.listAscmUnloadingPoint.Count();i++)
                //                {
                //                    string statusDefine = null;

                //                    if (statusdata[ascmUnloadingPoint.controllerAddress] == 1)
                //                    {
                //                        statusDefine = AscmUnloadingPoint.StatusDefine.inUse;
                //                    }
                //                    if (statusdata[ascmUnloadingPoint.controllerAddress] == 0)
                //                    {
                //                        statusDefine = AscmUnloadingPoint.StatusDefine.idle;
                //                    }
                //                    if (statusDefine != ascmUnloadingPoint.status)
                //                    {
                //                        bool flag = true;
                //                        if (ascmUnloadingPoint.status == AscmUnloadingPoint.StatusDefine.reserve && statusDefine == AscmUnloadingPoint.StatusDefine.idle)
                //                        {
                //                            DateTime dt = DateTime.Now;
                //                            if (DateTime.TryParse(ascmUnloadingPoint.modifyTime, out dt))
                //                            {
                //                                int reserveInvalid = 5;
                //                                int.TryParse(YnParameterService.GetInstance().GetValue(MyParameter.reserveInvalid), out reserveInvalid);
                //                                if (reserveInvalid < 5)
                //                                    reserveInvalid = 5;
                //                                flag = DateTime.Now.CompareTo(dt.AddMinutes(reserveInvalid)) > 0;
                //                            }
                //                        }
                //                        if (flag)
                //                        {
                //                            ascmUnloadingPoint.status = statusDefine;
                //                            AscmUnloadingPointService.GetInstance().UpdateStatus(ascmUnloadingPoint.id, ascmUnloadingPoint.status, "unloadingPointKey");
                //                        }
                //                        //ascmUnloadingPoint.status = statusDefine;
                //                        //AscmWsInterface.GetInstance().UnloadingPointUpdateStatus(ascmUnloadingPoint.id, ascmUnloadingPoint.status);
                //                        //AscmUnloadingPointService.GetInstance().UpdateStatus(ascmUnloadingPoint.id, ascmUnloadingPoint.status);

                //                    }
                //                }

                //            }
                //        }
                //        m_readerWriterLock_unloadingPoint.ReleaseReaderLock();
                //        System.Threading.Thread.Sleep(10*1000);//ms
                //    }
                //    catch (Exception ex)
                //    {
                //        //form.WriteLogFile("卸货点控制器读取异常：");// + ex.Message
                //        if (callBackDelegateWriteLog != null) callBackDelegateWriteLog("卸货点控制器读取异常：");// + ex.Message
                //    }
                //}

                //bThread = false;
                //if (threadReader != null)
                //{
                //    threadReader.Abort();
                //}
            }
            private void ProcessRfidRead(string tag) //显示数据
            {
                try
                {

                    ////在主窗口中显示数据                                     
                    //ActingThread AcT = new ActingThread(form.ProcessRfidTag);
                    //form.BeginInvoke(AcT, new object[] { rfidReader });

                }
                catch (Exception ex)
                {
                }
            }
            /*
            private void ProcessRfidRead_EmployeeCar(int readingHeadId, AscmRfid ascmRfid)//员工
            {
                try
                {
                    //AscmEmployee ascmEmployee = AscmEmployeeService.GetInstance().GetByRfid(tag.Trim());
                    AscmEmployeeCar ascmEmployeeCar = AscmEmployeeCarService.GetInstance().GetByRfid(ascmRfid.id);
                    if (ascmEmployeeCar != null)
                    {
                        bool pass=true;
                        //可以放行
                        AscmEmpCarSwipeLog ascmEmpCarSwipeLog = AscmEmpCarSwipeLogService.GetInstance().GetAddLog(readingHeadId, ascmRfid.id, pass, ascmEmployeeCar._employeeName + ":" + ascmEmployeeCar.plateNumber);

                        //写数据库
                        AscmEmpCarSwipeLogService.GetInstance().Save(ascmEmpCarSwipeLog);

                        //在主窗口中显示数据                                     
                        ActingThread AcT = new ActingThread(form.ShowRfidTag);
                        string strId = ascmRfid.id;
                        form.BeginInvoke(AcT, new object[] { strId, "EmployeeCar" });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void ProcessRfidRead_SupplierCar(string tag)//供应商
            {
            }*/
        }
        private void CloseUnloadingPointRead()
        {
            //try
            //{
            //    //timer_unloadingPoint.Stop();
            //    //unloadingPoint_threadRead = false;
            //    //if (unloadingPoint_thread != null)
            //    //{
            //    //    unloadingPoint_thread.Abort();
            //    //    unloadingPoint_thread = null;
            //    //}
            //    //if (listUnloadingPointController != null)
            //    //{
            //    //    foreach (UnloadingPointController unloadingPointController in listUnloadingPointController)
            //    //    {
            //    //        if (unloadingPointController.socket_Iocontroller.Connected)
            //    //        {
            //    //            unloadingPointController.socket_Iocontroller.Shutdown(SocketShutdown.Both);
            //    //            unloadingPointController.socket_Iocontroller.Disconnect(false);
            //    //        }
            //    //        unloadingPointController.socket_Iocontroller.Close();
            //    //    }
            //    //    listUnloadingPointController.Clear();
            //    }
              
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("关闭卸货点线程失败:" + ex.Message, "错误....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        #endregion

        #region 提取设备卸货点

        private void timer_unloadingPoint_Tick(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadUnloadingPoint), new object());
        }

        private static void LoadUnloadingPoint(object o)
        {
            try
            {
                List<UnloadingPointController> listUnloadingPointController = new  List<UnloadingPointController>();
                List<AscmUnloadingPointController> listAscmUnloadingPointController = AscmUnloadingPointControllerService.GetInstance().GetList("from AscmUnloadingPointController");
                List<AscmUnloadingPoint> listAscmUnloadingPoint = AscmUnloadingPointService.GetInstance().GetList("from AscmUnloadingPoint");
                foreach (AscmUnloadingPointController controller in listAscmUnloadingPointController)
                {
                    UnloadingPointController unloadingPointController = new UnloadingPointController();
                    unloadingPointController.ascmUnloadingPointController = controller;
                    foreach (AscmUnloadingPoint unloadingPoint in listAscmUnloadingPoint)
                    {
                        if (unloadingPoint.controllerId == controller.id)
                        {
                            unloadingPointController.listAscmUnloadingPoint.Add(unloadingPoint);
                        }
                    }
                    listUnloadingPointController.Add(unloadingPointController);
                }

                foreach (UnloadingPointController controller in listUnloadingPointController)
                {
                    Thread thread = new Thread(UpdateStatus);
                    thread.Start(controller);
                }
            }
            catch (Exception ex)
            {
                //this.AddLog("加载卸货点控制器异常：");// + ex.Message
            }
        }

        private static void UpdateStatus(Object o)
        {
            try
            {
                UnloadingPointController controller = (UnloadingPointController)o;
                int[] status = controller.ReadStatus();
                foreach (AscmUnloadingPoint point in controller.listAscmUnloadingPoint)
                {
                    if (!controller.Connected)
                    {
                        point.status = AscmUnloadingPoint.StatusDefine.breakdown;
                    }
                    else
                    {
                        if (status[point.controllerAddress] == 1)
                        {
                            point.status = AscmUnloadingPoint.StatusDefine.inUse;
                        }
                        if (status[point.controllerAddress] == 0)
                        {
                            point.status = AscmUnloadingPoint.StatusDefine.idle;
                        }
                    }
                    Oracle.AscmUnloadingPointUpdate(point);
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region 提取设备读写器
        private List<ReadingHead> listReadingHead = new List<ReadingHead>();
        private void timer_readingHead_Tick(object sender, EventArgs e)
        {
            if (!button_RfidReader_Run.Enabled)
            {
                timer_readingHead.Stop();
                LoadAscmReadingHead();
                timer_readingHead.Interval = 60 * 60 * 1000;//1个小时提一次
                //timer_readingHead.Interval = 60 * 1000;//60秒提一次
                timer_readingHead.Start();
            }
        }
        private void LoadAscmReadingHead()
        {
            List<AscmReadingHead> listAscmReadingHead = null;
            m_readerWriterLock_readingHead.AcquireReaderLock(10000);
            try
            {
                //listAscmReadingHead = AscmWsInterface.GetInstance().GetAllAscmReadingHeadList();
                listAscmReadingHead = AscmReadingHeadService.GetInstance().GetList(null, null, null, "", "");
                if (listAscmReadingHead != null)
                {
                    listAscmReadingHead = listAscmReadingHead.Where(P => P.ip.Trim() != "0.0.0.0").ToList();
                }
                //
            }
            catch (Exception ex)
            {
                this.AddLog("加载读写器异常：" + ex.Message);
            }
            m_readerWriterLock_readingHead.ReleaseReaderLock();
            try
            {
                //增加
                foreach (AscmReadingHead ascmReadingHead in listAscmReadingHead)
                {
                    ReadingHead readingHead = listReadingHead.Find(e => e.ascmReadingHead.id == ascmReadingHead.id);
                    if (readingHead == null)
                    {
                        readingHead = new ReadingHead();
                        readingHead.ascmReadingHead = ascmReadingHead;
                        //readingHead.StartThread();
                        ReadingHeadThreadClass readingHeadThreadClass = new ReadingHeadThreadClass(WriteLogFile, readingHead);
                        //线程终止后不能重启
                        if (readingHead.thread != null)
                        {
                            readingHead.thread.Abort();
                        }


                        //this.AddLog("加载读写器：" + ascmReadingHead.ip);
                        try
                        {
                            //连接
                            //ascmReadingHead.ip = "192.168.1.100";
                            //readingHead.modulerReader = Reader.Create(ascmReadingHead.ip, ModuleTech.Region.NA, 16);
                            readingHead.modulerReader = Reader.Create(ascmReadingHead.ip, ModuleTech.Region.NA, 4);
                            readingHead.moduleTech_isConnect = true;
                            //设置天线
                            int[] ants = new int[] { 1,2,3,4};//天线全部是四天线；
                            SimpleReadPlan plan = new SimpleReadPlan(ants);
                            readingHead.modulerReader.ParamSet("ReadPlan", plan);
                            //设置功率
                            List<AntPower> antspwr = new List<AntPower>();
                            antspwr.Add(new AntPower((byte)1, 3000, 3000));
                            antspwr.Add(new AntPower((byte)2, 3000, 3000));
                            antspwr.Add(new AntPower((byte)3, 3000, 3000));
                            antspwr.Add(new AntPower((byte)4, 3000, 3000));
                            readingHead.modulerReader.ParamSet("AntPowerConf", antspwr.ToArray());
                            //设置格式
                            readingHead.modulerReader.ParamSet("Gen2Session", ModuleTech.Gen2.Session.Session1);
                            //设置读取位置
                            EmbededCmdData ecd = new EmbededCmdData(ModuleTech.Gen2.MemBank.EPC, 2, 4);
                            readingHead.modulerReader.ParamSet("EmbededCmdOfInventory", ecd);

                            //开启线程
                            readingHead.thread = new Thread(readingHeadThreadClass.Read);
                            readingHead.thread.IsBackground = true;

                            readingHead.bThread = true;
                            readingHead.thread.Start();

                            listReadingHead.Add(readingHead);
                        }
                        catch (Exception ex)
                        {
                            this.AddLog("加载读写器失败[" + ascmReadingHead.ip + "]：" + ex.Message);
                        }
                    }
                }
                //删除没有的设备或失败的线程
                for (int iReadingHead = listReadingHead.Count - 1; iReadingHead >= 0; iReadingHead--)
                {
                    ReadingHead readingHead = listReadingHead[iReadingHead];
                    AscmReadingHead ascmReadingHead = listAscmReadingHead.Find(e => readingHead.ascmReadingHead.id == e.id);
                    if (ascmReadingHead == null || readingHead.thread == null || !readingHead.bThread)
                    {
                        readingHead.StopThread();
                        listReadingHead.RemoveAt(iReadingHead);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddLog("加载读写器异常：" + ex.Message);
            }
        }
        #endregion

        #region 读写器
        //private bool moduleTech_isConnect = false;
        public delegate void ActingThread(ReadingHead readingHead, TagReadData[] tags);//, RfidReader.RfidRead rfidRead
        private void button_RfidReader_Run_Click(object sender, EventArgs e)
        {
            button_RfidReader_Run.Enabled = false;
            button_RfidReader_Stop.Enabled = true;
            try
            {

                timer_readingHead.Interval = 100;//
                timer_readingHead.Start();
                ////线程终止后不能重启
                //try
                //{
                //    string sourceIp = "192.168.1.100";
                //    this.AddLog("加载读头：" + sourceIp);
                //    //modulerReader = Reader.Create(sourceIp, ModuleTech.Region.NA, 16);


                //    ModuleTechThreadClass moduleTechThreadClass = new ModuleTechThreadClass(this);
                //    moduleTech_thread = new Thread(moduleTechThreadClass.Read);
                //    moduleTech_thread.IsBackground = true;
                //    moduleTech_threadRead = true;
                //    moduleTech_thread.Start();

                //    //moduleTech_isInventory = true;
                //    //moduleTech_threadReader = new Thread(ModuleTechThread);
                //    //moduleTech_threadReader.Start();
                //}
                //catch (Exception ex)
                //{
                //    this.AddLog("加载读头：" + ex.Message);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动线程扫描失败:" + ex.Message, "错误....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_RfidReader_Stop_Click(object sender, EventArgs e)
        {
            CloseReadingHeadRead();
            button_RfidReader_Run.Enabled = true;
            button_RfidReader_Stop.Enabled = false;
        }
        private void CloseReadingHeadRead()
        {
            try
            {
                timer_readingHead.Stop();
                if (listReadingHead != null)
                {
                    foreach (ReadingHead readingHead in listReadingHead)
                    {
                        readingHead.StopThread();
                    }
                    listReadingHead.Clear();
                }

                //unloadingPoint_threadRead = false;
                //if (unloadingPoint_thread != null)
                //{
                //    unloadingPoint_thread.Abort();
                //    unloadingPoint_thread = null;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭读写器线程失败:" + ex.Message, "错误....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ProcessRfidTag(ReadingHead readingHead, TagReadData[] tags)
        {
            string[] epcdata = new string[tags.Count()];
            for (int i = 0; i < tags.Count(); i++)
            {
                if (tags[i] != null)
                {
                    epcdata[i] = tags[i].EPCString;
                    AddLog("读到值：" + epcdata[i]);
                    //ListViewItem item = new ListViewItem();
                    //listView1.Items.Add(epcdata[i]);
                    //MessageBox.Show(epcdata[i]);
                }
            }
        }
        public class ReadingHeadThreadClass
        {
            private ReaderParams rParms = new ReaderParams(200, 300, 1);
            //private MainForm form = null;
            private int sleepdur = 20;//
            private ReadingHead readingHead = null;
            // 回调委托        
            private CallBackDelegateWriteLog callBackDelegateWriteLog;
            public ReadingHeadThreadClass(CallBackDelegateWriteLog callBackDelegateWriteLog, ReadingHead readingHead)
            {
                //this.form = form;
                this.readingHead = readingHead;
                this.callBackDelegateWriteLog = callBackDelegateWriteLog;
            }
            public void Read()
            {
                while (readingHead.bThread)
                {
                    try
                    {
                        m_readerWriterLock_readingHead.AcquireReaderLock(10000);
                        if (readingHead.paused)
                        {
                            readingHead.resumeEvent.WaitOne();
                        }
                        //form.WriteLogFile(readingHead.ascmReadingHead.ip +"reading...");
                        //TagReadData[] reads = readingHead.modulerReader.Read(rParms.readdur);
                        TagReadData[] tags = readingHead.modulerReader.Read(1000);
                        //int a = tags.Count();
                        //count = a;

                        string[] epcdata = new string[tags.Count()];
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            if (tags[i] != null)
                            {

                                epcdata[i] = tags[i].EPCString;

                                //ListViewItem item = new ListViewItem();
                                //listView1.Items.Add(epcdata[i]);
                                //MessageBox.Show(epcdata[i]);
                            }
                        }
                        if (tags.Count() > 0)
                        {
                            ProcessRfidRead(tags);
                        }
                        m_readerWriterLock_readingHead.ReleaseReaderLock();
                        System.Threading.Thread.Sleep(sleepdur);//ms
                    }
                    catch (Exception ex)
                    {
                        //form.WriteLogFile("读写器[" + readingHead.ascmReadingHead.ip + "]读取异常：" + ex.Message);
                        // 通过委托, 将数据回传给回调函数
                        if (callBackDelegateWriteLog != null) callBackDelegateWriteLog("读写器[" + readingHead.ascmReadingHead.ip + "]读取异常：" + ex.Message);
                        readingHead.bThread = false;
                    }
                }
            }

            Dictionary<string, TagInfo> tagDic = new Dictionary<string, TagInfo>();

            private void ProcessRfidRead(TagReadData[] tags) //显示处理
            {
                try
                {
                    ////在主窗口中显示数据                                     
                    //ActingThread AcT = new ActingThread(form.ProcessRfidTag);
                    //form.BeginInvoke(AcT, new object[] { readingHead, tags });
                    //TimeSpan ts = DateTime.Now.Subtract(rfidReader.readTime);

                    string[] epcdata = new string[tags.Count()];

                    List<string> list = new List<string>();
                    foreach (KeyValuePair<string, TagInfo> tagInfo in tagDic)
                    {
                        if (DateTime.Now.Ticks - tagInfo.Value.time.Ticks > 10 * 10000000)
                        {
                            list.Add(tagInfo.Key);
                        }
                    }
                    foreach (string s in list)
                    {
                        tagDic.Remove(s);
                    }
                    list.Clear();

                    foreach (TagReadData tagData in tags)
                    {
                        if (!tagDic.ContainsKey(tagData.EPCString))
                        {
                            TagInfo tagInfo = new TagInfo();
                            tagInfo.ip = this.readingHead.ascmReadingHead.ip;
                            tagInfo.time = DateTime.Now;
                            tagInfo.tagEpc = tagData.EPCString;
                            tagDic.Add(tagData.EPCString, tagInfo);
                            //每次读取标签创建线程进行上传
                            ThreadPool.QueueUserWorkItem(new WaitCallback(UpLoad), tagInfo);
                        }
                    }

                    

                    //if (this.readingHead.ascmReadingHead.ip == "10.46.11.244")
                    //{
                    //    foreach (TagReadData tagData in tags)
                    //    {
                    //        WriteLogFile(Thread.CurrentThread.ManagedThreadId + " " + "汇总显示" + this.readingHead.ascmReadingHead.address + " " + tagData.EPCString);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void UpLoad(Object o)
            {
                //Dictionary<string, TagInfo> tagDic1 = (Dictionary<string, TagInfo>)o;
                TagInfo tagInfo = (TagInfo)o;
                string tag = tagInfo.tagEpc;
                DateTime readTime = tagInfo.time;
                string sn = tag.Substring(0, 6).Trim();
                //对读取标签进行过滤，将状态写入
                bool processed = false;
                //1.查找送货合单配送
                //List<AscmContainerDelivery> listAscmContainerDelivery = AscmContainerDeliveryService.GetInstance().GetCurrentList(sn, "readingHeadKey" + readingHead.ascmReadingHead.id);
                //if (listAscmContainerDelivery != null && listAscmContainerDelivery.Count() > 0)
                //{
                //    foreach (AscmContainerDelivery ascmContainerDelivery in listAscmContainerDelivery)
                //    {
                //        if (ascmContainerDelivery.ascmDeliBatSumMain != null &&
                //            (ascmContainerDelivery.ascmDeliBatSumMain.status == AscmDeliBatSumMain.StatusDefine.inPlant || ascmContainerDelivery.ascmDeliBatSumMain.status == AscmDeliBatSumMain.StatusDefine.confirm) &&
                //            (ascmContainerDelivery.ascmDeliveryOrderBatch != null && (string.IsNullOrEmpty(ascmContainerDelivery.ascmDeliveryOrderBatch.ascmStatus))))//||ascmContainerDelivery.ascmDeliveryOrderBatch.ascmStatus==AscmDeliveryOrderBatch.AscmStatusDefine.enterDoor
                //        {

                //            //待处理，写入ascmReadingHead读头id信息，已经获取ascmReadingHead.id
                //            ascmContainerDelivery.status = AscmContainerDelivery.StatusDefine.inWarehouseDoor;
                //            AscmContainerDeliveryService.GetInstance().Update(ascmContainerDelivery, this.readingHead.ascmReadingHead.id, "readingHeadKey" + readingHead.ascmReadingHead.id); //数据库容器标签状态更新并往container表里写入place;
                //            ascmContainerDelivery.ascmDeliveryOrderBatch.ascmStatus = AscmDeliveryOrderBatch.AscmStatusDefine.enterDoor;
                //            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDeliveryOrderBatch>(ascmContainerDelivery.ascmDeliveryOrderBatch, "readingHeadKey" + readingHead.ascmReadingHead.id);
                //            processed = true;
                //        }
                //    }
                //}
                //2.仓库备料单
                //AscmWmsContainerDelivery ascmWmsContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetCurrent(sn, "readingHeadKey" + readingHead.ascmReadingHead.id);
                //if (ascmWmsContainerDelivery != null)
                //{
                //    if (string.IsNullOrEmpty(ascmWmsContainerDelivery.status))
                //    {
                //        ascmWmsContainerDelivery.status = AscmWmsContainerDelivery.StatusDefine.outWarehouseDoor;
                //        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsContainerDelivery>(ascmWmsContainerDelivery, "readingHeadKey" + readingHead.ascmReadingHead.id);
                //    }
                //}

                //写数据库
                //AscmReadingHeadLogService.GetInstance().AddLog(this.readingHead.ascmReadingHead.id, tag, sn, readTime, processed, "readingHeadKey" + readingHead.ascmReadingHead.id);

                if (this.readingHead.ascmReadingHead.ip == "10.46.11.244")
                {
                    WriteLogFile(Thread.CurrentThread.ManagedThreadId + " " + this.readingHead.ascmReadingHead.address + " " + tag);
                }

                ////以下一条代码在合代码后要取消注释；by liuang 2013.8.21
                MideaAscm.Services.ContainerManage.AscmTagLogService.GetInstance().AscmTagLogSave(tag, this.readingHead.ascmReadingHead.id, "readingHeadKey" + readingHead.ascmReadingHead.id);//流转历史表里写数据；

            }

            public void WriteLogFile(string sMsg)
            {
                DateTime dtCurrent = System.DateTime.Today;
                String LogPathSub = dtCurrent.ToString("yyyyMM");
                //写日志文件
                //bool bReturn = true;
                string sLogDir = "", sLogFileWithPath = "";
                System.DateTime DtTmp;
                sLogDir = Application.StartupPath + "\\" + "Log" + "\\" + LogPathSub;
                DtTmp = System.DateTime.Now;

                try
                {
                    //判断日志目录是否存在，否则创建
                    if (!Directory.Exists(sLogDir))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(sLogDir);
                    }
                    //打开文件,一天一个日志
                    sLogFileWithPath = sLogDir + "\\YnServer" + DtTmp.ToString("yyyyMMdd") + ".txt";
                    StreamWriter MyStreamWriter = File.AppendText(sLogFileWithPath);
                    MyStreamWriter.WriteLine("{0}{1},{2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToString(), sMsg);
                    MyStreamWriter.Flush();
                    MyStreamWriter.Close();
                }
                catch
                {
                    //bReturn = false;
                }
                finally
                {
                }
                //return bReturn;
            }
        }

        public class TagInfo
        {
            public DateTime time { get; set; }

            public string tagEpc { get; set; }

            public String ip { get; set; }
        }

        public class ReaderParams
        {
            public ReaderParams(int rdur, int sdur, int sess)
            {
                readdur = rdur;
                sleepdur = sdur;
                gen2session = sess;
                isIpModify = false;
                isM5eModify = false;
                fisrtLoad = true;

                ip = "";
                subnet = "";
                gateway = "";
                macstr = "";
                hasIP = false;
                isGetIp = false;
                Gen2Qval = -2;
                isCheckConnection = false;
                isMultiPotl = false;
                antcnt = -1;
                isRevertAnts = false;
                weightgen2 = 30;
                weight180006b = 30;
                weightipx64 = 30;
                weightipx256 = 30;

                isIdtAnts = false;
                IdtAntsType = 0;
                DurIdtval = 0;
                AfterIdtWaitval = 0;

                FixReadCount = 0;
                isReadFixCount = false;
                isOneReadOneTime = false;

                usecase_ishighspeedblf = false;
                usecase_tagcnt = -1;
                usecase_readperform = -1;
                usecase_antcnt = -1;
            }

            public void resetParams()
            {
                isIpModify = false;
                isM5eModify = false;
                fisrtLoad = true;

                ip = "";
                subnet = "";
                gateway = "";
                macstr = "";
                hasIP = false;
                isGetIp = false;
                Gen2Qval = -2;
                isCheckConnection = false;
                isMultiPotl = false;
                antcnt = -1;
                SixteenDevsrp = null;
                SixteenDevConAnts = null;
                isRevertAnts = false;
                weightgen2 = 30;
                weight180006b = 30;
                weightipx64 = 30;
                weightipx256 = 30;

                isChangeColor = true;
                isUniByEmd = false;
                isUniByAnt = false;

                isIdtAnts = false;
                IdtAntsType = 0;
                DurIdtval = 0;
                AfterIdtWaitval = 0;

                FixReadCount = 0;
                isReadFixCount = false;
                isOneReadOneTime = false;

                usecase_ishighspeedblf = false;
                usecase_tagcnt = -1;
                usecase_readperform = -1;
                usecase_antcnt = -1;

            }

            public bool setGPO1;
            public int gen2session;
            public int readdur;
            public int sleepdur;
            public int antcnt;
            public string hardvir;
            public string softvir;
            public ReaderType readertype;

            public List<AntAndBoll> AntsState = new List<AntAndBoll>();
            public int ModuleReadervir;
            public string ip;
            public string subnet;
            public string gateway;
            public string macstr;
            public bool isGetIp;
            public bool isIpModify;
            public bool isM5eModify;
            public bool fisrtLoad;
            public bool hasIP;
            public int powermin;
            public int powermax;
            public int Gen2Qval;
            public bool isCheckConnection;
            public bool isMultiPotl;
            public SimpleReadPlan SixteenDevsrp = null;
            public int[] SixteenDevConAnts = null;
            public bool isRevertAnts;

            public int weightgen2;
            public int weight180006b;
            public int weightipx64;
            public int weightipx256;

            public bool isChangeColor;
            public bool isUniByEmd;
            public bool isUniByAnt;

            public bool isIdtAnts;
            public int IdtAntsType;
            public int DurIdtval;
            public int AfterIdtWaitval;

            public int FixReadCount;
            public bool isReadFixCount;
            public bool isOneReadOneTime;

            public bool usecase_ishighspeedblf;
            public int usecase_tagcnt;
            public int usecase_readperform;
            public int usecase_antcnt;

        }
        public class AntAndBoll
        {
            public AntAndBoll(int ant, bool conn)
            {
                antid = ant;
                isConn = conn;
            }

            public int antid;
            public bool isConn;
            public UInt16 rpower;
            public UInt16 wpower;
        }
        #endregion
    }
}
