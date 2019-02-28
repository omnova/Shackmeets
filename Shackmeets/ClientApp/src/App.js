import React, { Component } from 'react';
import { Route } from 'react-router';

import AuthProvider from './context/auth/AuthProvider'
import Layout from './components/Layout';
import Home from './components/Home';
import Archive from './components/Archive';
import Login from './components/Login';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <AuthProvider>
        <Layout>
          <Route exact path='/' component={Home} />
          <Route path='/archive' component={Archive} />
          <Route path='/login' component={Login} />
        </Layout>
      </AuthProvider>
    );
  }
}
