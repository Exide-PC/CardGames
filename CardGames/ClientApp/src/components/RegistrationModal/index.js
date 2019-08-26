import React, { Fragment, useState, useEffect, useRef } from 'react';
import { createPortal } from 'react-dom';
import { withRouter } from 'react-router-dom';

import Input from '../Input';
import Button from '../Button';
import useOnClickOutSide from '../Hooks/useOnClickOutSide';

import './style.css';

const RegistrationModal = ({ onClose, isCreate, history }) => {
  const [name, setName] = useState('');
  const [roomName, setRoomName] = useState('');
  const [error, setError] = useState(false);
  const modal = useRef(null);

  const showError = () => {
    if (error) return;
    setError(true);
    setTimeout(() => {
      setError(false);
    }, 1000);
  };

  const onSumbit = ({ keyCode }) => {
    if (keyCode && keyCode !== 13) return;
    const values = isCreate ? [name, roomName] : [name];
    if (values.every(value => value.trim())) {
      history.push('/Game');
    } else showError();
  };

  useOnClickOutSide(modal, onClose);

  useEffect(() => () => {
    clearTimeout(window.timeId);
    window.timeId = null;
  });

  const header = isCreate ? 'Введите имя и название комнаты' : 'Введите имя';
  const buttonText = isCreate ? 'Создать' : 'Войти';

  return createPortal(
    <Fragment>
      <div className="modal" ref={modal}>
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
        <Button
          text={buttonText}
          className="modal__submit"
          onClick={onSumbit}
        />
      </div>
      <div className="shadow-field" />
    </Fragment>,
    document.body
  );
};

export default withRouter(RegistrationModal);
