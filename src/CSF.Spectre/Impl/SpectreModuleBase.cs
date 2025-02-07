﻿using Spectre.Console;
using System;
using System.Threading.Tasks;
[assembly: CLSCompliant(true)]

namespace CSF.Spectre
{
    /// <inheritdoc/>
    public class SpectreModuleBase<T> : ModuleBase<T>
        where T : IContext
    {
        /// <inheritdoc/>
        public IResult Error(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{message}[/]");
            return ExecuteResult.Success();
        }

        /// <inheritdoc/>
        public Task<IResult> ErrorAsync(string message)
        {
            return Task.FromResult(Error(message));
        }

        /// <inheritdoc/>
        public IResult Info(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"[yellow]{message}[/]");
            return ExecuteResult.Success();
        }

        /// <inheritdoc/>
        public Task<IResult> InfoAsync(string message)
        {
            return Task.FromResult(Info(message));
        }

        /// <inheritdoc/>
        public IResult Success(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"[green]{message}[/]");
            return ExecuteResult.Success();
        }

        /// <inheritdoc/>
        public Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        /// <inheritdoc/>
        public override IResult Respond(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"{message}");
            return ExecuteResult.Success();
        }

        /// <inheritdoc/>
        public Task<IResult> RespondAsync(string message)
        {
            return Task.FromResult(Respond(message));
        }

        /// <summary>
        ///     Returns the input value of the requested question.
        /// </summary>
        /// <param name="question">The question to ask.</param>
        /// <returns>The <see cref="Task"/> containing the string with selected value within.</returns>
        public Task<string> AskAsync(string question)
        {
            return Task.FromResult(Ask(question));
        }

        /// <summary>
        ///     Returns the input value of the requested question.
        /// </summary>
        /// <param name="question">The question to ask.</param>
        /// <returns>The string with selected value within.</returns>
        public string Ask(string question)
        {
            return AnsiConsole.Ask<string>(question);
        }

        /// <summary>
        ///     Returns the input value of the requested prompt.
        /// </summary>
        /// <param name="prompt">The prompt to request a value for.</param>
        /// <returns>The <see cref="Task"/> containing the string with selected value within.</returns>
        [CLSCompliant(false)]
        public Task<string> PromptAsync(TextPrompt<string> prompt)
        {
            return Task.FromResult(Prompt(prompt));
        }

        /// <summary>
        ///     Returns the input value of the requested prompt.
        /// </summary>
        /// <param name="prompt">The prompt to request a value for.</param>
        /// <returns>The string with selected value within.</returns>
        [CLSCompliant(false)]
        public string Prompt(TextPrompt<string> prompt)
        {
            return AnsiConsole.Prompt(prompt);
        }

        /// <summary>
        ///     Returns the value of a selected <see cref="SelectionPrompt{T}"/> item.
        /// </summary>
        /// <param name="selection">The selection to pick from.</param>
        /// <returns>The <see cref="Task"/> containing the string with selected value within.</returns>
        [CLSCompliant(false)]
        public Task<string> SelectAsync(SelectionPrompt<string> selection)
        {
            return Task.FromResult(Select(selection));
        }

        /// <summary>
        ///     Returns the value of a selected <see cref="SelectionPrompt{T}"/> item.
        /// </summary>
        /// <param name="selection">The selection to pick from.</param>
        /// <returns>The string with selected value within.</returns>
        [CLSCompliant(false)]
        public string Select(SelectionPrompt<string> selection)
        {
            return AnsiConsole.Prompt(selection);
        }
    }
}
