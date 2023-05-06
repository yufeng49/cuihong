using BLL.Base;
using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class ClientVersionBll : BaseServer<ClientVersion>
    {

        public ClientVersionBll()
        {
            baseDal = new ClientVersionDal();
        }
    }
}
