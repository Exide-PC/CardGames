import React, { Fragment, useState, useEffect } from 'react';
import { createPortal } from 'react-dom';
import { withRouter } from 'react-router-dom';

import Input from '../Input';

import './style.css';

const RegistrationModal = ({ onClose, isCreate, history }) => {
  // переделать с children
  const [name, setName] = useState('');
  const [roomName, setRoomName] = useState('');
  const [error, setError] = useState(false);

  const showError = () => {
    if (window.timeId) return;
    setError(true);
    window.timeId = setTimeout(() => {
      setError(false);
      window.timeId = null;
    }, 1000);
  };

  const onSumbit = ({ keyCode }) => {
    const values = isCreate ? [name, roomName] : [name];
    if (keyCode !== 13) return;
    if (values.every(value => value.trim())) {
      history.push('/Game');
    } else showError();
  };

  useEffect(() => () => {
    clearTimeout(window.timeId);
    window.timeId = null;
  });

  const header = isCreate ? 'Введите имя и название комнаты' : 'Введите имя';

  return createPortal(
    <Fragment>
      <div className="modal">
        <h2 className={`${error && 'modal__error'}`}>{header}</h2>
        <Input
          value={name}
          placeholder="Имя"
          onChange={value => setName(value)}
          onSumbit={onSumbit}
        />
        {isCreate && (
          <Input
            value={roomName}
            placeholder="Название комнаты"
            onChange={value => setRoomName(value)}
            onSumbit={onSumbit}
          />
        )}
      </div>
      <div className="shadow-field" onMouseDown={onClose} />
    </Fragment>,
    document.body
  );
};

export default withRouter(RegistrationModal);
