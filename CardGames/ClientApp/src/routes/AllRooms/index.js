import React, { useState } from 'react';
import Room from '../../components/Room';
import Button from '../../components/Button'; // todo different game types
import RegistrationModal from '../../components/RegistrationModal';
import useApi from '../../components/Hooks/useApi';
import Loader from '../../components/Loader';

import './style.css';

const AllRooms = () => {
  const [activeModal, setActiveModal] = useState(false);
  const [activeCreateModal, setActiveCreateModal] = useState(false);
  const [rooms, loading] = useApi('api/Lobby/list', 2000);

  const openCreate = () => {
    setActiveModal(true);
    setActiveCreateModal(true);
  };

  const onClose = () => {
    setActiveModal(false);
    setActiveCreateModal(false);
  };

  return (
    <div className="bg-rooms">
      <div className="container">
        <Button text="Создать игру" onClick={openCreate} />
        {!loading && !rooms && <h1 className="no-games">Нет доступных игр</h1>}
        <div className="inner-container">
          {rooms &&
            rooms.map((room, index) => (
              <Room
                key={room.uid}
                number={index + 1}
                onClick={() => setActiveModal(true)}
                {...room}
              />
            ))}
        </div>
      </div>
      {activeModal && (
        <RegistrationModal onClose={onClose} isCreate={activeCreateModal} />
      )}
      {loading && <Loader />}
    </div>
  );
};

export default AllRooms;
