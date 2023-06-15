﻿
namespace Microsoft.Bot.Builder.M365.AI.Prompt
{
    public interface IPromptManager<TState> where TState : TurnState
    {
        /// <summary>
        /// Adds a custom function <paramref name="name"/> to the prompt manager.
        /// </summary>
        /// <remarks>
        /// Functions can be used with a prompt template using a syntax of `{{name}}`. Functions
        /// arguments are not currently supported.
        /// </remarks>
        /// <param name="name">The name of the function.</param>
        /// <param name="promptFunction">Delegate to return on name match.</param>
        /// <param name="allowOverrides">Whether to allow overriding an existing fuction.</param>
        /// <returns>The prompt manager for chaining.</returns>
        IPromptManager<TState> AddFunction(string name, PromptFunction<TState> promptFunction, bool allowOverrides = false);

        /// <summary>
        /// Adds a prompt template to the prompt manager.
        /// </summary>
        /// <param name="name">Name of the prompt template.</param>
        /// <param name="promptTemplate">Prompt template to add.</param>
        /// <returns>The prompt manager for chaining.</returns>
        IPromptManager<TState> AddPromptTemplate(string name, PromptTemplate promptTemplate);

        /// <summary>
        /// Invokes a function by <paramref name="name"/>.
        /// </summary>
        /// <param name="turnContext">Current application turn context.</param>
        /// <param name="turnState">Current turn state.</param>
        /// <param name="name">Name of the function to invoke.</param>
        /// <returns>The result returned by the function for insertion into a prompt.</returns>
        Task<string> InvokeFunction(TurnContext turnContext, TState turnState, string name);

        /// <summary>
        /// Loads a named prompt template from the filesystem.
        /// </summary>
        /// <param name="name">Name of the template to load.</param>
        /// <returns>The loaded prompt template.</returns>
        PromptTemplate LoadPromptTemplate(string name);
    }
}
