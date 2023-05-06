using BLL.Base;
using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class RelevanceCodeBll : BaseServer<RelevanceCode>
    {
        private RelevanceCodeDal relevanceCodeDal;
        public RelevanceCodeBll()
        {
            baseDal = new RelevanceCodeDal();
            relevanceCodeDal = baseDal as RelevanceCodeDal;
        }

        public int AddCode(string code, string baseCode)
        {
            return relevanceCodeDal.AddCode(code, baseCode);
        }

        public int GetCount()
        {
            return relevanceCodeDal.GetCount();
        }

        public int DelAll()
        {
            return relevanceCodeDal.ClearTable();
        }
    }

}
