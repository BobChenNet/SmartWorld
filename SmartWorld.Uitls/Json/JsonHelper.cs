using System.Web;
using System.Data;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace SmartWorld.Uitls
{
    /// <summary>
    /// Json字符串 序列化或反序列化
    /// </summary>
    public static class JsonHelper
    {
        #region 序列化JSON

        /// <summary>
        /// 将特定对象Json序列化。
        /// </summary>
        /// <param name="obj">需要Json序列化的对象。</param>
        public static string Serialize(object obj)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//这里使用自定义日期格式，默认是ISO8601格式           
            //timeConverter.DateTimeFormat = "yyyy-MM-dd hh:mm:ss";//设置时间格式   
            //timeConverter.DateTimeStyles = System.Globalization.DateTimeStyles.AssumeLocal;

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);//转换序列化的对象  
            return json;
        }

        /// <summary>
        /// 将DataTable对象Json序列化
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootName">不指定rootName,则输出Json数组</param>
        /// <returns></returns>
        public static string Serialize(DataTable dt, string rootName)
        {
            /**//* /****************************************************************************
             * Without goingin to the depth of the functioning of this Method, i will try to give an overview
             * As soon as this method gets a DataTable it starts to convert it into JSON String,
             * it takes each row and in each row it grabs the cell name and its data.
             * This kind of JSON is very usefull when developer have to have Column name of the .
             * Values Can be Access on clien in this way. OBJ.HEAD[0].<ColumnName>
             * NOTE: One negative point. by this method user will not be able to call any cell by its index.
            * *************************************************************************/
            StringBuilder JsonString = new StringBuilder();
            //Exception Handling        
            if (dt != null)
            {
                if (!string.IsNullOrEmpty(rootName))
                {
                    JsonString.Append("{ ");
                    JsonString.Append("\"" + rootName + "\":");
                }
                JsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    /**//*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");
                if (!string.IsNullOrEmpty(rootName))
                    JsonString.Append("}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 反序列化JSON

        /// <summary>
        /// 将特定的Json字符串还原成对象。
        /// </summary>
        /// <param name="jsonString">Json字符串。</param>
        public static T Deserialize<T>(string jsonString)
        {
            T obj = JsonConvert.DeserializeObject<T>(jsonString);

            return obj;
        }

        #endregion


        public static List<T> GetListEnum<T>(this Enum en, Type type)
        {
            List<T> resLsList = new List<T>();
            if (type == typeof(int))
            {
                var list = Enum.GetNames(en.GetType()).ToList();
                resLsList.AddRange(list.Select(s => (T)Convert.ChangeType(s, typeof(T))));
            }
            else
            {
                var values = Enum.GetValues(en.GetType());
                List<int> list = values.Cast<int>().ToList();
                resLsList.AddRange(list.Select(s => (T)Convert.ChangeType(s, typeof(T))));

            }
            return resLsList;
        }
    }
}