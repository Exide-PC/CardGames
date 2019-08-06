import React, { useState } from 'react';
import RegistrationModal from '../RegistrationModal';

import './style.css';

const Room = ({ players, number }) => {

  const [activeModal, setActiveModal] = useState(false);

  return (
      <div onClick={() => setActiveModal(true)} className='room-container'>
        <div className='room'>
          <div className='room__background' />
          <h2 className='room__number'>Комната № {number}</h2>
          <p className='room__players'>Игрков: {players.length}</p>
        </div>
        {activeModal && <RegistrationModal onClose={() => setActiveModal(false)} />}
      </div>
  )
}

export default Room;