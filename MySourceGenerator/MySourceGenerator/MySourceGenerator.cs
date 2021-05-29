using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace MySourceGenerator
{
    [Generator]
    public class MySourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // SpinWait.SpinUntil(() => Debugger.IsAttached);
            
            context.RegisterForSyntaxNotifications(
                () => new MySyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // SpinWait.SpinUntil(() => Debugger.IsAttached);

            AddDependencyAttribute(context);

            if (context.SyntaxReceiver is MySyntaxReceiver receiver)
            {
                foreach (var group in receiver.Properties.GroupBy(x => x.SyntaxTree))
                {
                    var syntaxTree = group.Key;
                    var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                    var properties = group
                        .Select(x => (IPropertySymbol) semanticModel.GetDeclaredSymbol(x))
                        .ToList();

                    var sb = new StringBuilder();

                    foreach (var typeGroup in properties.GroupBy(x => x.ContainingType))
                    {
                        sb.AppendLine($@"
namespace {typeGroup.Key.ContainingNamespace.Name}
{{
    partial class {typeGroup.Key.Name}
    {{
        public {typeGroup.Key.Name}(");
                        foreach (var property in typeGroup)
                        {
                            sb.AppendLine(
                                $@"            {property.Type} {property.Name},");
                        }

                        sb.AppendLine(
                            $@"            System.IServiceProvider serviceProvider)");
                        sb.AppendLine(
                            $@"        {{");

                        foreach (var property in typeGroup)
                        {
                            sb.AppendLine(
                                $@"                this.{property.Name} = {property.Name};");
                        }

                        sb.AppendLine($@"
        }}
    }}
}}");
                    }

                    context.AddSource(Path.GetFileNameWithoutExtension(syntaxTree.FilePath), sb.ToString());
                }
            }
        }

        private void AddDependencyAttribute(GeneratorExecutionContext context)
        {
            context.AddSource("DependencyAttribute", $@"
using System;
using System.Diagnostics;

namespace {context.Compilation.AssemblyName}
{{
    [Conditional(""NEVER"")]
    [AttributeUsage(Targets, Inherited = false, AllowMultiple = false)]
    internal sealed class DependencyAttribute : Attribute
    {{
        private const AttributeTargets Targets =
            AttributeTargets.Field | AttributeTargets.Property;
    }}
}}
");
        }
    }
}