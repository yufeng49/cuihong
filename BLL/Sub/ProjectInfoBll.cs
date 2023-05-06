using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class ProjectInfoBll : Base.BaseServer<ProjectInfo>
    {
        public ProjectInfoBll()
        {
            baseDal = new ProjectInfoDal();
        }
    }
}
