ParserInfo.PackageName:CSharpParser
ParserInfo.PackageVersion:0.0.2
ParserInfo.Arguments:-o 12
ParserInfo.PackageHash:JRe2yB6NnLPN6YAWz2+c7A==
ParserInfo.PackageHash.Algorithm.Name:md5

InterpreterInfo.PackageName:CSharpInterpreter
InterpreterInfo.PackageVersion:0.0.2
InterpreterInfo.Arguments:
InterpreterInfo.PackageHash:o8JCmV9PGS9TRmwh1GE4TA==
InterpreterInfo.PackageHash.Algorithm.Name:md5

{
    DocumentContext.IntermediateResultFilePath = @"Document.cs";
    DocumentContext.TargetFilePath = @"Target.md";
}# Test Source Document #

This document is intended to test the Interstitial Document Format.

This paragraph is a literal, and will thus be written verbatim into the target document.

It is possible to print any string value to the resulting document by calling the `Print(String)` method in a code element.
For example like so:
```cs
\{
    Print(DateTime.Now.ToString());	
\}
```

When compiled, this programmatic element will produce the following output: {Print(DateTime.Now.ToString());}

It is possible to escape element tokens (like '\{' or '\}') by using a backslash like so (escaping the backslash works the same way):
```
\\\{
	Print(@"Here is a backslash: \\");
\\\}
```
