import { serializeObject } from '../../utils';
export default {
  createRoom: data => fetch(`api/Lobby/create${serializeObject(data)}`)
};
