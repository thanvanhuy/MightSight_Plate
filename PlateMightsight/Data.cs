using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateMightsight
{
    public class Data
    {
        public string event_type { get; set; }
        public string device_name { get; set; }
        public string mac_address { get; set; }
        public string sn { get; set; }
        public string time { get; set; }
        public string detection_region { get; set; }
        public string detection_region_name { get; set; }
        public string license_plate { get; set; }
        public string country_region { get; set; }
        public string plate_type { get; set; }
        public string plate_color { get; set; }
        public string vehicle_type { get; set; }
        public string vehicle_color { get; set; }
        public string vehicle_brand { get; set; }
        public string direction { get; set; }
        public string speed { get; set; }
        public DataList data_list { get; set; }
        public string resolution_width { get; set; }
        public string resolution_height { get; set; }
        public string coordinate_x1 { get; set; }
        public string coordinate_y1 { get; set; }
        public string coordinate_x2 { get; set; }
        public string coordinate_y2 { get; set; }
        public string vehicle_tracking_box_x1 { get; set; }
        public string vehicle_tracking_box_y1 { get; set; }
        public string vehicle_tracking_box_x2 { get; set; }
        public string vehicle_tracking_box_y2 { get; set; }
        public string license_plate_snapshot { get; set; }
        public string vehicle_snapshot { get; set; }
        public string full_snapshot { get; set; }
        public string violation_snapshot { get; set; }
        public string evidence_snapshot0 { get; set; }
        public string evidence_snapshot1 { get; set; }
    }
    public class DataList
    {
        public string detection_trigger { get; set; }
        public string alarm_input_no { get; set; }
        public string attributes_rule { get; set; }
    }
}
