using BLL.Base;
using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class LineInfoBll : BaseServer<LineInfo>
    {
        public LineInfoBll()
        {
            baseDal = new LineInfoDal();
        }
    }
}
