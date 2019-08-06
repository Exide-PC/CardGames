import React from 'react';
import { createPortal } from 'react-dom';

import './style.css';

const Alert = ({ text, isVisible }) => {

  return createPortal(
    <div className='modal'>{text}</div>,
    document.body
  )
}

export default Alert;
