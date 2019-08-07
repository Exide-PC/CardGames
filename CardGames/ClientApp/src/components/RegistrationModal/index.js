import React, { Fragment, useState } from 'react';
import { createPortal } from 'react-dom';
import { withRouter } from 'react-router-dom';

import './style.css';

const RegistrationModal = ({ onClose, history }) => {

  const [name, setName] = useState('');
  const [error, setError] = useState(false);

  const showError = () => {
    if (window.timeId) return;
    setError(true);
    window.timeId = setTimeout(() => {
      setError(false);
      window.timeId = null;
    }, 1000);
  }

  const onSumbit = ({ keyCode }) => {
    if (keyCode !== 13) return;
    if (name.trim()) 
      history.push('/Game')
    else
      showError();
  }

  return createPortal(
    <Fragment>
        <div className="modal">
            <h2 className={`${error && "modal__error"}`}>Введите имя</h2>
            <div className="modal__input-container">
              <input 
                type="text"
                value={name}
                className="modal__input" 
                onChange={({ target: { value } }) => setName(value)} 
                onKeyDown={onSumbit} />
              <div className="modal__line" />
            </div>
        </div>
        <div className="shadow-field" onMouseDown={onClose} />
    </Fragment>,
    document.body
  )
}

export default withRouter(RegistrationModal);
