﻿#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFEnhance.Localization;
using static ASFEnhance.Utils;

namespace ASFEnhance.Event
{
    internal static class Command
    {
        /// <summary>
        /// 获取SIM4贴纸 10.19 - ?
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        internal static async Task<string?> ResponseEvent(Bot bot)
        {
            if (!bot.IsConnectedAndLoggedOn)
            {
                return bot.FormatBotResponse(Strings.BotNotConnected);
            }

            string token = await WebRequest.FetchEventToken(bot).ConfigureAwait(false);
            if (token == null)
            {
                return bot.FormatBotResponse(Langs.NetworkError);
            }

            uint[] door_indexs = { 1, 3, 4, 5, 2 };

            foreach (uint index in door_indexs)
            {
                await WebRequest.DoEventTask(bot, token, index).ConfigureAwait(false);
            }

            return bot.FormatBotResponse("Done!");
        }

        /// <summary>
        /// 获取活动徽章 (多个Bot)
        /// </summary>
        /// <param name="botNames"></param>
        /// <param name="gruopID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<string?> ResponseEvent(string botNames)
        {
            if (string.IsNullOrEmpty(botNames))
            {
                throw new ArgumentNullException(nameof(botNames));
            }

            HashSet<Bot>? bots = Bot.GetBots(botNames);

            if ((bots == null) || (bots.Count == 0))
            {
                return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            IList<string?> results = await Utilities.InParallel(bots.Select(bot => ResponseEvent(bot))).ConfigureAwait(false);

            List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

            return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
        }
    }
}
