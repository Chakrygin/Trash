using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MySourceGenerator
{
    public class MySyntaxReceiver : ISyntaxReceiver
    {
        public List<PropertyDeclarationSyntax> Properties { get; } =
            new List<PropertyDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is PropertyDeclarationSyntax propertySyntax)
            {
                var isDependency = propertySyntax.AttributeLists
                    .SelectMany(x => x.Attributes)
                    .Any(x => x.Name.ToString() == "Dependency");

                if (isDependency)
                {
                    Properties.Add(propertySyntax);
                }
            }
        }
    }
}