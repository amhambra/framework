﻿import * as React from 'react'
import { Lite, Entity } from '@framework/Signum.Entities'
import { OmniboxMessage } from './Signum.Entities.Omnibox'
import { OmniboxResult, OmniboxMatch, OmniboxProvider, specialActions } from './OmniboxClient'
import { QueryToken, FilterOperation, FindOptions, FilterOption } from '@framework/FindOptions'
import * as Navigator from '@framework/Navigator'
import * as Finder from '@framework/Finder'



export default class SpecialOmniboxProvider extends OmniboxProvider<SpecialOmniboxResult>
{
    getProviderName() {
        return "SpecialOmniboxResult";
    }

    icon() {
        return this.coloredIcon("cog", "limegreen");
    }

    renderItem(result: SpecialOmniboxResult): React.ReactChild[] {

        const array: React.ReactChild[] = [];

        array.push(this.icon());

        array.push("!");

        this.renderMatch(result.match, array)
     
        return array;
    }

    navigateTo(result: SpecialOmniboxResult) {
        return specialActions[result.key].onClick();
    }

    toString(result: SpecialOmniboxResult) {
        return "!" + result.key;
    }
}

interface SpecialOmniboxResult extends OmniboxResult {
    match: OmniboxMatch;
    key: string;
}
