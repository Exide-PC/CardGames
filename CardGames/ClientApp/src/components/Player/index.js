import React, { useState, Fragment } from 'react';
import PropTypes from 'prop-types';

import './style.css';
import Card from '../Card';
// import Modal from "../Modal";

const Player = ({ id, name, cards, index }) => {
  const [activeModal, setActiveModal] = useState(false);
  const [activeCard, setActiveCard] = useState('');

  const toggleModal = () => {
    if (activeModal) return;
    setActiveModal(true);
    setTimeout(() => {
      setActiveModal(false);
    }, 2000);
  };

  return (
    <Fragment>
      <div key={id} className={`player player-${index}`}>
        <div className="cards">
          {cards.map((card, index) => (
            <Card
              {...card}
              key={index}
              toggleModal={toggleModal}
              setActive={() => setActiveCard(`${card.value}-${card.type}`)}
              isActive={activeCard === `${card.value}-${card.type}`}
            />
          ))}
        </div>
        <div className="player__cards" />
        {name}
      </div>
      {/* {activeModal && <Modal />} */}
    </Fragment>
  );
};

Player.propTypes = {
  id: PropTypes.number,
  name: PropTypes.string,
  cards: PropTypes.arrayOf(PropTypes.object),
  index: PropTypes.number
};

export default Player;
