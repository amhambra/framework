﻿/// <reference path="../../../../framework/signum.react/scripts/globals.ts" />

import * as React from 'react'
import { Link } from 'react-router'
import { NavDropdown, MenuItem, NavItem } from 'react-bootstrap'
import { LinkContainer } from 'react-router-bootstrap'
import { AuthMessage, UserEntity } from '../Signum.Entities.Authorization'
import * as AuthClient from '../AuthClient'

export default class LoginUserControl extends React.Component<{}, { user: UserEntity }> {

    render() {
        var user = AuthClient.currentUser();

        if (!user)
            return <LinkContainer to="~/auth/login"><NavItem  className="sf-login">{AuthMessage.Login.niceToString() }</NavItem></LinkContainer>;

        return (
            <NavDropdown className="sf-user" title={user.userName} id="sfUserDropDown">
                <LinkContainer to="~/auth/changePassword"><MenuItem>{AuthMessage.ChangePassword.niceToString() }</MenuItem></LinkContainer>
                <MenuItem id="sf-auth-logout" onSelect={() => AuthClient.logout() }>{AuthMessage.Logout.niceToString() }</MenuItem>
            </NavDropdown>);
    }
}
