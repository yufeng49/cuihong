using Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MiGuoPacking.Tool
{
    public class LogPrint
    {
        private static string LogPath = Application.StartupPath + "\\Log";
        delegate void SetTextCallBack(RichTextBox rich, string text, bool isshowtime,bool isnewline);
        delegate void SetTextByColorCallBack(RichTextBox rich, string text, Color color, bool isshowtime,bool isnewline);
        delegate void SetTextCallBackByGb(GroupBox groupBox, string text);
        delegate void ClearTextCallBack(RichTextBox rich);
        delegate int AddGridRowData(DataGridView dgv);
        delegate void ClearGridData(DataGridView dgv);
        delegate void BandGridRowData(DataGridView dgv, List<Entity.entity.EshengEntity> e);
        delegate void BandGridRowDatas(DataGridView dgv, List<Entity.entity.qrcodeinfo> e);
        delegate void SetLabCallBack(Label lab, string text);



        public static void PrintLab(Label rich, string text)
        {
            if (rich.InvokeRequired)
            {
                SetLabCallBack stcb = new SetLabCallBack(PrintLab);
                rich.BeginInvoke(stcb, rich, text);
            }
            else
            {
                rich.Text = text;
                LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + " 控件 " + rich.Name + " 记录 " + text);
            }
        }



        public static void PrintLog(RichTextBox rich, string text, Color color,bool isshowtime,bool isnewline)
        {
            if (rich.InvokeRequired)
            {
                SetTextByColorCallBack stcb = new SetTextByColorCallBack(PrintLog);
                rich.BeginInvoke(stcb, rich, text, color, isshowtime,isnewline);
            }
            else
            {
                rich.SelectionStart = rich.TextLength;
                rich.SelectionLength = 0;
                rich.SelectionFont = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                rich.SelectionColor = color;
                rich.AppendText((isshowtime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " : "") + text + (isnewline ? "\r":""));
                rich.SelectionColor = rich.ForeColor;
                LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + " 控件 " + rich.Name + " 记录 " + text);
                rich.ScrollToCaret();
            }
        }

        public static void PrintLog(RichTextBox rich, string text, bool isshowtime,bool isnewline)
        {
            if (rich.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(PrintLog);
                rich.BeginInvoke(stcb, rich, text, isshowtime, isnewline);
            }
            else
            {
                rich.AppendText((isshowtime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" ":"")+ text + (isnewline ? "\r" : ""));
                LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + " 控件 " + rich.Name + " 记录 " + text);
                rich.ScrollToCaret();
            }
        }
        public static void PrintLog(GroupBox groupBox, string text)
        {
            if (groupBox.InvokeRequired)
            {
                SetTextCallBackByGb stcb = new SetTextCallBackByGb(PrintLog);
                groupBox.BeginInvoke(stcb, groupBox, text);
            }
            else
            {
                groupBox.Text = (text + "\r");
                LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + " 控件 " + groupBox.Name + " 记录 " + text);
            }
        }
        public static void ClearInfo(RichTextBox rich)
        {
            if (rich.InvokeRequired)
            {
                ClearTextCallBack ctcb = new ClearTextCallBack(ClearInfo);
                rich.BeginInvoke(ctcb, rich);
            }
            else
            {
                rich.Clear();
                LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + " 控件 " + rich.Name + " 清空");
                // rich.ScrollToCaret();
            }
        }
        public static void ClearData(DataGridView gridView)
        {
            if (gridView.InvokeRequired)
            {
                ClearGridData ctcb = new ClearGridData(ClearData);
                gridView.BeginInvoke(ctcb, gridView);
            }
            else
            {
                gridView.Rows.Clear();

                // rich.ScrollToCaret();
            }
        }
        internal static int AddRowData(DataGridView gridView)
        {
            int index = -1;
            if (gridView.InvokeRequired)
            {
                AddGridRowData ctcb = new AddGridRowData(AddRowData);
                IAsyncResult iar = gridView.BeginInvoke(ctcb, gridView);

            }
            else
            {
                index = gridView.Rows.Add();

                // rich.ScrollToCaret();
            }
            return index;
        }
        internal static void BandEntityData(DataGridView gridView, List<Entity.entity.EshengEntity> entity)
        {

            if (gridView.InvokeRequired)
            {
                BandGridRowData ctcb = new BandGridRowData(BandEntityData);
                IAsyncResult iar = gridView.BeginInvoke(ctcb, gridView, entity);

            }
            else
            {

                foreach (var item in entity)
                {
                    int index = gridView.Rows.Add();
                    gridView.Rows[index].Cells[0].Value = item.Id;
                    gridView.Rows[index].Cells[1].Value = item.QrCode;
                    gridView.Rows[index].Cells[2].Value = item.MaterialName;
                    gridView.Rows[index].Cells[3].Value = item.MaterialNo;
                    gridView.Rows[index].Cells[4].Value = item.WeightType;
                    gridView.Rows[index].Cells[5].Value = item.Printtime.Length > 0 ? Convert.ToDateTime(item.Printtime).ToString("yyyy-MM-dd HH:mm") : item.Printtime;
                    gridView.Rows[index].Cells[6].Value = item.Collecttime.Length > 0 ? Convert.ToDateTime(item.Collecttime).ToString("yyyy-MM-dd HH:mm") : item.Collecttime;
                    gridView.Rows[index].Cells[7].Value = getstatus(item.Inwarehouse);
                    gridView.Rows[index].Cells[8].Value = "补打";

                }

                // rich.ScrollToCaret();
            }
        }
        internal static void BandEntityData(DataGridView gridView, List<Entity.entity.qrcodeinfo> entity)
        {

            if (gridView.InvokeRequired)
            {
                BandGridRowDatas ctcb = new BandGridRowDatas(BandEntityData);
                IAsyncResult iar = gridView.BeginInvoke(ctcb, gridView, entity);

            }
            else
            {
                gridView.Rows.Clear();
                foreach (var item in entity)
                {
                    int index = gridView.Rows.Add();
                    gridView.Rows[index].Cells[0].Value = item.leaveFactoryNo;
                    gridView.Rows[index].Cells[1].Value = item.materialNo;
                    gridView.Rows[index].Cells[2].Value = item.materialName;
                    gridView.Rows[index].Cells[3].Value = item.qrCode;
                    gridView.Rows[index].Cells[4].Value = item.weight;
                    gridView.Rows[index].Cells[5].Value = item.weightType;
                    gridView.Rows[index].Cells[6].Value = "删除";
                }

                // rich.ScrollToCaret();
            }
        }
        private static string getstatus(string status)
        {
            string ret = "";
            if (status.Equals("1"))
            {
                ret = "已打印";
            }
            else if (status.Equals("2"))
            {
                ret = "已采集";
            }
            else if (status.Equals("3"))
            {
                ret = "已入库";
            }
            else if (status.Equals("4"))
            {
                ret = "未采集";
            }
            else
            {
                ret = "未知";
            }
            return ret;
        }
        public static void Add(RichTextBox rich, object text)
        {

            SafeInvoke(rich, () => { rich.AppendText(text + "\r"); });
        }

        public delegate void TaskDelegate();

        private delegate void InvokeMethodDelegate(Control control, TaskDelegate handler);

        public static void SafeInvoke(Control control, TaskDelegate handler)
        {
            if (control.InvokeRequired)
            {
                while (!control.IsHandleCreated)
                {
                    if (control.Disposing || control.IsDisposed)
                        return;
                }
                IAsyncResult result = control.BeginInvoke(new InvokeMethodDelegate(SafeInvoke), new object[] { control, handler });
                control.EndInvoke(result);//获取委托执行结果的返回值
                return;
            }
            IAsyncResult result2 = control.BeginInvoke(handler);
            control.EndInvoke(result2);
        }


    }
}
