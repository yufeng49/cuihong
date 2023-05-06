using BLL.Base;
using Dll.Sub;
using Entity.entity;
using System.Collections.Generic;

namespace BLL.Sub
{
    public class EliminateBll : BaseServer<EliminateModel>
    {
        private EliminateDal eedal;
        public EliminateBll()
        {
            baseDal = new EliminateDal();
            eedal = baseDal as EliminateDal;
        }


        public List<EliminateModel> SearchData(string condition)
        {
            return eedal.SelectBySearch(condition);
        }
    }
}