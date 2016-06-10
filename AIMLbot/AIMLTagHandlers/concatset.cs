using System;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The set element instructs the AIML interpreter to set the value of a predicate to the result 
    /// of processing the contents of the set element. The set element has a required attribute name, 
    /// which must be a valid AIML predicate name. If the predicate has not yet been defined, the AIML 
    /// interpreter should define it in memory. 
    /// 
    /// The AIML interpreter should, generically, return the result of processing the contents of the 
    /// set element. The set element must not perform any text formatting or other "normalization" on 
    /// the predicate contents when returning them. 
    /// 
    /// The AIML interpreter implementation may optionally provide a mechanism that allows the AIML 
    /// author to designate certain predicates as "return-name-when-set", which means that a set 
    /// operation using such a predicate will return the name of the predicate, rather than its 
    /// captured value. (See [9.2].) 
    /// 
    /// A set element may contain any AIML template elements.
    /// 
    /// The concatset element differs from from set that it allow to concatenate field names.
    /// Each attribute named concatname and name is concatenated with the other,
    /// eg. <concatset concatname="set_1" concatname="set_2"></concatenate> result with name "set_1set_2"
    /// There is possibility to use STAR with theri number in concatname and name element,
    /// eg. STAR, STAR_1, STAR_2, STAR_3
    /// </summary>
    public class concatset : AIMLbot.Utils.AIMLTagHandler
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
        public concatset(AIMLbot.Bot bot,
                        AIMLbot.User user,
                        AIMLbot.Utils.SubQuery query,
                        AIMLbot.Request request,
                        AIMLbot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "concatset")
            {
                if (this.bot.GlobalSettings.Count > 0)
                {
                    string concatenateName = "";

                    foreach(XmlAttribute attribute in this.templateNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == "name"
                            || attribute.Name.StartsWith("concatname"))
                        {
                            if(attribute.Value.StartsWith("STAR"))
                            {
                                Regex regex = new Regex(@"^STAR_(\d+)", RegexOptions.IgnoreCase);
                                Match match = regex.Match(attribute.Value);
                                if(match.Success && match.Groups.Count == 2)
                                {
                                    try
                                    {
                                        int starIndex = Convert.ToInt32(match.Groups[1].Value);
                                        starIndex--;
                                        if ((starIndex >= 0) & (starIndex < this.query.InputStar.Count))
                                        {
                                            concatenateName += (string)this.query.InputStar[starIndex];
                                        }
                                        else
                                        {
                                            this.bot.writeToLog("InputStar out of bounds reference caused by input: " + this.request.rawInput);
                                        }
                                    }
                                    catch
                                    {
                                        this.bot.writeToLog("Index set to non-integer value whilst processing star tag in response to the input: " + this.request.rawInput);
                                    }
                                }
                                else
                                {
                                    concatenateName += (string)this.query.InputStar[0];
                                }
                            }
                            else
                            {
                                concatenateName += attribute.Value;
                            }
                        }
                    }

                    if (this.templateNode.InnerText.Length > 0)
                    {
                        this.user.Predicates.addSetting(concatenateName, this.templateNode.InnerText);
                        return this.user.Predicates.grabSetting(concatenateName);
                    }
                    else
                    {
                        // remove the predicate
                        this.user.Predicates.removeSetting(concatenateName);
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }
    }
}
