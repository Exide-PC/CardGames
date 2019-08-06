import React, { Fragment } from 'react';
import { createPortal } from 'react-dom';

import './style.css';

const RegistrationModal = ({ onClose }) => {

  return createPortal(
    <Fragment>
        <div className="modal">
            <h2>Введите имя</h2>
            <div className="modal__input-container">
              <input className="modal__input" type="text" />
              <div className="modal__line" />
            </div>
        </div>
        <div className="shadow-field" onMouseDown={onClose} />
    </Fragment>,
    document.body
  )
}

export default RegistrationModal;
