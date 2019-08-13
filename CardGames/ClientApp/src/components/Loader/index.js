import React, { Fragment } from 'react';

import './style.css';

const loader = () => {
  return (
    <Fragment>
      <div className="shadow-field" />
      <div className="loader" />
    </Fragment>
  );
};

export default loader;
