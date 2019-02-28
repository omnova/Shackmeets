import React from 'react';
import AuthConsumer from '../context/auth/AuthConsumer';

class Login extends React.Component {
  displayName = Login.name

  constructor(props) {
    super(props);

    this.state = {
      username: '',
      password: ''
    }
  }

  handleChange = event => this.setState({ [event.target.id]: event.target.value })

  handleClick = async () => {
    const username = 'omnova';
    const password = 'bddf4';

    const { login } = this.props;

    login(username, password);

    this.setState({ username: '', password: '' });

    //onClose();
  }

  render() {
    return (
      <div>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p>Current count: <strong>{this.state.currentCount}</strong></p>

        <button onClick={this.handleClick}>Login</button>
      </div>
    );
  }
}

export default AuthConsumer(Login);