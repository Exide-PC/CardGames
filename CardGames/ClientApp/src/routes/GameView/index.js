import React from 'react';

import './style.css';
import { players } from './data.json';

import Player from '../../components/Player';

const GameView = () => {

  return (
    <div className='game-container'>
      {players.map((player, index) => (
        <Player key={player.id} index={index} {...player} />
      ))}
    </div>
  )
}

GameView.propTypes = {

}

export default GameView;