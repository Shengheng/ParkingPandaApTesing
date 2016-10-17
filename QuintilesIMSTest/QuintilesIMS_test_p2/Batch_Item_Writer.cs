using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace QuintilesIMS_test_p2
{
    class Batch_Item_Writer
    {
        public void GenerateXML(List<UserInfor> itemList)
        {
            //xml setting config
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = Encoding.UTF8;
            setting.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("../../Batch_Item.xml",setting))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Batch_Item");
                foreach (var item in itemList)
                {
                    writer.WriteStartElement("Item");

                    writer.WriteElementString(nameof(item.UserName), item.UserName);
                    writer.WriteElementString(nameof(item.Email), item.Email);
                    writer.WriteElementString(nameof(item.Init_Password), item.Init_Password);
                    writer.WriteElementString(nameof(item.Role), item.Role);
                    writer.WriteElementString(nameof(item.Reason_For_Access), item.Reason_For_Access);

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }

        public void GenerateCSV(List<UserInfor> itemList)
        {            
            
            StringBuilder sb = new StringBuilder();
            sb.Append("Name,Email,Init_Password,Role,Reason_For_Access" + Environment.NewLine);
            foreach (var item in itemList)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3},{4}", CSVFomater(item.UserName), CSVFomater(item.Email),
                    CSVFomater(item.Init_Password), CSVFomater(item.Role), CSVFomater(item.Reason_For_Access)));
            }
            File.WriteAllText("../../Batch_Item.csv", sb.ToString(),Encoding.UTF8);
        }
        private string CSVFomater(string data)
        {
            if (data.Contains("\""))
            {
                data = data.Replace("\"", "\"\"");
            }

            if (data.Contains(","))
            {
                data = String.Format("\"{0}\"", data);
            }

            if (data.Contains(System.Environment.NewLine))
            {
                data = String.Format("\"{0}\"", data);
            }
            return data;
        }
    }
}
