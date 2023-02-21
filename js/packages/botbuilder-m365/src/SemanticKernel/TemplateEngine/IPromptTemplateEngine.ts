/**
 * @module botbuilder-m365
 */
/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { SKContext, ContextVariables } from '../Orchestration';
import { Block } from './Blocks';

export interface IPromptTemplateEngine
{
    /**
     * Given a prompt template string, extract all the blocks (text, variables, function calls)
     * @param templateText Prompt template (see skprompt.txt files)
     * @param validate Whether to validate the blocks syntax, or just return the blocks found, which could contain invalid code
     * @returns A list of all the blocks, ie the template tokenized in text, variables and function calls
     */
    extractBlocks(templateText?: string, validate?: boolean): Block[];

    /**
     * Given a prompt template, replace the variables with their values and execute the functions replacing their
     * reference with the function result.
     * @param templateText Prompt template (see skprompt.txt files)
     * @param executionContext Access into the current kernel execution context
     * @returns The prompt template ready to be used for an AI request
     */
    render(templateText: string, executionContext: SKContext): Promise<string>;
    
    /**
     * Given a a list of blocks render each block and compose the final result
     * @param blocks Template blocks generated by ExtractBlocks
     * @param executionContext Access into the current kernel execution context
     * @returns The prompt template ready to be used for an AI request
     */
    render(blocks: Block[], executionContext: SKContext): Promise<string>;

    /**
     * Given a list of blocks, render the Variable Blocks, replacing placeholders with the actual value in memory
     * @param blocks List of blocks, typically all the blocks found in a template
     * @param variables Container of all the temporary variables known to the kernel
     * @returns An updated list of blocks where Variable Blocks have rendered to Text Blocks
     */
    renderVariables(blocks: Block[], variables?: ContextVariables): Block[];

    /**
     * Given a list of blocks, render the Code Blocks, executing the functions and replacing placeholders with the functions result
     * @param blocks List of blocks, typically all the blocks found in a template
     * @param executionContext Access into the current kernel execution context
     * @returns An updated list of blocks where Code Blocks have rendered to Text Blocks
     */
    renderCode(blocks: Block[], executionContext: SKContext): Promise<Block[]>;
}