### [How to use source generator](#how-use-sourceGenerator)

- In the project __Currency.Reference.Iso4217__ add the following commands (uncomment) in the project:
```json lines
    <PropertyGroup>
      <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
      <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\Generated</CompilerGeneratedFilesOutputPath>
    </PropertyGroup>
        
    <ItemGroup>
      <ProjectReference Include="..\..\packages\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false"/>
    </ItemGroup>

    <Target Name="CopyGeneratedCurrencyCode" AfterTargets="Build">
        <Copy SourceFiles="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.CurrencyCodeGenerator\CurrencyCode.g.cs" DestinationFiles="CurrencyCode.cs" SkipUnchangedFiles="true" />
    </Target>
    <Target Name="CopyGeneratedLocalDatabase" AfterTargets="Build">
        <Copy SourceFiles="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.LocalDatabaseGenerator\LocalDatabase.g.cs" DestinationFiles="LocalDatabase.cs" SkipUnchangedFiles="true" />
    </Target>
    <Target Name="DeleteGeneratedCurrencyCode" AfterTargets="CopyGeneratedCurrencyCode">
        <ItemGroup>
            <GeneratedFiles Include="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.CurrencyCodeGenerator\CurrencyCode.g.cs" />
        </ItemGroup>
        <Message Text="Deleting generator files: %(GeneratedFiles.Identity)" Importance="Low" Condition="'@(GeneratedFiles)' != ''" />
        <Delete Files="@(GeneratedFiles)" />
    </Target>
    <Target Name="DeleteGeneratedLocalDatabase" AfterTargets="CopyGeneratedLocalDatabase">
        <ItemGroup>
            <GeneratedFiles Include="$(CompilerGeneratedFilesOutputPath)\Currency.Reference.Iso4217.Generators\Currency.Reference.Iso4217.Generators.LocalDatabaseGenerator\LocalDatabase.g.cs" />
        </ItemGroup>
        <Message Text="Deleting generator files: %(GeneratedFiles.Identity)" Importance="Low" Condition="'@(GeneratedFiles)' != ''" />
        <Delete Files="@(GeneratedFiles)" />
    </Target>
```
- Save changes
- Remove data from ``CurrencyCode`` and ``LocalDatabase``files
- Rebuild the solution
- Review changes in ``CurrencyCode`` and ``LocalDatabase``files 
- Remove added commands from the project
- Remove folder `$(BaseIntermediateOutputPath)\Generated`
- Save changes again

### How to update currencies
- Download an XML file https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml and convert to JSON
- Save JSON data in the file ``Content\list-original-currencies.json``
- Download an XML file https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-three.xml and convert to JSON
- Save JSON data in the file ``Content\list-historical-currencies.json``
- Corrected names in the file ``Content\list-replacement-currency-names.json`` 
- Use command from section <a id="how-use-sourceGenerato">How to use source generator</a>