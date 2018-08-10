using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECharts.Entities.series
{
    public class Custom : Rectangular<Custom>
    {

        public object renderItem { get; set; }

        public object dimensions { get; set; }

        public object encode { get; set; }
        
        public Custom()
        {
            this.type = ChartType.custom;
        }
        public Custom RenderItem(object renderItem)
        {
            this.renderItem = renderItem;
            return this  ;
        }

        public Custom Dimensions(IList<string> dimensions)
        {
            this.dimensions = dimensions;
            return this;
        }

        public Custom Dimensions(params string[] dimensions)
        {
            this.dimensions = dimensions.ToList();
            return this;
        }

        public Custom Encode(object encode)
        {
            this.encode = encode;
            return this;
        }
        

    }
}
