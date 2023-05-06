using System;
using System.Windows.Forms;

namespace Common.esClass
{
    public partial class esRichTextBox : RichTextBox
    {
        public esRichTextBox() : base()
        {
        }

        public void InvokeOnUiThreadIfRequired(Action action, bool isblock = false)
        {

            if (this.InvokeRequired)
            {
                if (isblock)
                    this.Invoke(action);
                else
                    this.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
