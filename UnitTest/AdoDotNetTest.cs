using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspenWiki.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
namespace NUnitTest
{
    [TestClass]
    class AdoDotNetTest
    {
        private ADODotNetCRUD instance = null;

        [TestInitialize]
        public void Initialize()
        {
            instance = new ADODotNetCRUD();
        }
        
        /// <summary>
        /// 统计用户总数
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Count()
        {            
            Assert.IsTrue(instance.Count()>0);
        }

        
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="info">用户实体</param>
        /// <returns></returns>
        [TestMethod]
        public void Create()
        {
            UserInfo info = new UserInfo()
            {
                Age = 12,
                Email = "zzz@ccav.com",
                Mobile = "13812345678",
                Phone = "01012345678",
                RealName = "测试" + DateTime.Now.Millisecond.ToString(),
                Sex = true,
                UserName = "zhoufoxcn" + DateTime.Now.Millisecond.ToString()
            };
            instance.Create(info);
        }

        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        [TestMethod]
        public void Read()
        {
            UserInfo info = instance.Read(1);            
            Assert.IsNotNull(info);
        }
        [TestMethod]
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="info">用户实体</param>
        /// <returns></returns>
        public void Update()
        {
            UserInfo info = instance.Read(1);
            info.RealName = "测试" + DateTime.Now.Millisecond.ToString();
            instance.Update(info);
        }
        [TestMethod]
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public void DeleteByID()
        {
            int userId = instance.GetMaxUserId();
            instance.Delete(userId);
        }
    }
}