using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SmartWorld.Uitls
{
    /// <summary>
    /// 表示字符实用工具类。
    /// </summary>
    public static class StringUtil
    {
        private static readonly object lockObj = new object();

        private static int BaseNumber = 1000;  //分隔基数,用于导出书籍和上传书籍时二级目录间隔

        private static string bookFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ResourceDomainName"];

        private static string domainName = System.Web.Configuration.WebConfigurationManager.AppSettings["DomainName"];

        private static string swfResName = System.Web.Configuration.WebConfigurationManager.AppSettings["SwfResourceDomainName"];

        /// <summary>
        /// 将数据列表拼接成字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spliter">分隔符。</param>
        /// <param name="clips">拼接的各个部分。</param>
        /// <returns></returns>
        public static string CombineString<T>(char spliter, List<T> clips)
        {
            string ret = string.Empty;
            foreach (var item in clips)
            {
                ret += item.ToString() + spliter;
            }

            return ret.TrimEnd(spliter);
        }

        /// <summary>
        /// 将数据列表拼接成字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spliter">分隔符。</param>
        /// <param name="clips">拼接的各个部分。</param>
        /// <returns></returns>
        public static string CombineString(char spliter, List<string> clips)
        {
            return CombineString<string>(spliter, clips);
        }

        /// <summary>
        /// 将数据列表拼接成字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clips">拼接的各个部分。</param>
        /// <param name="spliter">分隔符。</param>
        /// <returns></returns>
        public static string CombineString<T>(char spliter, params T[] clips)
        {
            return CombineString<T>(spliter, clips.ToList<T>());
        }

        /// <summary>
        /// 将枚举列表拼接成字符串。
        /// </summary>
        public static string CombineEnums<T>(char spliter, params T[] clips)
        {
            string ret = string.Empty;
            foreach (var item in clips)
            {
                int enumValue = (int)Enum.Parse(typeof(T), item.ToString());
                ret += enumValue.ToString() + spliter;
            }

            return ret.TrimEnd(spliter);
        }

        /// <summary>
        /// 将有[flag]特性的枚举值拼接成字符串
        /// </summary>
        public static string CombineEnumValues(Enum value, char spliter = '|')
        {
            string ret = string.Empty;
            foreach (var e in Enum.GetValues(value.GetType()))
            {
                if (value.HasFlag((Enum)e))
                    ret += string.Format("{0}{1}", e, spliter);
            }

            return ret.TrimEnd(spliter);
        }

        /// <summary>
        /// 将主键列表转换为SQL的IN子句。
        /// </summary>
        /// <param name="primaryKeys">主键列表。</param>
        /// <param name="primaryKeyName">主键列表</param>
        /// <returns>Where条件子句表达式。</returns>
        public static string PrimaryKeyToInClause(List<int> primaryKeys, string primaryKeyName)
        {
            if (primaryKeys.Count == 0)
                return " 1=2 ";

            string ret = primaryKeys[0].ToString();
            for (int i = 1; i < primaryKeys.Count; i++)
                ret += "," + primaryKeys[i].ToString();

            return string.Format(" {0} in ({1})", primaryKeyName, ret);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetPrefixText(string content, int length)
        {
            string result = "";

            if (!string.IsNullOrEmpty(content))
            {
                if (content.Length > length)
                    result = content.Substring(0, length) + "...";
                else
                    result = content;
            }
            return result;
        }

        public static string GetPrefix(this string content, int length)
        {
            return GetPrefixText(content, length);
        }

        /// <summary>
        /// 给文本加掩码
        /// </summary>
        public static string MaskText(string text, int start, int end)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.Length < start)
                return text;

            if (text.Length < end)
                return "".PadLeft(text.Length, '*');

            return text.Substring(0, start) + "".PadLeft(end - start, '*') + text.Substring(end);
        }

        /// <summary>
        /// 评分等级
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static string GetScoreGrade(decimal score)
        {
            if (score > 0 && score <= 1) return "很差";
            if (score > 1 && score <= 2) return "差";
            if (score > 2 && score <= 3) return "一般";
            if (score > 3 && score <= 4) return "好";
            if (score > 4 && score <= 5) return "很好";
            if (score > 5) return "非常好";
            return "--";
        }
        /// <summary>
        /// 获取书路径
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static string GetBookFilePath(int bookId)
        {
            return string.Format("{0}/{1}/{2}/", bookFilePath, (bookId / BaseNumber), bookId);
        }

        /// <summary>
        /// 获取原始图文件名地址
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="coverName">原始图文件名</param>
        /// <returns></returns>
        public static string GetCoverFileName(int bookId, string coverName)
        {
            if (!string.IsNullOrEmpty(coverName))
            {
                string filePath = GetBookFilePath(bookId) + coverName;

                return filePath;
            }

            return GetNoPictureFile();
        }

        /// <summary>
        /// 获取缩略图文件名地址
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="coverName">原始图文件名</param>
        /// <returns></returns>
        public static string GetThumFileName(int bookId, string coverName)
        {
            if (!string.IsNullOrEmpty(coverName))
            {
                string fileName = Path.GetFileNameWithoutExtension(coverName);
                string extensionName = Path.GetExtension(coverName);

                string filePath = string.Empty;
                if (bookId >= 260)
                    filePath = GetBookFilePath(bookId) + string.Format("{0}_s{1}", fileName, extensionName);
                else
                    filePath = GetBookFilePath(bookId) + string.Format("cover_s{0}", extensionName);

                return filePath;
            }

            return GetNoPictureFile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetNoPictureFile()
        {
            return domainName + "/Content/Default/Images/nocover.jpg";
        }


        public static string ClearHtml(string Content)
        {
            Content = ReplaceHtml("&#[^>]*;", "", Content);
            Content = ReplaceHtml("</?marquee[^>]*>", "", Content);
            Content = ReplaceHtml("</?object[^>]*>", "", Content);
            Content = ReplaceHtml("</?param[^>]*>", "", Content);
            Content = ReplaceHtml("</?embed[^>]*>", "", Content);
            Content = ReplaceHtml("</?table[^>]*>", "", Content);
            Content = ReplaceHtml(" ", "", Content);
            Content = ReplaceHtml("</?tr[^>]*>", "", Content);
            Content = ReplaceHtml("</?th[^>]*>", "", Content);
            Content = ReplaceHtml("</?p[^>]*>", "", Content);
            Content = ReplaceHtml("</?a[^>]*>", "", Content);
            Content = ReplaceHtml("</?img[^>]*>", "", Content);
            Content = ReplaceHtml("</?tbody[^>]*>", "", Content);
            Content = ReplaceHtml("</?li[^>]*>", "", Content);
            Content = ReplaceHtml("</?span[^>]*>", "", Content);
            Content = ReplaceHtml("</?div[^>]*>", "", Content);
            Content = ReplaceHtml("</?th[^>]*>", "", Content);
            Content = ReplaceHtml("</?td[^>]*>", "", Content);
            Content = ReplaceHtml("</?script[^>]*>", "", Content);
            Content = ReplaceHtml("(javascript|jscript|vbscript|vbs):", "", Content);
            Content = ReplaceHtml("on(mouse|exit|error|click|key)", "", Content);
            Content = ReplaceHtml("<\\?xml[^>]*>", "", Content);
            Content = ReplaceHtml("<\\/?[a-z]+:[^>]*>", "", Content);
            Content = ReplaceHtml("</?font[^>]*>", "", Content);
            Content = ReplaceHtml("</?b[^>]*>", "", Content);
            Content = ReplaceHtml("</?u[^>]*>", "", Content);
            Content = ReplaceHtml("</?i[^>]*>", "", Content);
            Content = ReplaceHtml("</?strong[^>]*>", "", Content);
            string clearHtml = Content;
            return clearHtml;
        }
        /// <summary>  
        /// 清除文本中的Html标签  
        /// </summary>  
        /// <param name="patrn">要替换的标签正则表达式</param>  
        /// <param name="strRep">替换为的内容</param>  
        /// <param name="content">要替换的内容</param>  
        /// <returns></returns>  
        public static string ReplaceHtml(string patrn, string strRep, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }
            Regex rgEx = new Regex(patrn, RegexOptions.IgnoreCase);
            string strTxt = rgEx.Replace(content, strRep);
            return strTxt;
        }


        /// <summary>
        /// 评论star显示处理
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static string DealStarScore(Decimal score)
        {
            if (score > 0)
            {
                string s = score.ToString("0.0");
                string[] scores = s.Split('.');

                if (scores[1] == "0")
                    return scores[0];

                if (int.Parse(scores[1]) <= 5)
                {
                    return scores[0] + ".5";
                }
                else
                {
                    return (int.Parse(scores[0]) + 1).ToString();
                }
            }
            return "0";
        }

        public static string GetLocalIp()
        {
            IPHostEntry MyEntry = Dns.GetHostByName(Dns.GetHostName());
            IPAddress MyAddress = new IPAddress(MyEntry.AddressList[0].Address);
            return MyAddress.ToString();
        }


        /************* billy 2013-6-8 ***************/

        /// <summary>
        /// UTF8 转化 GB2312
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UTF8ToGB2312(string str)
        {
            Encoding utf8 = Encoding.GetEncoding(65001);

            Encoding gb2312 = Encoding.GetEncoding("gb2312");

            byte[] temp = utf8.GetBytes(str);

            byte[] temp1 = Encoding.Convert(utf8, gb2312, temp);

            string result = gb2312.GetString(temp1);

            return result;
        }

        public static string NoEmpty(string source, string defaultValue)
        {
            if (string.IsNullOrEmpty(source))
                return defaultValue;

            return source;
        }

        public static string NoEmpty(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "-";

            return source;
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="seed">自己的分销ID号</param>
        /// <returns></returns>
        public static string GenerateNumber(int seed)
        {
            string number = string.Empty;
            lock (lockObj)
            {
                Thread.Sleep(10);
                number = DateTime.Now.ToString("yy-MM-dd HH:mm:ss ff");
            }

            number = number.Replace(":", "");
            number = number.Replace("-", "");
            number = number.Replace(" ", "");

            return string.Format("{0}{1}", seed.ToString().PadLeft(4, '1'), number);
        }

        public static string GenerateBatchNumber()
        {
            //Thread.Sleep(10);
            //return DateTime.Now.Ticks.ToString();
            var seed = (new Random()).Next(1000, 9999);
            return GenerateNumber(seed);
        }

        public static string GetDomain()
        {
            return System.Web.HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 使HTML中图片滚动加载
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string TrimHtmlToScrollLoadingImage(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            html = Regex.Replace(html, " src=", " class='scrollLoading' data-url=", RegexOptions.IgnoreCase);

            return html;
        }

        public static string GetDayOfWeek(DayOfWeek week)
        {
            var weeks = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

            var idx = (int)week;
            if (idx > 6 || idx < 0)
                return "未定义";
            else
                return weeks[idx];
        }

        public static bool IsToday(DateTime date)
        {
            bool isToday = false;
            if (date.Date == DateTime.Now.Date)
                isToday = true;

            return isToday;
        }


        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyname);

            if (property == null) return string.Empty;

            object o = property.GetValue(t, null);

            if (o == null) return string.Empty;

            return o.ToString();
        }

        #region 手机号 身份证 等验证

        /// <summary>
        /// 验证手机号码 主要是判断是否以1开头的11位数字字符串
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>如果是手机号码，返回true，否则返回false</returns>
        public static bool IsMobilePhone(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 11)
                return false;

            var regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>如果是邮箱，返回true，否则返回false</returns>
        public static bool IsEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 验证时间格式HH:MM
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>如果是HH:MM，返回true，否则返回false</returns>
        public static bool IsTime(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var regex = new Regex(@"^([0-1][0-9]|[2][0-3]):([0-5][0-9])$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 身份证号码验证,可验证15位和18位的身份证号码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                return false;//校验码验证

            return true;//符合GB11643-1999标准

        }

        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            return true;//符合15位身份证标准
        }

        #endregion

        #region 数字 整数 等验证

        public static bool IsNumberString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var regex = new Regex("\\d$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 验证是否为整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIntegerString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var regex = new Regex("^-?[1-9]\\d*$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="IncludeZero">是否包含0</param>
        /// <returns></returns>
        public static bool IsPositiveIntegerString(string input, bool IncludeZero = false)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string expression = "";

            if (IncludeZero)
                expression = "^[1-9]\\d*|0$";
            else
                expression = "^[1-9]\\d*$";

            var regex = new Regex(expression);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 验证是否为负整数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="IncludeZero">是否包含0</param>
        /// <returns></returns>
        public static bool IsNegativeIntegerString(string input, bool IncludeZero = false)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string expression = "";

            if (IncludeZero)
                expression = "^-[1-9]\\d*|0$";
            else
                expression = "^-[1-9]\\d*$";

            var regex = new Regex(expression);
            return regex.IsMatch(input);
        }

        #endregion

        #region 拼音

        /// <summary>
        /// 获取一组汉字的首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += GetSpell(strText.Substring(i, 1));
            }

            return myStr;
        }

        /// <summary>
        /// 过滤字符串中除中文和字母以外的其它字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ChineseFilter(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string ret = "";
            string reg = @"[\u4E00-\u9FA5a-zA-Z]*";
            MatchCollection Matches = Regex.Matches(s, reg, RegexOptions.IgnoreCase);
            foreach (Match NextMatch in Matches)
            {
                ret += NextMatch.Value;
            }

            return ret;
        }

        private static string GetSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else
            {
                if ((arrCN[0] >= 'a' && arrCN[0] <= 'z') || (arrCN[0] >= 'A' && arrCN[0] <= 'Z'))
                {
                    return cnChar.ToUpper();
                }
                else
                    return "*";
            }
        }

        #endregion


        public static string GetSmallImagePath(this string imagePath, string postfix = "small")
        {
            if (string.IsNullOrEmpty(imagePath))
                return string.Empty;

            string fileName = Path.GetFileNameWithoutExtension(imagePath);
            string newFileName = "";
            if (Regex.IsMatch(fileName, @"\d+"))
                newFileName = Regex.Match(fileName, @"\d+").Value;

            string ret = Path.Combine(Path.GetDirectoryName(imagePath), newFileName + postfix + Path.GetExtension(imagePath));
            ret = ret.Replace(@"\", "/");

            return ret;
        }

        /// <summary>
        /// 过滤电话号码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string TrimPhoneNumber(string source, string replaceWith = "")
        {
            if (string.IsNullOrEmpty(source))
                return "";

            try
            {  
                //手机号,座机号
                string regex =@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";
                source = Regex.Replace(source, regex, replaceWith);

                string regex2 = @"0\d{2}-\d{8}|0\d{3}-\d{7}"; //带'-'座机号
                source = Regex.Replace(source, regex2, replaceWith);
            }
            catch
            {
            }

            return source;
        }

        /// <summary>
        /// 字符串中是否包含
        /// </summary>
        public static bool HasContains(string source, string value)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            string[] sourceArray = source.Split(new char[] { ',' });
            foreach (string s in sourceArray)
            {
                if (!string.IsNullOrEmpty(s) && s == value)
                    return true;
            }
            return false;
        }
    }
}
