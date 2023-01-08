ParserInfo.PackageName:CSharpParser
ParserInfo.PackageVersion:0.0.1
ParserInfo.Arguments:
ParserInfo.PackageHash:/vIX6UeP/VK9bkvHl9WnpQ==
ParserInfo.PackageHash.Algorithm.Name:md5

InterpreterInfo.PackageName:CSharpInterpreter
InterpreterInfo.PackageVersion:0.0.1
InterpreterInfo.Arguments:
InterpreterInfo.PackageHash:sKNuClGw26LrIFcTW2fs/w==
InterpreterInfo.PackageHash.Algorithm.Name:md5

{
	const String SOURCE_NAME = "README_source.ls";
    var source = new FileInfo(SOURCE_NAME);
    DocumentContext.TargetFilePath = @"README.md";
}# LogoSyn #

LogoSyn is a document format specification that intersperses textual with programatic elements.

Due its extensible nature, it may be used to:
* Generate Documents
* Implement Literate Programming Techniques
* Enhance Documentation

*Note: this readme was generated on {Print(DateTime.Now.ToString(System.Globalization.CultureInfo.GetCultureInfo("de-De")));} using {Print(SOURCE_NAME);}*

---
## **Features** ##

* Discriminator-based .icdf Document Parsing
* Generalized DOM Abstraction
* Extensible Configurations
* Proof of Concept Simplified C# Interpreter

---
## **Versioning** ##

ICDF uses [Semantic Versioning 2.0.0](https://semver.org/).

---

## **Planned Features** ##

* Default Interpreter Enabling Literate Programming

---

## **License** ##

This project is licensed to you under the [MIT License](https://github.com/PaulBraetz/LogoSyn/blob/master/LICENSE)

---

## **Contributors** ##

* [Paul Braetz](https://github.com/PaulBraetz/)

---
