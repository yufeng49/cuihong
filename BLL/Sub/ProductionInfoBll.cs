using BLL.Base;
using Entity.entity;

namespace BLL.Sub
{
    public class ProductionInfoBll : BaseServer<ProductionInfo>
    {
        public ProductionInfoBll()
        {

            baseDal = new Dll.Sub.ProductionInfoDal();

        }
    }
}
