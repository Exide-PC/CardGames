import React, { useState ,useEffect, Fragment } from 'react';
import PropTypes from "prop-types";
import cx from 'classnames';

import './style.css';

const Card = ({ value, type, setActive, isActive }) => {

  const [cardUrl, setCardUrl] = useState('');

  useEffect(() => {
    import(`../../images/card-${value}-${type}.svg`).then(resp => setCardUrl(`url(${resp}) center center no-repeat`));
  }, [value, type]);

  const cardClassName = cx('cards__card', {
    'cards__card--active': isActive
  })

  return (
    <Fragment>
        <div
          className={cardClassName}
          data-value={value}
          data-type={type}
          style={{ background: cardUrl }}
          onClick={setActive}
        />
    </Fragment>
  );
}

Card.propTypes = {
 value: PropTypes.number,
 type: PropTypes.string,
}

export default Card
