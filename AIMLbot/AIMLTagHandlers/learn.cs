using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The learn element instructs the AIML interpreter to retrieve a resource specified by a URI, 
    /// and to process its AIML object contents.
    /// 
    /// EDIT: Added input as string with possible use of EVAL tag
    /// </summary>
    public class learn : AIMLbot.Utils.AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public learn(AIMLbot.Bot bot,
                        AIMLbot.User user,
                        AIMLbot.Utils.SubQuery query,
                        AIMLbot.Request request,
                        AIMLbot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        private string evaluate(XmlDocument xmlDoc)
        {
            string outputXML = "";
            foreach (XmlNode node in xmlDoc.ChildNodes)
            {
                outputXML += evaluate(node);
            }
            return outputXML;
        }

        private string evaluate(XmlNode xmlNode)
        {
            string outputXML = "";

            if (xmlNode.Name == "eval")
            {   // Evaluate
                foreach(XmlNode node in xmlNode.ChildNodes)
                {
                    outputXML += bot.processNode(node, query, request, result, user);
                }
                return outputXML;
            }
            else if(xmlNode !=null)
            {
                outputXML += string.Format("<{0}", xmlNode.Name);

                if (xmlNode.Attributes != null)
                {
                    foreach (XmlAttribute attr in xmlNode.Attributes)
                    {
                        //outputXML += attr.InnerXml;
                        if (attr.Name != null)
                            outputXML += string.Format(" {0}", attr.Name);
                        if (attr.Name != null && attr.Value != null)
                            outputXML += string.Format("=\"{0}\"", attr.Value);
                    }
                }
                outputXML += ">";

                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    outputXML += evaluate(node);
                }
                //outputXML += xmlNode.InnerText;
                outputXML += string.Format("</{0}>", xmlNode.Name);
                
                return outputXML;
            }
            else
            {
                return "";
            }
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "learn")
            {
                if (this.templateNode.InnerText.Length > 0)
                {
                    XmlDocument doc = null;
                    bool learned = false;
                    // currently only AIML files in the local filesystem can be referenced
                    // ToDo: Network HTTP and web service based learning
                    string path = this.templateNode.InnerText;
                    bool isPathToFile = true;
                    try
                    {
                        FileInfo fi = new FileInfo(path);
                        if (fi.Exists)
                        {
                            doc = new XmlDocument();
                            try
                            {
                                doc.Load(path);
                                this.bot.loadAIMLFromXML(doc, path);
                                learned = true;
                            }
                            catch
                            {
                                this.bot.writeToLog("ERROR! Attempted (but failed) to <learn> some new AIML from the following URI: " + path);
                            }
                        }
                        else
                        {
                            isPathToFile = false;
                        }
                    }
                    catch (Exception)
                    {
                        isPathToFile = false;
                    }

                    if (!isPathToFile)
                    {
                        try
                        {
                            doc = new XmlDocument();
                            doc.LoadXml(this.templateNode.InnerXml);
                            // Valid XML string inside
                            // Evalaute!
                            string xml = evaluate(doc);
                            // Load evaluated AIML
                            doc.LoadXml(xml);
                            this.bot.loadAIMLFromXML(doc, "IN-APP <learn> AIML tag");
                            learned = true;
                        }
                        catch (Exception)
                        {
                            return "ERROR";
                        }
                    }

                    if (learned && this.templateNode.Attributes.Count > 1 && doc != null)
                    {
                        try
                        {
                            string directory = null;
                            string fileName = null;
                            for (int attrNum = 0; attrNum < this.templateNode.Attributes.Count; attrNum++)
                            {
                                XmlAttribute Attribute = this.templateNode.Attributes[attrNum];
                                if (Attribute.Name == "directory")
                                {
                                    directory = Attribute.Value;
                                }
                                else if (Attribute.Name == "filename")
                                {
                                    if (Attribute.Value == "PATTERN")
                                    {
                                        XmlNode node = doc.SelectSingleNode("/aiml/category/pattern");
                                        if (node != null)
                                        {
                                            fileName = node.InnerText;
                                            fileName = Regex.Replace(fileName, @"\r\n\s+", "");
                                            fileName += ".aiml";
                                        }
                                    }
                                    else
                                    {
                                        fileName = Attribute.Value;
                                    }
                                }
                            }
                            if (directory != null && fileName != null)
                            {
                                string fName = string.Format("{0}{1}{2}", RemoveIllegalChars(directory), Path.DirectorySeparatorChar, RemoveIllegalChars(fileName));
                                
                                FileInfo fileInfo = new FileInfo(fName);
                                if (!fileInfo.Exists)
                                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                                doc.Save(fName);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return string.Empty;
        }

        private string RemoveIllegalChars(string text)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(text, "");
        }
    }
}
