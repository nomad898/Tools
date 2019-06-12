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
    public class ControllerAccessAnalyzer : SourceAnalyzer
    {
        private const string RULE_NAME = "ControllerAccessRule";

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
