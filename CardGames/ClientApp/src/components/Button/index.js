import React from 'react';

import './style.css';

const Button = ({ text, ...props }) => {
  const className = `button ${props.className}`;

  return (
    <button {...props} className={className}>
      {text}
    </button>
  );
};

export default Button;
