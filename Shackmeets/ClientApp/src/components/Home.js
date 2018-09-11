import React from 'react';
import { connect } from 'react-redux';
import Listing from './Listing';

class Home extends React.Component {
  render() {
    return (
      <Listing />
    );
  }
}

export default connect()(Home);
