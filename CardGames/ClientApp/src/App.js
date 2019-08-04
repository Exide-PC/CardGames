import React, { Fragment } from 'react';
import { Route } from 'react-router-dom';

import GameView from './routes/GameView';
import AllRooms from './routes/AllRooms';

export default () => {

  return (
    <Fragment>
      {/* <GameView /> */}
      <AllRooms />
    </Fragment>
  )
}

