import * as React from 'react'
import * as draftjs from 'draft-js';
import HtmlEditor, { HtmlEditorController, HtmlEditorProps } from './HtmlEditor';
import { TypeContext } from '@framework/TypeContext';
import { ErrorBoundary } from '@framework/Components';
import { getTimeMachineIcon } from '@framework/Lines/TimeMachineIcon';
import ListCommandsPlugin from './Plugins/ListCommandsPlugin';
import BasicCommandsPlugin from './Plugins/BasicCommandsPlugin';
import './HtmlEditorLine.css';
import { classes } from '@framework/Globals';
import { FormGroup } from '@framework/Lines';

export interface HtmlEditorLineProps extends Omit<HtmlEditorProps & Partial<draftjs.EditorProps>, "binding"> {
  ctx: TypeContext<string | null | undefined>;
  htmlEditorRef?: React.Ref<HtmlEditorController>;
  extraButtons?: () => React.ReactNode;
  extraButtonsBefore?: () => React.ReactNode;
}

export default function HtmlEditorLine({ ctx, htmlEditorRef, readOnly, extraButtons, extraButtonsBefore, ...p }: HtmlEditorLineProps): React.JSX.Element {

  return (
    <FormGroup ctx={ctx} >
      {id =>
        <ErrorBoundary>
          <div className="d-flex">
            {extraButtonsBefore && <div className={ctx.inputGroupVerticalClass("before")}>
              {extraButtonsBefore()}
            </div>}
            <div className={classes("html-editor-line")} style={{ backgroundColor: readOnly ? "#e9ecef" : undefined, ...p.htmlAttributes?.style }} data-property-path={ctx.propertyPath} >
              {getTimeMachineIcon({ ctx: ctx })}
              <HtmlEditor

                binding={ctx.binding}
                ref={htmlEditorRef}
                plugins={p.plugins ?? [
                  new ListCommandsPlugin(),
                  new BasicCommandsPlugin(),
                ]}
                {...p}
              />
            </div>
            {extraButtons && <div className={ctx.inputGroupVerticalClass("after")}>
              {extraButtons()}
            </div>
            }
          </div>
        </ErrorBoundary>
      }
    </FormGroup>
  );
}
