﻿using CSF;

var framework = new CommandFramework<ImplementationFactory>();

while (true)
{
    var context = new CommandContext(Console.ReadLine()!);

    var result = await framework.ExecuteCommandAsync(context);

    if (!result.IsSuccess)
        framework.Logger.Write(new Log(LogLevel.Error, result.ErrorMessage, result.Exception));
}