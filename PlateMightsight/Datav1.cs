using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateMightsight
{
    public class Datav1
    {
        public string device { get; set; } = string.Empty;
        public DateTime time { get; set; } = DateTime.Now;
        public DateTime time_msec { get; set; } = DateTime.Now;
        public string plate { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string speed { get; set; } = string.Empty;
        public string direction { get; set; } = string.Empty;
        public string detection_region { get; set; } = string.Empty;
        public string region { get; set; } = string.Empty;
        public string resolution_width { get; set; } = string.Empty;
        public string resolution_height { get; set; } = string.Empty;
        public int coordinate_x1 { get; set; } = 0;
        public int coordinate_y1 { get; set; } = 0;
        public int coordinate_x2 { get; set; } = 0;
        public int coordinate_y2 { get; set; } = 0;
        public string confidence { get; set; } = string.Empty;
        public string plate_color { get; set; } = string.Empty;
        public string vehicle_type { get; set; } = string.Empty;
        public string vehicle_color { get; set; } = string.Empty;
        public string Vehicle_Brand { get; set; } = string.Empty;
        public string plate_image { get; set; } = string.Empty;
        public string full_image { get; set; } = string.Empty;
        public string evidence_image0 { get; set; } = string.Empty;
        public string evidence_image1 { get; set; } = string.Empty;
    }
}
