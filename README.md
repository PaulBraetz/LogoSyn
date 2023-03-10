# LogoSyn #

LogoSyn is a document format specification that intersperses textual with programatic elements.

Due its extensible nature, it may be used to:
* Generate Documents
* Implement Literate Programming Techniques
* Enhance Documentation

*Note: this readme was generated on 11.01.2023 22:35:14 using README_source.ls*

---
## **Features** ##

* Discriminator-based .ls Document Parsing
* Generalized DOM Abstraction
* Extensible Configurations
* Prerelease Default Parser
* Proof of Concept C# Interpreter

---
## **Versioning** ##

LogoSyn uses [Semantic Versioning 2.0.0](https://semver.org/).

---
## **Installation** ##

Download the latest release on the [GitHub Release View](https://github.com/PaulBraetz/LogoSyn/Releases).
Depending on your installation directory and system, it may be necessary to adjust the manifest file found at `Packages/Manifest.json`.
By default, it will assume packages to reside in the `C:/Program Files/LogoSyn/Packages` directory.

Make sure that your path variables include the installation directory, enabling you to directly use the `lswatch` and `logosyn` commands.

*Unfortunately, currently only the Windows ("Portable") release is working as intended.*

---
## **Quick Start Using Visual Studio Code** ##

1. Copy the sample [source file](https://github.com/PaulBraetz/LogoSyn/blob/master/Apps/TestFiles/Source.ls) to a directory of your choice
2. Open the directory in VSCode
3. Open source file in a VSCode panel
4. Open terminal and navigate to working directory
5. Start lswatcher, providing your source file as the argument
6. Save your source file
7. Open the generated markdown file
8. Pin a preview of the generated file on the right panel
9. Done!

[QuickStart.webm](https://user-images.githubusercontent.com/95855091/211635304-a5cd8129-c1d9-40b5-a5e7-44086efb4605.webm)

---
## **Compiling** ##

Using the `logosyn` application, it is possible to compile documents and manage packages:

### Arguments ###

Using the command `> logosyn .\Source.ls` is equivalent to using the command `> logosyn -c -cs .\Source.ls`.
#### *Compilation* ####

`-c` or `--compile`:
```
When set, source documents may be compiled.
```

`-cs` or `--compileSource`:
```
Supplies the .ls source file path
```

`-cm` or `--compileManifests`:
```
Supplies a comma-delimited list of local manifest files. If none are provided, an attempt will be made to locate a file at "Packages" in the executing directory and named "Manifest" and use it.
```

`-ct` or `--compileTarget`:
```
Supplies the target file to which to write the compilation result.
```

`-ce` or `--compileError`:
```
Supplies the file to which to write error details, should any arise.
```



#### *Packaging* ####

`-p` or `--package`:
```
When set, packages may be managed.
```

`-ph` or `--packageHeadless`:
```
When set, packages may be managed headlessly.
```

`-pi` or `--packageInfo`:
```
Supplies the package information file for creating a package.
```

`-pd` or `--packageDirectory`:
```
Supplies the package data directory containing all files required for creating a package.
```

`-pt` or `--packageTarget`:
```
Supplies the target directory for the package created.
```

`-pm` or `--packageManifest`:
```
Supplies the manifest file for registering the package created.
```



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

{
	This is a code element.
}
```

It is possible to escape any token using a backslash like so:
```
Escaping the curlybraces: \{ \}
Escaping the backslash: \\
```

*Taking a look at the [README_source.ls](https://github.com/PaulBraetz/LogoSyn/blob/master/README_source.ls) file, the recursive nature of escaping tokens becomes apparent.*

#### **The CSharpInterpreter Package** ####
As of version 0.0.1, the `CSharpInterpreter` package will embed code elements into a generated method.
Literal elements will be processed using a `Print(String)` function inside this method.
Therefore, all code elements must correctly compile when sequentially arranged inside this method body.
```
{
	var now = DateTime.Now;
}
```
*This code element will compile correctly.*

```
{
	private DateTime Now => DateTime.Now;
}
```
*This code element will not compile correctly.*

The aforementioned `Print(String)` method is in-scope, enabling the printing of computed data to the resulting document.
```
{
	Print("I want this string printed!");
}
```
is equivalent to writing:
```
I want this string printed!
```
*Emulating a literal element using the `Print(String)` method.*

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