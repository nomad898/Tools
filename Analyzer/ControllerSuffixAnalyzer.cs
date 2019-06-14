using StyleCop;
using StyleCop.CSharp;
using System;
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
            if (element is Class controllerElement)
            {
                var controllerSuffix = "Controller";
                var isController = controllerElement.BaseClass == controllerSuffix;
                var shouldShowWarning = isController 
                    && !controllerElement.Name.EndsWith(controllerSuffix, StringComparison.Ordinal);
                if (shouldShowWarning)
                {
                    this.AddViolation(element, RULE_NAME);
                }
                return false;
            }
            return true;
        }
    }
}
