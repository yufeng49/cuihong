using BLL.Base;
using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class BasicSettingBll : BaseServer<BasicSetting>
    {
        public BasicSettingBll()
        {
            baseDal = new BasicSettingDal();
        }
    }
}
