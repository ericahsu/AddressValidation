using System;
using System.Net;
using System.Xml.Linq;

namespace USPSApi_CLI
{
	class USPSAddressStandardization
	{
		public static void Main()
		{
			var address1 = "2335 S State";
			var address2 = "Suite 300";
			var city = "Provo";
			var state = "UT";
			var zip5 = "84604";
			var zip4 = "";
			XDocument requestDoc = new XDocument(
				new XElement("AddressValidateRequest",
					new XAttribute("USERID", "886LIGHT7477"),
					new XElement("Revision", "1"),
					new XElement("Address",
						new XAttribute("ID", "0"),
						new XElement("Address1", address1),
						new XElement("Address2", address2),
						new XElement("City", city),
						new XElement("State", state),
						new XElement("Zip5", zip5),
						new XElement("Zip4", zip4)
					)
				)
			);

			try
			{
				var url = "http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
				Console.WriteLine(url);
				var client = new WebClient();
				var response = client.DownloadString(url);

				var xdoc = XDocument.Parse(response.ToString());
				var stringxdoc = xdoc.ToString();
				var notfound = stringxdoc.Contains("Address Not Found");
				if (notfound)
                {
					Console.WriteLine("Address Not Found");
                }
				else
                {
					foreach (XElement element in xdoc.Descendants("Address"))
					{
						var raddressid = GetXMLAttribute(element, "ID");
						var raddress1 = GetXMLElement(element, "Address1");
						var raddress2 = GetXMLElement(element, "Address2");
						var rcity = GetXMLElement(element, "City");
						var rstate = GetXMLElement(element, "State");
						var rzip5 = GetXMLElement(element, "Zip5");
						var rzip4 = GetXMLElement(element, "Zip4");
						Console.WriteLine(raddress1);
						Console.WriteLine("-------------------------------");
						Console.WriteLine("Address ID:	" + raddressid);
						Console.WriteLine("Address1:	" + raddress1);
						Console.WriteLine("Address2:	" + raddress2);
						Console.WriteLine("City:		" + rcity);
						Console.WriteLine("State:		" + rstate);
						Console.WriteLine("Zip5:		" + rzip5);
						Console.WriteLine("Zip4:		" + rzip4);

					}
				}                
			}
			catch (WebException e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public static string GetXMLElement(XElement element, string name)
		{
			var el = element.Element(name);
			if (el != null)
			{
				return el.Value;
			}
			return "";
		}

		public static string GetXMLAttribute(XElement element, string name)
		{
			var el = element.Attribute(name);
			if (el != null)
			{
				return el.Value;
			}
			return "";
		}

	}
}



