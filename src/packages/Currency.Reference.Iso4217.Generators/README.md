### [How to use source generator](#how-use-sourceGenerator)

- In the project __Currency.Reference.Iso4217__ add the following commands (uncomment) in the project:
```json lines
  <PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\Generated</CompilerGeneratedFilesOutputPath>
  </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="..\..\packages\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.csproj"
                          OutputItemType="Analyzer"
                          ReferenceOutputAssembly="false" />
    </ItemGroup>
    <Target Name="CleanGeneratedFilesBeforeBuild" BeforeTargets="CoreCompile">
        <RemoveDir Directories="$(CompilerGeneratedFilesOutputPath)" />
        <Message Text="Removed generated files from $(CompilerGeneratedFilesOutputPath)" Importance="Low" />
    </Target>
    <ItemGroup>
        <Compile Remove="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\**\*.g.cs" />
        <None Include="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\**\*.g.cs" />
    </ItemGroup>
    <Target Name="CopyGeneratedFiles" AfterTargets="CoreCompile">
        <Copy SourceFiles="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.CurrencyCodeGenerator\CurrencyCode.g.cs"
              DestinationFiles="CurrencyCode.cs"
              SkipUnchangedFiles="false" />
        <Copy SourceFiles="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.LocalDatabaseGenerator\LocalDatabase.g.cs"
              DestinationFiles="LocalDatabase.cs"
              SkipUnchangedFiles="false" />
        <Message Text="Copied generated files into repository." Importance="Low" />
    </Target>
```
- Save changes
- Rebuild the solution
- Review changes in ``CurrencyCode`` and ``LocalDatabase``files 
- Remove added commands from the project
- Save changes again
- Rebuild the solution again

### How to update currencies
- Download an XML file https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml and convert to JSON
- Save JSON data in the file ``Content\list-original-currencies.json``
- Download an XML file https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-three.xml and convert to JSON
- Save JSON data in the file ``Content\list-historical-currencies.json``
- Corrected names in the file ``Content\list-replacement-currency-names.json`` 
- Use command from section <a id="how-use-sourceGenerato">How to use source generator</a>