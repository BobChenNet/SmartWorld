using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartWorld.Model;

namespace SmartWorld.WebAPI.Controllers
{
    /// <summary>
    /// 基础信息控制器
    /// </summary>
    public class HomeController : ApiController
    {
        //UserModel[] _userModels = new UserModel[]
        //{
        //    new UserModel { Id = 1, Name = "Tomato Soup" },
        //    new UserModel { Id = 2, Name = "Yo-yo"},
        //    new UserModel { Id = 3, Name = "Hammer"}
        //};

        readonly List<UserModel> _userModels = new List<UserModel>
        {   new UserModel { Id = 1, Name = "Tomato Soup" },
            new UserModel { Id = 2, Name = "Yo-yo"},
            new UserModel { Id = 3, Name = "Hammer"}
        };

        /// <summary>
        /// 得到所有用户
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _userModels;
        }
        /// <summary>
        /// get
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        /// <summary>
        /// 得到用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public UserModel GetUser(int id)
        {
            var user = _userModels.FirstOrDefault((p) => p.Id == id);
            if (user == null)
            {
                return user;
            }
            return user;
        }

        /// <summary>
        /// 得到用户Json
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public IHttpActionResult GetUserJson(int id)
        {
            var user = _userModels.FirstOrDefault((p) => p.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }

}
