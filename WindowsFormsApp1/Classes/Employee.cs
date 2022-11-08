using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DataCollector
{
    internal class Employee
    {
        private readonly string _xmlFilePath = "Employee.xml";
        private readonly XmlDocument _xmlDocument = new XmlDocument();
        private int _id;
        private string _name;
        private int _nodeId;
        private string _jobTitle;
        public List<string> Exceptions = new List<string>();

        private List<string> _employeesName = new List<string>();
        private string[,] _employeesData;

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

        public int Id
        {
            get
            {
                return this._id;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string JobTitle 
        {
            get
            {
                return this._jobTitle;
            }
        }

        public int NodeID
        {
            get
            {
                return this._nodeId;
            }
        }

        public void AddEmployee(string id, string jobPosition, string name)
        {
            try
            {
                if (this.Exceptions.Any())
                    CheckAndDeleteExceptions();

                if (!int.TryParse(id, out int cardId))
                    this.Exceptions.Add("Nem adtad meg a dolgozó kértyaszámát. Kérlek ellenőrizd!");

                if (String.IsNullOrWhiteSpace(jobPosition))
                    this.Exceptions.Add("Nem adtad meg a dolgozó munkakörét. Kérlek ellenőrizd!");

                if (String.IsNullOrWhiteSpace(name))
                    this.Exceptions.Add("Nem adtad meg a dolgozó nevét. Kérlek ellenőrizd!");

                if (this.Exceptions.Any())
                    throw new Exception("Hiba!");

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
                idElement.InnerText = cardId.ToString();
                employeeElement.AppendChild(idElement);

                XmlElement nameElement = this._xmlDocument.CreateElement("Name");
                nameElement.InnerText = name;
                employeeElement.AppendChild(nameElement);

                XmlElement jobElement = this._xmlDocument.CreateElement("Job");
                jobElement.InnerText = jobPosition;
                employeeElement.AppendChild(jobElement);

                this._xmlDocument.Save(_xmlFilePath);
            }
            catch (Exception)
            {

                this.Exceptions.Add("Hiba történt a dolgozó felvételekor.");
                throw new Exception("Hiba!");
            }
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

        public Array GetAllEmployeesData()
        {
            this._xmlDocument.Load(_xmlFilePath);


            var names = this._xmlDocument.GetElementsByTagName("Name");
            var ids = this._xmlDocument.GetElementsByTagName("Id");
            var jobs = this._xmlDocument.GetElementsByTagName("Job");
            var NodeIds = this._xmlDocument.DocumentElement.ChildNodes;

            var firstArraySize = names.Count;
            var secondArraySize = 4;


            if (firstArraySize == 0)
                this.Exceptions.Add("Nincs megjeleníthető elem. Először vegyél fel egy dolgozót.");

            if (Exceptions.Any())
                throw new Exception("Hiba!");

            this._employeesData = new string[firstArraySize, secondArraySize];

            for (var i = 0; firstArraySize > i; i++)
            {
                for (var j = 0; j < secondArraySize; j++)
                {

                    switch (j)
                    {
                        case 0:
                            this._employeesData[i, j] = NodeIds[i].Attributes[0].Value;
                            break;
                        case 1:
                            this._employeesData[i, j] = ids[i].InnerText;
                            break;
                        case 2:
                            this._employeesData[i, j] = names[i].InnerText;
                            break;
                        case 3:
                            this._employeesData[i, j] = jobs[i].InnerText;
                            break;
                    }
                }
            }

            return this._employeesData;

        }

        public void GetEmployeeDataByIndex(int index)
        {
            var allData = GetAllEmployeesData();

            this._id = Convert.ToInt32(allData.GetValue(index, 1));
            this._name = (string)allData.GetValue(index, 2);
            this._nodeId = Convert.ToInt32(allData.GetValue(index, 0));
            this._jobTitle = (string)allData.GetValue(index, 3);
        }

        public void EditEmployee(int nodeID, string newName, int newId, string newJobTitle)
        {
            try
            {
                this._xmlDocument.Load(_xmlFilePath);

                XmlNode targetEmployeeName = this._xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Name", nodeID));
                targetEmployeeName.InnerText = newName;
                XmlNode targetEmployeeId = this._xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Id", nodeID));
                targetEmployeeId.InnerText = newId.ToString();
                XmlNode targetJobTitle = this._xmlDocument.SelectSingleNode(string.Format("Employees/Employee[@NodeId='{0}']/Job", nodeID));
                targetJobTitle.InnerText = newJobTitle;

                this._xmlDocument.Save(_xmlFilePath);
            }
            catch (Exception)
            {
                this.Exceptions.Add("Hiba történt az xml fájl írásakor.");
                throw new Exception("Hiba!");
            }
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

        private void CheckAndDeleteExceptions()
        {
            var exceptionCount = this.Exceptions.Count();

            this.Exceptions.RemoveRange(0, exceptionCount);
        }
    }
}