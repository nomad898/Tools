using StyleCop;
using StyleCop.CSharp;
using System.Web.Mvc;

namespace Analyzer
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ControllerSuffixAnalyzer : SourceAnalyzer
    {
        private const string RULE_NAME = "ControllerSuffixRule";

        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = (CsDocument)document;
            csDocument.WalkDocument(
                new CodeWalkerElementVisitor<object>(this.VisitElement));
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            var controllerElement = element as Class;
            if (controllerElement == null)
            {
                return true;
            }
            if (!controllerElement.Name.EndsWith("Controller", System.StringComparison.Ordinal))
            {
                this.AddViolation(element, RULE_NAME);
            }
            return false;
        }
    }
}
