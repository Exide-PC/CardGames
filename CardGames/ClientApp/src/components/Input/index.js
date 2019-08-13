import React from 'react';

import './style.css';

const Input = ({ value, placeholder, onChange, onSumbit }) => {
  return (
    <div className="modal__input-container">
      <input
        type="text"
        value={value}
        className="modal__input"
        onChange={({ target: { value } }) => onChange(value)}
        onKeyDown={onSumbit}
        placeholder={placeholder}
      />
      <div className="modal__line" />
    </div>
  );
};

export default Input;
