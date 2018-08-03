﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ECharts
{
    /// <summary>
    /// Json Tools
    /// </summary>
    public class JsonTools
    {
 
        /// <summary>
        /// 序列化数据为Json数据格式.
        /// </summary>
        /// <param name="value">被序列化的对象</param>
        /// <returns></returns>
        public static dynamic ObjectToJson(object value)
        {
            return ObjectToJson(value, false);
        }

        /// <summary>
        /// 序列化数据为Json数据格式.
        /// </summary>
        /// <param name="value">被序列化的对象</param>
        /// <param name="clearLastZero">是否清除小数位后的0</param>
        /// <returns></returns>
        public static dynamic ObjectToJson(object value, bool clearLastZero)
        {
            Type type = value.GetType();
            JsonSerializer json = new JsonSerializer();
            //json.NullValueHandling = NullValueHandling.Ignore;
            json.ObjectCreationHandling = ObjectCreationHandling.Replace;           
            json.MissingMemberHandling = MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;     
                   
            json.Converters.Add(new StringEnumConverter());
            IsoDateTimeConverter timeFormate = new IsoDateTimeConverter();
            timeFormate.DateTimeFormat = "yyyy-MM-dd";
            json.Converters.Add(timeFormate);        
            json.Formatting = Formatting.Indented;
            json.NullValueHandling = NullValueHandling.Ignore;
           
            if (clearLastZero)
                json.Converters.Add(new MinifiedNumArrayConverter());
            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.None;
            writer.QuoteChar = '"';
            writer.QuoteName = false;
            json.Serialize(writer, value);
     
            string output = sw.ToString();
            writer.Close();
            sw.Close();
           // return JObject.Parse(output); 
            return output;
        }

        //把datatable中获取某一列数据转换成数组
        public static object[] DataTableToArray( DataTable dt,string columnsName)
        {
            if (dt.Rows.Count <= 0)
            {
                return null;
            }

            object[] result = dt.AsEnumerable().Select(d => d.Field<object>(columnsName)).ToArray();

            return result;
        }

        //把datatable中所有列列数据转换成数组
        public static object[] DataTableToDataSet(DataTable dt )
        {
            if (dt.Rows.Count <= 0)
            {
                return null;
            }

            List<object> resultList = new List<object>();
            foreach (DataColumn columnsName in dt.Columns )
            {
                object[] dataTemp = dt.AsEnumerable().Select(d => d.Field<object>(columnsName)).ToArray();
                resultList.Add(dataTemp);
            }

            return resultList.ToArray();
        }




        /// <summary>
        /// datatble 数据转换成地图数据
        /// </summary>
        /// <param name="dataSource"> 必须包含城市名称， 值可选</param>
        /// <returns></returns>
        public static object[] DataTableToGeoMap(DataTable dataSource)
        {
            if (dataSource.Rows.Count <= 0)
            {
                return null;
            }

            if (!dataSource.Columns.Contains("name"))
            {
                return null;
            }

            JArray  result = new JArray();

            foreach (DataRow drRow in dataSource.Rows)
            {
                JObject JobjRow = new JObject();
                JArray JarrayRow = GetMapLocation(drRow["name"].ToString());
                if (dataSource.Columns.Contains("value")) //有值则尾部追加值
                {
                    JarrayRow.Add(drRow["value"]);
                }     
                JobjRow.Add("name",drRow["name"].ToString());
                JobjRow.Add("value", JarrayRow);

                result.Add(JobjRow);
            }

            return result.ToArray();
        }



        private static JArray GetMapLocation(string name)
        {

            string strGeoCoordMap = "{\n    \'海门\':[121.15,31.89],\n    \'鄂尔多斯\':[109.781327,39.608266],\n    \'招远\':[120.38,37.35],\n    \'舟山\':[122.207216,29.985295],\n    \'齐齐哈尔\':[123.97,47.33],\n    \'盐城\':[120.13,33.38],\n    \'赤峰\':[118.87,42.28],\n    \'青岛\':[120.33,36.07],\n    \'乳山\':[121.52,36.89],\n    \'金昌\':[102.188043,38.520089],\n    \'泉州\':[118.58,24.93],\n    \'莱西\':[120.53,36.86],\n    \'日照\':[119.46,35.42],\n    \'胶南\':[119.97,35.88],\n    \'南通\':[121.05,32.08],\n    \'拉萨\':[91.11,29.97],\n    \'云浮\':[112.02,22.93],\n    \'梅州\':[116.1,24.55],\n    \'文登\':[122.05,37.2],\n    \'上海\':[121.48,31.22],\n    \'攀枝花\':[101.718637,26.582347],\n    \'威海\':[122.1,37.5],\n    \'承德\':[117.93,40.97],\n    \'厦门\':[118.1,24.46],\n    \'汕尾\':[115.375279,22.786211],\n    \'潮州\':[116.63,23.68],\n    \'丹东\':[124.37,40.13],\n    \'太仓\':[121.1,31.45],\n    \'曲靖\':[103.79,25.51],\n    \'烟台\':[121.39,37.52],\n    \'福州\':[119.3,26.08],\n    \'瓦房店\':[121.979603,39.627114],\n    \'即墨\':[120.45,36.38],\n    \'抚顺\':[123.97,41.97],\n    \'玉溪\':[102.52,24.35],\n    \'张家口\':[114.87,40.82],\n    \'阳泉\':[113.57,37.85],\n    \'莱州\':[119.942327,37.177017],\n    \'湖州\':[120.1,30.86],\n    \'汕头\':[116.69,23.39],\n    \'昆山\':[120.95,31.39],\n    \'宁波\':[121.56,29.86],\n    \'湛江\':[110.359377,21.270708],\n    \'揭阳\':[116.35,23.55],\n    \'荣成\':[122.41,37.16],\n    \'连云港\':[119.16,34.59],\n    \'葫芦岛\':[120.836932,40.711052],\n    \'常熟\':[120.74,31.64],\n    \'东莞\':[113.75,23.04],\n    \'河源\':[114.68,23.73],\n    \'淮安\':[119.15,33.5],\n    \'泰州\':[119.9,32.49],\n    \'南宁\':[108.33,22.84],\n    \'营口\':[122.18,40.65],\n    \'惠州\':[114.4,23.09],\n    \'江阴\':[120.26,31.91],\n    \'蓬莱\':[120.75,37.8],\n    \'韶关\':[113.62,24.84],\n    \'嘉峪关\':[98.289152,39.77313],\n    \'广州\':[113.23,23.16],\n    \'延安\':[109.47,36.6],\n    \'太原\':[112.53,37.87],\n    \'清远\':[113.01,23.7],\n    \'中山\':[113.38,22.52],\n    \'昆明\':[102.73,25.04],\n    \'寿光\':[118.73,36.86],\n    \'盘锦\':[122.070714,41.119997],\n    \'长治\':[113.08,36.18],\n    \'深圳\':[114.07,22.62],\n    \'珠海\':[113.52,22.3],\n    \'宿迁\':[118.3,33.96],\n    \'咸阳\':[108.72,34.36],\n    \'铜川\':[109.11,35.09],\n    \'平度\':[119.97,36.77],\n    \'佛山\':[113.11,23.05],\n    \'海口\':[110.35,20.02],\n    \'江门\':[113.06,22.61],\n    \'章丘\':[117.53,36.72],\n    \'肇庆\':[112.44,23.05],\n    \'大连\':[121.62,38.92],\n    \'临汾\':[111.5,36.08],\n    \'吴江\':[120.63,31.16],\n    \'石嘴山\':[106.39,39.04],\n    \'沈阳\':[123.38,41.8],\n    \'苏州\':[120.62,31.32],\n    \'茂名\':[110.88,21.68],\n    \'嘉兴\':[120.76,30.77],\n    \'长春\':[125.35,43.88],\n    \'胶州\':[120.03336,36.264622],\n    \'银川\':[106.27,38.47],\n    \'张家港\':[120.555821,31.875428],\n    \'三门峡\':[111.19,34.76],\n    \'锦州\':[121.15,41.13],\n    \'南昌\':[115.89,28.68],\n    \'柳州\':[109.4,24.33],\n    \'三亚\':[109.511909,18.252847],\n    \'自贡\':[104.778442,29.33903],\n    \'吉林\':[126.57,43.87],\n    \'阳江\':[111.95,21.85],\n    \'泸州\':[105.39,28.91],\n    \'西宁\':[101.74,36.56],\n    \'宜宾\':[104.56,29.77],\n    \'呼和浩特\':[111.65,40.82],\n    \'成都\':[104.06,30.67],\n    \'大同\':[113.3,40.12],\n    \'镇江\':[119.44,32.2],\n    \'桂林\':[110.28,25.29],\n    \'张家界\':[110.479191,29.117096],\n    \'宜兴\':[119.82,31.36],\n    \'北海\':[109.12,21.49],\n    \'西安\':[108.95,34.27],\n    \'金坛\':[119.56,31.74],\n    \'东营\':[118.49,37.46],\n    \'牡丹江\':[129.58,44.6],\n    \'遵义\':[106.9,27.7],\n    \'绍兴\':[120.58,30.01],\n    \'扬州\':[119.42,32.39],\n    \'常州\':[119.95,31.79],\n    \'潍坊\':[119.1,36.62],\n    \'重庆\':[106.54,29.59],\n    \'台州\':[121.420757,28.656386],\n    \'南京\':[118.78,32.04],\n    \'滨州\':[118.03,37.36],\n    \'贵阳\':[106.71,26.57],\n    \'无锡\':[120.29,31.59],\n    \'本溪\':[123.73,41.3],\n    \'克拉玛依\':[84.77,45.59],\n    \'渭南\':[109.5,34.52],\n    \'马鞍山\':[118.48,31.56],\n    \'宝鸡\':[107.15,34.38],\n    \'焦作\':[113.21,35.24],\n    \'句容\':[119.16,31.95],\n    \'北京\':[116.46,39.92],\n    \'徐州\':[117.2,34.26],\n    \'衡水\':[115.72,37.72],\n    \'包头\':[110,40.58],\n    \'绵阳\':[104.73,31.48],\n    \'乌鲁木齐\':[87.68,43.77],\n    \'枣庄\':[117.57,34.86],\n    \'杭州\':[120.19,30.26],\n    \'淄博\':[118.05,36.78],\n    \'鞍山\':[122.85,41.12],\n    \'溧阳\':[119.48,31.43],\n    \'库尔勒\':[86.06,41.68],\n    \'安阳\':[114.35,36.1],\n    \'开封\':[114.35,34.79],\n    \'济南\':[117,36.65],\n    \'德阳\':[104.37,31.13],\n    \'温州\':[120.65,28.01],\n    \'九江\':[115.97,29.71],\n    \'邯郸\':[114.47,36.6],\n    \'临安\':[119.72,30.23],\n    \'兰州\':[103.73,36.03],\n    \'沧州\':[116.83,38.33],\n    \'临沂\':[118.35,35.05],\n    \'南充\':[106.110698,30.837793],\n    \'天津\':[117.2,39.13],\n    \'富阳\':[119.95,30.07],\n    \'泰安\':[117.13,36.18],\n    \'诸暨\':[120.23,29.71],\n    \'郑州\':[113.65,34.76],\n    \'哈尔滨\':[126.63,45.75],\n    \'聊城\':[115.97,36.45],\n    \'芜湖\':[118.38,31.33],\n    \'唐山\':[118.02,39.63],\n    \'平顶山\':[113.29,33.75],\n    \'邢台\':[114.48,37.05],\n    \'德州\':[116.29,37.45],\n    \'济宁\':[116.59,35.38],\n    \'荆州\':[112.239741,30.335165],\n    \'宜昌\':[111.3,30.7],\n    \'义乌\':[120.06,29.32],\n    \'丽水\':[119.92,28.45],\n    \'洛阳\':[112.44,34.7],\n    \'秦皇岛\':[119.57,39.95],\n    \'株洲\':[113.16,27.83],\n    \'石家庄\':[114.48,38.03],\n    \'莱芜\':[117.67,36.19],\n    \'常德\':[111.69,29.05],\n    \'保定\':[115.48,38.85],\n    \'湘潭\':[112.91,27.87],\n    \'金华\':[119.64,29.12],\n    \'岳阳\':[113.09,29.37],\n    \'长沙\':[113,28.21],\n    \'衢州\':[118.88,28.97],\n    \'廊坊\':[116.7,39.53],\n    \'菏泽\':[115.480656,35.23375],\n    \'合肥\':[117.27,31.86],\n    \'武汉\':[114.31,30.52],\n    \'大庆\':[125.03,46.58]\n}";
            var JobjGeoCoordMap = JObject.Parse(strGeoCoordMap);

            JArray jarrayLocation = (JArray)JobjGeoCoordMap[name];

            return jarrayLocation;

        }


        /// <summary>
        /// 将Json数据转为对象
        /// </summary>
        /// <typeparam name="T">目标对象</typeparam>
        /// <param name="jsonText">json数据字符串</param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonText)
        {
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();

            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            StringReader sr = new StringReader(jsonText);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            T result = default(T);
            try
            {
                result = (T)json.Deserialize(reader, typeof(T));
            }
            catch
            {
            }
            finally
            {
                reader.Close();
            }
            return result;
        }

        /// <summary>
        /// Generate Json string from the object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Json String</returns>
        public static string ObjectToJsonTest(object obj)
        {
            //System.Runtime.Serialization.Json.DataContractJsonSerializer;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            String dataString = Encoding.UTF8.GetString(dataBytes);
            return dataString;
        }

        /// <summary>
        /// Generate a object from Json string
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <param name="obj">Object</param>
        /// <returns>Object</returns>
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            obj = serializer.ReadObject(mStream);
            return obj;
        }

        /// <summary>
        /// Generate a object from Json string
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <param name="obj">Object</param>
        /// <returns>Object</returns>
        public static T JsonToObjectTest<T>(string jsonString)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)serializer.ReadObject(mStream);
            }
        }



        /// <summary>
        /// 普通集合转换Json
        /// </summary>
        /// <param name="array">集合对象</param>
        /// <returns>Json字符串</returns>
        public static string ListToJson(IEnumerable array)
        {

            string jsonString = "[";

            foreach (object item in array)
            {
                jsonString += ObjectToJson(item) + ",";
            }
            int t = jsonString.LastIndexOf(',');
            string strTmp = jsonString.Substring(0, t);
            return strTmp + "]";

        }


        /// <summary>   
        /// DataTable to json   
        /// </summary>   
        /// <param name="jsonName">返回json的名称</param>   
        /// <param name="dt">转换成json的表</param>   
        /// <returns></returns>   
        public string DataTableToJson(string jsonName, System.Data.DataTable dt, string strTotal = "")
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[{\"TotalCount\":\"" + strTotal + "\",\"Head\":[");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Json.Append("{\"ColumnHead\":\"" + dt + dt.Columns[i].ColumnName + "\"}");

                if (i < dt.Columns.Count - 1)
                {
                    Json.Append(",");
                }
            }
            Json.Append("],");

            Json.Append("\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}]");
            return Json.ToString();
        }


        public class MinifiedNumArrayConverter : JsonConverter
        {
            private void dumpNumArray<T>(JsonWriter writer, T[] array)
            {
                foreach (T n in array)
                {
                    var s = n.ToString();
                    //此處可考慮改用string.format("{0:#0.####}")[小數後方#數目依最大小數位數決定]
                    //感謝網友vencin提供建議
                    if (s.EndsWith(".0"))
                        writer.WriteRawValue(s.Substring(0, s.Length - 2));
                    else if (s.Contains("."))
                        writer.WriteRawValue(s.TrimEnd('0'));
                    else
                        writer.WriteRawValue(s);
                }
            }

            private void dumpNum<T>(JsonWriter writer, T value)
            {
                var s = value.ToString();
                //此處可考慮改用string.format("{0:#0.####}")[小數後方#數目依最大小數位數決定]
                //感謝網友vencin提供建議
                if (s.EndsWith(".0"))
                    writer.WriteRawValue(s.Substring(0, s.Length - 2));
                else if (s.Contains("."))
                    writer.WriteRawValue(s.TrimEnd('0'));
                else
                    writer.WriteRawValue(s);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Type t = value.GetType();
                if (t == dblArrayType)
                {
                    writer.WriteStartArray();
                    dumpNumArray<double>(writer, (double[])value);
                    writer.WriteEndArray();
                }
                else if (t == decArrayType)
                {
                    writer.WriteStartArray();
                    dumpNumArray<decimal>(writer, (decimal[])value);
                    writer.WriteEndArray();
                }
                else if (t == decType || t == decNullType)
                {
                    dumpNum<decimal>(writer, (decimal)value);
                }
                else
                    throw new NotImplementedException();
            }

            private Type dblArrayType = typeof(double[]);
            private Type decArrayType = typeof(decimal[]);
            private Type decType = typeof(decimal);
            private Type decNullType = typeof(decimal?);

            public override bool CanConvert(Type objectType)
            {
                if (objectType == dblArrayType || objectType == decArrayType || objectType == decType || objectType == decNullType)
                    return true;
                return false;
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }

}