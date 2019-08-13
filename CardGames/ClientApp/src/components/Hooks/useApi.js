import { useState, useEffect } from 'react';

const Api = (url, timeout = 1000) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const timerId = setInterval(() => {
      fetch(url)
        .then(resp => resp.json())
        .then(setData)
        .catch(() => {});
    }, timeout);

    setTimeout(() => setLoading(false), timeout + 300);

    return () => clearInterval(timerId);
  }, []);

  return [data, loading];
};

export default Api;
