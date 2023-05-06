using CCWin.SkinControl;
using System.Text.RegularExpressions;

namespace Common.Utils
{
    public static class DataGridViewHelper
    {
        /// <summary>
        /// 隐藏没有中文头的列
        /// </summary>
        /// <param name="skinDataGridView"></param>
        public static void VisibleCloumns(SkinDataGridView skinDataGridView)
        {
            for (int i = 0; i < skinDataGridView.ColumnCount; i++)
            {

                string pattern = "[\u4e00-\u9fbb]";
                bool result = Regex.IsMatch(skinDataGridView.Columns[i].HeaderText, pattern);
                skinDataGridView.Columns[i].Visible = result;
            }
        }
    }
}
