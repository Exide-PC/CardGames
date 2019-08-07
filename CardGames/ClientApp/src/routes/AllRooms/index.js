import React, { useState } from 'react';
import { rooms } from './data.json';
import Room from '../../components/Room';
import Button from '../../components/Button'; // todo different game types
import RegistrationModal from '../../components/RegistrationModal';

import './style.css';

const AllRooms = () => {

  const [activeModal, setActiveModal] = useState(false);

  const onClick = () => setActiveModal(true);

  return (
    <div className="bg-rooms">
      <div className="container">
        <Button text='Создать игру' onClick={onClick} />
        <div className="inner-container">
          {rooms.map((room, index) => (
            <Room key={room.uuid} number={index} onClick={onClick} {...room} />
          ))}
        </div>
      </div>
      {activeModal && <RegistrationModal onClose={() => setActiveModal(false)} />}
    </div>
  )

}

export default AllRooms;
