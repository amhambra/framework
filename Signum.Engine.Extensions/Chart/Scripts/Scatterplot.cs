
using Signum.Logic.Chart;
using Signum.Entities.Chart;
using Signum.Engine.Chart;
using System.Collections.Generic;

namespace Signum.Logic.Chart.Scripts 
{
    public class ScatterplotChartScript : ChartScript                
    {
        public ScatterplotChartScript() : base(D3ChartScript.Scatterplot)
        {
            this.Icon = ChartScriptLogic.LoadIcon("points.png");
            this.Columns = new List<ChartScriptColumn>
            {
                new ChartScriptColumn("Color", ChartColumnType.Groupable),
                new ChartScriptColumn("Horizontal Axis", ChartColumnType.Positionable) ,
                new ChartScriptColumn("Vertical Axis", ChartColumnType.Positionable) 
            };
            this.Parameters = new List<ChartScriptParameter>
            {
                new ChartScriptParameter("UnitMargin", ChartParameterType.Number) {  ValueDefinition = new NumberInterval { DefaultValue = 40m } },
                new ChartScriptParameter("HorizontalScale", ChartParameterType.Enum) { ColumnIndex = 1,  ValueDefinition = EnumValueList.Parse("ZeroMax (M)|MinMax|Log (M)") },
                new ChartScriptParameter("VerticalScale", ChartParameterType.Enum) { ColumnIndex = 2,  ValueDefinition = EnumValueList.Parse("ZeroMax (M)|MinMax|Log (M)") },
                new ChartScriptParameter("PointSize", ChartParameterType.Number) {  ValueDefinition = new NumberInterval { DefaultValue = 4m } },
                new ChartScriptParameter("DrawingMode", ChartParameterType.Enum) {  ValueDefinition = EnumValueList.Parse("Svg|Canvas") },
                new ChartScriptParameter("ColorScale", ChartParameterType.Enum) {  ValueDefinition = EnumValueList.Parse("Ordinal|ZeroMax|MinMax|Sqrt|Log") },
                new ChartScriptParameter("ColorInterpolate", ChartParameterType.Enum) {  ValueDefinition = EnumValueList.Parse("YlGn|YlGnBu|GnBu|BuGn|PuBuGn|PuBu|BuPu|RdPu|PuRd|OrRd|YlOrRd|YlOrBr|Purples|Blues|Greens|Oranges|Reds|Greys|PuOr|BrBG|PRGn|PiYG|RdBu|RdGy|RdYlBu|Spectral|RdYlGn") },
                new ChartScriptParameter("ColorScheme", ChartParameterType.Enum) {  ValueDefinition = EnumValueList.Parse("category10|accent|dark2|paired|pastel1|pastel2|set1|set2|set3|BrBG[K]|PRGn[K]|PiYG[K]|PuOr[K]|RdBu[K]|RdGy[K]|RdYlBu[K]|RdYlGn[K]|Spectral[K]|Blues[K]|Greys[K]|Oranges[K]|Purples[K]|Reds[K]|BuGn[K]|BuPu[K]|OrRd[K]|PuBuGn[K]|PuBu[K]|PuRd[K]|RdPu[K]|YlGnBu[K]|YlGn[K]|YlOrBr[K]|YlOrRd[K]") },
                new ChartScriptParameter("ColorSchemeSteps", ChartParameterType.Enum) {  ValueDefinition = EnumValueList.Parse("3|4|5|6|7|8|9|10|11") }
            };
        }      
    }                
}
