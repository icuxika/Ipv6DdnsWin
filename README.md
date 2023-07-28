# Ipv6DdnsWin

## 环境
.NET 7.0 

## 环境变量
在系统中添加`ALIBABA_CLOUD_ACCESS_KEY_ID`与`ALIBABA_CLOUD_ACCESS_KEY_SECRET`两个环境变量用来访问阿里云，可以以`管理员模式`在`PowerShell`中执行以下命令来在命令行添加环境变量
```
[System.Environment]::SetEnvironmentVariable('ALIBABA_CLOUD_ACCESS_KEY_ID','','Machine')
[System.Environment]::SetEnvironmentVariable('ALIBABA_CLOUD_ACCESS_KEY_SECRET','','Machine')
```

## 运行所需脚本
在用户目录下创建文件`getipv6.ps1`，示例内容如下
```
@(Get-NetIPInterface -AddressFamily IPv6 | Where-Object InterfaceAlias -CLike "*WSL2_External*" | Select-Object -ExpandProperty ifIndex) | ForEach-Object { Get-NetIPAddress -AddressFamily IPv6 -SuffixOrigin Random -InterfaceIndex $_ } | Where-Object IPAddress -CNotLike "fe80*" | Select-Object -ExpandProperty IPAddress
```
此脚本输出结果为本机的`临时 IPv6 地址`
```
❯ pwd

Path
----
C:\Users\icuxika

❯ .\getipv6.ps1
240e:3a1:86a:a3f0:ac3b:f9d1:5974:6680
```

## 包（通过`工具-NuGet 包管理器-管理解决方案的 NuGet 程序包`）
- `AlibabaCloud.SDK.Alidns20150109` 3.0.7
- `Microsoft.PowerShell.SDK` 7.3.6

## 发布
```
cd .\Ipv6DdnsWin\
```
> 从解决方案根目录进入项目目录下
```
dotnet publish -c Release -o .\bin\Release\net7.0\publish\win-x64 -r win10-x64 --sc -p:PublishSingleFile=false -p:PublishReadyToRun=false -p:PublishTrimmed=false
```
> 通过 Visual Studio 提供的图形界面工具发布时，需要手动修改`FolderProfile.pubxml`中`<RuntimeIdentifier>`的值为`win10-x64`，否则可能会遇到`Microsoft.Management.Infrastructure.dll`的错误

## 命令行构建（位于解决方案根目录下，与上面的发布命令所基于的路径不同）
```
dotnet restore .\Ipv6DdnsWin\Ipv6DdnsWin.csproj
dotnet publish .\Ipv6DdnsWin\Ipv6DdnsWin.csproj -c Release -o .\Ipv6DdnsWin\bin\Release\net7.0\publish\win-x64 -r win10-x64 --sc -p:PublishSingleFile=false -p:PublishReadyToRun=false -p:PublishTrimmed=false
Compress-Archive .\Ipv6DdnsWin\bin\Release\net7.0\publish\win-x64\* .\Ipv6DdnsWin\bin\Release\net7.0\publish\win-x64.zip
```