using System;
using System.Linq;
using System.Xml.Linq;

namespace CPO_Xml1
{
    class Program
    {
        static void Main(string[] args)
        {
            Linq19();
            Console.WriteLine("====================================================================");
            Linq32();
            Console.WriteLine("====================================================================");
            Linq45();
            Console.WriteLine("====================================================================");
            Linq58();
            Console.WriteLine("====================================================================");
            Linq71();
            Console.WriteLine("====================================================================");
            Linq84();

            Console.ReadLine();
        }

        static void Linq19()
        {
            XDocument XDoc = XDocument.Load(@"C:\Users\SRuza\Desktop\XML\linq19.xml");

            var mas = XDoc.Root.Elements().GroupBy(n => n.Name, j => j.Descendants().Where(n=>n.Descendants().Count() == n.Ancestors().Reverse().ElementAt(1).Descendants().Max(p=>p.Descendants().Count())).Select(n=> new { name = n.Name, count = n.Descendants().Count()}));
            foreach (var firstLevelEl in mas)
            {
                Console.WriteLine("Элемент 1-го уровня: " + firstLevelEl.Key.LocalName);
                var col = firstLevelEl.SelectMany(n=>n);
                if (col.Count() != 0)
                {
                    Console.WriteLine("Максимальное кол-во потомков у дочерних элементов: " + col.First().count);
                    Console.Write("Имена узлов: ");
                    foreach (var des in col)
                    {
                        Console.Write(des.name + ",");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Максимальное кол-во потомков у дочерних элементов: -1");
                    Console.WriteLine("Имена узлов: no child");
                }
                Console.WriteLine();
            }

        }
        static void Linq32()
        {
            XDocument XDoc = XDocument.Load(@"C:\Users\SRuza\Desktop\XML\linq32.xml");
            Console.Write("Введите S1: ");
            //string s1 = Console.ReadLine();
            string s1 = "company";
            Console.Write("Введите S2: ");
            //string s2 = Console.ReadLine();
            string s2 = "pra";

            var els2 = XDoc.Root.Elements().SelectMany(t => t.Elements()).Where(t => t.Name.LocalName == s1);
            foreach (var el in els2)
            {
                XElement newEl = new XElement(s2, el.Attributes().LastOrDefault(), el.Elements().FirstOrDefault());
                el.AddBeforeSelf(newEl);
            }
            XDoc.Save(@"C:\Users\SRuza\Desktop\XML\linq32.xml");
            Console.WriteLine(XDoc);
        }
        static void Linq45()
        {
            XDocument XDoc = XDocument.Load(@"C:\Users\SRuza\Desktop\XML\linq45.xml");
            Console.WriteLine(XDoc);
            var allElAttr = from t in XDoc.Descendants()
                            where t.Attributes().Count() > 0
                            select t;
            foreach (var el in allElAttr)
            {
                if (el.Attributes().Count() % 2 == 0)
                    el.AddFirst(new XElement("odd-attr-count", true));
                else
                    el.AddFirst(new XElement("odd-attr-count", false));
            }
            XDoc.Save(@"C:\Users\SRuza\Desktop\XML\linq45.xml");
            Console.WriteLine(XDoc);
        }
        static void Linq58()
        {
            XDocument XDoc = XDocument.Load(@"C:\Users\SRuza\Desktop\XML\linq58.xml");
            Console.Write("Введите строку S: ");
            string s = Console.ReadLine();
            XNamespace ns = s;


            XDoc.Root.Add(new XAttribute(XNamespace.Xmlns + "node", s));
            foreach (var el in XDoc.Root.Elements())
            {
                el.Add(new XAttribute(ns + "count", el.DescendantNodes().Count()),
                    new XAttribute(XNamespace.Xml + "count", el.Descendants().Count()));
            }
            XDoc.Save(@"C:\Users\SRuza\Desktop\XML\linq58.xml");
        }
        static void Linq71()
        {
            XDocument XDoc = XDocument.Load(@"C:\Users\SRuza\Desktop\XML\linq71.xml");
            XDocument XDocNew = new XDocument(new XElement("petrol"));

            var col = (from t in XDoc.Root.Elements()
                       select new
                       {
                           Brand = t.Element("info").Attribute("brand").Value,
                           Price = Convert.ToInt32(t.Element("info").Attribute("price").Value),
                           Street = t.Attribute("street").Value,
                           Company = t.Attribute("company").Value
                       }).OrderByDescending(n => n.Brand).ThenByDescending(n => n.Price).ThenBy(n => n.Street).ThenBy(n => n.Company);

            foreach (var el in col)
            {
                if (XDocNew.Root.Element("b" + el.Brand) == null)
                {
                    XDocNew.Root.Add(new XElement("b" + el.Brand));
                }
                if (XDocNew.Root.Element("b" + el.Brand).Element("p" + el.Price) == null)
                {
                    XDocNew.Root.Element("b" + el.Brand).Add(new XElement("p" + el.Price));
                }
                XDocNew.Root.Element("b" + el.Brand).Element("p" + el.Price).Add(new XElement("info", new XAttribute("street", el.Street), new XAttribute("company", el.Company)));
            }
            XDocNew.Save(@"C:\Users\SRuza\Desktop\XML\linq71-new.xml");
        }
        static void Linq84()
        {
            XDocument XDoc = XDocument.Load(@"C:\Users\SRuza\Desktop\XML\linq84.xml");
            XDocument XDocNew = new XDocument(new XElement("marks"));

            var col = (from t in XDoc.Root.Elements()
                       select new
                       {
                           Name = t.Attribute("name").Value,
                           Class = Convert.ToInt32(t.Attribute("class").Value),
                           Subject = t.Element("subject").Value,
                           Mark = Convert.ToInt32(t.Element("mark").Value)
                       }).OrderBy(n => n.Class).ThenBy(n => n.Name).ThenBy(n => n.Subject).ThenBy(n => n.Mark);
            foreach (var el in col)
            {
                XDocNew.Root.Add(new XElement("class" + el.Class, el.Mark, new XAttribute("name", el.Name), new XAttribute("subject", el.Subject)));
            }
            XDocNew.Save(@"C:\Users\SRuza\Desktop\XML\linq84-new.xml");
        }
    }
}
