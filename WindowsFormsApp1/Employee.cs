using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DataCollector
{
    internal class Employee
    {
        private readonly string XmlFilePath = "Employee.xml";
        private readonly XmlDocument xmlDocument = new XmlDocument();
        public int Id { get; set; }
        public string Name { get; set; }

        private List<string> _employeesName = new List<string>();

        public Employee()
        {
            if (!File.Exists(XmlFilePath))
            {
                File.Create(XmlFilePath).Close();
                

                xmlDocument.LoadXml("<Employees>" +
                                    "</Employees>");
                XmlDeclaration xmlDeclaration;
                xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
                XmlElement employeesElement = xmlDocument.DocumentElement;
                xmlDocument.InsertBefore(xmlDeclaration, employeesElement);
                xmlDocument.Save(XmlFilePath);
            }
        }

        public void AddEmployee(int id, string jobPosition, string name)
        {
            xmlDocument.Load(XmlFilePath);
            int childCount = xmlDocument.ChildNodes.Count;
            var lastChildAttribute = xmlDocument.ChildNodes.Item(childCount - 1).LastChild.Attributes;
            var lastNodeId = Convert.ToInt32(lastChildAttribute.Item(0).Value);


            XmlElement employeeElement = xmlDocument.CreateElement("Employee");
            xmlDocument.DocumentElement.AppendChild(employeeElement);
            XmlAttribute attribute = xmlDocument.CreateAttribute("NodeId");
            attribute.Value = (lastNodeId + 1).ToString();
            employeeElement.Attributes.Append(attribute);

            XmlElement idElement = xmlDocument.CreateElement("Id");
            idElement.InnerText = id.ToString();
            employeeElement.AppendChild(idElement);

            XmlElement nameElement = xmlDocument.CreateElement("Name");
            nameElement.InnerText = name;
            employeeElement.AppendChild(nameElement);

            XmlElement jobElement = xmlDocument.CreateElement("Job");
            jobElement.InnerText = jobPosition;
            employeeElement.AppendChild(jobElement);

            xmlDocument.Save(XmlFilePath);
        }

        public List<string> GetAllEmployeesName()
        {
            xmlDocument.Load(XmlFilePath);

            var names = xmlDocument.GetElementsByTagName("Name");

            foreach (XmlNode name in names)
            {
                this._employeesName.Add(name.InnerText);
            }

            return this._employeesName;
        }

        public void EditEmployee(int employeeId, string newName, int newId, string newJobTitle)
        {
            xmlDocument.Load(XmlFilePath);

            XmlNode targetEmployeeName = xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Name", employeeId));
            targetEmployeeName.InnerText = newName;
            XmlNode targetEmployeeId = xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Id", employeeId));
            targetEmployeeId.InnerText = newId.ToString();
            XmlNode targetJobTitle = xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Job", employeeId));
            targetJobTitle.InnerText = newJobTitle;

            xmlDocument.Save(XmlFilePath);

        }
    }
}
