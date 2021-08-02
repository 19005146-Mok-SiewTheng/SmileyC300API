using System;
using System.Collections.Generic;

namespace SmileyC300API.Models
{
    public partial class Sensor
    {
        public Sensor()
        {
            SensorData = new HashSet<SensorData>();
        }

        public int SensorId { get; set; }
        public int AccessPointId { get; set; }
        public string SensorName { get; set; }

        public virtual AccessPoint AccessPoint { get; set; }
        public virtual ICollection<SensorData> SensorData { get; set; }
    }
}
