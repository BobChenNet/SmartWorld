using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartWorld.DAL;
using SmartWorld.Uitls;
using SmartWorld.Model;
using SmartWorld.Model.Enum;

namespace SmartWorld.Business
{
    [Reset(49, "Nuha Ali", "10/10/2012", Message = "Unused variable")]
    public class InfoProvider
    {
        public Result<List<RentHouse>> GetRentHouseList(PagingInfo page)
        {
            Result<List<RentHouse>> ret = new Result<List<RentHouse>>()
            {
                Data = new List<RentHouse>()
            };

            using (var smartConn = new SmartWorldEntities())
            {
            }
            return ret;
        }
        [Reset(49, "Nuha Ali", "10/10/2012", Message = "Unused variable")]
        public Result InsertHouseInfos()
        {
            Result ret = new Result();
            //var task = GetHouseInfosAsync();
            var re1 = this.GetHouseInfosAsync();
            //Thread.Sleep(3000);
            //var re2 = this.GetHouseInfosAsync_2();
            //Thread.Sleep(3000);
            int i = 1;
            i += 1;
            var r = re1.Result;

            return ret;
        }

        public async Task<Result<List<D_House>>> GetHouseInfosAsync()
        {
            Result<List<D_House>> ret = new Result<List<D_House>>();
            using (var smartConn = new SmartWorldEntities())
            {
                //var res = smartConn.GetAsync(url).Result.Content.tos;
                //smartConn.D_House.Add(new D_House()
                //{
                //    Id = Guid.NewGuid(),
                //    HouseCode = "Test",
                //    CreateDate = DateTime.Now
                //});
                //await smartConn.SaveChangesAsync();

                ret.Data = await smartConn.D_House.Where(x => x.IsDelete == false).ToListAsync();
            }
            return ret;
        }

        public async Task<string> GetHouseInfosAsync_1()
        {
            return await "456";
        }
        public async Task<string> GetHouseInfosAsync_2()
        {
            return await "123";
        }
    }
    public static class helper
    {
        public static TaskAwaiter<string> GetAwaiter(this string s)
        {
            return new Task<string>(() => { return s; }).GetAwaiter();
        }
    }
}




