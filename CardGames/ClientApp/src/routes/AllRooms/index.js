import React from 'react';
import { rooms } from './data.json';
import Room from '../../components/Room';

import './style.css';

const AllRooms = () => {
  console.log(rooms)

  return (
    <div className='container'>
      <div className='inner-container'>
        {rooms.map((room, index) => (
          <Room key={room.uuid} number={index} {...room} />
        ))}
      </div>
    </div>
  )

}

export default AllRooms;