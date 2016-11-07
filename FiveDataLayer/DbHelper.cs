using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.AttributeModel;

namespace FiveDataLayer
{
    public class DbHelper
    {
        public DbHelper(string ConnectionStrings)
        {
            this.StrConn = ConnectionStrings;
        }
        readonly string StrConn;

        public DbHelper()
        {
            this.StrConn = ConfigurationManager.ConnectionStrings["StockMarket"].ConnectionString;
        }

        private static bool PrimaryisIdentity = false;
        /// <summary>
        /// 获取单表所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetData<T>() where T : new()
        {
            //1.初始化
            IList<T> result = new List<T>();
            //2.锁定表名
            var tblName = $"{typeof(T).Name}";
            var keys = GetKeys(new T());
            string insertColumn = string.Join(",", keys.ToArray());
            //3.数据库连接初始化
            using (SqlConnection myConnection = new SqlConnection(StrConn))
            {
                myConnection.Open();
                string sql = $"select {insertColumn} from [{tblName}]";
                GetValue(sql, myConnection, result);
            }
            return result;
        }
        /// <summary>
        /// 获取单表所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>(string id) where T : new()
        {
            //1.初始化
            T singleT = new T();
            IList<T> result = new List<T>();
            string primeryKey = string.Empty;
            foreach (var info in singleT.GetType().GetProperties())
            {
                primeryKey = GetPrimeryKey(info);
                if (!string.IsNullOrEmpty(primeryKey)) break;
            }
            if (string.IsNullOrEmpty(primeryKey)) primeryKey = "*";
            //2.锁定表名
            var tblName = $"{typeof(T).Name}";
            //3.数据库连接初始化
            using (SqlConnection myConnection = new SqlConnection(StrConn))
            {
                myConnection.Open();
                string sql = $"select {primeryKey} from [{tblName}] where {primeryKey}={id}";
                GetValue(sql, myConnection, result);
            }
            return result.FirstOrDefault();
        }
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetData<T>(int id) where T : new()
        {
            //1.初始化
            T singleT = new T();
            IList<T> result = new List<T>();
            string primeryKey = string.Empty;
            foreach (var info in singleT.GetType().GetProperties())
            {
                primeryKey = GetPrimeryKey(info);
            }
            //2.锁定表名
            var tblName = $"{typeof(T).Name}";
            //3.数据库连接初始化
            using (SqlConnection myConnection = new SqlConnection(StrConn))
            {
                myConnection.Open();
                string sql = $"select * from [{tblName}] where {primeryKey}={id}";
                GetValue(sql, myConnection, result);
            }
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 插入新的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="addModel"></param>
        /// <returns></returns>
        public bool Add<T>(T addModel)
        {
            string sbSql = string.Empty;
            List<SqlParameter> listPar = buildSql(addModel, SQLType.Insert, out sbSql);
            return MainSql(sbSql, listPar);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        public bool Update<T>(T updateModel)
        {
            string sbSql = string.Empty;
            List<SqlParameter> listPar = buildSql(updateModel, SQLType.Update, out sbSql);
            return MainSql(sbSql, listPar);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deleteModel"></param>
        /// <returns></returns>
        public bool Delete<T>(T deleteModel)
        {
            string sbSql = string.Empty;
            List<SqlParameter> listPar = buildSql(deleteModel, SQLType.Delete, out sbSql);
            return MainSql(sbSql, listPar);
        }

        private bool MainSql(string TSql, List<SqlParameter> par)
        {
            bool result = false;
            using (SqlConnection myConnection = new SqlConnection(StrConn))
            {
                try
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = new SqlCommand())
                    {
                        myCommand.Connection = myConnection;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = TSql;
                        myCommand.Parameters.AddRange(par.ToArray());
                        result = myCommand.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(DateTime.Now);
                    throw ex.InnerException;
                }

            }
            return result;
        }
        /// <summary>  
        /// 判断SqlDataReader是否存在某列  
        /// </summary>  
        /// <param name="dr">SqlDataReader</param>  
        /// <param name="columnName">列名</param>  
        /// <returns></returns>  
        private bool ColumnExists(SqlDataReader dr, string columnName)
        {
            //非空验证
            if (dr.GetSchemaTable() == null) return false;
            //查询当前行对应的字段名
            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            //确认是否找到
            return (dr.GetSchemaTable().DefaultView.Count > 0);
        }
        private string GetPrimeryKey(PropertyInfo baseProperties)
        {
            string PrimaryKey = string.Empty;
            foreach (var item in baseProperties.GetCustomAttributes(false))
            {
                if (item.GetType() == typeof(PrimaryKeyAttribute))
                {
                    PrimaryKeyAttribute IsKey = item as PrimaryKeyAttribute;
                    PrimaryKey = baseProperties.Name;
                    if (IsKey.IsIdentity == true) PrimaryisIdentity = true;
                }
            }
            if (string.IsNullOrEmpty(PrimaryKey)) return null;
            return PrimaryKey;
        }
        /// <summary>
        /// 生产SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="type"></param>
        /// <param name="tsql"></param>
        /// <returns></returns>
        private List<SqlParameter> buildSql<T>(T t, SQLType type, out string tsql)
        {
            Dictionary<string, object> primeryKey = new Dictionary<string, object>();

            var tblName = $"{typeof(T).Name}";
            List<SqlParameter> listPar = new List<SqlParameter>();
            List<string> keys = new List<string>();
            //3.遍历泛型读取字段
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                if (!string.IsNullOrEmpty(GetPrimeryKey(item)))
                {
                    primeryKey.Add(GetPrimeryKey(item), item.GetValue(t));
                    continue;
                }
                var value = item.GetValue(t);
                if (value == null) continue;
                SqlParameter par = new SqlParameter("@" + item.Name, value);
                listPar.Add(par);
                keys.Add(item.Name);
            }
            StringBuilder sbSql = new StringBuilder();
            //4.拼接SQL
            switch (type)
            {
                case SQLType.Insert:
                    return InsertSql(out tsql, sbSql, tblName, keys, listPar,primeryKey);
                case SQLType.Update:
                    return UpdateSql(out tsql, primeryKey, listPar, sbSql, tblName, keys);
                case SQLType.Delete:
                    return DeleteSql(out tsql, primeryKey, listPar, sbSql, tblName);
                default:
                    break;
            }
            tsql = string.Empty;
            return null;
        }

        private static List<SqlParameter> DeleteSql(out string tsql, Dictionary<string, object> primeryKey, List<SqlParameter> listPar, StringBuilder sbSql,
            string tblName)
        {
            SqlParameter keydelpar = new SqlParameter("@" + primeryKey.FirstOrDefault().Key,
                primeryKey[primeryKey.FirstOrDefault().Key]);
            listPar.Add(keydelpar);
            sbSql.Append($"DELETE dbo.[{tblName}]");
            sbSql.Append($" where {primeryKey.FirstOrDefault().Key} = @{primeryKey.FirstOrDefault().Key}");
            tsql = sbSql.ToString();
            return listPar;
        }

        private static List<SqlParameter> UpdateSql(out string tsql, Dictionary<string, object> primeryKey, List<SqlParameter> listPar, StringBuilder sbSql,
            string tblName, List<string> keys)
        {
            SqlParameter keypar = new SqlParameter("@" + primeryKey.FirstOrDefault().Key,
                primeryKey[primeryKey.FirstOrDefault().Key]);
            listPar.Add(keypar);
            sbSql.Append($"UPDATE dbo.[{tblName}] SET ");
            StringBuilder UpdateColumn = new StringBuilder();
            foreach (string str in keys.ToArray())
            {
                UpdateColumn.Append('[' + str + ']' + " = @" + str + ",");
            }
            sbSql.Append(UpdateColumn.ToString().Substring(0, UpdateColumn.ToString().Length - 1));
            sbSql.Append($" where {primeryKey.FirstOrDefault().Key} = @{primeryKey.FirstOrDefault().Key}");
            tsql = sbSql.ToString();
            return listPar;
        }

        private static List<SqlParameter> InsertSql(out string tsql, StringBuilder sbSql, string tblName, List<string> keys, List<SqlParameter> listPar, Dictionary<string, object> primeryKey = null)
        {
            if (primeryKey != null&&PrimaryisIdentity==false)
            {
                SqlParameter keypar = new SqlParameter("@" + primeryKey.FirstOrDefault().Key,
                primeryKey[primeryKey.FirstOrDefault().Key]);
                listPar.Add(keypar);
                keys.Add(primeryKey.FirstOrDefault().Key);
            }


            sbSql.Append($"INSERT INTO dbo.[{tblName}]");
            sbSql.Append("(");
            string insertColumn = string.Join(",", keys.ToArray());
            sbSql.Append(insertColumn);
            sbSql.Append(")");
            sbSql.Append("Values(@");
            string parColumn = string.Join(",@", keys.ToArray());
            sbSql.Append(parColumn);
            sbSql.Append(")");
            tsql = sbSql.ToString();
            return listPar;
        }

        /// <summary>
        /// 对象赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="myConnection"></param>
        /// <param name="result"></param>
        private void GetValue<T>(string sql, SqlConnection myConnection, IList<T> result) where T : new()
        {
            using (SqlCommand myCommand = new SqlCommand(sql, myConnection))
            {
                try
                {
                    SqlDataReader reader = myCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        //4.实例化单列
                        T singleResult = new T();
                        //5.遍历对象内部所有属性
                        foreach (var item in singleResult.GetType().GetProperties())
                        {
                            string fieldName = item.Name; //属性名
                            //6.赋值
                            if (ColumnExists(reader, fieldName))
                            {
                                if (!item.CanWrite)
                                {
                                    continue;
                                }
                                var value = reader[fieldName];
                                if (value != DBNull.Value)
                                {
                                    item.SetValue(singleResult, value);
                                }
                            }
                        }
                        //7.将单列加入List
                        result.Add(singleResult);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<string> GetKeys<T>(T t)
        {
            return t.GetType().GetProperties().Select(item => item.Name).ToList();
        }

        public void Dispose()
        {

        }

        public enum SQLType
        {
            Insert = 1,
            Update = 2,
            Delete = 3,
        }
    }
}
