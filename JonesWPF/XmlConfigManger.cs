using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF
{
    interface IConfiguratable
    {
        void AddInfToConfig(string loadPath, string savePath);
        void AddInfToConfig(List<CheckBox> selectedCheckBoxes);
        void ReadInitialPathLoad();
        void ReadInitialPathSave();
    }

    class XmlConfigManger : IConfiguratable
    {
        string fileName;
        XDocument configDoc;
        FileStream stream;

        public XmlConfigManger()
        {
            fileName = "cofig.xml";

            //InitializeConfig();
        }

        private void InitializeConfig()
        {
            if (File.Exists(fileName))
            {
                stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                configDoc = XDocument.Load(stream);
            }
            else
            {
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                configDoc = new XDocument();
            }
        }

        public void AddInfToConfig(List<CheckBox> selectedCheckBoxes)
        {
            InitializeConfig();
            var xElement = new XElement("CheckBoxes");

            configDoc.Descendants().Where(e => e.Name == "CheckBox").Remove();

            foreach (var checkBox in selectedCheckBoxes)
            {
                xElement.Add(new XElement("CheckBox", $"{checkBox}"));           
            }

            configDoc.Add(xElement);
            Debug.WriteLine(configDoc);
            configDoc.Save(stream);
            stream.Close();
        }

        public void AddInfToConfig(string loadPath, string savePath)
        {
            throw new NotImplementedException();
        }

        public void ReadInitialPathLoad()
        {
            throw new NotImplementedException();
        }

        public void ReadInitialPathSave()
        {
            throw new NotImplementedException();
        }
    }
}
