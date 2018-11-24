import * as React from 'react'
import * as d3 from 'd3'
import D3ChartBase from './D3ChartBase';
import * as ChartClient from '../ChartClient';
import * as ChartUtils from '../Templates/ChartUtils';
import { translate, scale, rotate, skewX, skewY, matrix, scaleFor, rule, ellipsis, PivotRow, PivotColumn } from '../Templates/ChartUtils';
import { ChartTable, ChartColumn } from '../ChartClient';


export default class MultiLineChart extends D3ChartBase {

  drawChart(data: ChartTable, chart: d3.Selection<SVGElement, {}, null, undefined>, width: number, height: number) {

    var c = data.columns;
    var keyColumn = c.c0 as ChartColumn<unknown>;
    var valueColumn0 = c.c2 as ChartColumn<number>;

    var pivot = c.c1 == null ?
      ChartUtils.toPivotTable(data, c.c0!, [c.c2, c.c3, c.c4, c.c5, c.c6].filter(cn => cn != undefined) as ChartColumn<number>[]) :
      ChartUtils.groupedPivotTable(data, c.c0!, c.c1, c.c2 as ChartColumn<number>);

    var xRule = rule({
      _1: 5,
      title: 15,
      _2: 10,
      labels: parseInt(data.parameters["UnitMargin"]),
      _3: 5,
      ticks: 4,
      content: '*',
      _4: 10,
    }, width);
    //xRule.debugX(chart)

    var yRule = rule({
      _1: 5,
      legend: 15,
      _2: 20,
      content: '*',
      ticks: 4,
      _3: 5,
      labels0: 15,
      labels1: 15,
      _4: 10,
      title: 15,
      _5: 5,
    }, height);
    //yRule.debugY(chart);

    var x = d3.scaleBand()
      .domain(pivot.rows.map(r => keyColumn.getKey(r.rowValue)))
      .range([0, xRule.size('content')]);

    var allValues = pivot.rows.flatMap(r => pivot.columns.map(function (c) { return r.values[c.key] && r.values[c.key].value; }));

    var y = scaleFor(valueColumn0, allValues, 0, yRule.size('content'), data.parameters["Scale"]);

    chart.append('svg:g').attr('class', 'x-tick').attr('transform', translate(xRule.start('content') + (x.bandwidth() / 2), yRule.start('ticks')))
      .enterData(pivot.rows, 'line', 'x-tick')
      .attr('y2', (d, i) => yRule.start('labels' + (i % 2)) - yRule.start('ticks'))
      .attr('x1', r => x(keyColumn.getKey(r.rowValue))!)
      .attr('x2', d => x(keyColumn.getKey(d.rowValue))!)
      .style('stroke', 'Black');

    if ((x.bandwidth() * 2) > 60) {
      chart.append('svg:g').attr('class', 'x-label').attr('transform', translate(xRule.start('content') + (x.bandwidth() / 2), yRule.middle('labels0')))
        .enterData(pivot.rows, 'text', 'x-label')
        .attr('x', d => x(keyColumn.getKey(d.rowValue))!)
        .attr('y', function (d, i) { return yRule.middle('labels' + (i % 2)) - yRule.middle('labels0'); })
        .attr('dominant-baseline', 'middle')
        .attr('text-anchor', 'middle')
        .text(v => keyColumn.getNiceName(v.rowValue))
        .each(function (v) { ellipsis(this as SVGTextElement, x.bandwidth() * 2); });
    }

    chart.append('svg:g').attr('class', 'x-title').attr('transform', translate(xRule.middle('content'), yRule.middle('title')))
      .append('svg:text').attr('class', 'x-title')
      .attr('text-anchor', 'middle')
      .attr('dominant-baseline', 'middle')
      .text(keyColumn.title);

    var yTicks = y.ticks(10);
    var yTickFormat = y.tickFormat(height / 50);
    chart.append('svg:g').attr('class', 'y-line').attr('transform', translate(xRule.start('content'), yRule.end('content')))
      .enterData(yTicks, 'line', 'y-line')
      .attr('x2', xRule.size('content'))
      .attr('y1', t => -y(t))
      .attr('y2', t => -y(t))
      .style('stroke', 'LightGray');

    chart.append('svg:g').attr('class', 'y-tick').attr('transform', translate(xRule.start('ticks'), yRule.end('content')))
      .enterData(yTicks, 'line', 'y-tick')
      .attr('x2', xRule.size('ticks'))
      .attr('y1', t => -y(t))
      .attr('y2', t => -y(t))
      .style('stroke', 'Black');

    chart.append('svg:g').attr('class', 'y-label').attr('transform', translate(xRule.end('labels'), yRule.end('content')))
      .enterData(yTicks, 'text', 'y-label')
      .attr('y', t => -y(t))
      .attr('dominant-baseline', 'middle')
      .attr('text-anchor', 'end')
      .text(yTickFormat);

    chart.append('svg:g').attr('class', 'y-label').attr('transform', translate(xRule.middle('title'), yRule.middle('content')) + rotate(270))
      .append('svg:text').attr('class', 'y-label')
      .attr('text-anchor', 'middle')
      .attr('dominant-baseline', 'middle')
      .text(pivot.title);

    var color = d3.scaleOrdinal(ChartUtils.getColorScheme(data.parameters["ColorCategory"], parseInt(data.parameters["ColorCategorySteps"]))).domain(pivot.columns.map(s => s.key));

    var pInterpolate = data.parameters["Interpolate"];

    //paint graph - line

    const me = this;

    chart.enterData(pivot.columns, 'g', 'shape-serie').attr('transform', translate(xRule.start('content') + x.bandwidth() / 2, yRule.end('content')))
      .each(function (s) {
        d3.select<SVGGElement, PivotColumn>(this as SVGGElement)
          .append('svg:path').attr('class', 'shape')
          .attr('stroke', s => s.color || color(s.key))
          .attr('fill', 'none')
          .attr('stroke-width', 3)
          .attr('shape-rendering', 'initial')
          .attr('d', function (s) {
            return d3.line<PivotRow>()
              .x(r => x(keyColumn.getKey(r.rowValue))!)
              .y(r => -y(r.values[s.key] && r.values[s.key].value))
              .defined(r => r.values[s.key] && r.values[s.key].value != null)
              .curve(ChartUtils.getCurveByName(pInterpolate)!)
              (pivot.rows);
          });

        //paint graph - hover area trigger
        d3.select(this).enterData(pivot.rows, 'circle', 'hover')
          .filter(r => r.values[s.key] != undefined)
          .attr('cx', r => x(keyColumn.getKey(r.rowValue))!)
          .attr('cy', r => -y(r.values[s.key] && r.values[s.key].value))
          .attr('r', r => 15)
          .attr('fill', '#fff')
          .attr('fill-opacity', 0)
          .attr('stroke', 'none')
          .on('click', r => me.props.onDrillDown(r.values[s.key].rowClick))
          .style("cursor", "pointer")
          .append('svg:title')
          .text(r => r.values[s.key].value);

        d3.select(this).enterData(pivot.rows, 'circle', 'point')
          .filter(r => r.values[s.key] != undefined)
          .attr('fill', r => s.color || color(s.key))
          .attr('r', 5)
          .attr('cx', r => x(keyColumn.getKey(r.rowValue))!)
          .attr('cy', r => -y(r.values[s.key] && r.values[s.key].value))
          .attr('shape-rendering', 'initial')
          .on('click', r => me.props.onDrillDown(r.values[s.key].rowClick))
          .style("cursor", "pointer")
          .append('svg:title')
          .text(v => v.values[s.key].valueTitle);

        if (parseFloat(data.parameters["NumberOpacity"]) > 0) {
          //paint graph - points-labels
          d3.select(this).enterData(pivot.rows, 'text', 'point-label')
            .filter(r => r.values[s.key] != undefined)
            .attr('text-anchor', 'middle')
            .attr('opacity', data.parameters["NumberOpacity"])
            .attr('x', r => x(keyColumn.getKey(r.rowValue))!)
            .attr('y', r => -y(r.values[s.key] && r.values[s.key].value) - 8)
            .on('click', r => me.props.onDrillDown(r.values[s.key].rowClick))
            .style("cursor", "pointer")
            .attr('shape-rendering', 'initial')
            .text(r => r.values[s.key].value);
        }
      });

    var legendScale = d3.scaleBand()
      .domain(pivot.columns.map((s, i) => i.toString()))
      .range([0, xRule.size('content')]);

    if (legendScale.bandwidth() > 50) {

      var legendMargin = yRule.size('legend') + 4;

      chart.append('svg:g').attr('class', 'color-legend').attr('transform', translate(xRule.start('content'), yRule.start('legend')))
        .enterData(pivot.columns, 'rect', 'color-rect')
        .attr('x', (e, i) => legendScale(i.toString())!)
        .attr('width', yRule.size('legend'))
        .attr('height', yRule.size('legend'))
        .attr('fill', s => s.color || color(s.key));

      chart.append('svg:g').attr('class', 'color-legend').attr('transform', translate(xRule.start('content') + legendMargin, yRule.middle('legend') + 1))
        .enterData(pivot.columns, 'text', 'color-text')
        .attr('x', (e, i) => legendScale(i.toString())!)
        .attr('dominant-baseline', 'middle')
        .text(s => s.niceName!)
        .each(function (s) { ellipsis(this as SVGTextElement, legendScale.bandwidth() - legendMargin); });
    }

    chart.append('svg:g').attr('class', 'x-axis').attr('transform', translate(xRule.start('content'), yRule.end('content')))
      .append('svg:line')
      .attr('class', 'x-axis')
      .attr('x2', xRule.size('content'))
      .style('stroke', 'Black');

    chart.append('svg:g').attr('class', 'y-axis').attr('transform', translate(xRule.start('content'), yRule.start('content')))
      .append('svg:line')
      .attr('class', 'y-axis')
      .attr('y2', yRule.size('content'))
      .style('stroke', 'Black');

  }
}
