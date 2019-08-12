import { useState, useEffect } from 'react';

const Api = (url, timeout = 1000) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {

    const timerId = setInterval(() => {
      fetch(url)
        .then(resp => resp.json())
        .then(setData)
    }, timeout);
    
    setTimeout(() => setLoading(false), timeout + 200);

    return () => clearInterval(timerId)

  }, [])

  return [data, loading]
}

export default Api;