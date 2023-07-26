// See https://aka.ms/new-console-template for more information

// https://learn.microsoft.com/zh-cn/dotnet/core/tutorials/top-level-templates
// https://learn.microsoft.com/zh-cn/dotnet/csharp/whats-new/tutorials/top-level-statements?source=recommendations

using Ipv6DdnsWin;
Console.WriteLine("Hello, World!");
AlibabaCloudSDK.RunShell();

// 命令行发布，当前目录为 Ipv6DdnsWin\Ipv6DdnsWin
// dotnet publish --self-contained -c Release -o .\bin\release\net7.0\publish\win-x64 -r win10-x64 -p:PublishSingleFile=false -p:PublishReadyToRun=false -p:PublishTrimmed=false

// Visual Studio发布
// 生成-发布选定内容，修改 FolderProfile.pubxml 文件中 <RuntimeIdentifier>win10-x64</RuntimeIdentifier> 的值为 win10-x64，不勾选文件发布选项中任意一项