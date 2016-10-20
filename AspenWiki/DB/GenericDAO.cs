using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AspenWiki.DB
{
    public enum ComparisonOperator
    {
        Equal,Like,GreatThan, LessThan
    }

    public class GenericDAO<T> where T : new()
    {
        protected SqlDbHelper db = new SqlDbHelper();

        private string _tableName;
        private string _idName = "Id";
        private bool _autoId = true;
        public string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_tableName))
                    _tableName = typeof(T).Name + "s";
                return this._tableName;
            }
            set { this._tableName = value; }
        }
        public string IdName
        {
            get { return this._idName; }
            set { this._idName = value; }
        }
        public bool AutoId
        {
            get { return this._autoId; }
            set { this._autoId = value; }
        }

        public GenericDAO() { }

        public GenericDAO(string tableName=null, string idName="Id", bool autoId=true)
        {
            this._tableName = tableName;
            this._idName = idName;
            this._autoId = autoId;
        }

        public IList<T> ListAll()
        {        
            var table = db.ExecuteDataTable("select * from " + TableName);
            return ModelConvertHelper<T>.ConvertToModel(table);            
        }

        public IList<T> Find(string whereClause, string orderClause, SqlParameter[] parameters)
        {
            var sql = "select * from " + TableName;
            if (!string.IsNullOrEmpty(whereClause))
                sql += " where " + whereClause;
            if(!string.IsNullOrEmpty(orderClause))
                sql += " order by " + orderClause;
            var table = db.ExecuteDataTable(sql, CommandType.Text, parameters);
            return ModelConvertHelper<T>.ConvertToModel(table); 
        }

        public IList<T> FindBy(string columnName, object value)
        {
            string where = columnName + "=@" + columnName;
            var parameters = new SqlParameter[] { new SqlParameter("@" + columnName, value) };
            return Find(where, null, parameters);
        }

        public virtual T Read(object id)
        {
            var tableName = typeof(T).Name;
            string sql = string.Format("select * from {0} where {1} = @id", TableName, _idName);
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            DataTable table = db.ExecuteDataTable(sql, CommandType.Text, parameters);
            if (table.Rows.Count > 0)
            {
                return ModelConvertHelper<T>.ConvertToModel(table)[0];    
            }
            else
            {
                return default(T);
            }
        }

        public virtual bool Delete(object id)
        {
            string sql = string.Format("delete from {0} where {1} = @id", TableName, _idName);
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            return db.ExecuteNonQuery(sql, CommandType.Text, parameters) > 0;
        }

        public virtual bool Create(T t)
        {
            string sql;
            SqlParameter[] parameters;
            ModelConvertHelper<T>.BuildInsertStatement(t, TableName, IdName, AutoId, out sql, out parameters);
            //TODO retrieve id from db;
            bool ret= db.ExecuteNonQuery(sql, CommandType.Text, parameters) > 0;
            if (ret && AutoId)
            {
                sql = string.Format("SELECT IDENT_CURRENT('{0}')", TableName);
                int id = int.Parse(db.ExecuteScalar(sql).ToString());

                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.Name.Equals(IdName))
                    {
                        pi.SetValue(t, id);
                    }                   
                }  
            }
            return ret;
        }

        public virtual bool Update(T t, string[] columnsToUpdate = null)
        {
            string sql;
            SqlParameter[] parameters;
            ModelConvertHelper<T>.BuildUpdateStatement(t, TableName, IdName, out sql, out parameters, columnsToUpdate);
            //TODO retrieve id from db;
            return db.ExecuteNonQuery(sql, CommandType.Text, parameters) > 0;
        }
    }
}