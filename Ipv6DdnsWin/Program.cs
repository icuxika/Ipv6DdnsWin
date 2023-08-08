// See https://aka.ms/new-console-template for more information

// https://learn.microsoft.com/zh-cn/dotnet/core/tutorials/top-level-templates
// https://learn.microsoft.com/zh-cn/dotnet/csharp/whats-new/tutorials/top-level-statements?source=recommendations

using Ipv6DdnsWin;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Windows.Win32;

var builder = new ConfigurationBuilder();
builder.AddCommandLine(args);
var config = builder.Build();

var domainName = config["domainName"];

if (domainName == null)
{
    // 为了执行应用时不会打开控制台窗口，OutputType 设置为 WinExe，但是为了出错时能够弹出控制台打印一些消息
    // 使用 https://github.com/microsoft/CsWin32 来调用一些 控制台函数 https://learn.microsoft.com/zh-cn/windows/console/allocconsole
    // 函数名需要在文件 NativeMethods.txt 中声明
    PInvoke.AllocConsole();
    Console.WriteLine("Ipv6DdnsWin.exe --domainName=xxx.com，运行程序需要指定domainName的值");
    Console.ReadKey();
    PInvoke.FreeConsole();
}
else
{
    // 由于OutputType 设置为 WinExe，AlibabaCloudSDK.cs 中的 Console.WriteLine 目前不生效
    AlibabaCloudSDK.Execute(domainName);
}


// Visual Studio发布
// 生成-发布选定内容，修改 FolderProfile.pubxml 文件中 <RuntimeIdentifier>win10-x64</RuntimeIdentifier> 的值为 win10-x64，不勾选文件发布选项中任意一项