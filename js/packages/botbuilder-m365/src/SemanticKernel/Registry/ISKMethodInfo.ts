/**
 * @module botbuilder-m365
 */
/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { ParameterView } from "./ParameterView";

/**
 * @private
 * Extended function metadata added by decorators.
 */
export interface ISKMethodInfo extends Function {
    hasSkFunctionAttribute: boolean;
    description: string;
    parameters: ParameterView[];
}
