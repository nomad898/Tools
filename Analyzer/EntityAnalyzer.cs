using StyleCop;
using StyleCop.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    [SourceAnalyzer(typeof(CsParser))]
    public class EntityAnalyzer : SourceAnalyzer
    {
        private const string RULE_NAME = "EntityRule";

        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = (CsDocument)document;
            csDocument.WalkDocument(
                new CodeWalkerElementVisitor<object>(this.VisitElement));
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            if (element is Class entityElement)
            {
                string debugNamespace = string.Empty;
                string debugProps = string.Empty;
                var namespaceName = entityElement.FullNamespaceName;
                var inNamespace = namespaceName.Contains("Entities");
                debugNamespace = namespaceName;
                var containsId = false;
                var containsName = false;
                if (inNamespace)
                {
                    var isPublic = entityElement.AccessModifier == AccessModifierType.Public;
                    if (isPublic)
                    {                       
                        var props = entityElement.ChildElements;

                        foreach (var prop in props)
                        {
                            var shouldShowError = true;
                            if (prop is Property)
                            {
                                debugProps += prop.Name + " - ";
                                if (prop.Name == "property Name")
                                {
                                    containsName = true;
                                }
                                else if (prop.Name == "property Id")
                                {
                                    containsId = true;
                                }
                                foreach (var attr in prop.Attributes)
                                {
                                    if (attr.Text == "[DataContract]")
                                    {
                                        shouldShowError = false;
                                        break;
                                    }
                                }
                                if (shouldShowError)
                                {
                                    this.AddViolation(element, RULE_NAME, debugNamespace + "- datacontract", debugProps);
                                }
                            }
                        }
                        if (!containsName && !containsId)
                        {
                            this.AddViolation(element, RULE_NAME, debugNamespace + "- contains", debugProps);
                        }
                    }
                    else
                    {
                        this.AddViolation(element, RULE_NAME, debugNamespace + "- not public", debugProps);
                    }
                }
                return false;
            }
            return true;
        }
    }
}
