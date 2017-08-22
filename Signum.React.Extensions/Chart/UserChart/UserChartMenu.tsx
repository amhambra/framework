﻿
import * as React from 'react'
import { ButtonDropdown, DropdownItem } from 'reactstrap'
import { Dic, classes } from '../../../../Framework/Signum.React/Scripts/Globals'
import * as Finder from '../../../../Framework/Signum.React/Scripts/Finder'
import { Lite, toLite } from '../../../../Framework/Signum.React/Scripts/Signum.Entities'
import { ResultTable, FindOptions, FilterOption, QueryDescription } from '../../../../Framework/Signum.React/Scripts/FindOptions'
import { SearchMessage, JavascriptMessage, parseLite, is } from '../../../../Framework/Signum.React/Scripts/Signum.Entities'
import * as Navigator from '../../../../Framework/Signum.React/Scripts/Navigator'
import SearchControl from '../../../../Framework/Signum.React/Scripts/SearchControl/SearchControl'
import { UserChartEntity, ChartRequest, ChartMessage } from '../Signum.Entities.Chart'
import * as UserChartClient from './UserChartClient'
import ChartRequestView from '../Templates/ChartRequestView'

export interface UserChartMenuProps {
    chartRequestView: ChartRequestView;
}

export default class UserChartMenu extends React.Component<UserChartMenuProps, { currentUserChart?: UserChartEntity, userCharts?: Lite<UserChartEntity>[], dropdownOpen: boolean; }> {

    constructor(props: UserChartMenuProps) {
        super(props);
        this.state = { currentUserChart: props.chartRequestView.props.userChart, dropdownOpen: false };
    }


    handleSelectedToggle = () => {

        if (this.state.dropdownOpen && this.state.userCharts == undefined)
            this.reloadList().done();
    }

    reloadList(): Promise<void> {
        return UserChartClient.API.forQuery(this.props.chartRequestView.props.chartRequest!.queryKey)
            .then(list => this.setState({ userCharts: list }));
    }


    handleSelect = (uc: Lite<UserChartEntity>) => {

        Navigator.API.fetchAndForget(uc).then(userChart => {
            const chartRequest = this.props.chartRequestView.props.chartRequest!;
            UserChartClient.Converter.applyUserChart(chartRequest, userChart, undefined)
                .then(newChartRequest => {
                    this.props.chartRequestView.props.onChange(newChartRequest);
                    this.setState({ currentUserChart: userChart, });
                })
                .done();
        }).then();
    }

    handleEdit = () => {
        Navigator.API.fetchAndForget(toLite(this.state.currentUserChart!))
            .then(userQuery => Navigator.navigate(userQuery))
            .then(() => this.reloadList())
            .done();
    }


    handleCreate = () => {

        UserChartClient.API.fromChartRequest(this.props.chartRequestView.props.chartRequest!)
            .then(userQuery => Navigator.view(userQuery))
            .then(uc => {
                if (uc && uc.id) {
                    this.reloadList()
                        .then(() => this.setState({ currentUserChart: uc}))
                        .done();
                }
            }).done();
    }

    render() {
        const label = <span><i className="glyphicon glyphicon-stats"></i> &nbsp; {UserChartEntity.nicePluralName()}</span>;
        const userCharts = this.state.userCharts;
        return (
            <ButtonDropdown title={label as any} id="userQueriesDropDown" className="sf-userquery-dropdown"
                toggle={this.handleSelectedToggle}>
                {
                    userCharts && userCharts.map((uc, i) =>
                        <DropdownItem key={i}
                            className={classes("sf-userquery", is(uc, this.state.currentUserChart) && "active") }
                            onSelect={() => this.handleSelect(uc) }>
                            { uc.toStr }
                        </DropdownItem>)
                }
                {userCharts && userCharts.length > 0 && <DropdownItem divider/> }
                {this.state.currentUserChart && <DropdownItem onSelect={this.handleEdit} >{ChartMessage.EditUserChart.niceToString()}</DropdownItem> }
                <DropdownItem onSelect={this.handleCreate}>{ChartMessage.CreateNew.niceToString()}</DropdownItem>
            </ButtonDropdown>
        );
    }
 
}
