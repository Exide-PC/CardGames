import React, { useState, Fragment } from 'react';
import PropTypes from 'prop-types';

import './style.css';
import Card from '../Card'
import Modal from "../Modal";

const Player = ({ id, name, cards, index }) => {

  const [activeModal, setActiveModal] = useState(false);

  const toggleModal = () => {
    if (window.modalId) return;
    setActiveModal(true);
    window.modalId = setTimeout(() => {
      setActiveModal(false);
      window.modalId = null;
    }, 2000);
  }

  return (
    <Fragment>
      <div key={id} className={`player player-${index}`}>
        <div className="cards">
          {cards.map((card, index) => (
            <Card key={index} {...card} toggleModal={toggleModal} />
          ))}
        </div>
        <div className="player__cards" />
        {name}
      </div>
      {activeModal && <Modal />}
    </Fragment>
  );
}

Player.propTypes = {
  id: PropTypes.number,
  name: PropTypes.string,
  cards: PropTypes.arrayOf(PropTypes.object),
  index: PropTypes.number,
}

export default Player;
