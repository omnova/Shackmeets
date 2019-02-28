import React from 'react'
import { AuthContext } from './AuthContext'

// This is the provider and initial state wrapped in one.  I blame Nixxed.
class AuthProvider extends React.Component {
  constructor(props) {
    super(props);

    const { username, token } = this.loadStorage();

    this.state = {
      isLoggedIn: (username && token),
      username,
      token
    };
  }

  loadStorage() {
    try {
      const storageValue = localStorage.getItem('auth') || '{}';

      return JSON.parse(storageValue) || {};
    } catch (ex) {
      console.log('Invalid storage value: auth', ex);
    }
  }

  login(username, password) {
    //const body = { username, password };

    //fetch('api/Login', {
    //    method: "POST",
    //    headers: {
    //      "Content-Type": "application/json; charset=utf-8",
    //      // "Content-Type": "application/x-www-form-urlencoded",
    //    },
    //    body: JSON.stringify(body),
    //  })
    //  .then(response => response.json())
    //  .then(data => {
    //    if (data.result = 'success') {
    //      const token = data.token;

    //      localStorage.setItem('auth', JSON.stringify({ username, token }));

    //      this.setState({ isLoggedIn: true, username, token });
    //    } else {
    //      console.log('Invalid login credentials.');
    //    }
    //  });

    const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im9tbm92YSIsIm5iZiI6MTUzOTMxNjM0MSwiZXhwIjoxNTQxOTA4MzQxLCJpYXQiOjE1MzkzMTYzNDF9.W-pjz3wrl4OjxNAf2slejbiuitxtBXlCLlxxkzs007A';

    localStorage.setItem('auth', JSON.stringify({ username, token }));
    this.setState({ isLoggedIn: true, username, token });

    //try {
    //  const data = { username, password };

    //  const result = fetch('api/Login', {
    //    method: "POST",
    //    headers: {
    //      "Content-Type": "application/json; charset=utf-8",
    //      // "Content-Type": "application/x-www-form-urlencoded",
    //    },
    //    body: JSON.stringify(data),
    //  })
    //  .then(response => response.json())
    //  .then(function (data) {

    //    if (data.result = 'success') {
    //      const token = data.token;

    //      localStorage.setItem('auth', JSON.stringify({ username, token }));

    //      this.setState({ isLoggedIn: true, username, token });
    //    } else {
    //      console.log('Invalid login credentials.');
    //      // TODO: toast user
    //    }
    //  });
    //} catch (ex) {
    //  console.log('Error while logging in', ex);
    //}
  }

  logout() {
    localStorage.removeItem('auth')

    this.setState({ isLoggedIn: false, username: null, token: null })
  }

  render() {
    const contextValue = {
      ...this.state,

      login: this.login,
      logout: this.logout
    }

    return (
      <AuthContext.Provider value={contextValue}>
        {this.props.children}
      </AuthContext.Provider>
    )
  }
}

export default AuthProvider;