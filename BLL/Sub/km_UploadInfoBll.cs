using BLL.Base;
using Dll.Sub;
using Entity.entity;
using System;
using System.Collections.Generic;

namespace BLL.Sub
{
    public class km_UploadInfoBll : BaseServer<km_UploadInfo>
    {
        private km_UploadInfoDal km_UploadInfoDal;
        public km_UploadInfoBll()
        {
            km_UploadInfoDal = new km_UploadInfoDal();
        }
    }

    public class km_CollectCodeBll : BaseServer<km_CollectCode>
    {
        private km_CollectCodeDal km_CollectCodeDal;
        public km_CollectCodeBll()
        {
            km_CollectCodeDal = new km_CollectCodeDal();
        }
        /// <summary>
        /// 新增订单号
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int AddOrder(km_UploadInfo kmui)
        {
            int ret = 0;
            km_UploadInfoDal ii = new km_UploadInfoDal();
            ii.AddOrder(kmui);
            return ret;

        }
        public long AddList(List<km_CollectCode> ls)
        {
            return km_CollectCodeDal.AddList(ls);
        }

        /// <summary>
        /// 检测订单是否上传，已上传则为true 表示可以切换
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public bool CheckOrderStatus(string orderno)
        {

            km_UploadInfoDal ii = new km_UploadInfoDal();
            bool fag =  ii.CheckOrder(orderno);

           List<km_CollectCode> ls =  km_CollectCodeDal.SearchBaglistBycode(orderno, "UploadOrder");
            if (fag == false &&ls.Count==0)//当单号状态为上传 并且采集列表无数据时，默认此单号废弃
            { fag = true; }
            return fag;
        }

        /// <summary>
        /// 装箱,装箱后修改小码状态为1，表示已装箱
        /// </summary>
        /// <param name="packls">待装箱的小码</param>
        /// <param name="code">箱码</param>
        /// <param name="bagcode">要修改状态的临时箱码</param>
        /// <returns></returns>
        public int Package(List<string> packls, string code, string bagcode)
        {
            return km_CollectCodeDal.Package(packls, code, bagcode);
        }
        /// <summary>
        /// 通过订单号查询所有采集的码
        /// </summary>
        /// <param name="order"></param>
        /// <param name="status">状态1.已组箱 2.已上传 3.所有数据</param>
        /// <returns></returns>
        public List<km_CollectCode> GetCollectedData(string order,int status )
        {
            return km_CollectCodeDal.GetCollectedData(order, status);
        }

        public void DeleteData(List<string> packls)
        {
            km_CollectCodeDal.DeleteData(packls);
        }

        /// <summary>
        /// 通过小码查询本次采集的小码(相机单次采集的数据)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<km_CollectCode> Searchlcode2bylocde1(string code)
        {
           return km_CollectCodeDal.Searchlcode2bylocde1(code);
        }
        /// <summary>
        /// 根据小码 查询对应箱码下的所有数据(该箱码包括正式箱码下的所有数据 或者是临时箱码下的所有数据)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<km_CollectCode> SearchBaglistBycode(string code)
        {
            return km_CollectCodeDal.SearchBaglistBycode(code);
        }
        /// <summary>
        /// 根据码查询所属箱码下的所有小码
        /// </summary>
        /// <param name="code">码</param>
        /// <param name="level">等级 lcode1 ,lcode2,lcode3</param>
        /// <returns></returns>
        public List<km_CollectCode> SearchBaglistBycode(string code,string level)
        {
            return km_CollectCodeDal.SearchBaglistBycode(code, level);
        }
        /// <summary>
        /// 查询码等级
        /// </summary>
        /// <param name="code"></param>
        /// <returns>1.小码 2.件码 3.箱码</returns>
        public int CheckLevel(string code)
        {
            return km_CollectCodeDal.CheckLevel(code);
        }
        public km_UploadInfo GetUploadinfo(string v)
        {
            km_UploadInfoDal ii = new km_UploadInfoDal();
            return ii.GetUploadinfo(v);
        }
        /// <summary>
        /// 根据箱码查询订单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string SearchOrderBycode(string code)
        {
            return km_CollectCodeDal.SearchCodeByOrder(code,"lcode3");
        }

        /// <summary>
        /// 箱码
        /// </summary>
        /// <param name="bagcode"></param>
        public void UploadCollectStatus(string bagcode)
        {
            km_CollectCodeDal.km_UpdateCollectedStatus(bagcode);
        }
        public long UploadFinish(string orderno)
        {
            km_UploadInfoDal ii = new km_UploadInfoDal();
         return  ii.km_UpdateStatus(orderno);//更新主订单

        }

        public bool Isexist(string code)
        {
            return km_CollectCodeDal.Isexist(code);
        }

        public bool ReplacementCode(List<string> oldls, List<string> newls, out string msg)
        {
          return  km_CollectCodeDal.ReplacementCode(oldls, newls,out msg);
        }

        /// <summary>
        /// 删除采集记录
        /// </summary>
        /// <param name="code">要删除的码</param>
        /// <param name="level">等级</param>
        public int DelCollectByCode(string code, string level)
        {
            return km_CollectCodeDal.DelCollectByCode(code,level);
        }

    }
}