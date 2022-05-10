using System.Collections.Generic;
using System.IO;
using SourceGenerator.Entities;

namespace SourceGenerator.Parsers;

public interface IParser
{
    IReadOnlyCollection<FileInfo> ControllerFiles { get; }
    IReadOnlyCollection<FileInfo> EntityFiles { get; }

    IParser CalculateFilesToParse(FileInfo directoryPath);
    IReadOnlyList<ControllerDeclaration> ParseControllerDeclarations(List<FileInfo> fileInfos);
    IReadOnlyList<EntityDeclaration> ParseEntityDeclarations(List<FileInfo> fileInfos);
    List<CodeScope> GetScopes(string[] code);
}