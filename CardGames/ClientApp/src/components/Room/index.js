import React from 'react';

import './style.css';

const Room = ({ players, number }) => {

  console.log(players)

  return (
      <div className='room-container'>
        <div className='room'>
          <div className='room__background' />
          <h2 className='room__number'>Комната № {number}</h2>
          <p className='room__players'>Игрков: {players.length}</p>
          <button className='room__join'>Присоединиться</button>
        </div>
      </div>
  )
}

export default Room;