import React from 'react';

import './style.css';
import { players } from './data.json';

import Player from '../Player';

const GameView = () => {

  return (
    <div className='container'>
      {players.map((player, index) => (
        <Player key={player.id} index={index} {...player} />
      ))}
    </div>
  )
}

GameView.propTypes = {

}

export default GameView;