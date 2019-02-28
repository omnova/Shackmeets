import React from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

class NavMenu extends React.Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to={'/'}>shackmeets</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            <LinkContainer to={'/'} exact>
              <NavItem>
                <Glyphicon glyph='home' /> Home
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/archive'}>
              <NavItem>
                <Glyphicon glyph='th-list' /> Archive
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/login'}>
              <NavItem>
                <Glyphicon glyph='th-list' /> Login
              </NavItem>
            </LinkContainer>       
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}

export default NavMenu;