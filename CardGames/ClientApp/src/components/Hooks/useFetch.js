import { useState, useEffect } from 'react';

const useFetch = (url, method, data, headers, callback) => {
  const [loading, setLoading] = useEffect(false);
  const [result, setResult] = useState('');

  url =
    mathod.toLowerCase() === 'get' && data ? url + serializeObject(data) : url;
  data = method.toLowerCase() === 'post' && data;

  const doFetch = () => {
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
  };

  return [doFetch, result, loading];
};

export default useFetch;
