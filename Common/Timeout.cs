using System;
using System.Threading;

namespace Common
{
    public delegate void DoHandler();
    public class Timeout
    {
        private ManualResetEvent mTimeoutObject;

        private bool mBoTimeout;

        public DoHandler Do;

        public Timeout()
        {
            //  初始状态为 停止  
            this.mTimeoutObject = new ManualResetEvent(true);
        }

        ///<summary>  
        /// 指定超时时间 异步执行某个方法  
        ///</summary>  
        ///<returns>执行 是否超时</returns>  
        public bool DoWithTimeout(TimeSpan timeSpan)
        {
            if (this.Do == null)
            {
                return false;
            }

            this.mTimeoutObject.Reset();
            this.mBoTimeout = true; //标记  
            Do.BeginInvoke(DoAsyncCallBack, null);

            // 等待 信号Set  
            if (!this.mTimeoutObject.WaitOne(timeSpan, false))
            {
                this.mBoTimeout = true;
            }
            return this.mBoTimeout;
        }

        ///<summary>  
                /// 异步委托 回调函数  
                ///</summary>  
                ///<param name="result"></param>  
        private void DoAsyncCallBack(IAsyncResult result)
        {
            try
            {
                this.Do.EndInvoke(result);
                // 指示方法的执行未超时  
                this.mBoTimeout = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.mBoTimeout = true;
            }
            finally
            {
                this.mTimeoutObject.Set();
            }
        }
        //————————————————
        //版权声明：本文为CSDN博主「沫小浩」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
        //原文链接：https://blog.csdn.net/qhr2617869/article/details/51305259
    }
}
