import React, { useState } from 'react';
import './Style.css'; 
//import Admin from './Admin';

const Login = () => {

    const [loggedIn, setLoggedIn] = useState(false);

  const handleLogin = () => {
    setLoggedIn(true);
  };


  return (

    <div className='main'>

<div className="container">
        {loggedIn ? (
        <></> 
      ) : (
     <form onSubmit={handleLogin}>
        
          <table>
            <tbody>
            <h1>Login Page</h1>
                
              <tr>
                <td className="td-class">
                  <label htmlFor="id">Username:</label>
                </td >
                <td >
                  <input className="input-class" 
                    type="text"
                  />
                </td >
              </tr>

              <tr>
                <td className="td-class">
                  <label htmlFor="password ">Password:</label>
                </td>
                <td >
                  <input className="input-class"
                    type="password"/>
                </td>
              </tr>
             
              <tr>
                <td className="td-class" colSpan="2">
                  <button className="button" type="submit">Login</button>
                </td >
              </tr>
            </tbody>
          </table>
        </form>)}
    </div>

    </div>

    
  );
}

export default Login;