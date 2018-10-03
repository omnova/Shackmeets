import React from 'react';
import { Route } from 'react-router';

// Components
import Layout from './components/Layout';

import Home from './components/Home';
import Login from './components/Login';
import Listing from './components/Listing';
import Archive from './components/Archive';
import CreateShackmeet from './components/CreateShackmeet';
import EditShackmeet from './components/EditShackmeet';
import ViewShackmeet from './components/ViewShackmeet';

import FetchData from './components/FetchData';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/listing' component={Listing} />
    <Route path='/archive' component={Archive} />
    <Route path='/create' component={CreateShackmeet} />
    <Route path='/edit' component={EditShackmeet} />
    <Route path='/view' component={ViewShackmeet} />
    <Route path='/fetchdata/:startDateIndex?' component={FetchData} />
    <Route path='/login' component={Login} />
  </Layout>
);
