using System;
using System.Collections.Generic;  
using System.Text;  
using System.Data;  
using System.Reflection; 
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AspenWiki.DB
{
     /// <summary>  
    /// 实体转换辅助类  
    /// </summary>  
    public class ModelConvertHelper<T> where  T : new()  
    {  
        public static IList<T> ConvertToModel(DataTable dt)  
        {  
            // 定义集合  
            IList<T> ts = new List<T>();  
  
            // 获得此模型的类型  
            Type type = typeof(T);  
  
            string tempName = "";  
  
            foreach (DataRow dr in dt.Rows)  
            {  
                T t = new T();  
  
                // 获得此模型的公共属性  
                PropertyInfo[] propertys = t.GetType().GetProperties();  
  
                foreach (PropertyInfo pi in propertys)  
                {  
                    tempName = pi.Name;  
  
                    // 检查DataTable是否包含此列  
                    if (dt.Columns.Contains(tempName))  
                    {  
                        // 判断此属性是否有Setter  
                        if (!pi.CanWrite) continue;  
  
                        object value = dr[tempName];  
                        if (value != DBNull.Value)  
                            pi.SetValue(t, value, null);  
                    }  
                }  
  
                ts.Add(t);  
            }  
  
            return ts;  
  
        }

        public static void BuildInsertStatement(T t, string tableName, string idName, bool autoId, out string sql, out SqlParameter[] parameters)
        {
            List<SqlParameter> theParameters = new List<SqlParameter>();

            PropertyInfo[] propertys = t.GetType().GetProperties();
            StringBuilder columnsPart = new StringBuilder();
            StringBuilder valuesPart = new StringBuilder();
            foreach (PropertyInfo pi in propertys)
            {
                //bool isKey = pi.CustomAttributes.FirstOrDefault(attr=>attr.AttributeType.FullName.Equals("System.ComponentModel.DataAnnotations.KeyAttribute"))!=null;
                //if (isKey && pi.PropertyType.Name.StartsWith("Int"))//TODO
                //    continue;
                if (IsDbColumn(pi) && !(pi.Name.Equals(idName) && autoId))
                {
                    var value = pi.GetValue(t);
                    if (value != null)
                    {
                        theParameters.Add(new SqlParameter("@" + pi.Name, value));
                        columnsPart.Append(pi.Name).Append(",");
                        valuesPart.Append("@" + pi.Name).Append(",");
                    }
                }
            }

            sql = string.Format("insert into {0}({1}) values({2})", tableName, columnsPart.Remove(columnsPart.Length - 1, 1), valuesPart.Remove(valuesPart.Length - 1, 1));
            parameters = theParameters.ToArray();
        }

        public static void BuildUpdateStatement(T t, string tableName, string idName, out string sql, out SqlParameter[] parameters, string[] columnsToUpdate=null)
        {
            List<SqlParameter> theParameters = new List<SqlParameter>();
            
            PropertyInfo[] propertys = t.GetType().GetProperties();          
            StringBuilder valuesPart = new StringBuilder();
            object idValue = null;
            foreach (PropertyInfo pi in propertys)
            {
                var value = pi.GetValue(t);
                if (pi.Name.Equals(idName))
                {
                    idValue = value;
                }
                else if (IsDbColumn(pi) && (columnsToUpdate == null || (columnsToUpdate != null && columnsToUpdate.Contains(pi.Name))))
                {
                    if (value != null) { 
                        theParameters.Add(new SqlParameter("@" + pi.Name, value));
                        valuesPart.Append(pi.Name + "=@" + pi.Name).Append(",");
                    }
                    else
                    {
                        valuesPart.Append(pi.Name + "=null").Append(",");
                    }
                }
            }
            theParameters.Add(new SqlParameter("@" + idName, idValue));

            sql = string.Format("update {0} set {1} where {2} = @{2}", tableName, valuesPart.Remove(valuesPart.Length - 1, 1), idName);
            parameters = theParameters.ToArray();
        }

        private static bool IsDbColumn(PropertyInfo pi)
        {
            return pi.PropertyType.IsValueType || pi.PropertyType.Name.Equals("String");
        }
    }  
}