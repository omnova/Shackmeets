import React from 'react';
import Shackmeet from '../shackmeet/Shackmeet';

class Archive extends React.Component {
  displayName = Archive.name

  constructor(props) {
    super(props);
    this.state = { meets: [], loading: true };

    fetch('api/GetArchivedShackmeets')
      .then(response => response.json())
      .then(data => {
        this.setState({ meets: data, loading: false });
      });
  }

  static renderShackmeets(meets) {
    return (
      <div>
        {meets.map(meet =>
          <Shackmeet meet={meet}/>
        )}
      </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Archive.renderShackmeets(this.state.meets);

    return (
      <div>
        <h1>Past Shackmeets</h1>
        {contents}
      </div>
    );
  }
}

export default Archive;