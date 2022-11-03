using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DataCollector
{
    internal class Employee
    {
        private readonly string _xmlFilePath = "Employee.xml";
        private readonly XmlDocument _xmlDocument = new XmlDocument();
        public int Id { get; set; }
        public string Name { get; set; }

        private List<string> _employeesName = new List<string>();

        public Employee()
        {
            if (!File.Exists(_xmlFilePath))
            {
                File.Create(_xmlFilePath).Close();


                this._xmlDocument.LoadXml("<Employees>" +
                                    "</Employees>");
                XmlDeclaration xmlDeclaration;
                xmlDeclaration = this._xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
                XmlElement employeesElement = this._xmlDocument.DocumentElement;
                this._xmlDocument.InsertBefore(xmlDeclaration, employeesElement);
                this._xmlDocument.Save(_xmlFilePath);
            }
        }

        public void AddEmployee(int id, string jobPosition, string name)
        {
            this._xmlDocument.Load(_xmlFilePath);
            int childCount = this._xmlDocument.ChildNodes.Count;
            var lastChildAttribute = this._xmlDocument.ChildNodes.Item(childCount - 1).LastChild.Attributes;
            var lastNodeId = Convert.ToInt32(lastChildAttribute.Item(0).Value);


            XmlElement employeeElement = this._xmlDocument.CreateElement("Employee");
            this._xmlDocument.DocumentElement.AppendChild(employeeElement);

            XmlAttribute attribute = this._xmlDocument.CreateAttribute("NodeId");
            attribute.Value = (lastNodeId + 1).ToString();
            employeeElement.Attributes.Append(attribute);

            XmlElement idElement = this._xmlDocument.CreateElement("Id");
            idElement.InnerText = id.ToString();
            employeeElement.AppendChild(idElement);

            XmlElement nameElement = this._xmlDocument.CreateElement("Name");
            nameElement.InnerText = name;
            employeeElement.AppendChild(nameElement);

            XmlElement jobElement = this._xmlDocument.CreateElement("Job");
            jobElement.InnerText = jobPosition;
            employeeElement.AppendChild(jobElement);

            this._xmlDocument.Save(_xmlFilePath);
        }

        public List<string> GetAllEmployeesName()
        {
            this._xmlDocument.Load(_xmlFilePath);

            var names = this._xmlDocument.GetElementsByTagName("Name");

            foreach (XmlNode name in names)
            {
                this._employeesName.Add(name.InnerText);
            }

            return this._employeesName;
        }

        public void EditEmployee(int employeeId, string newName, int newId, string newJobTitle)
        {
            this._xmlDocument.Load(_xmlFilePath);

            XmlNode targetEmployeeName = this._xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Name", employeeId));
            targetEmployeeName.InnerText = newName;
            XmlNode targetEmployeeId = this._xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Id", employeeId));
            targetEmployeeId.InnerText = newId.ToString();
            XmlNode targetJobTitle = this._xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Job", employeeId));
            targetJobTitle.InnerText = newJobTitle;

            this._xmlDocument.Save(_xmlFilePath);

        }

        public void DeleteEmployee(int nodeId)
        {
            this._xmlDocument.Load(this._xmlFilePath);

            XmlNode targetNode = this._xmlDocument.SelectSingleNode(String.Format("Employees/Employee[@NodeId='{0}']", nodeId.ToString()));

            try
            {
                targetNode.ParentNode.RemoveChild(targetNode);
            }
            catch (Exception)
            {

                //
            }

            this._xmlDocument.Save(_xmlFilePath);
        }
    }
}