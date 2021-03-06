import React, { useState, useEffect, useRef } from 'react';
import Room from '../../components/Room';
import Button from '../../components/Button'; // todo different game types
import RegistrationModal from '../../components/RegistrationModal';
import useApi from '../../components/Hooks/useApi';
import Loader from '../../components/Loader';
import Api from './api';

import cx from 'classnames';

import './style.css';

const AllRooms = () => {
  const [activeModal, setActiveModal] = useState(false);
  const [activeCreateModal, setActiveCreateModal] = useState(false);
  const [rooms, loading] = useApi('api/lobby/list', 2000);
  const [scrollHeight, setScrollHeight] = useState(0);

  useEffect(() => {
    setScrollHeight(document.documentElement.scrollHeight);
  }, [document.documentElement.scrollHeight]);

  // Api.createRoom({ nick: 'name', room: 'room' })

  const openCreate = () => {
    setActiveModal(true);
    setActiveCreateModal(true);
  };

  const onClose = () => {
    setActiveModal(false);
    setActiveCreateModal(false);
  };

  const containerClassName = cx('bg-rooms', {
    'bg-rooms--full-height':
      scrollHeight === document.documentElement.clientHeight
  });

  return (
    <div className={containerClassName}>
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
