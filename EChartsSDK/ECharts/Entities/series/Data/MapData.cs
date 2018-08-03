using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECharts.Entities.series.data
{
    public class MapData
    {
        public string name { get; set; }
        public object value { get; set; }
        public bool? selected { get; set; }

        public MapData Name(string name)
        {
            this.name = name;
            return this;
        }

        public MapData Value(object value)
        {
            this.value = value;
            return this;
        }
        public MapData Data(params object[] values)
        {
            if (values == null)
                return default(MapData);
            this.value = values.ToList();
            return this ;
        }
        public MapData Selected(bool selected)
        {
            this.selected = selected;
            return this;
        }



    }
}
