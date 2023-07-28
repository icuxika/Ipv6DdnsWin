// See https://aka.ms/new-console-template for more information

// https://learn.microsoft.com/zh-cn/dotnet/core/tutorials/top-level-templates
// https://learn.microsoft.com/zh-cn/dotnet/csharp/whats-new/tutorials/top-level-statements?source=recommendations

using Ipv6DdnsWin;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder();
builder.AddCommandLine(args);
var config = builder.Build();

var domainName = config["domainName"];
Console.WriteLine(domainName);

if (domainName == null)
{
    Console.Error.WriteLine("Ipv6DdnsWin.exe --domainName=xxx.com，运行程序需要指定domainName的值");
}
else
{
    AlibabaCloudSDK.Execute(domainName);
}


// Visual Studio发布
// 生成-发布选定内容，修改 FolderProfile.pubxml 文件中 <RuntimeIdentifier>win10-x64</RuntimeIdentifier> 的值为 win10-x64，不勾选文件发布选项中任意一项