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
	//Data
	//DocumentContext.IntermediateResultFilePath = @"Intermediate.cs";
    DocumentContext.TargetFilePath = @"README.md";

	var template = new
	{
		Source = new FileInfo("README_template.ls"),
		License = (name: "MIT", url: "LICENSE"),
		Name = "My Project",
		PrintGenerateDateInfo = true,
		Description = ((Action)description),
		Contributors = new[]
			{
				(name: "Paul Brätz", url: "https://github.com/PaulBraetz/")
			},
		Features = new (string name, Action content)[]
			{
				(name: "Templated Sections", content:feature1),
				(name: "Feature 2", content:feature2)
			}
	};
	var featureSections = template.Features.Select(f=>(f.name, depth: 2, content:f.content));
	IDictionary<string, (string name, int depth, Action content, Guid id)> sections = null;
	sections = new (string name, int depth, Action content)[]
	{
		(name: "Description", depth: 1, content: description),
		(name: "Table of Contents", depth: 1, content: tableOfContents),
		(name: "Features", depth: 1, content: features)
	}.Concat(featureSections)
	.Concat(new (string name, int depth, Action content)[]
	{
		(name: "Installation", depth: 1, content:installation),
		(name: "License", depth: 1, content:license),
		(name: "Contributors", depth: 1, content:contributors)
	})
	.ToDictionary(t=>t.name, t=>(t.name, t.depth, t.content, id: Guid.NewGuid()));
	//End Data

	//Functions
    void sectionHeader(String name){
        var section = sections[name];
        var tokens = String.Concat(Enumerable.Repeat('#', section.depth+1));
        var line = $"{tokens} {section.name} <a name=\"{section.id}\"></a>\n\n";
        Print(line);
    }    
    void tableOfContents(){
        var sectionStack = new Stack<Int32>();

        foreach(var section in sections.Values)
        {
            if(sectionStack.Count < section.depth)
            {
                sectionStack.Push(1);
            }else if(sectionStack.Count == section.depth)
            {
                var siblingIndex = sectionStack.Pop();
                sectionStack.Push(siblingIndex + 1);
            }else
            {
                while(sectionStack.Count > section.depth)
                {
                    sectionStack.Pop();
                    var lastOnThisDepth = sectionStack.Pop();
                    sectionStack.Push(lastOnThisDepth + 1);
                }
            }

            var indentation = section.depth > 1 ? "\t" : String.Empty;
            var index = sectionStack.Select(s=>$"{s}.").Aggregate((s1, s2) =>s2+s1);
            var line = $"{indentation}{index} [{section.name}](#{section.id})\n\n";
            Print(line);
        }
    }
	void license(){
		Print($"This software is licensed to you under the [{template.License.name}]({template.License.url}) license.\n");
	}
	void contributors(){
		foreach(var contributor in template.Contributors)
		{
			Print($"* [{contributor.name}]({contributor.url} \"Go to Profile\")\n");
		}
	}
	void features(){
		foreach(var feature in template.Features)
		{
			Print($"* {feature.name}\n");
		}
	}
	//End Functions

	//Generation
	Print($"# {template.Name}\n");
	
	if(template.PrintGenerateDateInfo){
		var timeStamp = DateTimeOffset.UtcNow.ToString(System.Globalization.CultureInfo.GetCultureInfo("de-De"));
		Print($"*Note: this readme was generated on {timeStamp} using {template.Source.Name}*\n\n");
	}
	
	foreach(var section in sections.Values){
		sectionHeader(section.name);
		section.content?.Invoke();
	}
	//End Generation

	//Custom Content
	void installation() =>
}TODO: installation
{

	void description() =>
}
This is a readme file template, intended for use in conjunction with logosyn.
{
	void feature1() =>
}
Use templated sections like this one.
{
	void feature2() =>
}
TODO: Long Description for Feature 2

