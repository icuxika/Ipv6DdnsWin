using AlibabaCloud.SDK.Alidns20150109.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tea;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace Ipv6DdnsWin
{
    public static class AlibabaCloudSDK
    {
        public static AlibabaCloud.SDK.Alidns20150109.Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = accessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // Endpoint 请参考 https://api.aliyun.com/product/Alidns
            config.Endpoint = "alidns.cn-hangzhou.aliyuncs.com";
            return new AlibabaCloud.SDK.Alidns20150109.Client(config);
        }

        public static void Execute()
        {
            // 请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_ID 和 ALIBABA_CLOUD_ACCESS_KEY_SECRET。
            // 工程代码泄露可能会导致 AccessKey 泄露，并威胁账号下所有资源的安全性。以下代码示例使用环境变量获取 AccessKey 的方式进行调用，仅供参考，建议使用更安全的 STS 方式，更多鉴权访问方式请参见：https://help.aliyun.com/document_detail/378671.html
            AlibabaCloud.SDK.Alidns20150109.Client client = CreateClient(Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_SECRET"));
            DescribeDomainRecordsResponseBody.DescribeDomainRecordsResponseBodyDomainRecords.DescribeDomainRecordsResponseBodyDomainRecordsRecord record = GetRecord(client);
            string newIp = RunShell();
            UpdateRecord(client, record, newIp);
        }

        public static DescribeDomainRecordsResponseBody.DescribeDomainRecordsResponseBodyDomainRecords.DescribeDomainRecordsResponseBodyDomainRecordsRecord GetRecord(AlibabaCloud.SDK.Alidns20150109.Client client)
        {
            DescribeDomainRecordsRequest request = new DescribeDomainRecordsRequest();
            request.DomainName = "icuxika.com";
            request.Type = "AAAA";
            DescribeDomainRecordsResponse response = client.DescribeDomainRecords(request);
            List<DescribeDomainRecordsResponseBody.DescribeDomainRecordsResponseBodyDomainRecords.DescribeDomainRecordsResponseBodyDomainRecordsRecord> records = response.Body.DomainRecords.Record;
            DescribeDomainRecordsResponseBody.DescribeDomainRecordsResponseBodyDomainRecords.DescribeDomainRecordsResponseBodyDomainRecordsRecord record = records.First();
            return record;
        }

        public static void UpdateRecord(AlibabaCloud.SDK.Alidns20150109.Client client, DescribeDomainRecordsResponseBody.DescribeDomainRecordsResponseBodyDomainRecords.DescribeDomainRecordsResponseBodyDomainRecordsRecord record, string value)
        {
            UpdateDomainRecordRequest request = new UpdateDomainRecordRequest();
            request.RecordId = record.RecordId;
            request.RR = record.RR;
            request.Type = record.Type;
            request.Value = value;
            client.UpdateDomainRecord(request);
        }

        public static string RunShell()
        {
            string newIp = "";
            // string script = File.ReadAllText(@"C:\Users\icuxika\Desktop\Scripts\test.ps1");

            // 读取 临时 IPv6 地址
            // C:\Users\用户名\getipv6.ps1
            // Set-ExecutionPolicy -ExecutionPolicy RemoteSigned
            // @(Get-NetIPInterface -AddressFamily IPv6 | Where-Object InterfaceAlias -CLike "*WSL2_External*" | Select-Object -ExpandProperty ifIndex) | ForEach-Object { Get-NetIPAddress -AddressFamily IPv6 -SuffixOrigin Random -InterfaceIndex $_ } | Where-Object IPAddress -CNotLike "fe80*" | Select-Object -ExpandProperty IPAddress
            string script = File.ReadAllText(Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "getipv6.ps1"));
            Console.WriteLine("--------------------------------");
            Console.WriteLine("当前脚本内容");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(script);
            Console.WriteLine("--------------------------------");

            var iss = InitialSessionState.CreateDefault();
            iss.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
            using (PowerShell powershell = PowerShell.Create(iss))
            {
                powershell.AddScript(Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "getipv6.ps1"));

                Console.WriteLine("开始执行脚本，获取ipv6地址");
                Console.WriteLine("--------------------------------");
                Collection<PSObject> results = powershell.Invoke();
                if (powershell.Streams.Error.Count > 0)
                {
                    foreach (ErrorRecord error in powershell.Streams.Error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                }
                else
                {
                    foreach (PSObject result in results)
                    {
                        Console.WriteLine(result.ToString());
                        newIp = result.ToString();
                    }
                }
                Console.WriteLine("--------------------------------");
            }
            return newIp;
        }
    }
}
