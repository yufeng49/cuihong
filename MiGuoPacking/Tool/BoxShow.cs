using System.Threading.Tasks;

namespace MiGuoPacking.Tool
{
    public static class BoxShow
    {
        private static LoadProcessBar loadProcessBar = new LoadProcessBar();//窗体加载过程

        public static void OpenLoadProcessBar()
        {
            Task thread = new Task(() =>
                {
                    loadProcessBar.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    loadProcessBar.ShowDialog();
                });
            //thread.IsBackground = true;
            //thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public static void CloseLoadProcessBar()
        {
            loadProcessBar.CloseForm();
        }
    }
}
