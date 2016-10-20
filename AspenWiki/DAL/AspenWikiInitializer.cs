using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspenWiki.Models;
using AspenWiki.DB;

namespace AspenWiki.DAL
{
    public class AspenWikiInitializer
    {
        CategoryDAO categoryDAO = new CategoryDAO();
        GenericDAO<User> userDAO = new GenericDAO<User>();
        GenericDAO<WorkItem> workItemDAO = new GenericDAO<WorkItem>();
        public void Init()
        {
            var categories = new List<Category>
            {
            new Category{Name="Category A"},
            new Category{Name="Category B"},
            new Category{Name="Category C"}          
            };
            categories.ForEach(s => categoryDAO.Create(s));

            var users = new List<User>
            {
                new User{Id="salauddm", Location="SH", FullName="Enki Zhang", CQLoginName="Mohammad_Salauddin", Email="mohammad.salauddin@aspentech.com", SupervisorId=null},
                new User{Id="zhangp", Location="SH", FullName="Enki Zhang", CQLoginName="Ping_Zhang", Email="Ping.Zhang@aspentech.com", SupervisorId="salauddm"},
                new User{Id="jinja", Location="SH", FullName="Jacky Jin", CQLoginName="Jacky_Jin", Email="Jacky.Jin@aspentech.com", SupervisorId="salauddm"},
                new User{Id="liusm", Location="SH", FullName="Tom Liu", CQLoginName="ShaoMing_Liu", Email="ShaoMing.Liu@aspentech.com", SupervisorId="zhangp"},
                new User{Id="suyu", Location="SH", FullName="Susie Su", CQLoginName="Susie_Su", Email="YuFengSusie.Su@aspentech.com", SupervisorId="zhangp"},
                new User{Id="wenk", Location="SH", FullName="Kelly Wen", CQLoginName="Ping_Zhang", Email="HuiLingKelly.Wen@aspentech.com", SupervisorId="zhangp"}
            };
            users.ForEach(s => userDAO.Create(s));

            var workItems = new List<WorkItem>
            {
                new WorkItem{OrderIdx=0, Id="None", WorkType="None", Name=""},
                new WorkItem{OrderIdx=1, Id="Defect", WorkType="Defect", Name="Defect"},
                new WorkItem{OrderIdx=2, Id="Review", WorkType="Review", Name="Code Review"},
                new WorkItem{OrderIdx=3, Id="Meeting", WorkType="Meeting", Name="Meeting"},
                new WorkItem{OrderIdx=4, Id="Other", WorkType="Other", Name="Other"},
                new WorkItem{OrderIdx=5, Id="AnnualLeave", WorkType="Leave", Name="Annual Leave"},
                new WorkItem{OrderIdx=6, Id="SickLeave", WorkType="Leave", Name="Sick Leave"},
                new WorkItem{OrderIdx=7, Id="Vacation", WorkType="Leave", Name="Vacation"},
                new WorkItem{OrderIdx=8, Id="Task", WorkType="Task", Name="Task"}
            };
            workItems.ForEach(s => workItemDAO.Create(s));

            DataImporter importer = new DataImporter();
            importer.ImportDefect(@"D:\projects\AspenWiki\AspenWiki\App_Data\QueryResult.xls"); 
        }
    }
}