using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECharts.Entities.series.data
{
    public class ScatterData 
    {
       public  List<object> value;

        public ScatterData()
        {

           
        }
        public ScatterData(object width, object height)
        {
            
            this.Value(width, height);
        }

        public ScatterData(object width, object height, object size)
        {
            this.Value(width, height, size);
        }
        public List<object> Value()
        {
            if (this.value == null)
            {
                this.value = new List<object>();
            }
            return this.value;
        }

        public ScatterData Value( params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                return this;
            }
            this.Value().Add(values.ToArray());
            return this;
        }

    }
}
