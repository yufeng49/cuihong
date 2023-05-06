using BLL.Base;
using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class UplaodInfoBll : BaseServer<UplaodInfo>
    {
        public UplaodInfoBll()
        {
            baseDal = new UplaodInfoDal();
        }
    }
}
