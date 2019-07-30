import React, { useState ,useEffect, Fragment } from 'react';
import PropTypes from "prop-types";
import Draggable from "react-draggable";

import './style.css';

const Card = ({ value, type }) => {

  const [cardUrl, setCardUrl] = useState('');
  const [initialPosition, setInitialPosition] = useState(null);

  useEffect(() => {
    import(`../../images/card-${value}-${type}.svg`).then(resp => setCardUrl(`url(${resp}) center center no-repeat`));
  }, [value, type]);

  const stopAction = () => {
    setInitialPosition({ x: 0, y: 0 })
  }

  const onStart = () => {
    setInitialPosition(null)
  }

  return (
    <Fragment>
      <Draggable position={initialPosition} onStart={onStart}>
        <div
          className="cards__card"
          data-value={value}
          data-type={type}
          style={{ background: cardUrl }}
        />
      </Draggable>
    </Fragment>
  );
}

Card.propTypes = {
 value: PropTypes.number,
 type: PropTypes.string,
}

export default Card
