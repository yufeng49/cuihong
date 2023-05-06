using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Common.Utils
{
    public class TableHelper
    {
        /// <summary>
        ///  DataSet 转 List<>集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<T> DataSetToList<T>(DataSet ds) where T : new()
        {

            List<T> list = new List<T>();
            //需要使用反射技术,动态设置属性，得到属性，动态调用方法
            Type type = typeof(T);//得到类型对象
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                T t = new T();
                //循环遍历所有的列
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    //在这个类型里，去查找这个属性名，
                    FieldInfo prop = type.GetField(GetFiledName(col.ColumnName));
                    if (prop != null)
                    {
                        //存在的话
                        // 设置t对象的prop属性值，值就是列的值
                        //if (row[col] is DBNull)
                        {
                            if (prop.FieldType.Name.Equals("String"))
                            {
                                prop.SetValue(t, row[col].ToString());
                            }
                            else if (prop.FieldType.Name.Equals("Int32"))
                            {
                                prop.SetValue(t, Convert.ToInt32(row[col].ToString().Length>0? row[col]:0 ));
                            }
                            else if (prop.FieldType.Name.Equals("DateTime"))
                            {
                                prop.SetValue(t, DateTime.Now);
                            }
                            else if (prop.FieldType.Name.Equals("Boolean"))
                            {
                                prop.SetValue(t, Convert.ToBoolean(row[col]));
                            }
                            else if (prop.FieldType.Name.Equals("Int64"))
                            {
                                prop.SetValue(t, Convert.ToInt64(row[col]));
                            }
                        }
                        //  else
                        //  {
                        //       prop.SetValue(t, row[col]);
                        //  }

                    }
                }

                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// DataSet 转 List<>集合,不做列名转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<T> DataSetToList<T>(DataSet ds, bool flag) where T : new()
        {

            List<T> list = new List<T>();
            //需要使用反射技术,动态设置属性，得到属性，动态调用方法
            Type type = typeof(T);//得到类型对象
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                T t = new T();
                //循环遍历所有的列
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    //在这个类型里，去查找这个属性名，
                    FieldInfo prop = type.GetField(col.ColumnName);
                    if (prop != null)
                    {
                        //存在的话
                        // 设置t对象的prop属性值，值就是列的值
                        //if (row[col] is DBNull)
                        {
                            if (prop.FieldType.Name.Equals("String"))
                            {
                                prop.SetValue(t, row[col]);
                            }
                            else if (prop.FieldType.Name.Equals("Int32"))
                            {
                                prop.SetValue(t, Convert.ToInt32(row[col]));
                            }
                            else if (prop.FieldType.Name.Equals("DateTime"))
                            {
                                prop.SetValue(t, DateTime.Now);
                            }
                            else if (prop.FieldType.Name.Equals("Boolean"))
                            {
                                prop.SetValue(t, Convert.ToBoolean(row[col]));
                            }

                        }
                        //  else
                        //  {
                        //       prop.SetValue(t, row[col]);
                        //  }

                    }
                }

                list.Add(t);
            }
            return list;
        }

        //根据sql字段名 获取属性
        private static string GetFiledName(String sqlName)
        {
            //SyasParam   0 ays 3 aram
            sqlName = sqlName.Substring(0, 1).ToUpper() + sqlName.Substring(1);
            string[] subNames = sqlName.Split('_');
            StringBuilder FiledName = new StringBuilder(subNames[0]); //属性名

            for (int i = 0; i < subNames.Length; i++)
            {
                if (i == 0) continue;
                FiledName.Append(subNames[i].Substring(0, 1).ToUpper() + subNames[i].Substring(1));
            }
            return FiledName.ToString();
        }

    }
}
