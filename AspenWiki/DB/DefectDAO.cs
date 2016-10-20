using AspenWiki.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AspenWiki.DB
{
    public class DefectDAO: GenericDAO<Defect>
    {
        public DefectDAO() : base("Defects","Id",false) { }
        //WorkingHours,DevNote
        //public bool UpdateNote(Defect defect)
        //{
        //    string sql = "update Defects set DevNote=@DevNote,WorkingHours=@WorkingHours where Id=@Id";
        //    SqlParameter[] paramters = new SqlParameter[]{
        //        new SqlParameter("@DevNote",defect.DevNote),
        //        new SqlParameter("@RealName",info.RealName),
        //        new SqlParameter("@Age",info.Age),
        //        new SqlParameter("@Sex",info.Sex),
        //        new SqlParameter("@Mobile",info.Mobile),
        //        new SqlParameter("@Email",info.Email),
        //        new SqlParameter("@Phone",info.Phone),
        //        new SqlParameter("@UserID",info.UserId),
        //    };
        //    SqlDbHelper db = new SqlDbHelper();
        //    return db.ExecuteNonQuery(sql, CommandType.Text, paramters) > 0;
        //}
    }
}