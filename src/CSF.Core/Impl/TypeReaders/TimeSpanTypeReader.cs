﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CSF
{
    internal class TimeSpanTypeReader : TypeReader<TimeSpan>
    {
        private readonly IReadOnlyDictionary<string, Func<string, TimeSpan>> _callback;
        private readonly Regex _regex = new(@"(\d*)\s*([a-zA-Z]*)\s*(?:and|,)?\s*", RegexOptions.Compiled);

        public TimeSpanTypeReader()
        {
            _callback = new Dictionary<string, Func<string, TimeSpan>>
            {
                ["second"] = Seconds,
                ["seconds"] = Seconds,
                ["sec"] = Seconds,
                ["s"] = Seconds,
                ["minute"] = Minutes,
                ["minutes"] = Minutes,
                ["min"] = Minutes,
                ["m"] = Minutes,
                ["hour"] = Hours,
                ["hours"] = Hours,
                ["h"] = Hours,
                ["day"] = Days,
                ["days"] = Days,
                ["d"] = Days,
                ["week"] = Weeks,
                ["weeks"] = Weeks,
                ["w"] = Weeks,
                ["month"] = Months,
                ["months"] = Months
            };
        }

        public override ValueTask<TypeReaderResult> ReadAsync(IContext context, BaseParameter parameter, object value, CancellationToken cancellationToken)
        {
            var str = value.ToString();
            if (!TimeSpan.TryParse(str, out TimeSpan span))
            {
                str = str.ToLower().Trim();
                MatchCollection matches = _regex.Matches(str);
                if (matches.Count != 0)
                {
#pragma warning disable IDE0220 // Add explicit cast
                    foreach (Match match in matches)
                        if (_callback.TryGetValue(match.Groups[2].Value, out var result))
                            span += result(match.Groups[1].Value);
#pragma warning restore IDE0220 // Add explicit cast
                }
                else
                    return Error($"The provided value is no timespan. Expected {typeof(TimeSpan).Name}, got: '{str}'. At: '{parameter.Name}'");
            }

            return Success(span);
        }

        private static TimeSpan Seconds(string match)
            => new(0, 0, int.Parse(match));

        private static TimeSpan Minutes(string match)
            => new(0, int.Parse(match), 0);

        private static TimeSpan Hours(string match)
            => new(int.Parse(match), 0, 0);

        private static TimeSpan Days(string match)
            => new(int.Parse(match), 0, 0, 0);

        private static TimeSpan Weeks(string match)
            => new((int.Parse(match) * 7), 0, 0, 0);

        private static TimeSpan Months(string match)
            => new(((int)(int.Parse(match) * 30.437)), 0, 0, 0);
    }
}
