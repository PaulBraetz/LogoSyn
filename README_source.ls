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
* Proof of Concept C# Parser & Interpreter

---
## **Versioning** ##

ICDF uses [Semantic Versioning 2.0.0](https://semver.org/).

---
## **Installation** ##

Download the latest release on the [GitHub Release View](https://github.com/PaulBraetz/LogoSyn/Releases).
Depending on your installation directory and system, it may be necessary to adjust the manifest file found at `Packages/Manifest.json`.
By default, it will assume packages to reside in the `C:/Program Files/LogoSyn/Packages` directory.

---
## **Compiling** ##

TODO

---
## **Document Instructions** ##

LogoSyn is currently comprised of the following application components:
* LogoSyn Commandline Interface
* Parser Packages
* Interpreter Packages

Documents are passed to a parser package by the LogoSyn application.
The parser package will then parse a document object model from the document passed. 
This model may be passed on to an interpreter package for producing the final result, e.g.: a generated markdown file.
Depending on the parser and interpreter used, arguments may be supplied in the metadata supplied at the beginning of the document file.
This metadata is formally required by the LogoSyn application in order to identify and load the parser and interpreter desired.
It includes the following datapoints for both the parser and interpreter to be used:
* Name
* Version
* Arguments
* Checksum Hash
* Checksum Algorithm Name

Parser and interpreter packages are essentially standalone applications compressed into a .lspkg file. 
This file also contains metadata on the package. These package files are indexed by a manifest file.
When attempting to compile a .ls file, the LogoSyn application may be passed filepaths pointing to manifest files.
If not supplied a manifest file via an argument, the application will attempt to locate a manifest file at `Packages/Manifest.json`.
This path will be evaluated relative to the executing assembly's location, which in most cases will be the installation directory.
It is possible to supply multiple manifest files to the application. In this case, they will be queried in the same order that they were passed.

After successfully retrieving the .lspkg file from a manifest, its integrity will be verified using the checksum hash and algorithm provided in the document metadata.
If both the parser and interpreter have successfully been retrieved and verified, they will be invoked accodringly, passing arguments defined in the metadata section to them.

Currently, there are only two packages, the `CSharpParser`and `CSharpInterpreter` packages.
*Due to the application utilizing an extensible package system, other languages and document schemas may be supported in the future.*

#### **The CSharpParser Package** ####
As of version 0.0.1, the `CSharpParser` package expects the following document elements:
* Literal Elements - intended to be printed to the resulting document as-is
* Code Elements - intended to be evaluated by the interpreter

While literal elements require no special tokens, code elements must be surrounded by curly braces like so:
```
This is a literal element.

\{
	This is a code element.
\}
```

It is possible to escape any token using a backslash like so:
```
Escaping the curlybraces: \\\{ \\\}
Escaping the backslash: \\\\
```

*Taking a look at the [{Print(SOURCE_NAME);}](https://github.com/PaulBraetz/LogoSyn/blob/master/README_source.ls) file, the recursive nature of escaping tokens becomes apparent.*

#### **The CSharpInterpreter Package** ####
As of version 0.0.1, the `CSharpInterpreter` package will embed code elements into a generated method.
Literal elements will be processed using a `Print(String)` function inside this method.
Therefore, all code elements must correctly compile when sequentially arranged inside this method body.
```
\{
	var now = DateTime.Now;
\}
```
*This code element will compile correctly.*

```
\{
	private DateTime Now => DateTime.Now;
\}
```
*This code element will not compile correctly.*

The aforementioned `Print(String)` method is in-scope, enabling the printing of computed data to the resulting document.
```
\{
	Print("I want this string printed!");
\}
```
is equivalent to writing:
```
I want this printed!
```
*Emulating a literal element using the `Print(String)` method.*

---
## **Packaging** ##

TODO

---
## **Planned Features** ##

* Interpreter Enabling Literate Programming Techniques
* Relative Manifest For Better Indexing of Packages

---
## **License** ##

This project is licensed to you under the [MIT License](https://github.com/PaulBraetz/LogoSyn/blob/master/LICENSE)

---
## **Contributors** ##

* [Paul Braetz](https://github.com/PaulBraetz/)

---
