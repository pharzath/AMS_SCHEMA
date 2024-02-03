using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.ComponentModel;
using Olive;

namespace AMS_SCHEMA.Class;


public static class ExtensionMethods
{
    public static string ToDescriptionString(this Enum val)
    {
        var attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString())
            ?.GetCustomAttributes(typeof(DescriptionAttribute), false)!;
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }


    public static SyntaxTrivia? MethodIsInRegion(this string filePath, string methodName, string[] parameterTypes)
    {
        // Load the C# syntax tree from the file
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

        // Get the root of the syntax tree
        var root = syntaxTree.GetRoot();

        // Find the method declaration
        var methodDeclaration = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
        .FirstOrDefault(m =>
                m.Identifier.ValueText == methodName &&
                m.ParameterList.Parameters.Count == parameterTypes.Length &&
                m.ParameterList.Parameters.Select(p => p.Type.ToString()).SequenceEqual(parameterTypes));

        if (methodDeclaration == null) return null;

        // Deep search for SyntaxTrivia within the root node
        var regions = root.DescendantTrivia(node => true, true)
            .Where(x => x.IsKind(SyntaxKind.RegionDirectiveTrivia) || x.IsKind(SyntaxKind.EndRegionDirectiveTrivia));

        SyntaxTrivia? rg = null;
        // Output the trivia
        foreach (var region in regions)
        {
            if (region.IsKind(SyntaxKind.RegionDirectiveTrivia) && methodDeclaration.SpanStart > region.SpanStart)
                rg = region;
            else if (region.IsKind(SyntaxKind.EndRegionDirectiveTrivia) && rg != null && methodDeclaration.SpanStart < region.SpanStart)
                return rg;
            else
                rg = null;
        }

        return null;
    }

    public static async Task AddMethodToFile(this string filePath, string methodSourceCode)
    {
        var methodSyntax = SyntaxFactory.ParseMemberDeclaration(methodSourceCode) as MethodDeclarationSyntax;
        var syntaxTree = CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync(filePath));
        var root = await syntaxTree.GetRootAsync();
        // Deep search for SyntaxTrivia within the root node
        var regions = root.DescendantTrivia(node => true,true)
            .Where(x=>x.IsKind(SyntaxKind.RegionDirectiveTrivia) || x.IsKind(SyntaxKind.EndRegionDirectiveTrivia));

        // Output the trivia
        foreach (var region in regions)
        {
            Console.WriteLine($"Trivia: {region} - {region.Kind()}");

            Console.WriteLine($"Span: {region.SpanStart}");
        }

        //
        // // Print the names of the region directives
        // string code = @"#region NAME
        //                     // Some code here
        //                 #endregion";
        // var syntaxTree1 = CSharpSyntaxTree.ParseText(code);
        // SyntaxNode root1 = await syntaxTree1.GetRootAsync();
        //     
        // RegionDirectiveTriviaSyntax? regionDirective = root.DescendantNodes()
        //     .OfType<RegionDirectiveTriviaSyntax>()
        //     .FirstOrDefault();
        //     
        // SyntaxNode updatedRoot = root1.ReplaceNode(regionDirective, new[] { regionDirective, node });
        //
        //
        //
        // var regionName = @"myTest";
        //
        // var lead = SyntaxFactory.Trivia(SyntaxFactory.RegionDirectiveTrivia(true)
        //     .WithEndOfDirectiveToken(
        //         SyntaxFactory.Token(SyntaxFactory.TriviaList(
        //                 SyntaxFactory.PreprocessingMessage(regionName)),
        //             SyntaxKind.EndOfDirectiveToken,
        //             SyntaxFactory.TriviaList())
        //     )
        // );
        //     
        // var trail = SyntaxFactory.Trivia(SyntaxFactory.EndRegionDirectiveTrivia(true));
        // var region = SyntaxFactory.TriviaList(lead, trail);
        //
        //
        //
        // var x = region.ToFullString();
        return;

        var classDeclaration = root?.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        if (methodSyntax != null)
        {
            var updatedClassDeclaration = classDeclaration?.AddMembers(methodSyntax);
            if (classDeclaration != null && updatedClassDeclaration != null)
            {
                var updatedSyntaxRoot = root?.ReplaceNode(classDeclaration, updatedClassDeclaration);
                if (updatedSyntaxRoot != null)
                {
                    await File.WriteAllTextAsync(filePath, updatedSyntaxRoot.ToFullString());
                }
            }
        }
    }

    public static string ToJoining(this char seperator, params object?[] list)
    {
        return string.Join(seperator, list.Where(x=>x != null && x.ToString().HasValue()));
    }

    public static string JOIN(this IEnumerable<string> list, string seperator)
    {
        return string.Join(seperator, list.Where(x=>x.HasValue()));
    }

    public static string Fmt(this string frmt, string value)
    {
        return string.Format(frmt, value);
    }


}