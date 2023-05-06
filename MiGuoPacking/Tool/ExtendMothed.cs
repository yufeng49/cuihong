using Entity.entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MiGuoPacking.Tool
{
    public static class ExtendMothed
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rich"></param>
        /// <param name="text">需要显示的内容</param>
        /// <param name="isshowtime">是否显示时间</param>
        public static void AddText(this RichTextBox rich, string text,bool isshowtime=false,bool isnewline =true)
        {
            LogPrint.PrintLog(rich, text, isshowtime,isnewline);
        }
        public static void AddText(this RichTextBox rich, string text, Color color)
        {
            LogPrint.PrintLog(rich, text, color,false,true);
        }
        public static void ClrearData(this DataGridView gridView)
        {
            LogPrint.ClearData(gridView);
        }
        public static int AddRowData(this DataGridView gridView)
        {
            return LogPrint.AddRowData(gridView);
        }
        public static void BandEntityData(this DataGridView gridView, List<EshengEntity> entity)
        {
            LogPrint.BandEntityData(gridView, entity);
        }
        public static void BandEntityData(this DataGridView gridView, List<qrcodeinfo> entity)
        {
            LogPrint.BandEntityData(gridView, entity);
        }
        public static void AddText(this GroupBox rich, string text)
        {
            LogPrint.PrintLog(rich, text);
        }

        public static void ClearInfo(this RichTextBox rich)
        {
            LogPrint.ClearInfo(rich);
        }

        public static void AddLab(this Label label, string text)
        {
            LogPrint.PrintLab(label, text);
        }

        public static void AddList(this RichTextBox ric, List<string> list)
        {
            ric.BeginInvoke(new Action(() =>
            {
                try
                {
                    string s = "";
                    foreach (var item in list)
                    {
                        s += item + "\r";

                    }
                    ric.Text = s;
                }
                catch (Exception)
                {
                    ric.Text = "";
                }
            }));
        }

        private static object locker = new object();
        public static void AddOrDel(this List<string> list, string value, int choose)
        {
            lock (locker)
            {
                if (choose == 1)
                {
                    list.Add(value);
                }
                if (choose == 2)
                {
                    list.Remove(value);
                }
            }
        }
    }
}
