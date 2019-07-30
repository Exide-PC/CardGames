import { useEffect } from 'react';

const UseOnClickOutSide = (ref, callback) => {

  useEffect(() => {

    const func = e => {
      if (e.target !== ref.current) callback()
    }

    window.addEventListener('click', func)

    return () => window.removeEventListener('click', func)
  }, [ref, callback])

}

export default UseOnClickOutSide;
