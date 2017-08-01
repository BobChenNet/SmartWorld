using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SmartWorld.Uitls
{
    public static class DateTimeHelper
    {
        #region 公共属性

        /// <summary>
        /// 本周结束开始
        /// </summary>
        public static DateTime CurrentWeekStartDate
        {
            get
            {
                int day = (int) DateTime.Now.DayOfWeek;
                return DateTime.Now.AddDays(-day).Date;
            }
        }

        /// <summary>
        /// 本周结束日期
        /// </summary>
        public static DateTime CurrentWeekEndDate
        {
            get
            {
                int day = (int) DateTime.Now.DayOfWeek;
                return DateTime.Now.AddDays(-day + 6).Date;
            }
        }
      
        #endregion

        #region 公共方法

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        public static DateTime StampToDateTime(int timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long timestamp = long.Parse(timeStamp + "0000000");
            TimeSpan timeSpan = new TimeSpan(timestamp);
            return dateTimeStart.Add(timeSpan);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        public static int DateTimeToStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int) (time - startTime).TotalSeconds;
        }

        public static DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            DateTime temp;
            return DateTime.TryParse(value, out temp) ? temp : (DateTime?) null;
        }
         
        public static int GetMinutesForDate(string dateStr)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(dateStr))
            {
                string[] tem = dateStr.Replace("天", "|").Replace("小时", "|").Replace("分钟", "|").Split('|');
                switch (tem.Length)
                {
                    case 4:
                        result = int.Parse(tem[0])*24*60 + int.Parse(tem[1])*60 + int.Parse(tem[2]);
                        break;
                    case 3:
                        result = int.Parse(tem[0])*60 + int.Parse(tem[1]);
                        break;
                    case 2:
                        result = int.Parse(tem[0]);
                        break;
                }
            }
            return result;
        }


        public static List<KeyValuePair<string, string>> ParseChHourByMinute(int minute, int stopHour = 20)
        {
            int day = 0;
            string time = "";

            if (minute < stopHour*60 + 1)
                time = DateTime.Now.Date.AddHours(stopHour).AddMinutes(minute*-1).ToString("H:mm");

            else
            {
                day = (minute - stopHour*60 - 1)/(60*24) + 1;
                time = DateTime.Now.Date.AddMinutes((minute - stopHour*60)%(60*24)*-1).ToString("HH:mm");
            }

            List<KeyValuePair<string, string>> listValue = new List<KeyValuePair<string, string>>();

            listValue.Add(new KeyValuePair<string, string>("day", day.ToString()));
            listValue.Add(new KeyValuePair<string, string>("time", time));

            return listValue;
        }

        public static string ParseDeadlineStr(int minute)
        {
            List<KeyValuePair<string, string>> listValue = ParseChHourByMinute(minute);

            if (Convert.ToInt32(listValue.Find(p => p.Key == "day").Value) == 0)
                return "当天" + listValue.Find(p => p.Key == "time").Value;
            if (Convert.ToInt32(listValue.Find(p => p.Key == "day").Value) > 0)
                return "前" + listValue.Find(p => p.Key == "day").Value + "天" + listValue.Find(p => p.Key == "time").Value;

            return "";
        }

        /// <summary>
        /// 分钟转小时显示
        /// </summary>
        public static string MinuteToHourStr(int minute)
        {
            if (minute < 60)
                return $"{minute}分钟";

            return $"{Math.Round((decimal)minute / 60, 1)}小时";
        }

        public static string GetDeadlineTime(int deadline, string type)
        {
            List<KeyValuePair<string, string>> listValue = ParseChHourByMinute(deadline);

            switch (type)
            {
                case "day":
                    return listValue.Find(p => p.Key == "day").Value;
                case "time":
                    return listValue.Find(p => p.Key == "time").Value;
            }

            return "";
        }


        public static string ConvertDateTimeToTimeline(DateTime dt)
        {
            string ret;
            var tSpan = DateTime.Now - dt;
            if (tSpan.TotalDays > 1)
            {
                ret = $"{Math.Floor(tSpan.TotalDays):f0}天前发布";
            }
            else if (tSpan.TotalHours > 1)
            {
                ret = $"{Math.Floor(tSpan.TotalHours):f0}小时前发布";
            }
            else if (tSpan.TotalMinutes > 1)
            {
                ret = $"{Math.Floor(tSpan.TotalMinutes):f0}分钟前发布";
            }
            else
            {
                ret = "1分钟内发布";
            }
            return ret;
        }

        /// <summary>
        /// 获取一年中指定的一周的开始日期和结束日期。开始日期遵循ISO 8601即星期一。
        /// </summary>
        /// <remarks>Write by vrhero</remarks>
        /// <param name="year">年（1 到 9999）</param>
        /// <param name="weeks">周（1 到 53）</param>
        /// <param name="weekrule">确定首周的规则</param>
        /// <param name="first">当此方法返回时，则包含参数 year 和 weeks 指定的周的开始日期的 System.DateTime 值；如果失败，则为 System.DateTime.MinValue。如果参数 year 或 weeks 超出有效范围，则操作失败。该参数未经初始化即被传递。</param>
        /// <param name="last">当此方法返回时，则包含参数 year 和 weeks 指定的周的结束日期的 System.DateTime 值；如果失败，则为 System.DateTime.MinValue。如果参数 year 或 weeks 超出有效范围，则操作失败。该参数未经初始化即被传递。</param>
        /// <returns>成功返回 true，否则为 false。</returns>
        public static bool GetDaysOfWeeks(int year, int weeks, CalendarWeekRule weekrule, out DateTime first, out DateTime last)
        {
            //初始化 out 参数
            first = DateTime.MinValue;
            last = DateTime.MinValue;

            //不用解释了吧...
            if(year < 1 | year > 9999)
                return false;

            //一年最多53周地球人都知道...
            if(weeks < 1 | weeks > 53)
                return false;

            //取当年首日为基准...为什么？容易得呗...
            DateTime firstCurr = new DateTime(year, 1, 1);
            //取下一年首日用于计算...
            DateTime firstNext = new DateTime(year + 1, 1, 1);

            //将当年首日星期几转换为数字...星期日特别处理...ISO 8601 标准...
            int dayOfWeekFirst = (int)firstCurr.DayOfWeek;
            if(dayOfWeekFirst == 0) dayOfWeekFirst = 7;

            //得到未经验证的周首日...
            first = firstCurr.AddDays((weeks - 1) * 7 - dayOfWeekFirst + 1);

            //周首日是上一年日期的情况...
            if(first.Year < year)
            {
                switch(weekrule)
                {
                    case CalendarWeekRule.FirstDay:
                        //不用解释了吧...
                        first = firstCurr;
                        break;
                    case CalendarWeekRule.FirstFullWeek:
                        //顺延一周...
                        first = first.AddDays(7);
                        break;
                    case CalendarWeekRule.FirstFourDayWeek:
                        //周首日距年首日不足4天则顺延一周...
                        if(firstCurr.Subtract(first).Days > 3)
                        {
                            first = first.AddDays(7);
                        }
                        break;
                    default:
                        break;
                }
            }
            //得到未经验证的周末日...
            last = first.AddDays(7).AddSeconds(-1);
            switch(weekrule)
            {
                case CalendarWeekRule.FirstDay:
                    //周末日是下一年日期的情况... 
                    if(last.Year > year)
                        last = firstNext.AddSeconds(-1);
                    else if(last.DayOfWeek != DayOfWeek.Monday)
                        last = first.AddDays(7 - (int)first.DayOfWeek).AddSeconds(-1);
                    break;
                case CalendarWeekRule.FirstFourDayWeek:
                    //周末日距下一年首日不足4天则提前一周... 
                    if(last.Year > year && firstNext.Subtract(first).Days < 4)
                    {
                        first = first.AddDays(-7);
                        last = last.AddDays(-7);
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        #endregion

    }
}
