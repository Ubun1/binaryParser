﻿using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF
{
    enum InitialPaths
    {
        SavePath,
        LoadPath
    }
    static class XmlConfigManger
    {
        static string fileName;
        static XDocument configDoc;
        static XElement checkBoxElement;
        static XElement pathsElement;

        static XmlConfigManger()
        {
            fileName = "config.xml";

            InitializeConfig();
        }

        public static void AddInfToConfig(List<CheckBox> selectedCheckBoxes)
        {
            DeleteOldNodes("CheckBoxes");

            checkBoxElement = new XElement("CheckBoxes");

            foreach (var checkBox in selectedCheckBoxes)
            {
                checkBoxElement.Add(new XElement("CheckBox", $"{(int)checkBox}"));
            }

            configDoc.Add(checkBoxElement);
            Debug.WriteLine(configDoc);
        }

        public static void AddInfToConfig(string loadPath, string savePath)
        {
            DeleteOldNodes("Paths");
            pathsElement = new XElement("Paths");
            pathsElement.Add(
                new XElement("LoadPath", $"{loadPath}"),
                new XElement("SavePath", $"{savePath}")
                );
            configDoc.Add(pathsElement);
        }

        public static string ReadInitialPath(InitialPaths initialPaths)
        {
            var result = @"C:\";

            if (configDoc.Element("Paths") == null)
            {
                return result;
            }
            result = configDoc.Element("Paths").Element($"{initialPaths}").Value;
            return result;
        }
      
        public static List<CheckBox> ReadInitialCheckBoxes()
        {
            var result = new List<CheckBox>();

            if (configDoc.Element("CheckBoxes") == null)
            {
                return result;
            }

            foreach (var node in configDoc.Element("CheckBoxes").Elements("CheckBox"))
            {
                result.Add((CheckBox)int.Parse(node.Value));
            }

            return result;
        }

        public static void SaveConfig()
        {
            configDoc.Save(new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite));
        }

        private static void InitializeConfig()
        {
            if (File.Exists(fileName))
            {
                var stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                configDoc = XDocument.Load(stream);
                stream.Close();
            }
            else
            {
                configDoc = new XDocument();
            }
        }
        private static void DeleteOldNodes(string node)
        {
            if (configDoc.Element(node) != null)
            {
                configDoc.Element(node).Remove();
            }
        }

    }
}