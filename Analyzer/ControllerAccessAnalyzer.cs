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
            if (element is Class controllerElement)
            {
                string v = string.Empty;
                var className = "Controller";
                var isController = controllerElement.BaseClass == className;
                if (isController)
                {
                    var shouldShowError = true;
                    var attrs = element.Attributes;
                    foreach (var attr in attrs)
                    {
                        if (attr.Text == "[Authorize]")
                        {
                            shouldShowError = false;
                            break;
                        }
                    }
                    if (shouldShowError)
                    {
                        foreach (var method in element.ChildElements)
                        {
                            if (method is Method)
                            {
                                var shouldShowMethodError = true;
                                var methodAttr = method.Attributes;
                                foreach (var attr in methodAttr)
                                {
                                    v += attr.Text + " - ";
                                    if (attr.Text == "[Authorize]")
                                    {
                                        shouldShowMethodError = false;
                                        shouldShowError = false;
                                        break;
                                    }
                                }
                                if (shouldShowMethodError)
                                {
                                    this.AddViolation(element, RULE_NAME, v);
                                }
                            }
                        }
                    }
                    if (shouldShowError)
                    {
                        this.AddViolation(element, RULE_NAME, v);
                    }
                }
                return false;
            }
            return true;
        }
    }
}
