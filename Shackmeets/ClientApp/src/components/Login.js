import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators } from '../store/Login';

class Login extends Component {
  componentWillMount() {
    // This method runs when the component is first added to the page
    //this.props.login();
  }

  componentWillReceiveProps(nextProps) {
    // This method runs when incoming props (e.g., route params) change
    //this.props.login();
  }

  render() {
    return (
      <div>
        <h1>Login</h1>

        <p>This is to test the login functionality.</p>

        <p>Current login status: <strong>{this.props.isLoggedIn ? "Logged in" : "Logged out"}</strong></p>
        <p><strong>{this.props.isLoading ? "Loading..." : "Done"}</strong></p>

        <button onClick={this.props.login}>Login</button>
        <button onClick={this.props.logout}>Logout</button>
      </div>
    );
  }
}

export default connect(
  state => state.login,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Login);
