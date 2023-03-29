
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;

XmlDocument doc = new XmlDocument();
doc.Load("C:\\test\\EsmaSample.xml");
Validate(doc);

var lei = doc.GetElementsByTagName("LEI");
if (lei != null)
{
    foreach (XmlNode node in lei)
    {
        node.InnerText = "“##{TokenName}##";
    }
    doc.Save("C:\\test\\ESMA_Sample_Anonymised.xml");
}

static void Validate(XmlDocument doc)
{

    XmlReaderSettings validatorSettings = new XmlReaderSettings();
    validatorSettings.Schemas.Add("urn:esma:xsd:DRAFT1auth.098.001.04", "C:\\test\\DRAFT1auth.098.001.04_1.3.0.xsd");
    validatorSettings.ValidationType = ValidationType.Schema;
    validatorSettings.ValidationEventHandler += OnValidationEvent;

    MemoryStream xmlStream = new MemoryStream();
    doc.Save(xmlStream);
    xmlStream.Position = 0;
    XmlReader validator = XmlReader.Create(xmlStream, validatorSettings);


    while (validator.Read()) { }

}

static void OnValidationEvent(object sender, ValidationEventArgs e)
{
    if (e.Severity == XmlSeverityType.Warning)
    {
        Console.Write("WARNING: ");
        Console.WriteLine(e.Message);
    }
    else if (e.Severity == XmlSeverityType.Error)
    {
        Console.Write("ERROR: ");
        Console.WriteLine(e.Message);
    }
}
