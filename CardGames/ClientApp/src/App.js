import React, { Fragment } from 'react';
import { Route } from 'react-router-dom';

import routes from './routes';

const App = () => {
  return (
    <Fragment>
      {routes.map(({ Component, ...props }, index) => (
          <Route key={index} {...props} />
      ))}
    </Fragment>
  )
}

export default App;

