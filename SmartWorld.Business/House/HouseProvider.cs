using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorld.DAL;
using SmartWorld.Uitls;
using SmartWorld.Model;
using SmartWorld.Model.Enum;

namespace SmartWorld.Business
{
    public class HouseProvider
    {
        public Result<List<RentHouse>> GetRentHouseList(PagingInfo page)
        {
            Result<List<RentHouse>> ret = new Result<List<RentHouse>>()
            {
                Data = new List<RentHouse>()
            };
            using (var smartConn = new SmartWorldEntities())
            {
                var list = smartConn.D_House.Where(x => x.IsDelete == false);
                foreach (var i in list)
                {
                    RentHouse r = new RentHouse()
                    {
                        Id = i.Id,
                        Address = i.Address,
                        Floor = i.Floor.GetValueOrDefault(0),
                        HomeCode = i.HouseCode,
                        HomeTitle = i.HouseTitle,
                        Price = i.Price.GetValueOrDefault(0),
                        Text = i.Remark
                    };
                    ret.Data.Add(r);
                }
            }
            return ret;
        }

        public Result InsertHouseInfos()
        {
            Result ret = new Result();
            using (var smartConn = new SmartWorldEntities())
            {
                for (int i = 1; i <= 3; i++)
                {
                    D_House r = new D_House()
                    {
                        Id = Guid.NewGuid(),
                        Address = "成都",
                        Floor = i,
                        HouseCode = DateTime.Now.Date.ToString(),
                        HouseTitle = "测试" + i,
                        Price = 100,
                        Remark = "说明",
                        Property = (int)PropertyType.私证,
                        CreateDate = DateTime.Now,
                        HouseType = (int)HouseType.出租,
                        YearPrice = 1000,
                        IsDelete = false
                    };

                    //List<int> GetListEnum = PropertyType.私证.GetListEnum<int>(typeof(int));
                    //List<string> GetListEnum2 = PropertyType.私证.GetListEnum<string>(typeof(string));

                    smartConn.D_House.Add(r);
                }
                ret.Success = smartConn.SaveChanges() > 0;
            }
            return ret;
        }

        public string GetTimeTest()
        {
            List<D_House> list1 = new List<D_House>();
            List<D_House> list2 = new List<D_House>();
            List<D_House> list3 = new List<D_House>();
            using (var smartConn = new SmartWorldEntities())
            {
                Stopwatch watch1 = new Stopwatch();
                watch1.Start();
                for (int i = 1; i <= 2000; i++)
                {
                    D_House r = new D_House()
                    {
                        Id = Guid.NewGuid(),
                        Address = "成都",
                        Floor = i,
                        HouseCode = DateTime.Now.Date.ToString(),
                        HouseTitle = "测试" + i,
                        Price = 100,
                        Remark = "说明",
                        Property = (int)PropertyType.私证,
                        CreateDate = DateTime.Now,
                        HouseType = (int)HouseType.出租,
                        YearPrice = 1000,
                        IsDelete = false
                    };
                    list1.Add(r);
                }
                smartConn.D_House.AddRange(list1);
                
                watch1.Stop();


                Stopwatch watch2 = new Stopwatch();
                watch2.Start();
                for (int i = 1; i <= 2000; i++)
                {
                    D_House r = new D_House()
                    {
                        Id = Guid.NewGuid(),
                        Address = "成都",
                        Floor = i,
                        HouseCode = DateTime.Now.Date.ToString(),
                        HouseTitle = "测试" + i,
                        Price = 100,
                        Remark = "说明",
                        Property = (int)PropertyType.私证,
                        CreateDate = DateTime.Now,
                        HouseType = (int)HouseType.出租,
                        YearPrice = 1000,
                        IsDelete = false
                    };
                    list2.Add(r);
                }
                smartConn.BulkInsert(list2);
                
                smartConn.SaveChanges();
                watch2.Stop();

                Stopwatch watch3 = new Stopwatch();
                //watch3.Start();
                //for (int i = 1; i <= 2000; i++)
                //{
                //    D_House r = new D_House()
                //    {
                //        Id = Guid.NewGuid(),
                //        Address = "成都",
                //        Floor = i,
                //        HouseCode = DateTime.Now.Date.ToString(),
                //        HouseTitle = "测试" + i,
                //        Price = 100,
                //        Remark = "说明",
                //        Property = (int)PropertyType.私证,
                //        CreateDate = DateTime.Now,
                //        HouseType = (int)HouseType.出租,
                //        YearPrice = 1000,
                //        IsDelete = false
                //    };
                //    smartConn.D_House.Add(r);
                //}
                //smartConn.SaveChanges();
                watch3.Stop();

                string ret = string.Format(@" AddRange:{0}；
                                        BulkInsert:{1}；
                                        先添加后保存:{2}；", watch1.Elapsed, watch2.Elapsed, watch3.Elapsed);
                return ret;
            }
        }
    }
}
