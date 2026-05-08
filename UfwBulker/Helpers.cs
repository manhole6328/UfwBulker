using System.Net;
using System.Text;

namespace UfwBulker;

internal class Helpers
{
    public static string CreateAddRuleCommand(IPNetwork iPNetwork, string appName, string comment)
    {
        return $"ufw allow from {iPNetwork} to any app {appName} comment '{comment}'";
    }

    public static string CreateRemoveRuleCommand(IPNetwork iPNetwork, string appName, string comment)
    {
        return $"ufw delete allow from {iPNetwork} to any app {appName} comment '{comment}'";
    }

    public static string FormatBashScript(IEnumerable<SubnetWithComment> subnetsWithComments, string appName, Func<IPNetwork, string, string, string> command)
    {
        StringBuilder sb = new();
        sb.AppendLine("#!/bin/bash");
        sb.AppendLine("set -e");

        foreach (SubnetWithComment item in subnetsWithComments)
        {
            sb.AppendLine(command(item.Subnet, appName, item.Comment));
        }

        return sb.ToString();
    }
}
