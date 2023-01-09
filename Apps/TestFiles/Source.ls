ParserInfo.PackageName:CSharpParser
ParserInfo.PackageVersion:0.0.1
ParserInfo.Arguments:
ParserInfo.PackageHash:HPKCm6IuUmqWRStXf/fyTu639yZkoMemREspa1Q4ykQLi/V5OSP4KkNbtfkBzGU4DrcbN45Cbc/qb+6qcW4DBg==
ParserInfo.PackageHash.Algorithm.Name:sha512

InterpreterInfo.PackageName:CSharpInterpreter
InterpreterInfo.PackageVersion:0.0.1
InterpreterInfo.Arguments:
InterpreterInfo.PackageHash:DRNZZM/zoG/x4HKsvMQX+uvA4NXqk3t5dPM1okhMIOfy1f18Gb9pe2CNLnSCw62hJZuZQ32y07CqQV3zCsIAHQ==
InterpreterInfo.PackageHash.Algorithm.Name:sha512

{
    //DocumentContext.IntermediateResultFilePath = @"Document.cs";
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
