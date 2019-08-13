import { useState, useEffect } from 'react';

const useFetch = (url, method, data, headers, callback) => {
  const [loading, setLoading] = useEffect(true);
  const [result, setResult] = useState('');

  useEffect(() => {
    fetch(url, {
      method,
      data
    })
      .then(resp => resp.json())
      .then(resp => {
        setResult(resp);
        setLoading(false);
      })
      .catch(console.log);
  }, []);

  return [result, loading];
};

export default useFetch;
