import React from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export default props => (
  <Navbar inverse fixedTop fluid collapseOnSelect>
    <Navbar.Header>
      <Navbar.Brand>
        <Link to={'/'}>Shackmeets</Link>
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
        <LinkContainer to={'/create'}>
          <NavItem>
            <Glyphicon glyph='th-list' /> Create
          </NavItem>
        </LinkContainer>
        <LinkContainer to={'/edit'}>
          <NavItem>
            <Glyphicon glyph='th-list' /> Edit
          </NavItem>
        </LinkContainer>
        <LinkContainer to={'/view'}>
          <NavItem>
            <Glyphicon glyph='th-list' /> View
          </NavItem>
        </LinkContainer>
        <LinkContainer to={'/login'}>
          <NavItem>
            <Glyphicon glyph='education' /> Login
          </NavItem>
        </LinkContainer>
      </Nav>
    </Navbar.Collapse>
  </Navbar>
);
