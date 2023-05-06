using System;
using System.Windows.Forms;

namespace Common.Utils
{
    public static class ThreadHelper
    {

        /// <summary>
        /// 跨线程访问
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="isblock"></param>
        public static void InvokeOnUiThreadIfRequired(Control control, Action action, bool isblock = false)
        {
            if (control.InvokeRequired)
            {
                if (isblock)
                    control.Invoke(action);
                else
                    control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
