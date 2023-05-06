using Common.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Dll.Base
{
    /// <summary>
    /// 反射T生成sql语句
    /// </summary>
    public static class GenerateSql
    {

        public static string connectionString = ConfigHelper.GetValue("Connstr", "0");
        public static string MongoDBconnectionString = ConfigHelper.GetValue("MogoConnstr", "mongodb://localhost:27017/kmdb");

        public static string GenerateInsert<T>(T t)
        {
            Type type = typeof(T);
            string className = type.Name;
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo[] subfields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            // FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            fields.AddRange(subfields);
            //  fields.AddRange(basefields);
            StringBuilder sb = new StringBuilder("insert into " + GetSqlTableName(className) + " (");
            List<string> fieldValues = new List<string>();
            foreach (var item in fields)
            {
                if (item.Name == "Id") continue;
                sb.Append(GetSqlFiledName(item.Name) + ",");
                if (item.GetValue(t) == null)
                {
                    fieldValues.Add("");
                }
                else
                {
                    fieldValues.Add(item.GetValue(t).ToString());
                }
            }
            sb.Remove(sb.Length - 1, 1).Append(" ) VALUES (");
            foreach (var item in fieldValues)
            {
                sb.Append("'" + item + "',");
            }
            sb.Remove(sb.Length - 1, 1).Append(" );");
            return sb.ToString();
        }

        public static string GenerateDelete<T>(int id)
        {
            Type type = typeof(T);
            string className = type.Name;

            StringBuilder sb = new StringBuilder("delete from " + GetSqlTableName(className));
            if (id != 0)
            {
                sb.Append(" where id = " + id);
            };
            return sb.ToString();
        }

        public static string GenerateDeleteByWhere<T>(Dictionary<string, string> pojoParams)
        {
            Type type = typeof(T);
            string className = type.Name;

            StringBuilder sb = new StringBuilder("delete from " + GetSqlTableName(className) + " where 1=1 ");
            foreach (var pojo in pojoParams)
            {
                sb.Append(string.Format(" and {0} = '{1}'", GetSqlFiledName(pojo.Key), pojo.Value));
            }
            return sb.ToString();
        }

        public static string GenerateUpdate<T>(T t)
        {
            Type type = typeof(T);
            string className = type.Name; //SyasParam
            StringBuilder stringBuilder = new StringBuilder("update " + GetSqlTableName(className) + " set ");
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo[] subfields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            // FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            fields.AddRange(subfields);
            //fields.AddRange(basefields);
            int id = 0;
            foreach (FieldInfo field in fields)
            {
                if (field.Name.Equals("Id"))
                {
                    id = Convert.ToInt32(field.GetValue(t));
                    continue;
                }
                stringBuilder.Append(GetSqlFiledName(field.Name) + " = '" + field.GetValue(t) + "',");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1).Append(string.Format(" where id ={0}", id));
            return stringBuilder.ToString();
        }

        public static string SelectBySearch<T>(Dictionary<string, string> pojoParams)
        {
            Type t = typeof(T);
            string className = t.Name;
            StringBuilder stringBuilder = new StringBuilder("select *  from " + GetSqlTableName(className) + " where 1=1 ");
            if (pojoParams == null)
                return stringBuilder.ToString();
            foreach (var pojo in pojoParams)
            {
                stringBuilder.Append(string.Format(" and {0} = '{1}'", GetSqlFiledName(pojo.Key), pojo.Value));
            }
            return stringBuilder.ToString();
        }

        //根据类名获取表名 
        public static string GetSqlTableName(string className)
        {
            //SysParam   0 ays 3 aram
            string[] subNames = Regex.Split(className, "[A-Z]");
            StringBuilder tableName = new StringBuilder("t"); //表名

            for (int i = 0; i < subNames.Length; i++)
            {
                if (i == 0) continue;
                tableName.Append("_");

                tableName.Append(className[tableName.Length - 1 - i].ToString().ToLower());

                tableName.Append(subNames[i]);

            }
            return tableName.ToString();
        }

        //根据属性获取sql字段名 
        public static string GetSqlFiledName(string filedName)
        {
            //SyasParam   0 ays 3 aram   code codeName
            string[] subNames = Regex.Split(filedName, "[A-Z]");
            StringBuilder sqlFiledName = new StringBuilder(); //sql server中字段名

            for (int i = 0; i < subNames.Length; i++)
            {
                if (i == 0) continue;
                sqlFiledName.Append("_");

                sqlFiledName.Append(filedName[sqlFiledName.Length - i].ToString().ToLower());

                sqlFiledName.Append(subNames[i]);
            };
            return sqlFiledName.Remove(0, 1).ToString();
        }

        public static string GenerateAddTable<T>()
        {
            Type type = typeof(T);
            string className = type.Name;
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo[] subfields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            //FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            fields.AddRange(subfields);
            // fields.AddRange(basefields);
            StringBuilder sb = new StringBuilder("create table " + GetSqlTableName(className) + " (");
            List<string> fieldValues = new List<string>();
            foreach (var item in fields)
            {
                if (item.FieldType.Name == "String")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " varchar,");
                }
                if (item.FieldType.Name == "Int32")
                {
                    if (item.Name == "Id")
                    {
                        sb.Append(GetSqlFiledName(item.Name) + " INTEGER PRIMARY KEY AUTOINCREMENT,");
                    }
                    else
                    {
                        sb.Append(GetSqlFiledName(item.Name) + " int,");
                    }
                }
                if (item.FieldType.Name == "Int64")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " bigint,");
                }
                if (item.FieldType.Name == "DateTime")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " varchar,");
                }
                if (item.FieldType.Name == "Boolean")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " varchar,");
                }
                if (item.FieldType.Name == "Double")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " real,");
                }
            }
            sb.Remove(sb.Length - 1, 1).Append(" )");
            return sb.ToString();
        }

        public static string GenerateAddTableMysql<T>()
        {
            Type type = typeof(T);
            string className = type.Name;
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo[] subfields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            //PropertyInfo[] subfieldss = type.GetProperties();
            // FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            fields.AddRange(subfields);
            // fields.AddRange(basefields);
            StringBuilder sb = new StringBuilder("create table " + GetSqlTableName(className) + " (");
            List<string> fieldValues = new List<string>();
            foreach (var item in fields)
            {
                if (item.FieldType.Name == "String")
                {
                    if (item.Name == "SubmitInfo" || item.Name == "Childrens")
                        sb.Append(GetSqlFiledName(item.Name) + " text,");
                    else
                        sb.Append(GetSqlFiledName(item.Name) + " varchar(200),");
                }
                if (item.FieldType.Name == "Int32")
                {
                    if (item.Name == "Id")
                    {
                        sb.Append(GetSqlFiledName(item.Name) + " INTEGER PRIMARY KEY AUTO_INCREMENT,");
                    }
                    else
                    {
                        sb.Append(GetSqlFiledName(item.Name) + " int,");
                    }
                }
                if (item.FieldType.Name == "Inta64")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " bigint,");
                }
                if (item.FieldType.Name == "DateTime")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " varchar(50),");
                }
                if (item.FieldType.Name == "Boolean")
                {
                    sb.Append(GetSqlFiledName(item.Name) + " varchar(50),");
                }
            }
            sb.Remove(sb.Length - 1, 1).Append(" )");
            return sb.ToString();

        }

    }

    #region    以前的
    ///// <summary>
    ///// 生成insert语句
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public static string GenerateInsert<T>(T t) {
    //    Type type = typeof(T);
    //    List<string> fieldValues = new List<string>();
    //    string className = type.Name;
    //    StringBuilder sb = new StringBuilder("insert into " + GetClassNameByTypeName(className)+"(");
    //    List<FieldInfo> listField = new List<FieldInfo>();
    //    FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);//父类字段
    //    listField.AddRange(fields);
    //    listField.AddRange(basefields);
    //    foreach (var field in listField) {
    //        if (field.Name.Equals("id")) continue;
    //        if (field.Name.Equals("kk_")) continue;

    //        if (field.ToString().Contains("Int32") || field.ToString().Contains("Int64"))
    //        { //只做这四种简单类型
    //            sb.Append(getSqlFiledName(field.Name) + ",");
    //            fieldValues.Add(field.GetValue(t).ToString());
    //        }
    //        else if (field.ToString().Contains("String"))
    //        {
    //            sb.Append(getSqlFiledName(field.Name) + ",");
    //            if (field.GetValue(t) == null)
    //            {
    //                fieldValues.Add("");
    //            }
    //            else
    //            {
    //                fieldValues.Add(field.GetValue(t).ToString());
    //            }

    //        }
    //        else if (field.ToString().Contains("DateTime"))
    //        {
    //            sb.Append(getSqlFiledName(field.Name) + ",");
    //            if (field.Name.Equals("createDate"))
    //            {//创建时间，自动添加
    //                fieldValues.Add(DateTime.Now.ToShortDateString().Replace('/', '-') + " " + DateTime.Now.ToShortTimeString());
    //            }
    //            else if (field.Name.Equals("updateDate") || ((DateTime)field.GetValue(t)).Year == 1)
    //            {
    //                fieldValues.Add(DBNull.Value.ToString());
    //            }
    //            else
    //            {
    //                fieldValues.Add(field.GetValue(t).ToString());
    //            }
    //        }
    //    }
    //    sb.Remove(sb.Length - 1, 1).Append(" ) VALUES ( ");
    //    foreach (string value in fieldValues)
    //    {
    //        sb.Append(string.Format("'{0}',", value));
    //    }
    //    sb.Remove(sb.Length - 1, 1).Append(");");
    //    return sb.ToString();
    //}
    ///// <summary>
    ///// 生成select语句
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public static string GenerateMaxId<T>(T t) {
    //    Type type = typeof(T);
    //    string className = type.Name;
    //    StringBuilder strBuider = new StringBuilder("select Max(id) from " + GetClassNameByTypeName(className));
    //    return strBuider.ToString();
    //}

    //public static string GenerateSelect<T>(Dictionary<string,string> dic)
    //{
    //    Type type = typeof(T);
    //    string className = GetClassNameByTypeName(type.Name);
    //    StringBuilder sb = new StringBuilder("select ");
    //    List<FieldInfo> fields = new List<FieldInfo>();
    //    FieldInfo[] subfields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    fields.AddRange(basefields);
    //    fields.AddRange(subfields);
    //    foreach (var item in fields)
    //    {
    //        if (item.Name.Equals("Kk_")) continue;
    //        sb.Append(item.Name + ",");
    //    }
    //    sb.Remove(sb.Length - 1, 1).Append("from " + className);
    //    sb.Append(" where 1=1 ");
    //    foreach (var item in dic)
    //    {
    //        sb.Append(string.Format("and {0} ={1}",getSqlFiledName(item.Key),item.Value));
    //    }
    //    return sb.ToString();
    //}

    //public static string GenerateSelect<T>(long id) {
    //    Type type = typeof(T);
    //    string className = GetClassNameByTypeName(type.Name);
    //    StringBuilder sb = new StringBuilder("select ");
    //    List<FieldInfo> fields = new List<FieldInfo>();
    //    FieldInfo[] subfields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    fields.AddRange(basefields);
    //    fields.AddRange(subfields);
    //    foreach (var item in fields)
    //    {
    //        if (item.Name.Equals("Kk_")) continue;
    //        sb.Append(item.Name+",");
    //    }
    //    sb.Remove(sb.Length - 1, 1).Append("from " + className);
    //    sb.Append(" where id = " + id);
    //    return sb.ToString() ;
    //}

    //public static string GenerateSelect<T>() {
    //    Type type = typeof(T);
    //    string className = GetClassNameByTypeName(type.Name);
    //    StringBuilder sb = new StringBuilder("select ");
    //    List<FieldInfo> fields = new List<FieldInfo>();
    //    FieldInfo[] subfields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    FieldInfo[] basefields = type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    fields.AddRange(basefields);
    //    fields.AddRange(subfields);
    //    foreach (var item in fields)
    //    {
    //        if (item.Name.Equals("Kk_")) continue;
    //        sb.Append(getSqlFiledName(item.Name) + ",");
    //    }
    //    sb.Remove(sb.Length - 1, 1).Append(" from " + className);
    //    return sb.ToString();
    //}
    ///// <summary>
    ///// 生成delte语句
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public static string GenerateDelete<T>(long id)
    //{
    //    Type type = typeof(T);
    //    string className = type.Name; //SyasParam
    //    StringBuilder stringBuilder = new StringBuilder("delete from " + GetClassNameByTypeName(className) + " where id = " + id);
    //    return stringBuilder.ToString();
    //}

    //public static string GenerateDelete<T>()
    //{
    //    Type type = typeof(T);
    //    string className = type.Name; //SyasParam
    //    StringBuilder stringBuilder = new StringBuilder("delete from " + GetClassNameByTypeName(className));
    //    return stringBuilder.ToString();
    //}

    ///// <summary>
    ///// 生成Update语句
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public static string GenerateUpdate<T>(T t) {
    //    return "";
    //}


    ///// <summary>
    ///// 获取表名
    ///// </summary>
    ///// <param name="typeName"></param>
    ///// <returns></returns>
    //private static string GetClassNameByTypeName(string typeName) {
    //   var varr  =  typeName.ToArray ();
    //    StringBuilder sb = new StringBuilder("t");
    //    for (int i = 0; i < varr.Length; i++)
    //    {
    //        if (Regex.IsMatch(varr[i].ToString(), "[A-Z]")) {
    //            sb.Append("_" + varr[i].ToString().ToLower());
    //        }
    //        else{
    //            sb.Append(varr[i].ToString());
    //        }
    //    }
    //   //string str = typeName[0].ToString().ToLower() + typeName.Substring(1);//将第一个字符小写
    //    return sb.ToString();
    //}

    //private static string getSqlFiledName(string filedName) {
    //    var varr = filedName.ToArray();
    //    StringBuilder sb = new StringBuilder();
    //    for (int i = 0; i < varr.Length; i++)
    //    {
    //        if (Regex.IsMatch(varr[i].ToString(), "[A-Z]"))
    //        {
    //            sb.Append("_" + varr[i].ToString().ToLower());
    //        }
    //        else
    //        {
    //            sb.Append(varr[i].ToString());
    //        }
    //    }
    //    return sb.ToString();
    //}
    #endregion
    //}
}
