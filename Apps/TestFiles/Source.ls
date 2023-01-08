ParserInfo.PackageName:CSharpParser
ParserInfo.PackageVersion:0.0.1
ParserInfo.Arguments:
ParserInfo.PackageHash:/T/GqLoAvJ4AmMoaALARnHeuv0eOFesFm8+ulyH6Bj57vme5AfWwJpnPwQWMr5OD12OQ/k0uP2u9c8t+C8+/2g==
ParserInfo.PackageHash.Algorithm.Name:sha512

InterpreterInfo.PackageName:CSharpInterpreter
InterpreterInfo.PackageVersion:0.0.1
InterpreterInfo.Arguments:
InterpreterInfo.PackageHash:Q4Clrwl3MCrC63g0TvHaMNHo1KFIkOxev+EJ9qZOrUg5WSnVyLACwqkRC7wOKcfZPUJkrPpFQL0YDZoiR6bbLw==
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
