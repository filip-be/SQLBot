using System;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The get element tells the AIML interpreter that it should substitute the contents of a 
    /// predicate, if that predicate has a value defined. If the predicate has no value defined, 
    /// the AIML interpreter should substitute the empty string "". 
    /// 
    /// The AIML interpreter implementation may optionally provide a mechanism that allows the 
    /// AIML author to designate default values for certain predicates (see [9.3.]). 
    /// 
    /// The get element must not perform any text formatting or other "normalization" on the predicate
    /// contents when returning them. 
    /// 
    /// The get element has a required name attribute that identifies the predicate with an AIML 
    /// predicate name. 
    /// 
    /// The get element does not have any content.
    /// 
    /// The concatget element differs from from get that it allow to concatenate field names.
    /// Each attribute named concatname and name is concatenated with the other,
    /// eg. <concatget concatname1="set_1" concatname1="set_2"></concatenate> result with name "set_1set_2"
    /// There is possibility to use STAR with theri number in concatname and name element,
    /// eg. STAR, STAR_1, STAR_2, STAR_3
    /// </summary>
    public class concatget : AIMLbot.Utils.AIMLTagHandler
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
        public concatget(AIMLbot.Bot bot,
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
            if (this.templateNode.Name.ToLower() == "concatget")
            {
                if (this.bot.GlobalSettings.Count > 0)
                {
                    string concatenateName = "";

                    foreach (XmlAttribute attribute in this.templateNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == "name"
                            || attribute.Name.ToLower().StartsWith("concatname"))
                        {
                            if (attribute.Value.StartsWith("STAR"))
                            {
                                Regex regex = new Regex(@"^STAR_(\d+)", RegexOptions.IgnoreCase);
                                Match match = regex.Match(attribute.Value);
                                if (match.Success && match.Groups.Count == 2)
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

                    if (concatenateName.Length > 0)
                    {
                        return this.user.Predicates.grabSetting(concatenateName);
                    }
                }
            }
            return string.Empty;
        }
    }
}
