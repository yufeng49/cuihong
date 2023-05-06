using Common.Utils;
using Dll.Base;
using System.Collections.Generic;

namespace BLL.Base
{
    public class BaseServer<T> where T : new()
    {
        public BaseServer()
        {

        }

        public BaseDal<T> baseDal;

        public void Add(List<T> list)
        {
            foreach (var item in list)
            {
                baseDal.Add(item);
            }
        }

        public void Update(List<T> list)
        {
            foreach (var item in list)
            {
                baseDal.Update(item);
            }
        }

        public int Add(T t)
        {
            return baseDal.Add(t);
        }

        public int Delete(int id)
        {
            return baseDal.Delete(id);
        }

        public int Delete()
        {
            return baseDal.Delete(0);
        }

        public int Delete(Dictionary<string, string> dic)
        {
            return baseDal.Delete(dic);
        }

        public T Update(T t)
        {
            return baseDal.Update(t);
        }

        public List<T> SelectBySearch(Dictionary<string, string> dic)
        {
            return TableHelper.DataSetToList<T>(baseDal.Select(dic));
            //  return baseDal.Select(dic);
        }

        public List<T> SelectAll()
        {
            return TableHelper.DataSetToList<T>(baseDal.Select());
        }

        public List<T> GetDataById(int Id)
        {
            return TableHelper.DataSetToList<T>(baseDal.Select(Id));
        }

        public int GetSum()
        {
            return baseDal.GetSum();
        }


    }
}
