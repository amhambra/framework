import * as React from 'react'
import {
  FilterOptionParsed, QueryDescription, QueryToken, SubTokensOptions,
  isList, isFilterGroupOptionParsed, isCheckBox, getFilterGroupUnifiedFilterType
} from '../FindOptions'
import { ValueLine, FormGroup } from '../Lines'
import { Binding, IsByAll, tryGetTypeInfos, toLuxonFormat } from '../Reflection'
import { TypeContext } from '../TypeContext'
import "./FilterBuilder.css"
import { ComplexConditionSyntax, createFilterValueControl, FilterTextArea, MultiEntity, MultiValue } from './FilterBuilder';
import { SearchMessage } from '../Signum.Entities';
import { classes } from '../Globals';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { ValueLineController } from '../Lines/ValueLine'

interface PinnedFilterBuilderProps {
  filterOptions: FilterOptionParsed[];
  onFiltersChanged?: (filters: FilterOptionParsed[], avoidSearch?: boolean) => void;
  pinnedFilterVisible?: (fo: FilterOptionParsed) => boolean 
  onSearch?: () => void;
  showSearchButton?: boolean;
  extraSmall?: boolean;
  colClassName?: string;
}
export default function PinnedFilterBuilder(p: PinnedFilterBuilderProps) {

  const timeoutWriteText = React.useRef<number | null>(null);

  var allPinned = getAllPinned(p.filterOptions).filter(fop => p.pinnedFilterVisible == null || p.pinnedFilterVisible(fop));

  if (allPinned.length == 0)
    return null;

  return (
    <div onKeyUp={handleFiltersKeyUp }>
      <div className={classes("row", p.extraSmall ? "" : "mt-3 mb-3")}>
        {
          allPinned
            .groupBy(fo => (fo.pinned!.column ?? 0).toString())
            .orderBy(gr => parseInt(gr.key))
            .map(gr => <div key={gr.key} className={p.colClassName ?? (gr.elements.length > 4 ? "col-sm-2" : "col-sm-3")}>
              {gr.elements.orderBy(a => a.pinned!.row ?? 0).map((f, i) => <div key={i}>{renderValue(f)}</div>)}
            </div>)
        }
      </div>
      {p.showSearchButton &&
        <button className={classes("sf-query-button sf-search btn btn-primary")} onClick={() => p.onSearch && p.onSearch()} title="Enter">
          <FontAwesomeIcon icon={"magnifying-glass"} />&nbsp;{SearchMessage.Search.niceToString()}
        </button>}

    </div>
  );

  function renderValue(filter: FilterOptionParsed) {

    const f = filter;
    const readOnly = f.frozen;
    var label = f.pinned!.label || (f.token?.queryTokenType == "AnyOrAll" || f.token?.queryTokenType == "Element" ? f.token.parent?.niceName : f.token?.niceName);

    if (f.pinned && (isCheckBox(f.pinned.active))) {
      return (
        <div className="checkbox mt-4">
          <label>
            <input type="checkbox" className="form-check-input me-1" checked={f.pinned.active == "Checkbox_Checked" || f.pinned.active == "NotCheckbox_Checked"} readOnly={readOnly} onChange={() => {
              f.pinned!.active =
                f.pinned!.active == "Checkbox_Checked" ? "Checkbox_Unchecked" :
                  f.pinned!.active == "Checkbox_Unchecked" ? "Checkbox_Checked" :
                    f.pinned!.active == "NotCheckbox_Checked" ? "NotCheckbox_Unchecked" :
                      f.pinned!.active == "NotCheckbox_Unchecked" ? "NotCheckbox_Checked" : undefined!;
              p.onFiltersChanged && p.onFiltersChanged(p.filterOptions);
            }} />{label}</label>
        </div>
      );
    }

    const ctx = new TypeContext<any>(undefined, { formGroupStyle: "Basic", readOnly: readOnly, formSize: p.extraSmall ? "xs" : "sm" }, undefined as any, Binding.create(f, a => a.value));

    if (isFilterGroupOptionParsed(f)) {

      if (f.filters.some(sf => !isFilterGroupOptionParsed(sf) && (sf.operation == "ComplexCondition" || sf.operation == "FreeText"))) {
        var isComplex = f.filters.some(sf => !isFilterGroupOptionParsed(sf) && sf.operation == "ComplexCondition");
        return <FilterTextArea ctx={ctx}
          isComplex={isComplex}
          onChange={(() => handleValueChange(f, isComplex))}
          label={label || SearchMessage.Search.niceToString()} />;

      }

      if (f.filters.some(a => !a.token))
        return <ValueLine ctx={ctx} type={{ name: "string" }} onChange={() => handleValueChange(f)} label={label || SearchMessage.Search.niceToString()} />

      if (f.filters.map(a => getFilterGroupUnifiedFilterType(a.token!.type) ?? "").distinctBy().onlyOrNull() == null && ctx.value)
        ctx.value = undefined;

      var tr = f.filters.map(a => a.token!.type).distinctBy(a => a.name).onlyOrNull();
      var format = (tr && f.filters.map((a, i) => a.token!.format ?? "").distinctBy().onlyOrNull() || null) ?? undefined;
      var unit = (tr && f.filters.map((a, i) => a.token!.unit ?? "").distinctBy().onlyOrNull() || null) ?? undefined;
      const vlt = tr && ValueLineController.getValueLineType(tr);

      return <ValueLine ctx={ctx} type={vlt != null ? tr! : { name: "string" }} format={format} unit={unit} onChange={() => handleValueChange(f)} label={label || SearchMessage.Search.niceToString()} />

    }

    if (f.operation == "ComplexCondition" || f.operation == "FreeText") {
      const isComplex = f.operation == "ComplexCondition";
      return <FilterTextArea ctx={ctx}
        isComplex={isComplex}
        onChange={(() => handleValueChange(f, isComplex))}
        label={label || SearchMessage.Search.niceToString()} />
    }

    if (isList(f.operation!))
      return (
        <FormGroup ctx={ctx} label={label}>
          {inputId => f.token?.filterType == "Lite" ?
            <MultiEntity values={f.value} readOnly={readOnly} type={f.token.type.name} onChange={() => handleValueChange(f)} /> :
            <MultiValue values={f.value} readOnly={readOnly} onChange={() => handleValueChange(f)}
              onRenderItem={ctx => createFilterValueControl(ctx, f.token!, () => handleValueChange(f), { mandatory: true })} />}
        </FormGroup>
      );

    return createFilterValueControl(ctx, f.token!, () => handleValueChange(f), { label, forceNullable: f.pinned!.active == "WhenHasValue" });
  }

  function handleValueChange(f: FilterOptionParsed, avoidSearch?: boolean) {

    if (isFilterGroupOptionParsed(f) || f.token && f.token.filterType == "String") {

      if (timeoutWriteText.current)
        clearTimeout(timeoutWriteText.current);

      timeoutWriteText.current = window.setTimeout(() => {
        p.onFiltersChanged && p.onFiltersChanged(p.filterOptions, avoidSearch);
        timeoutWriteText.current = null;
      }, 200);

    } else {
      p.onFiltersChanged && p.onFiltersChanged(p.filterOptions, avoidSearch);
    }
  }

  function handleFiltersKeyUp(e: React.KeyboardEvent<HTMLDivElement>) {
    if (p.onSearch && e.keyCode == 13) {
      window.setTimeout(() => {
        p.onSearch!();
      }, 200);
    }
  }

}

function getAllPinned(filterOptions: FilterOptionParsed[]): FilterOptionParsed[] {
  var direct = filterOptions.filter(a => a.pinned != null);

  var recursive = filterOptions
    .flatMap(f => f.pinned == null && isFilterGroupOptionParsed(f) ? getAllPinned(f.filters) : []);

  return direct.concat(recursive);
}
