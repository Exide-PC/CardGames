import { useEffect } from 'react';

const useOnClickOutSide = (ref, callback) => {
  useEffect(() => {
    const listener = ({ target }) => {
      if (ref.current.contains(target)) return;

      callback();
    };

    window.addEventListener('mousedown', listener);
    return () => window.removeEventListener('mousedown', listener);
  }, [ref, callback]);
};

export default useOnClickOutSide;
