using System;
using System.IO;
using System.Xml;

namespace Angular_Utility
{
    class Program {
        static void Main(string[] args){
            string lPath = Directory.GetCurrentDirectory();

            XmlDocument lXmlDoc = new XmlDocument();
            lXmlDoc.Load(lPath + "/Projects");

            XmlNodeList lXmlProjectsNodes = lXmlDoc.GetElementsByTagName("project");

            for (int i = 0; i < lXmlProjectsNodes.Count; i++)
            {
                showProject(i, lXmlProjectsNodes[i]);
            }

            //int lSelectedProjectNumber = int.TryParse();

            Console.ReadKey();
        }

        private static void showProject(int index_, XmlNode node_) {
            Console.WriteLine(index_ + 1);
            Console.WriteLine("\tИмя проекта:\t" + node_.ChildNodes[0].InnerText);
            Console.WriteLine("\tПуть к проекту:\t" + node_.ChildNodes[1].InnerText);
            Console.WriteLine("\tОписание:\t" + node_.ChildNodes[2].InnerText);
        }
    }
}
