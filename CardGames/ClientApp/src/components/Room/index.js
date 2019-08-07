import React, { useState } from 'react';

import './style.css';

const Room = ({ players, number, onClick }) => {

  return (
      <div className='room-container' onClick={onClick}>
        <div className='room'>
          <div className='room__background' />
          <h2 className='room__number'>Комната № {number}</h2>
          <p className='room__players'>Игрков: {players.length}</p>
        </div>
      </div>
  )
}

export default Room;