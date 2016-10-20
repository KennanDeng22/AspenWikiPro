using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
 
namespace AspenWiki.DB
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    /// <summary>
    /// 用ADO.NET实现CRUD功能
    /// </summary>
    public class ADODotNetCRUD
    {
        /// <summary>
        /// 统计用户总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            string sql = "select count(1) from UserInfo";
            SqlDbHelper db = new SqlDbHelper();
            return int.Parse(db.ExecuteScalar(sql).ToString());
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="info">用户实体</param>
        /// <returns></returns>
        public bool Create(UserInfo info)
        {
            string sql = "insert UserInfo(UserName,RealName,Age,Sex,Mobile,Email,Phone)values(@UserName,@RealName,@Age,@Sex,@Mobile,@Email,@Phone)";
            SqlParameter[] paramters = new SqlParameter[]{
                new SqlParameter("@UserName",info.UserName),
                new SqlParameter("@RealName",info.RealName),
                new SqlParameter("@Age",info.Age),
                new SqlParameter("@Sex",info.Sex),
                new SqlParameter("@Mobile",info.Mobile),
                new SqlParameter("@Email",info.Email),
                new SqlParameter("@Phone",info.Phone),
            };
            SqlDbHelper db = new SqlDbHelper();
            return db.ExecuteNonQuery(sql, CommandType.Text, paramters) > 0;
        }
        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public UserInfo Read(int userId)
        {
            string sql = "select * from UserInfo Where UserId="+userId;
            SqlDbHelper db = new SqlDbHelper();
            DataTable data = db.ExecuteDataTable(sql);
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                UserInfo info = new UserInfo()
                {
                    UserId=int.Parse(row["UserId"].ToString()),
                    UserName=row["UserName"].ToString(),
                    Age=byte.Parse(row["Age"].ToString()),
                    Email=row["Email"].ToString(),
                    Mobile=row["Mobile"].ToString(),
                    Phone=row["Phone"].ToString(),
                    RealName=row["RealName"].ToString(),
                    Sex=bool.Parse(row["Sex"].ToString())
                };
                return info;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="info">用户实体</param>
        /// <returns></returns>
        public bool Update(UserInfo info)
        {
            string sql = "update UserInfo set UserName=@UserName,RealName=@RealName,Age=@Age,Sex=@Sex,Mobile=@Mobile,Email=@Email,Phone=@Phone where UserID=@UserID";
            SqlParameter[] paramters = new SqlParameter[]{
                new SqlParameter("@UserName",info.UserName),
                new SqlParameter("@RealName",info.RealName),
                new SqlParameter("@Age",info.Age),
                new SqlParameter("@Sex",info.Sex),
                new SqlParameter("@Mobile",info.Mobile),
                new SqlParameter("@Email",info.Email),
                new SqlParameter("@Phone",info.Phone),
                new SqlParameter("@UserID",info.UserId),
            };
            SqlDbHelper db = new SqlDbHelper();
            return db.ExecuteNonQuery(sql, CommandType.Text, paramters) > 0;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public bool Delete(int userId)
        {
            string sql = "delete from UserInfo where UserId=" + userId;
            SqlDbHelper db = new SqlDbHelper();
            return db.ExecuteNonQuery(sql) > 0;
        }
        /// <summary>
        /// 获取用户表中编号最大的用户
        /// </summary>
        /// <returns></returns>
        public int GetMaxUserId()
        {
            string sql = "select max(userId) from UserInfo";
            SqlDbHelper db = new SqlDbHelper();
            return int.Parse(db.ExecuteScalar(sql).ToString());
        }
    }
}