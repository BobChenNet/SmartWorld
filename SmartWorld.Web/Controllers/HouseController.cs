using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using SmartWorld.Business;
using SmartWorld.Uitls;
using SmartWorld.Web.Common;

namespace SmartWorld.Web.Controllers
{
    public class HouseController : BaseController
    {
        readonly HouseProvider _houseProvider = new HouseProvider();
        readonly InfoProvider _infoProvider = new InfoProvider();

        // GET: House
        public ActionResult HouseRent()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult HouseRentPartial()
        {
            Pageinfo.nPage = 1;
            var infos = _houseProvider.GetRentHouseList(Pageinfo);
            return PartialView(infos);
        }

        public ActionResult HouseSale()
        {
            var task = _infoProvider.GetHouseInfosAsync();

            int i = 1;
            i += 1;

            //task.Wait();
            

            return View(task.Result);
        }

        public ActionResult HouseDetail()
        {
            return View();
        }

        public ActionResult AddHouseInfo()
        {
            _infoProvider.InsertHouseInfos();
            //var ret = _houseProvider.InsertHouseInfos();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Upload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称

                    fileData.SaveAs(filePath + saveName);
                    return Json(new { Success = true, FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}