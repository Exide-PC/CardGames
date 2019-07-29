import React from 'react';
import PropTypes from 'prop-types';

import './style.css';

import card_2_clubs from '../../images/card-2-clubs.svg'

const Player = ({ id, name, cards, index }) => {

  return (
    <div 
      key={id} 
      className={`player player-${index}`}
    >
      <div className='cards'>
        {cards.map((card, index) => (
          <div className='cards__card' key={index} style={{ background: `url(${card_2_clubs})` }} />
        ))}
      </div>
      <div className='player__cards' />
      {name}
    </div>
  )
}

// Перенести карты в отдельный компонент

Player.propTypes = {
  id: PropTypes.number,
  name: PropTypes.string,
  cards: PropTypes.arrayOf(PropTypes.object),
  index: PropTypes.number,
}

export default Player;