import React, { useState } from 'react';

import './style.css';

const Room = ({ players, number, hasSlots, onClick }) => {
  const [show, setShow] = useState(false);

  const onMouseOver = () => setShow(true);
  const onMouseOut = () => setShow(false);

  return (
    <div className="room-container" onClick={onClick}>
      <div className="room" onMouseOver={onMouseOver} onMouseOut={onMouseOut}>
        <div className="room__background" />
        <h2 className="room__number">Комната № {number}</h2>
        {(!show || !players.length) && (
          <p className="room__players">Игрков: {players.length}</p>
        )}
        {show && !!players.length && (
          <p className="room__players">{players.join(' | ')}</p>
        )}
        {hasSlots ? <p>Есть места</p> : <p className="room__full">Мест нет</p>}
      </div>
    </div>
  );
};

export default Room;
