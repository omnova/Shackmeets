import React from 'react';
import AuthConsumer from '../context/auth/AuthConsumer';

class Shackmeet extends React.Component {

  constructor(props) {
    super(props);

    const { username } = this.props;

    this.state = { username };
  }

  static renderOwnedShackmeet(meet) {
    return (
      <div class="panel panel-primary" key={meet.meetId}>
        <div class="panel-heading">
          <h3 class="panel-title">{meet.name}</h3>
        </div>
        <div class="panel-body">
          {meet.description}
        </div>
        <div class="panel-footer">Panel footer</div>
      </div>
      //<tr key={this.props.meet.meetId}>
      //  <td></td>
      //  <td></td>
      //  <td>{this.props.meet.organizerUsername}</td>
      //  <td>{this.props.meet.eventDate}</td>
      //</tr>
    );
  }

  static renderAttendingShackmeet(meet) {
    return (
      <div class="panel panel-success" key={meet.meetId}>
        <div class="panel-heading">
          <h3 class="panel-title">{meet.name}</h3>
        </div>
        <div class="panel-body">
          {meet.description}
        </div>
        <div class="panel-footer">Panel footer</div>
      </div>
      //<tr key={this.props.meet.meetId}>
      //  <td></td>
      //  <td></td>
      //  <td>{this.props.meet.organizerUsername}</td>
      //  <td>{this.props.meet.eventDate}</td>
      //</tr>
    );
  }

  static renderShackmeet(meet) {
    return (
      <div class="panel panel-default" key={meet.meetId}>
        <div class="panel-heading">
          <h3 class="panel-title">{meet.name}</h3>
        </div>
        <div class="panel-body">
          {meet.description}
        </div>
        <div class="panel-footer">Panel footer</div>
      </div>
      //<tr key={this.props.meet.meetId}>
      //  <td></td>
      //  <td></td>
      //  <td>{this.props.meet.organizerUsername}</td>
      //  <td>{this.props.meet.eventDate}</td>
      //</tr>
    );
  }

  render() {
    const meet = this.props.meet;
    const username = this.state.username;

    const isOwned = (username === meet.organizerUsername);
    const isAttending = (meet.rsvps && meet.rsvps.filter(r => r.username === username && r.rsvpType != 0).length > 0);

    if (isOwned)
      return Shackmeet.renderOwnedShackmeet(meet);
    else if (isAttending)
      return Shackmeet.renderAttendingShackmeet(meet);
    else
      return Shackmeet.renderShackmeet(meet);
  }
}

export default AuthConsumer(Shackmeet);