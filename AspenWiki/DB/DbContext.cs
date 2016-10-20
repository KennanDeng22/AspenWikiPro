using AspenWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspenWiki.DB
{
    public class DbContext
    {
        public IList<Defect> ListDefects(){
            SqlDbHelper db = new SqlDbHelper();
            var table = db.ExecuteDataTable("select * from defects");
            return ModelConvertHelper<Defect>.ConvertToModel(table);
        }
    }
}