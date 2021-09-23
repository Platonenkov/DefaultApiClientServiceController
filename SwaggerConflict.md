### SwaggerGeneratorException: Conflicting method/path combination
neet to resolve conflicts in configuration ResolveConflictingActions
```c#
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiServer", Version = "v1" });
        c.ResolveConflictingActions(apiDescriptions =>
        {
            var descriptions = apiDescriptions as ApiDescription[] ?? apiDescriptions.ToArray();
            var first = descriptions.First(); // build relative to the 1st method
            var parameters = descriptions.SelectMany(d => d.ParameterDescriptions).ToList();

            first.ParameterDescriptions.Clear();
            // add parameters and make them optional
            foreach (var parameter in parameters)
                if (first.ParameterDescriptions.All(x => x.Name != parameter.Name))
                {
                    first.ParameterDescriptions.Add(new ApiParameterDescription
                    {
                        ModelMetadata = parameter.ModelMetadata,
                        Name = parameter.Name,
                        ParameterDescriptor = parameter.ParameterDescriptor,
                        Source = parameter.Source,
                        IsRequired = false,
                        DefaultValue = null
                    });
                }
            return first;
        });
    });
```
