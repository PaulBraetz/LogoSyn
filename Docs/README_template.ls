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
	//DocumentContext.IntermediateResultFilePath = @"Intermediate.cs";
    DocumentContext.TargetFilePath = @"README.md";

	var template = new
	{
		Source = new FileInfo("README_template.ls"),
		Name = "My Project",
		PrintGenerateDateInfo = true,
		Description = "TODO\n",
		Features = new[]{
			(name: "Feature 1", shortDescription:"Short Description for Feature 1" ),
			(name: "Feature 2", shortDescription:"Short Description for Feature 2" )
		}
	};
	var sections = new []
		{
			(name: "Description", depth: 1),
			(name: "Table of Contents", depth: 1),
			(name: "Features", depth: 1)
		}.Concat(template.Features.Select(f=>(f.name, depth: 2)))
		.Concat(new (String name, Int32 depth)[]
		{

		})
		.ToDictionary(t=>t.name, t=>(t.name, t.depth, id: Guid.NewGuid()));    

    void printSectionHeader(String name)
    {
        var section = sections[name];
        var tokens = String.Concat(Enumerable.Repeat('#', section.depth+1));
        var line = $"{tokens} {section.name} <a name=\"{section.id}\"></a>\n\n";
        Print(line);
    }
    
    void printTableOfContents()
    {
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

	Print($"# {template.Name}\n");
	
	if(template.PrintGenerateDateInfo)
	{
		var timeStamp = DateTimeOffset.UtcNow.ToString(System.Globalization.CultureInfo.GetCultureInfo("de-De"));
		Print($"*Note: this readme was generated on {timeStamp} using {template.Source.Name}*\n\n");
	}
	
	printSectionHeader("Description");
	Print(template.Description);

	printSectionHeader("Table of Contents");
	printTableOfContents();

	printSectionHeader("Features");
	foreach(var feature in template.Features)
	{
		Print($"* {feature.shortDescription}\n");
	}
	foreach(var feature in template.Features)
	{
		printSectionHeader(feature.name);
		Print("TODO\n");
	}
}