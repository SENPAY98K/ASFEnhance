using ArchiSteamFarm.Helpers.Json;
using ASFEnhance.Data;
using System.Text.RegularExpressions;

namespace ASFEnhance.Curator;

internal static class HtmlParser
{
    /// <summary>
    /// 解析关注的鉴赏家页
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static HashSet<CuratorItem>? ParseCuratorListPage(AjaxGetCuratorsResponse? response)
    {
        if (response == null)
        {
            return null;
        }

        Match match = RegexUtils.MatchCuratorPayload().Match(response.Html);

        if (match.Success)
        {
            try
            {
                string jsonStr = match.Groups[1].Value;
                var data = jsonStr.ToJsonObject<HashSet<CuratorItem>>();
                return data;
            }
            catch (Exception ex)
            {
                ASFLogger.LogGenericError(ex.Message);
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
