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
                var className = "Controller";
                var isController = controllerElement.BaseClass == className;
                if (isController)
                {
                    var shouldShowError = true;
                    var attrs = element.Attributes;
                    foreach (var attr in attrs)
                    {
                        if (attr is StyleCop.CSharp.Attribute a)
                        {
                            if (a.Text == "Authorize")
                            {
                                shouldShowError = false;
                                break;
                            }
                        }
                    }
                    if (shouldShowError)
                    {
                        foreach (var method in element.ChildElements)
                        {
                            if (method is StyleCop.CSharp.Method m)
                            {
                                var methodAttr = m.Attributes;
                                var shouldShowMethodError = true;
                                foreach (var attr in methodAttr)
                                {
                                    if (attr is StyleCop.CSharp.Attribute a)
                                    {
                                        if (a.Text == "Authorize")
                                        {
                                            shouldShowMethodError = false;
                                            break;
                                        }
                                    }
                                }
                                if (shouldShowMethodError)
                                {
                                    this.AddViolation(element, RULE_NAME);
                                    shouldShowError = false;
                                }
                            }
                        }
                    }
                    if (shouldShowError)
                    {
                        this.AddViolation(element, RULE_NAME);
                    }
                }
                return false;
            }
            return true;
        }
    }
}
