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
                var inNamespace = entityElement.FullNamespaceName.Contains("Entities");               
                if (inNamespace)
                {
                    var isPublic = entityElement.AccessModifier == AccessModifierType.Public;
                    if (isPublic)
                    {
                        var props = entityElement.ChildElements;
                        var res = props
                            .Where(p => (p as Property).Name == "Name" && (p as Property).AccessModifier == AccessModifierType.Public)
                            .Where(p => (p as Property).Name == "Id" && (p as Property).AccessModifier == AccessModifierType.Public);
                        if (!res.Any())
                        {
                           
                        }
                    }
                    this.AddViolation(element, RULE_NAME);
                    return true;
                }
            }
            return false;
        }
    }
}
