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
                File.AppendAllText(XmlFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>\r\n<Employees>\r\n</Employees>\r\n");
        }

        public void AddEmployee(int id, string jobPosition, string name)
        {
            xmlDocument.Load(XmlFilePath);

            XmlElement employeeElement = xmlDocument.CreateElement("Employee");
            xmlDocument.DocumentElement.AppendChild(employeeElement);
            XmlAttribute attribute = xmlDocument.CreateAttribute("EmployeeId");
            attribute.Value = id.ToString();
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
    }
}
