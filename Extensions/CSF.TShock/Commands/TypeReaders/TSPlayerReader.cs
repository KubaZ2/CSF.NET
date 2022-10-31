﻿using System;
using System.Linq;
using System.Threading.Tasks;
using TShockAPI;

namespace CSF.TShock
{
    internal class TSPlayerReader : TypeReader<TSPlayer>
    {
        public override Task<TypeReaderResult> ReadAsync(IContext context, Parameter info, object value, IServiceProvider provider)
        {
            var players = TSPlayer.FindByNameOrID(value.ToString());

            if (!players.Any())
                return Task.FromResult(TypeReaderResult.FromError("No player found."));

            else if (players.Count > 1)
                return Task.FromResult(TypeReaderResult.FromError($"Multiple players found: \n{string.Join(", ", players.Select(x => x.Name))}"));

            else
                return Task.FromResult(TypeReaderResult.FromSuccess(players[0]));
        }
    }
}
